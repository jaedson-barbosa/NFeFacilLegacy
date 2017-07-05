using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using BibliotecaCentral;

namespace BibliotecaCentral.Migrations
{
    [DbContext(typeof(AplicativoContext))]
    [Migration("20170705012424_Loja1.2.7")]
    partial class Loja127
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2");

            modelBuilder.Entity("BibliotecaCentral.ItensBD.ClienteDI", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Bairro");

                    b.Property<string>("CEP");

                    b.Property<string>("CNPJ");

                    b.Property<string>("CPF");

                    b.Property<int>("CPais");

                    b.Property<int>("CodigoMunicipio");

                    b.Property<string>("Complemento");

                    b.Property<string>("Email");

                    b.Property<string>("ISUF");

                    b.Property<string>("IdEstrangeiro");

                    b.Property<int>("IndicadorIE");

                    b.Property<string>("InscricaoEstadual");

                    b.Property<string>("Logradouro");

                    b.Property<string>("Nome");

                    b.Property<string>("NomeMunicipio");

                    b.Property<string>("Numero");

                    b.Property<string>("SiglaUF");

                    b.Property<string>("Telefone");

                    b.Property<DateTime>("UltimaData");

                    b.Property<string>("XPais");

                    b.HasKey("Id");

                    b.ToTable("Clientes");
                });

            modelBuilder.Entity("BibliotecaCentral.ItensBD.EmitenteDI", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Bairro");

                    b.Property<string>("CEP");

                    b.Property<string>("CNAE");

                    b.Property<string>("CNPJ");

                    b.Property<int>("CPais");

                    b.Property<int>("CodigoMunicipio");

                    b.Property<string>("Complemento");

                    b.Property<string>("IEST");

                    b.Property<string>("IM");

                    b.Property<string>("InscricaoEstadual");

                    b.Property<string>("Logradouro");

                    b.Property<string>("Nome");

                    b.Property<string>("NomeFantasia");

                    b.Property<string>("NomeMunicipio");

                    b.Property<string>("Numero");

                    b.Property<int>("RegimeTributario");

                    b.Property<string>("SiglaUF");

                    b.Property<string>("Telefone");

                    b.Property<DateTime>("UltimaData");

                    b.Property<string>("XPais");

                    b.HasKey("Id");

                    b.ToTable("Emitentes");
                });

            modelBuilder.Entity("BibliotecaCentral.ItensBD.Imagem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<byte[]>("Bytes");

                    b.HasKey("Id");

                    b.ToTable("Imagens");
                });

            modelBuilder.Entity("BibliotecaCentral.ItensBD.MotoristaDI", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CNPJ");

                    b.Property<string>("CPF");

                    b.Property<string>("InscricaoEstadual");

                    b.Property<string>("Nome");

                    b.Property<string>("UF");

                    b.Property<DateTime>("UltimaData");

                    b.Property<string>("XEnder");

                    b.Property<string>("XMun");

                    b.HasKey("Id");

                    b.ToTable("Motoristas");
                });

            modelBuilder.Entity("BibliotecaCentral.ItensBD.NFeDI", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CNPJEmitente")
                        .IsRequired();

                    b.Property<string>("DataEmissao")
                        .IsRequired();

                    b.Property<bool>("Exportada");

                    b.Property<bool>("Impressa");

                    b.Property<string>("NomeCliente")
                        .IsRequired();

                    b.Property<string>("NomeEmitente")
                        .IsRequired();

                    b.Property<int>("NumeroNota");

                    b.Property<ushort>("SerieNota");

                    b.Property<int>("Status");

                    b.Property<DateTime>("UltimaData");

                    b.Property<string>("XML")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("NotasFiscais");
                });

            modelBuilder.Entity("BibliotecaCentral.ItensBD.ProdutoDI", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CFOP");

                    b.Property<string>("CodigoBarras");

                    b.Property<string>("CodigoBarrasTributo");

                    b.Property<string>("CodigoProduto");

                    b.Property<string>("Descricao");

                    b.Property<string>("EXTIPI");

                    b.Property<string>("NCM");

                    b.Property<DateTime>("UltimaData");

                    b.Property<string>("UnidadeComercializacao");

                    b.Property<string>("UnidadeTributacao");

                    b.Property<double>("ValorUnitario");

                    b.Property<double>("ValorUnitarioTributo");

                    b.HasKey("Id");

                    b.ToTable("Produtos");
                });

            modelBuilder.Entity("BibliotecaCentral.ItensBD.RegistroCancelamento", b =>
                {
                    b.Property<string>("ChaveNFe")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("DataHoraEvento");

                    b.Property<int>("TipoAmbiente");

                    b.Property<string>("XML");

                    b.HasKey("ChaveNFe");

                    b.ToTable("Cancelamentos");
                });

            modelBuilder.Entity("BibliotecaCentral.ItensBD.ResultadoSincronizacaoCliente", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("MomentoSincronizacao");

                    b.Property<int>("NumeroDadosBaseTrafegados");

                    b.Property<int>("NumeroNotasTrafegadas");

                    b.Property<bool>("SincronizacaoAutomatica");

                    b.HasKey("Id");

                    b.ToTable("ResultadosCliente");
                });

            modelBuilder.Entity("BibliotecaCentral.ItensBD.ResultadoSincronizacaoServidor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("MomentoRequisicao");

                    b.Property<bool>("SucessoSolicitacao");

                    b.Property<int>("TipoDadoSolicitado");

                    b.HasKey("Id");

                    b.ToTable("ResultadosServidor");
                });
        }
    }
}
