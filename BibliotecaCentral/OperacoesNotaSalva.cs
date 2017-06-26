using BibliotecaCentral.Log;
using BibliotecaCentral.ModeloXML.PartesProcesso;
using BibliotecaCentral.Validacao;
using BibliotecaCentral.WebService;
using BibliotecaCentral.WebService.Pacotes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Storage.Pickers;

namespace BibliotecaCentral
{
    public class OperacoesNotaSalva
    {
        ILog Log { get; }
        AnalisadorNFe Analisador { get; }

        public OperacoesNotaSalva(ILog log, AnalisadorNFe analisador)
        {
            Log = log;
            Analisador = analisador;
        }

        public async Task<bool> Assinar(NFe nota)
        {
            try
            {
                Analisador.Normalizar();
                var assina = new Certificacao.AssinaFacil(nota);
                await assina.Assinar(nota.Informações.Id);
                return true;
            }
            catch (Exception e)
            {
                Log.Escrever(TitulosComuns.ErroSimples, e.Message);
            }
            return false;
        }

        public async Task<bool> Exportar(XElement xml, string id)
        {
            try
            {
                FileSavePicker salvador = new FileSavePicker
                {
                    DefaultFileExtension = ".xml",
                    SuggestedFileName = $"{id}.xml",
                    SuggestedStartLocation = PickerLocationId.DocumentsLibrary
                };
                salvador.FileTypeChoices.Add("Arquivo XML", new List<string> { ".xml" });
                var arquivo = await salvador.PickSaveFileAsync();
                if (arquivo != null)
                {
                    using (var stream = await arquivo.OpenStreamForWriteAsync())
                    {
                        xml.Save(stream);
                        await stream.FlushAsync();
                    }
                    return true;
                }
            }
            catch (Exception e)
            {
                Log.Escrever(TitulosComuns.ErroSimples, e.Message);
            }
            return false;
        }

        public async Task<(bool sucesso, ProtocoloNFe protocolo, string motivo)> Transmitir(NFe nota, bool homologacao)
        {
            try
            {
                var resultadoTransmissao = await new GerenciadorGeral<EnviNFe, RetEnviNFe>(nota.Informações.emitente.endereco.SiglaUF, Operacoes.Autorizar, homologacao)
                    .EnviarAsync(new EnviNFe(nota.Informações.identificação.Numero, nota));
                if (resultadoTransmissao.cStat == 103)
                {
                    await Task.Delay(new TimeSpan(0, 0, 10));
                    var resultadoResposta = await new GerenciadorGeral<ConsReciNFe, RetConsReciNFe>(resultadoTransmissao.cUF, Operacoes.RespostaAutorizar, homologacao)
                        .EnviarAsync(new ConsReciNFe(resultadoTransmissao.tpAmb, resultadoTransmissao.infRec.nRec));
                    if (resultadoResposta.protNFe.InfProt.cStat == 100)
                    {
                        return (true, resultadoResposta.protNFe, resultadoResposta.xMotivo);
                    }
                    else
                    {
                        Log.Escrever(TitulosComuns.ErroSimples, $"A nota fiscal foi processada, mas recusada. Mensagem de retorno: \n{resultadoResposta.protNFe.InfProt.xMotivo}");
                    }
                }
                else
                {
                    Log.Escrever(TitulosComuns.ErroSimples, $"A NFe não foi aceita. Mensagem de retorno: \n{resultadoTransmissao.xMotivo}\nPor favor, exporte esta nota fiscal e envie o XML gerado para o desenvolvedor do aplicativo para que o erro possa ser corrigido.");
                }
            }
            catch (Exception e)
            {
                Log.Escrever(TitulosComuns.ErroSimples, e.Message);
            }
            return (false, null, null);
        }
    }
}
