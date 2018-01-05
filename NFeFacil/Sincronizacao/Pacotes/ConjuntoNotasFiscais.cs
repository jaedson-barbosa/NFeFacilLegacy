using NFeFacil.ItensBD;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NFeFacil.Sincronizacao.Pacotes
{
    public sealed class ConjuntoNotasFiscais
    {
        public NFeDI[] NotasFiscais { get; set; }
        bool VerificarEmissao(int atual) => atual >= (int)StatusNFe.Emitida;

        public DateTime InstanteSincronizacao { get; set; }

        public ConjuntoNotasFiscais() { }
        public ConjuntoNotasFiscais(DateTime minimo)
        {
            using (var db = new AplicativoContext())
            {
                NotasFiscais = db.NotasFiscais.Where(x => x.UltimaData > minimo && VerificarEmissao(x.Status)).ToArray();
            }
        }

        public ConjuntoNotasFiscais(ConjuntoNotasFiscais existente, DateTime minimo, DateTime atual)
        {
            InstanteSincronizacao = atual;
            using (var db = new AplicativoContext())
            {
                NotasFiscais = (from local in db.NotasFiscais
                                let servidor = existente.NotasFiscais.FirstOrDefault(x => x.Id == local.Id)
                                where VerificarEmissao(local.Status)
                                where local.UltimaData > (servidor == null ? minimo : servidor.UltimaData)
                                select local).ToArray();
            }
        }

        public void AnalisarESalvar()
        {
            using (var db = new AplicativoContext())
            {
                List<NFeDI> Adicionar = new List<NFeDI>();
                List<NFeDI> Atualizar = new List<NFeDI>();

                if (NotasFiscais != null)
                {
                    for (int i = 0; i < NotasFiscais.Length; i++)
                    {
                        var novo = NotasFiscais[i];
                        var atual = db.NotasFiscais.FirstOrDefault(x => x.Id == novo.Id);
                        if (atual == null)
                        {
                            novo.UltimaData = InstanteSincronizacao;
                            Adicionar.Add(novo);
                        }
                        else if (novo.UltimaData > atual.UltimaData)
                        {
                            novo.UltimaData = InstanteSincronizacao;
                            Atualizar.Add(novo);
                        }
                    }
                }

                db.AddRange(Adicionar);
                db.UpdateRange(Atualizar);
                db.SaveChanges();
            }
        }

        public void AtualizarPadrao()
        {
            using (var db = new AplicativoContext())
            {
                NotasFiscais = db.NotasFiscais.Where(x => VerificarEmissao(x.Status)).ToArray();
            }
        }
    }
}
