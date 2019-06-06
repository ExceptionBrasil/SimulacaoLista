
$(document).ready(function () {
    var formulaBase = document.querySelector("#FormulaBase");
    var formulaSimulacao = document.querySelector("#FormulaSimulacao");
    var codigoProduto = document.querySelector("#CodigoDoProduto");

    document.querySelector("#Filial").focus();

    formulaBase.addEventListener("focus", function () {
        GetFormula(codigoProduto.value);
    });

    formulaSimulacao.addEventListener("focus", function () {
        GetFormula(codigoProduto.value);
    });

});


/**
 * Obtem a fórmula do produto
 * @param {any} produto
 */
var GetFormula = function (searchText) {
    var toUrl = "/SimulacaoLista/Produto/GetFormulaByProduto";
    var fil = document.querySelector("#Filial");

    //Valida se preecheu a Filial
    if (searchText === "") {

        $.notify({
            message: 'Informe um Produto antes de continuar'
        }, {
                type: 'danger',
                placement: {
                    from: "top"
                },
                animate: {
                    enter: 'animated fadeInRight',
                    exit: 'animated flipOutX'
                },
                allow_dismiss: false
            });
        document.querySelector("#CodigoDoProduto").focus();
        $("#CodigoDoProduto").addClass("is-invalid");
        return;
    } else {
        $("#CodigoDoProduto").removeClass("is-invalid");
    }

    WebService.Init({ produto: searchText, filial: fil.value }, toUrl, function (response) {
        var ListFormulas = document.querySelector("#ListFormulas");
        var listaHtml = "";

        for (var i = 0; i < response.formulas.length; i++) {
            var texto = response.formulas[i].trim();
            listaHtml += "<option value='" + texto + "'>";
        }

        ListFormulas.innerHTML = listaHtml;
    });
}