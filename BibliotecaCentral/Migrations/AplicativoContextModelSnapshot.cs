using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using BibliotecaCentral;

namespace BibliotecaCentral.Migrations
{
    [DbContext(typeof(AplicativoContext))]
    partial class AplicativoContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.1");

            modelBuilder.Entity("BibliotecaCentral.ItensBD.NFeDI", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CNPJEmitente")
                        .IsRequired();

                    b.Property<string>("DataEmissao")
                        .IsRequired();

                    b.Property<string>("IdNotaFiscal")
                        .IsRequired();

                    b.Property<string>("NomeCliente")
                        .IsRequired();

                    b.Property<string>("NomeEmitente")
                        .IsRequired();

                    b.Property<long>("NumeroNota");

                    b.Property<ushort>("SerieNota");

                    b.Property<int>("Status");

                    b.HasKey("Id");

                    b.ToTable("NotasFiscais");
                });

            modelBuilder.Entity("BibliotecaCentral.ItensBD.RegistroMudanca", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("MomentoMudanca");

                    b.Property<int>("TipoDadoModificado");

                    b.Property<int>("TipoOperacaoRealizada");

                    b.HasKey("Id");

                    b.ToTable("MudancasBanco");
                });

            modelBuilder.Entity("BibliotecaCentral.ItensBD.ResultadoSincronizacaoCliente", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("MomentoSincronizacao");

                    b.Property<int>("NumeroDadosEnviados");

                    b.Property<int>("NumeroDadosRecebidos");

                    b.Property<int>("NumeroNotasEnviadas");

                    b.Property<int>("NumeroNotasRecebidas");

                    b.Property<bool>("PodeSincronizarDadoBase");

                    b.Property<bool>("PodeSincronizarNota");

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

            modelBuilder.Entity("BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.enderecoCompleto", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Bairro");

                    b.Property<string>("CEP");

                    b.Property<int>("CPais");

                    b.Property<int>("CodigoMunicipio");

                    b.Property<string>("Complemento");

                    b.Property<string>("Logradouro");

                    b.Property<string>("NomeMunicipio");

                    b.Property<string>("Numero");

                    b.Property<string>("SiglaUF");

                    b.Property<string>("Telefone");

                    b.Property<string>("XPais");

                    b.HasKey("Id");

                    b.ToTable("enderecoCompleto");
                });

            modelBuilder.Entity("BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.Destinatario", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CNPJ");

                    b.Property<string>("CPF");

                    b.Property<string>("Documento");

                    b.Property<string>("ISUF");

                    b.Property<string>("email");

                    b.Property<Guid?>("enderecoId");

                    b.Property<string>("idEstrangeiro");

                    b.Property<int>("indicadorIE");

                    b.Property<string>("inscricaoEstadual");

                    b.Property<string>("nome");

                    b.HasKey("Id");

                    b.HasIndex("enderecoId");

                    b.ToTable("Clientes");
                });

            modelBuilder.Entity("BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.Emitente", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CNAE");

                    b.Property<string>("CNPJ");

                    b.Property<string>("IEST");

                    b.Property<string>("IM");

                    b.Property<Guid?>("enderecoId");

                    b.Property<string>("inscricaoEstadual");

                    b.Property<string>("nome");

                    b.Property<string>("nomeFantasia");

                    b.Property<int>("regimeTributario");

                    b.HasKey("Id");

                    b.HasIndex("enderecoId");

                    b.ToTable("Emitentes");
                });

            modelBuilder.Entity("BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.BaseProdutoOuServico", b =>
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

                    b.Property<string>("UnidadeComercializacao");

                    b.Property<string>("UnidadeTributacao");

                    b.Property<double>("ValorUnitario");

                    b.Property<double>("ValorUnitarioTributo");

                    b.HasKey("Id");

                    b.ToTable("Produtos");
                });

            modelBuilder.Entity("BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesTransporte.Motorista", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CNPJ");

                    b.Property<string>("CPF");

                    b.Property<string>("Documento");

                    b.Property<string>("InscricaoEstadual");

                    b.Property<string>("Nome");

                    b.Property<string>("UF");

                    b.Property<string>("XEnder");

                    b.Property<string>("XMun");

                    b.HasKey("Id");

                    b.ToTable("Motoristas");
                });

            modelBuilder.Entity("BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.Destinatario", b =>
                {
                    b.HasOne("BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.enderecoCompleto", "endereco")
                        .WithMany()
                        .HasForeignKey("enderecoId");
                });

            modelBuilder.Entity("BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.Emitente", b =>
                {
                    b.HasOne("BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.enderecoCompleto", "endereco")
                        .WithMany()
                        .HasForeignKey("enderecoId");
                });
        }
    }
}
