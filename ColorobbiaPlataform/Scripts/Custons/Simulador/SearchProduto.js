

/*
 Inicialização do script
 */
$(document).ready(function () {
    var codigoProduto = document.querySelector("#CodigoDoProduto");
    codigoProduto.addEventListener("keyup", function () {

        if (codigoProduto.value.length >= 2) {
            montaListaProdutos(codigoProduto.value);
        }

    });

});


/**
 * Monta a lista de produtos disponíveis conforme o usuário vai digitando a pesquisa
 * @param {string} searchText
 * @returns {option} list
 * */
function montaListaProdutos(searchText) {

    var toUrl = "/SimulacaoLista/Produto/GetProdutoBySeachAsync";    
    var fil = document.querySelector("#Filial");

    //Valida se preecheu a Filial
    if (fil.value === "") {

        $.notify({            
            message: 'Escolha uma Filial antes de continuar'
        }, {                
                type: 'danger',
                placement: {
                    from: "top"
                },
                animate: {
                    enter: 'animated fadeInRight',
                    exit: 'animated flipOutX'
                },
                allow_dismiss:false
            });
        fil = document.querySelector("#Filial").focus();
        $("#Filial").addClass("is-invalid");
        return;
    } else {
        $("#Filial").removeClass("is-invalid");
    }

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
