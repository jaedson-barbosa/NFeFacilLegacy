function DadosNFe() {
    this.NomeEmitente = new String();
    this.TipoEmissao = new Number();
    this.NumeroNota = new Number();
    this.SerieNota = new Number();
    this.PaginaAtual = new Number();
    this.QuantPaginas = new Number();
    this.Chave = new String();
    this.NumeroProtocolo = new String();
    this.DataHoraRecibo = new String();
    this.NatOp = new String();
    this.IE = new String();
    this.CNPJEmit = new String();
    this.Endereco = new Endereço();
    this.Att = function(bruto) {
        this.NomeEmitente = bruto.NomeEmitente;
        this.TipoEmissao = bruto.TipoEmissao;
        this.Endereco = bruto.Endereco;
        this.NumeroNota = bruto.NumeroNota;
        this.SerieNota = bruto.SerieNota;
        this.PaginaAtual = bruto.PaginaAtual;
        this.QuantPaginas = bruto.QuantPaginas;
        this.Chave = bruto.Chave;
        this.NumeroProtocolo = bruto.NumeroProtocolo;
        this.DataHoraRecibo = bruto.DataHoraRecibo;
        this.NatOp = bruto.NatOp;
        this.IE = bruto.IE;
        this.CNPJEmit = bruto.CNPJEmit;
    }
}

function Endereço() {
    this.UF = new String();
    this.Municipio = new String();
    this.CEP = new String();
    this.Bairro = new String();
    this.Logradouro = new String();
    this.Numero = new Number();
    this.Fone = new String();
    this.Create = function(uf, mun, cep, bairro, logradouro, numero, fone){
        this.UF = uf;
        this.Municipio = mun;
        this.CEP = cep;
        this.Bairro = bairro;
        this.Logradouro = logradouro;
        this.Numero = numero;
        this.Fone = fone;
    }
    this.Att = function(bruto){
        this.UF = bruto.UF;
        this.Municipio = bruto.Municipio;
        this.CEP = bruto.CEP;
        this.Bairro = bruto.Bairro;
        this.Logradouro = bruto.Logradouro;
        this.Numero = bruto.Numero;
        this.Fone = bruto.Fone;
    }
}

function DadosCliente() {
    this.nomeCliente = "";
    this.DocCliente = "";
    this.dataEmissao = "";
    this.dataEntradaSaida = "";
    this.IECliente = "";
    this.horaEntradaSaida = "";
    this.Endereco = new Endereço();
    this.Att = function (bruto) {
        this.nomeCliente = bruto.nomeCliente;
        this.DocCliente = bruto.DocCliente
        this.dataEmissao = bruto.dataEmissao;
        this.dataEntradaSaida = bruto.dataEntradaSaida;
        this.IECliente = bruto.IECliente;
        this.horaEntradaSaida = bruto.horaEntradaSaida;
        this.Endereco = bruto.Endereco;
    }
}

function DadosMotorista() {
    this.nomeMotorista = "";
    this.modalidadeFrete = "";
    this.codigoANTT = "";
    this.placa = "";
    this.ufPlaca = "";
    this.documentoMotorista = "";
    this.enderecoMotorista = "";
    this.municipioMotorista = "";
    this.ufMotorista = "";
    this.IEMotorista = "";
    this.quantidadeVolume = "";
    this.especieVolume = "";
    this.marcaVolume = "";
    this.numeroVolume = "";
    this.pesoBrutoVolume = "";
    this.pesoLiquidoVolume = "";
    this.Att = function (bruto) {
        this.nomeMotorista = bruto.nomeMotorista;
        this.modalidadeFrete = bruto.modalidadeFrete;
        this.codigoANTT = bruto.codigoANTT;
        this.placa = bruto.placa;
        this.ufPlaca = bruto.ufPlaca;
        this.documentoMotorista = bruto.documentoMotorista;
        this.enderecoMotorista = bruto.enderecoMotorista;
        this.municipioMotorista = bruto.municipioMotorista;
        this.ufMotorista = bruto.ufMotorista;
        this.IEMotorista = bruto.IEMotorista;
        this.quantidadeVolume = bruto.quantidadeVolume;
        this.especieVolume = bruto.especieVolume;
        this.marcaVolume = bruto.marcaVolume;
        this.numeroVolume = bruto.numeroVolume;
        this.pesoBrutoVolume = bruto.pesoBrutoVolume;
        this.pesoLiquidoVolume = bruto.pesoLiquidoVolume;
    }
}

function DadosImposto() {
    this.baseCalculoICMS = "";
    this.valorICMS = "";
    this.baseCalculoICMSST = "";
    this.valorICMSST = "";
    this.valorTotalProdutos = "";
    this.valorFrete = "";
    this.valorSeguro = "";
    this.desconto = "";
    this.despesasAcessorias = "";
    this.valorIPI = "";
    this.totalNota = "";
    this.Att = function (bruto) {
        this.baseCalculoICMS = bruto.baseCalculoICMS;
        this.valorICMS = bruto.valorICMS;
        this.baseCalculoICMSST = bruto.baseCalculoICMSST;
        this.valorICMSST = bruto.valorICMSST;
        this.valorTotalProdutos = bruto.valorTotalProdutos;
        this.valorFrete = bruto.valorFrete;
        this.valorSeguro = bruto.valorSeguro;
        this.desconto = bruto.desconto;
        this.despesasAcessorias = bruto.despesasAcessorias;
        this.valorIPI = bruto.valorIPI;
        this.totalNota = bruto.totalNota;
    }
}

function DadosProduto() {
    this.cProd = "";
    this.xProd = "";
    this.NCM = "";
    this.CSTICMS = "";
    this.CFOP = "";
    this.uCom = "";
    this.qCom = "";
    this.vUnCom = "";
    this.vProd = "";
    this.vUnTrib = "";
    this.vICMS = "";
    this.vIPI = "";
    this.pICMS = "";
    this.pIPI = "";
    this.Att = function (bruto) {
        this.cProd = bruto.cProd;
        this.xProd = bruto.xProd;
        this.NCM = bruto.NCM;
        this.CSTICMS = bruto.CSTICMS;
        this.CFOP = bruto.CFOP;
        this.uCom = bruto.uCom;
        this.qCom = bruto.qCom;
        this.vUnCom = bruto.vUnCom;
        this.vProd = bruto.vProd;
        this.vUnTrib = bruto.vUnTrib;
        this.vICMS = bruto.vICMS;
        this.vIPI = bruto.vIPI;
        this.pICMS = bruto.pICMS;
        this.pIPI = bruto.pIPI;
    }
}

function DadosCabecalho() {
    this.nomeEmitente = new String();
    this.numeroNota = new String();
    this.serieNota = new String();
    this.Att = function (bruto) {
        this.nomeEmitente = bruto.nomeEmitente;
        this.numeroNota = bruto.numeroNota;
        this.serieNota = bruto.serieNota;
    }
}

function DadosAdicionais(){
    this.dados = new String();
    this.fisco = new String();
    this.Att = function (bruto) {
        this.dados = bruto.dados;
        this.fisco = bruto.fisco;
    }
}