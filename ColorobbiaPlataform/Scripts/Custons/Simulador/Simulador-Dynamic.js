
/**
 * Módulo do simulador de fórmulas
 * 22/02/2019
 * */


var Simulador = (function () {

    var jsonNivel = {
        Formula: 0,
        CustProduto: 0,
        CustFixComponente: 0,
        CustFixProduto: 0,
        CustTotalComponente: 0,
        CustTotalProduto: 0
    };

    const colProduto = 2;
    const colComponente = 3;
    const colTipo = 4;
    const colUnidade = 5;
    const colFormula = 6;
    const colCustoMateriaPrima = 7;
    const colCustoTotal = 8;



    /**
       * valida e retorna um número float parseado
       * @param {any} x
       */
    function superParseFloat(x) {

        if (x === undefined) {
            return 0;
        }

        if (x.innerHTML === undefined) {
            if (x.innerText === undefined) {
                return 0;
            }
        }

        x = x.innerHTML;

        if (isNaN(parseFloat(x))) {
            return 0;
        }
        if (!typeof (parseFloat(x)) === "number") {
            return 0;
        } else {
            return parseFloat(x.replace(",", "."));
        }
    }
    /**
     * valida e retorna um número int parseado
     * @param {any} x
     */
    function superParseInt(x) {

        if (x === undefined) {
            return 0;
        }

        if (isNaN(parseInt(x))) {
            return 0;
        }
        if (!typeof (parseInt(x)) === "number") {
            return 0;
        } else {
            return parseInt(x);
        }
    }

    /**
     * @description Retorna o rendimento escolhido 
     * */
    function getRendimento() {
        return parseFloat(document
            .getElementById("RendimentoEscolhido")
            .innerHTML.replace(",", "."));
    }

    /**
     * @description Soma todas as colunas e retorna Array com os totais
     * @returns Array of Soma
     * */

    function TotalizaColunas() {
        var tabelas = $("table#tabEstrutura");
        var rendimento = getRendimento();
        var totalPercentual = 0;
        var totalCustoProdutoVariavel = 0;
        var totalCustoComponenteFix = 0;
        var totalCustoProdutoFix = 0;
        var totalComponente = 0;
        var totalProduto = 0;
        var totalLine = 0; //Linha do Total

        var CustoIndustrial = document.querySelector("#CustoIndustrial");
        var Embalagem = document.querySelector("#Embalagem");
        var CustoOperacional = document.querySelector("#CustoOperacional");
        var DespesasOperacionaisCalculada = document.querySelector("#DespesasOperacionaisCalculada");
        var DespesaOperacional = document.querySelector("#DespesaOperacional");
        var CustoTotalDoProduto = document.querySelector("#CustoTotalDoProduto");
        var CustoTotalDoProdutoMargem = document.querySelector("#CustoTotalDoProdutoMargem");
        var PrecoBase = document.querySelector("#PrecoBase");
        var MargemLucro = document.querySelector("#MargemLucro");
        var Icms7 = document.querySelector("#Icms7");
        var Icms12 = document.querySelector("#Icms12");
        var Icms18 = document.querySelector("#Icms18");
        var PrecoBaseCalc = 0;
        var CustoTotalDoProdutoMargemCalc = 0;
        var CustoTotalDoProdutoCalc = 0;
        var CustoIndustrialCalc = 0;

        for (var k = 0; k < tabelas.length; k++) {
            var totais = new Array();

            for (var i = 0; i < tabelas[k].rows.length; i++) {
                if ($(tabelas[k].rows[i]).attr("total-line") === "true") {
                    totalLine = i;
                } else {
                    for (var j = 0; j < tabelas[k].rows[i].cells.length; j++) {
                        var valor = tabelas[k].rows[i].cells[j];
                        if (i === 0) {
                            totais[j] = superParseFloat(valor);
                        } else {
                            totais[j] += superParseFloat(valor);
                        }
                    }
                }
            }

            //Atualiza totais da tabela
            tabelas[k].rows[totalLine].cells[colFormula].innerHTML = totais[colFormula].toFixed(2);
            tabelas[k].rows[totalLine].cells[colCustoTotal].innerHTML = (totais[colCustoTotal] / (rendimento / 100)).toFixed(4);

            //Atualiza o Cabeçalho
            if (k === 0) {

                CustoIndustrialCalc = (totais[colCustoTotal] / (rendimento / 100)) + superParseFloat(Embalagem) + superParseFloat(CustoOperacional);
                CustoIndustrial.innerHTML = CustoIndustrialCalc.toFixed(4);
                DespesasOperacionaisCalculada.innerHTML = (CustoIndustrialCalc * (superParseFloat(DespesaOperacional) / 100)).toFixed(4);
                CustoTotalDoProdutoCalc = superParseFloat(DespesasOperacionaisCalculada) + superParseFloat(CustoIndustrial);
                CustoTotalDoProduto.innerHTML = CustoTotalDoProdutoCalc.toFixed(4);
                CustoTotalDoProdutoMargemCalc = superParseFloat(CustoTotalDoProduto) * 1.05;
                CustoTotalDoProdutoMargem.innerHTML = CustoTotalDoProdutoMargemCalc.toFixed(4);
                PrecoBaseCalc = CustoTotalDoProdutoMargemCalc / ((100 - superParseFloat(MargemLucro)) / 100);
                PrecoBase.innerHTML = PrecoBaseCalc.toFixed(4);
                Icms7.innerHTML = (PrecoBaseCalc / 0.8375).toFixed(4);
                Icms12.innerHTML = (PrecoBaseCalc / 0.7875).toFixed(4);
                Icms18.innerHTML = (PrecoBaseCalc / 0.7275).toFixed(4);
            }


            if (totais[colFormula] > 100.009) {
                Alerta('A fórmula do ' +
                    tabelas[k].rows[totalLine - 2].cells[1].innerText +
                    ' ultrapassa os 100%', 'danger');

                $(tabelas[k].rows[totalLine].cells[colFormula]).addClass("alert alert-danger");
            } else {
                $(tabelas[k].rows[totalLine].cells[colFormula]).removeClass("alert alert-danger");
            }
        }
        SimuladorChart.Update();
    }

    /**
     * Exibe um Alerta em tela como se fosse um toast
     * @param {string} _message
     * @param {string} _type
     */
    function Alerta(_message, _type) {
        $.notify({
            icon: 'http://192.168.1.15:10300/Content/Imagens/Icons/error-30_amarelo.png',
            message: _message
        }, {
                type: _type,
                placement: {
                    from: "bottom"
                },
                animate: {
                    enter: 'animated fadeInRight',
                    exit: 'animated flipOutX'
                },
                allow_dismiss: true,
                icon_type: 'image'
            });
    }

    /**
     * Faz o cálculo da linha atual
     * @param {object} ThisRow
     */
    function CalculaLinha(ThisRow) {
        var rendimento = getRendimento();
        var attribute = $(ThisRow).attr("compute");

        if (attribute !== "NO") {

            //Obtem os valores atuais
            var percentualFormula = superParseFloat(ThisRow.cells[colFormula]);
            var custoMP = superParseFloat(ThisRow.cells[colCustoMateriaPrima]);
            var custoTotalProduto = custoMP * (percentualFormula / 100);

            //Atualizacao

            if (ThisRow.cells[colCustoTotal] !== undefined) {
                ThisRow.cells[colCustoTotal].innerText = custoTotalProduto.toFixed(4);
            }
        }



    }


    /***********
     * @description Adiciona um elemento do tipo Input no caller
     * @param {any} id
     ************/
    let ModifyValue = function (event) {
        event.stopPropagation();
        event.preventDefault();

        //Impede de habilitar edição no Input atual
        if (this.innerHTML.search("input") > 0) {
            return;
        }
        thisRow = this.parentElement;
        totalColumn = 0;


        this.innerHTML = '<input style="width:5em;" type="text" id="inputFor"'+
             'class="form-control" value="' + this.innerText + '" >';

        $('#inputFor').keyup(function () {
            $(this).val($(this).val().replace(".",","));
        });

        let inputElement = this.getElementsByTagName("input")[0];
        inputElement.focus();

        //Exit from element
        inputElement.addEventListener("blur", function () {
            this.outerHTML = this.value; //devolve o valor digitado a tabela          
            thisRow.cells[totalColumn].focus();
            CalculaLinha(thisRow);
            TotalizaColunas();
        });
    };

    /***********
 * @description Adiciona um elemento do tipo Input no caller
 * @param {any} id
 ************/
    let ModifyHeader = function (event) {
        event.stopPropagation();
        event.preventDefault();

        //Impede de habilitar edição no Input atual
        if (this.innerHTML.search("input") > 0) {
            return;
        }
        thisRow = this.parentElement;
        totalColumn = 0;


        this.innerHTML = '<input style="width:5em;" type="text" parent-id="' +
            this.id + '" class="form-control" value="' + this.innerText + '" >';


        let inputElement = this.getElementsByTagName("input")[0];
        inputElement.focus();

        //Exit from element
        inputElement.addEventListener("blur", function () {
            this.outerHTML = this.value; //devolve o valor digitado a tabela                     
            TotalizaColunas();
        });
    };

    /***********
     * @description Alteração de produto 
     * @param {any} id
     ************/
    let ModifyProduct = function (event) {
        event.stopPropagation();
        event.preventDefault();

        //Impede de habilitar edição no Input atual
        if (this.innerHTML.search("input") > 0) {
            return;
        }

        var TRAtual = this.parentElement;

        //Add Element
        this.innerHTML = '<input style="width:7em;" type="text" id="CodigoDoProduto" parent-id="' +
            this.id + '" class="form-control" value="' + this.innerText + '" list="ListCodigoDoProduto"  >' +
            '<datalist id="ListCodigoDoProduto">' +
            '<option></option>' +
            '</datalist >';

        $('#CodigoDoProduto').keyup(function () {
            $(this).val($(this).val().toUpperCase());
        });

        var codigoProduto = document.querySelector("#CodigoDoProduto");
        codigoProduto.addEventListener("keyup", function () {
            if (codigoProduto.value.length >= 2) {
                montaListaProdutos(codigoProduto.value);
            }
        });

        let inputElement = this.getElementsByTagName("input")[0];
        inputElement.focus();

        //Exit from element
        inputElement.addEventListener("blur", function () {
            this.outerHTML = this.value.substring(0, 15); //devolve o valor digitado a tabela           
            //Atualiza a descrição

            var toUrl = "/SimulacaoLista/Produto/GetInformationByCodigo";
            var fil = document.querySelector("#Filial").value;
            var formulaAtual = document.querySelector("#FormulaEscolhida");

            WebService.Init({ filial: fil, codigo: this.value, formula: formulaAtual.innerHTML }, toUrl, function (response) {
                if (response.success) {
                    TRAtual.cells[1].innerHTML = "1";
                    TRAtual.cells[colComponente].setAttribute("data-tippy", "");
                    tippy('[data-tippy]', { content: response.estrutura.DescricaoComponente });
                    TRAtual.cells[colCustoMateriaPrima].innerHTML = response.estrutura.CustoMateriPrima;
                    TRAtual.cells[colTipo].innerHTML = response.estrutura.Tipo;
                    TRAtual.cells[colUnidade].innerHTML = response.estrutura.Unidade;
                    TRAtual.cells[colProduto].innerHTML = document.querySelector("#Produto").innerHTML;
                    CalculaLinha(TRAtual);
                    TotalizaColunas();
                }
            });


            
        });
    };


    /**
     *  Monta a lista de produtos com códigos semelhantes
     * @param {any} searchText
     */
    function montaListaProdutos(searchText) {

        var toUrl = "/SimulacaoLista/Produto/GetProdutoBySeachAsync";
        var fil = document.querySelector("#Filial");

        WebService.Init({ search: searchText, filial: fil.value }, toUrl, function (response) {
            var ListCodigoDoProduto = document.querySelector("#ListCodigoDoProduto");
            var listaHtml = "";

            for (var i = 0; i < response.produtos.length; i++) {
                var texto = response.produtos[i].trim();
                listaHtml += "<option value='" + texto + "'>";
            }

            ListCodigoDoProduto.innerHTML = listaHtml;
        });
    }

    /**
     * função de remoção de linha
     * */
    let RemoveLine = function (event) {
        event.stopPropagation();
        event.preventDefault();
        this.focus();
        this.parentElement.outerHTML = "";
        TotalizaColunas();

    };

    /**
     * Adiciona uma linha na Tabela
     * */
    function AddLine() {

        //Adição da Linha NA PRIMEIRA TABELA
        var tabelas = $("table#tabEstrutura");
        // for (var k = 0; k < tabelas.length; k++) {
        for (var i = 0; i < tabelas[0].rows.length; i++) {
            if ($(tabelas[0].rows[i]).attr("total-line") === "true") {
                tabelas[0].insertRow(i);
                tabelas[0].rows[i].innerHTML = `<td remove='true'><a href='#'><img src='/Content/Imagens/Icons/menos-16.png' /></a></td>
                                <td></td>
                                <td></td>
                                <td editableProduct='true' > </td>                                                     
                                <td></td>
                                <td></td>
                                <td editable='true'></td>
                                <td></td>
                                <td total-column='true'></td>`;

                i += 2;
            }
        }
        //}

        //Habiliata os eventos na linha 
        addEvent();

    }

    /**
     * Compara a simulacao 
     * */
    function CompararSimulacao(event) {
        event.stopPropagation();
        event.preventDefault();

        tabelasBase = $("table#tabBase");
        tabelasEstrutura = $("table#tabEstrutura");
        Compare(tabelasBase[0], tabelasEstrutura[0]);

        var resultado = CompareTotais();
        var mensagem = "";
        var type = "";
        var icon = "";

        if (resultado > 0) {
            mensagem = 'A Fórmula ficou ' + resultado + '% mais cara';
            type = 'warning';
            icon = 'http://192.168.1.15:10300/Content/Imagens/Icons/high-priority-40.png';
        } else {
            if (resultado === 0) {
                mensagem = "Sem alterações valor da fórmula";
            } else {
                mensagem = "A Fórmula ficou " + resultado + "% mais barata";
            }
            
            type = 'success';
            icon = 'http://192.168.1.15:10300/Content/Imagens/Icons/ok-48.png';
        }

        $.notify({
            icon: icon,
            message: mensagem
        }, {
                type: type,
                placement: {
                    from: "top",
                    align: "right"
                },
                animate: {
                    enter: 'animated fadeInRight',
                    exit: 'animated flipOutX'
                },
                allow_dismiss: true,
                icon_type: 'image',
                delay:2000
            });
        
    }

    /**
     * Faz a comparação dos Totais
     * */
    function CompareTotais() {
        var TotalDoProdutoBase = superParseFloat(document.querySelector("#TotalDoProdutoBase"));
        var TotalDoProdutoSimulacao = superParseFloat(document.querySelector("#CustoTotalDoProduto"));

        if (TotalDoProdutoBase === TotalDoProdutoSimulacao) {
            return 0;
        } else {

            return ((1 - (TotalDoProdutoBase / TotalDoProdutoSimulacao)) * 100).toFixed(2);
        }
        
    }

    /**
     * Função para fazer a comparação entre tabelas
     * @param {any} tabBase
     * @param {any} tabEstru
     */
    function Compare(tabBase, tabEstru) {
        var comparado = "";

        //remove os flags de comparação anterior se houver
        for (var i = 0; i < tabBase.rows.length; i++) {
            $(tabBase.rows[i]).removeClass("alert");
            $(tabBase.rows[i]).removeClass("alert-dark");
            $(tabBase.rows[i]).removeAttr("data-compared");
            
        }
        for ( i = 0; i < tabEstru.rows.length; i++) {
            $(tabEstru.rows[i]).removeClass("alert");
            $(tabEstru.rows[i]).removeClass("alert-warning");
            $(tabEstru.rows[i]).removeAttr("data-compared");
        }

        //Comparação em Cross
        for (var linhaEstru = 0; linhaEstru < tabEstru.rows.length; linhaEstru++) {
            for (var linhaBase = 0; linhaBase < tabBase.rows.length; linhaBase++) {

                comparado = $(tabBase.rows[linhaBase]).attr("data-compared");
                if (comparado !== "true") {
                    if (tabBase.rows[linhaBase].innerText === tabEstru.rows[linhaEstru].innerText) {
                        $(tabEstru.rows[linhaEstru]).removeClass("alert alert-warning");
                        $(tabBase.rows[linhaBase]).attr("data-compared", "true");
                        break;
                    } else {
                        $(tabEstru.rows[linhaEstru]).addClass("alert alert-warning");
                    }
                }
            }
        }


        for ( linhaBase = 0; linhaBase < tabBase.rows.length; linhaBase++) {
            for ( linhaEstru = 0; linhaEstru < tabEstru.rows.length; linhaEstru++) {

                comparado = $(tabEstru.rows[linhaEstru]).attr("data-compared");
                if (comparado !== "true") {
                    if (tabEstru.rows[linhaEstru].innerText === tabBase.rows[linhaBase].innerText) {
                        $(tabBase.rows[linhaBase]).removeClass("alert alert-dark");
                        $(tabEstru.rows[linhaEstru]).attr("data-compared", "true");
                        break;
                    } else {
                        $(tabBase.rows[linhaBase]).addClass("alert alert-dark");
                    }
                }
            }
        }
      
    }


    /**
     * Adiciona os eventos aos formulários
     * */
    function addEvent() {

        var CustoOperacional = document.querySelector("#CustoOperacional");
        var Embalagem = document.querySelector("#Embalagem");
        var DespesaOperacional = document.querySelector("#DespesaOperacional");
        var MargemLucro = document.querySelector("#MargemLucro");
        var btnNovaLinha = document.querySelector('#AdicionaLinha');
        var btnComparar = document.querySelector("#Comparar");

        if (CustoOperacional !== null) {
            btnComparar.addEventListener("click", CompararSimulacao);
            CustoOperacional.addEventListener("click", ModifyHeader);
            Embalagem.addEventListener("click", ModifyHeader);
            DespesaOperacional.addEventListener("click", ModifyHeader);
            MargemLucro.addEventListener("click", ModifyHeader);
            btnNovaLinha.addEventListener("click", AddLine);
        }

        var tables = $("table#tabEstrutura");
        for (var i = 0; i < tables.length; i++) {
            for (var k = 0; k < tables[i].rows.length; k++) {

                compute = $(tables[i].rows[k]).attr("compute");
                if (compute !== "NO") {
                    if (tables[i].rows[k] !== undefined) {
                        for (var j = 0; j < tables[i].rows[k].cells.length; j++) {

                            //eventos de edição/alteração do valor da célula 
                            editable = $(tables[i].rows[k].cells[j]).attr("editable");
                            if (editable === "true") {
                                tables[i].rows[k].cells[j].addEventListener("click", ModifyValue);
                            }

                            //Edição do Produto
                            editableProduct = $(tables[i].rows[k].cells[j]).attr("editableProduct");
                            if (editableProduct === "true") {
                                tables[i].rows[k].cells[j].addEventListener("click", ModifyProduct);
                            }

                            //remoção de linha
                            remove = $(tables[i].rows[k].cells[j]).attr("remove");
                            if (remove === "true") {
                                tables[i].rows[k].cells[j].addEventListener("click", RemoveLine);
                            }

                        }
                    }
                }
            }
        }
    }

    return {
        Init: addEvent
    };

})();
