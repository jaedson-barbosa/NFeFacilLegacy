using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using NFeFacil;

namespace NFeFacil.Migrations
{
    [DbContext(typeof(AplicativoContext))]
    partial class AplicativoContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.1");

            modelBuilder.Entity("NFeFacil.ItensBD.ClienteDI", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CNPJ");

                    b.Property<string>("CPF");

                    b.Property<string>("ISUF");

                    b.Property<string>("email");

                    b.Property<int?>("endereçoId");

                    b.Property<string>("idEstrangeiro");

                    b.Property<int>("indicadorIE");

                    b.Property<string>("inscricaoEstadual");

                    b.Property<string>("nome");

                    b.HasKey("Id");

                    b.HasIndex("endereçoId");

                    b.ToTable("Clientes");
                });

            modelBuilder.Entity("NFeFacil.ItensBD.EmitenteDI", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CNAE");

                    b.Property<string>("CNPJ");

                    b.Property<string>("IEST");

                    b.Property<string>("IM");

                    b.Property<int?>("endereçoId");

                    b.Property<string>("inscricaoEstadual");

                    b.Property<string>("nome");

                    b.Property<string>("nomeFantasia");

                    b.Property<int>("regimeTributario");

                    b.HasKey("Id");

                    b.HasIndex("endereçoId");

                    b.ToTable("Emitentes");
                });

            modelBuilder.Entity("NFeFacil.ItensBD.MotoristaDI", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CNPJ");

                    b.Property<string>("CPF");

                    b.Property<string>("UF");

                    b.Property<string>("inscricaoEstadual");

                    b.Property<string>("nome");

                    b.Property<string>("xEnder");

                    b.Property<string>("xMun");

                    b.HasKey("Id");

                    b.ToTable("Motoristas");
                });

            modelBuilder.Entity("NFeFacil.ItensBD.NFeDI", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("DataEmissao")
                        .IsRequired();

                    b.Property<string>("NomeCliente")
                        .IsRequired();

                    b.Property<string>("NomeEmitente")
                        .IsRequired();

                    b.Property<string>("NumeroNota")
                        .IsRequired();

                    b.Property<int>("Status");

                    b.HasKey("Id");

                    b.ToTable("NotasFiscais");
                });

            modelBuilder.Entity("NFeFacil.ItensBD.ProdutoDI", b =>
                {
                    b.Property<int>("Id")
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

            modelBuilder.Entity("NFeFacil.ItensBD.ResultadoSincronizacaoCliente", b =>
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

            modelBuilder.Entity("NFeFacil.ItensBD.ResultadoSincronizacaoServidor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("MomentoRequisicao");

                    b.Property<bool>("SucessoSolicitacao");

                    b.Property<int>("TipoDadoSolicitado");

                    b.HasKey("Id");

                    b.ToTable("ResultadosServidor");
                });

            modelBuilder.Entity("NFeFacil.ModeloXML.PartesProcesso.PartesNFe.EnderecoCompleto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Bairro");

                    b.Property<string>("CEP");

                    b.Property<int>("CPais");

                    b.Property<long>("CodigoMunicipio");

                    b.Property<string>("Complemento");

                    b.Property<string>("Logradouro");

                    b.Property<string>("NomeMunicipio");

                    b.Property<string>("Numero");

                    b.Property<string>("SiglaUF");

                    b.Property<string>("Telefone");

                    b.Property<string>("XPais");

                    b.HasKey("Id");

                    b.ToTable("EnderecoCompleto");
                });

            modelBuilder.Entity("NFeFacil.ItensBD.ClienteDI", b =>
                {
                    b.HasOne("NFeFacil.ModeloXML.PartesProcesso.PartesNFe.EnderecoCompleto", "endereço")
                        .WithMany()
                        .HasForeignKey("endereçoId");
                });

            modelBuilder.Entity("NFeFacil.ItensBD.EmitenteDI", b =>
                {
                    b.HasOne("NFeFacil.ModeloXML.PartesProcesso.PartesNFe.EnderecoCompleto", "endereço")
                        .WithMany()
                        .HasForeignKey("endereçoId");
                });
        }
    }
}
