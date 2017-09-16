﻿using NFeFacil.Sincronizacao.Pacotes;
using Restup.Webserver.Attributes;
using Restup.Webserver.Models.Contracts;
using Restup.Webserver.Models.Schemas;
using System;
using System.Linq;

namespace NFeFacil.Sincronizacao.Servidor
{
    [RestController(InstanceCreationType.PerCall)]
    internal sealed class ControllerSincronizacaoDadosBase
    {
        [UriFormat("/Dados/{senha}")]
        public IPostResponse ClienteServidorAsync(int senha, [FromContent] ConjuntoBanco pacote)
        {
            using (var DB = new AplicativoContext())
            {
                if (senha != ConfiguracoesSincronizacao.SenhaPermanente)
                    throw new SenhaErrada(senha);

                var Mudanca = new Repositorio.MudancaOtimizadaBancoDados(DB);
                Mudanca.AdicionarEmitentes(pacote.Emitentes);
                Mudanca.AdicionarClientes(pacote.Clientes);
                Mudanca.AdicionarMotoristas(pacote.Motoristas);
                Mudanca.AdicionarProdutos(pacote.Produtos);
                var resposta = new PostResponse(PostResponse.ResponseStatus.Created);

                DB.SaveChanges();
                return resposta;
            }
        }

        [UriFormat("/Dados/{senha}/{ultimaSincronizacaoCliente}")]
        public IGetResponse ServidorCliente(int senha, long ultimaSincronizacaoCliente)
        {
            DateTime momento = DateTime.FromBinary(ultimaSincronizacaoCliente);
            if (ultimaSincronizacaoCliente > 10) momento = momento.AddSeconds(-10);
            using (var DB = new AplicativoContext())
            {
                if (senha != ConfiguracoesSincronizacao.SenhaPermanente)
                    throw new SenhaErrada(senha);

                var resposta = new GetResponse(GetResponse.ResponseStatus.OK,
                    new ConjuntoBanco
                    {
                        Emitentes = DB.Emitentes.Where(x => x.UltimaData > momento).ToList(),
                        Clientes = DB.Clientes.Where(x => x.UltimaData > momento).ToList(),
                        Motoristas = DB.Motoristas.Where(x => x.UltimaData > momento).ToList(),
                        Produtos = DB.Produtos.Where(x => x.UltimaData > momento).ToList()
                    });

                return resposta;
            }
        }

        [UriFormat("/DadosCompleto/{senha}")]
        public IGetResponse SincronizacaoCompleta(int senha, [FromContent] ConjuntoBanco pacote)
        {
            using (var DB = new AplicativoContext())
            {
                if (senha != ConfiguracoesSincronizacao.SenhaPermanente)
                    throw new SenhaErrada(senha);

                var Mudanca = new Repositorio.MudancaOtimizadaBancoDados(DB);
                Mudanca.AnalisarAdicionarEmitentes(pacote.Emitentes);
                Mudanca.AnalisarAdicionarClientes(pacote.Clientes);
                Mudanca.AnalisarAdicionarMotoristas(pacote.Motoristas);
                Mudanca.AnalisarAdicionarProdutos(pacote.Produtos);

                var resposta = new GetResponse(GetResponse.ResponseStatus.OK,
                    new ConjuntoBanco
                    {
                        Emitentes = DB.Emitentes.ToList(),
                        Clientes = DB.Clientes.ToList(),
                        Motoristas = DB.Motoristas.ToList(),
                        Produtos = DB.Produtos.ToList()
                    });

                DB.SaveChanges();
                return resposta;
            }
        }
    }
}
