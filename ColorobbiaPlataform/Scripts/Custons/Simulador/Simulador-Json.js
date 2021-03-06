﻿

var SaveToJson = (function () {

    let json = new Object();

    function prepare() {


        json.Id = 0;
        json.Codigo = getCodigoRef();
        json.Descricao = getDescricao();
        json.Formula = getFormula();
        json.Familia = getFamilia();
        json.CentroCusto = getCentroCusto();
        json.DescricaoCCusto = getDescricaoCCusto();
        json.Rendimento = getRendimento();
        json.Filial = getFilial();
        json.Niveis = getNiveis();
        json.Estrutura = getEstrutura();
        json.CustoEmbalagemPercent = getCustoEmbalagemPercent();
        json.CustoEmbalagem = getCustoEmbalagem();
        json.CustoOperacional = getCustoOperacional();
        json.CustoIndustrial = getCustoIndustrial();
        json.DespesasOperacionais = getDespesasOperacionais();
        json.DespesasOperacionaisCalculada = getDespesasOperacionaisCalculada();
        json.CustoTotalDoProduto = getCustoTotalDoProduto();
        json.CustoTotalDoProdutoMargem = getCustoTotalDoProdutoMargem();
        json.MargemLucro = getMargemLucro();
        json.PrecoBase = getPrecoBase();
        json.PrecoBaseIcm7 = getPrecoBaseIcm7();
        json.PrecoBaseIcm12 = getPrecoBaseIcm12();
        json.PrecoBaseIcm18 = getPrecoBaseIcm18();

        return json;
    }

    function getCodigoRef() {
        return document.querySelector("#Produto").innerText;
    }

    function getDescricao() {
        return document.querySelector("#Descricao").innerText;
    }

    function getFormula() {
        return document.querySelector("#FormulaEscolhida").innerText;
    }
    function getFamilia() {
        return document.querySelector("#Familia").innerText;
    }
    function getCentroCusto() {
        if (document.querySelector("#CentroCusto") === null) {
            return "";
        }
        return document.querySelector("#CentroCusto").innerText;

    }
    function getDescricaoCCusto() {
        if (document.querySelector("#DescricaoCCusto") === null) {
            return "";
        }
        return document.querySelector("#DescricaoCCusto").innerText;
    }
    function getRendimento() {
        return toFloat(document.querySelector("#RendimentoEscolhido").innerText);
    }
    function getFilial() {
        return document.querySelector("#Filial").value;
    }

    function getNiveis() {
        var n = $("table");
        return n.length;
    }

    function getEstrutura() {
        var estru = new Array();
        var item = new Array();
        var tabs = $("table");

        for (var i = 0; i < tabs.length/2; i++) { //Tabelas
            for (var j = 0; j < tabs[i].rows.length; j++) { //linhas               
                if (tabs[i].rows[j].cells.length === 9) {                    
                    for (var k = 0; k < tabs[i].rows[j].cells.length; k++) {//Colunas
                        item.push(tabs[i].rows[j].cells[k].innerText);
                    }
                }
                if (item.length === 9) {
                    estru.push(item);
                }
                
                item = new Array();
            }
        }
        return estru;
    }
    
    function getCustoEmbalagemPercent() {
        if (document.querySelector("#CustoEmbalagemPercent") === null) {
            return 0;
        }
        return toFloat(document.querySelector("#CustoEmbalagemPercent").innerText);
    }
    function getCustoEmbalagem() {
        if (document.querySelector("#Embalagem") === null) {
            return 0;
        }
        return toFloat(document.querySelector("#Embalagem").innerText);
    }
    function getCustoOperacional() {
        if (document.querySelector("#CustoOperacional") === null) {
            return 0;
        }
        return toFloat(document.querySelector("#CustoOperacional").innerText);
    }
    function getCustoIndustrial() {
        if (document.querySelector("#CustoIndustrial") === null) {
            return 0;
        }
        return toFloat(document.querySelector("#CustoIndustrial").innerText);
    }
    function getDespesasOperacionais() {
        if (document.querySelector("#DespesaOperacional") === null) {
            return 0;
        }
        return toFloat(document.querySelector("#DespesaOperacional").innerText);
    }
    function getDespesasOperacionaisCalculada() {
        if (document.querySelector("#DespesasOperacionaisCalculada") === null) {
            return 0;
        }
        return toFloat(document.querySelector("#DespesasOperacionaisCalculada").innerText);
    }
    function getCustoTotalDoProduto() {
        if (document.querySelector("#CustoTotalDoProduto") === null) {
            return 0;
        }
        return toFloat(document.querySelector("#CustoTotalDoProduto").innerText);
    }
    function getCustoTotalDoProdutoMargem() {
        if (document.querySelector("#CustoTotalDoProdutoMargem") === null) {
            return 0;
        }
        return toFloat(document.querySelector("#CustoTotalDoProdutoMargem").innerText);
    }
    function getMargemLucro() {
        if (document.querySelector("#MargemLucro") === null) {
            return 0;
        }
        return toFloat(document.querySelector("#MargemLucro").innerText);
    }
    function getPrecoBase() {
        if (document.querySelector("#PrecoBase") === null) {
            return 0;
        }
        return toFloat(document.querySelector("#PrecoBase").innerText);
    }
    function getPrecoBaseIcm7() {
        if (document.querySelector("#Icms7") === null) {
            return 0;
        }
        return toFloat(document.querySelector("#Icms7").innerText);
    }
    function getPrecoBaseIcm12() {
        if (document.querySelector("#Icms12") === null) {
            return 0;
        }
        return toFloat(document.querySelector("#Icms12").innerText);
    }
    function getPrecoBaseIcm18() {
        if (document.querySelector("#Icms18") === null) {
            return 0;
        }
        return toFloat(document.querySelector("#Icms18").innerText);
    }

    return {
        GenerateJson: prepare
    };


    /**
       * valida e retorna um número float parseado
       * @param {any} x
       */
    function toFloat(x) {

        if (x === undefined) {
            return 0;
        }      
        if (isNaN(parseFloat(x))) {
            return 0;
        }
        if (!typeof (parseFloat(x)) === "number") {
            return 0;
        } else {
            return parseFloat(x.replace(",", "."));
        }
    }
})();