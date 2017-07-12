using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using BibliotecaCentral;

namespace BibliotecaCentral.Migrations
{
    [DbContext(typeof(AplicativoContext))]
    [Migration("20170712211943_Loja1.2.8")]
    partial class Loja128
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2");

            modelBuilder.Entity("BibliotecaCentral.ItensBD.AlteracaoEstoque", b =>
                {
                    b.Property<DateTime>("Id");

                    b.Property<double>("Alteração");

                    b.Property<Guid?>("EstoqueId");

                    b.HasKey("Id");

                    b.HasIndex("EstoqueId");

                    b.ToTable("AlteracaoEstoque");
                });

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

            modelBuilder.Entity("BibliotecaCentral.ItensBD.Estoque", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("LocalizacaoGenerica");

                    b.Property<string>("Locação");

                    b.Property<string>("Prateleira");

                    b.Property<string>("Segmento");

                    b.HasKey("Id");

                    b.ToTable("Estoque");
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

                    b.Property<Guid>("Veiculo");

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

            modelBuilder.Entity("BibliotecaCentral.ItensBD.ProdutoSimplesVenda", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<double>("Desconto");

                    b.Property<double>("DespesasExtras");

                    b.Property<double>("Frete");

                    b.Property<Guid?>("ProdutoBaseId");

                    b.Property<double>("Quantidade");

                    b.Property<Guid?>("RegistroVendaId");

                    b.Property<double>("Seguro");

                    b.Property<double>("TotalLíquido");

                    b.HasKey("Id");

                    b.HasIndex("ProdutoBaseId");

                    b.HasIndex("RegistroVendaId");

                    b.ToTable("ProdutoSimplesVenda");
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

            modelBuilder.Entity("BibliotecaCentral.ItensBD.RegistroVenda", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("ClienteId");

                    b.Property<DateTime>("DataHoraVenda");

                    b.Property<double>("DescontoTotal");

                    b.Property<Guid?>("EmitenteId");

                    b.Property<Guid?>("MotoristaId");

                    b.Property<string>("NotaFiscalRelacionadaId");

                    b.Property<string>("Observações");

                    b.Property<DateTime>("UltimaData");

                    b.Property<Guid?>("VendedorId");

                    b.HasKey("Id");

                    b.HasIndex("ClienteId");

                    b.HasIndex("EmitenteId");

                    b.HasIndex("MotoristaId");

                    b.HasIndex("NotaFiscalRelacionadaId");

                    b.HasIndex("VendedorId");

                    b.ToTable("Vendas");
                });

            modelBuilder.Entity("BibliotecaCentral.ItensBD.VeiculoDI", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Descricao");

                    b.Property<string>("Placa");

                    b.Property<string>("RNTC");

                    b.Property<string>("UF");

                    b.HasKey("Id");

                    b.ToTable("Veiculos");
                });

            modelBuilder.Entity("BibliotecaCentral.ItensBD.Vendedor", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("CPF");

                    b.Property<string>("Endereço")
                        .IsRequired();

                    b.Property<string>("Nome")
                        .IsRequired();

                    b.Property<DateTime>("UltimaData");

                    b.HasKey("Id");

                    b.ToTable("Vendedores");
                });

            modelBuilder.Entity("BibliotecaCentral.ItensBD.AlteracaoEstoque", b =>
                {
                    b.HasOne("BibliotecaCentral.ItensBD.Estoque")
                        .WithMany("Alteracoes")
                        .HasForeignKey("EstoqueId");
                });

            modelBuilder.Entity("BibliotecaCentral.ItensBD.ProdutoSimplesVenda", b =>
                {
                    b.HasOne("BibliotecaCentral.ItensBD.ProdutoDI", "ProdutoBase")
                        .WithMany()
                        .HasForeignKey("ProdutoBaseId");

                    b.HasOne("BibliotecaCentral.ItensBD.RegistroVenda")
                        .WithMany("Produtos")
                        .HasForeignKey("RegistroVendaId");
                });

            modelBuilder.Entity("BibliotecaCentral.ItensBD.RegistroVenda", b =>
                {
                    b.HasOne("BibliotecaCentral.ItensBD.ClienteDI", "Cliente")
                        .WithMany()
                        .HasForeignKey("ClienteId");

                    b.HasOne("BibliotecaCentral.ItensBD.EmitenteDI", "Emitente")
                        .WithMany()
                        .HasForeignKey("EmitenteId");

                    b.HasOne("BibliotecaCentral.ItensBD.MotoristaDI", "Motorista")
                        .WithMany()
                        .HasForeignKey("MotoristaId");

                    b.HasOne("BibliotecaCentral.ItensBD.NFeDI", "NotaFiscalRelacionada")
                        .WithMany()
                        .HasForeignKey("NotaFiscalRelacionadaId");

                    b.HasOne("BibliotecaCentral.ItensBD.Vendedor", "Vendedor")
                        .WithMany()
                        .HasForeignKey("VendedorId");
                });
        }
    }
}
