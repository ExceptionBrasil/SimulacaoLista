

$(document).ready(function () {
    var btnSalvar = document.querySelector("#SalvarJson");
    btnSalvar.addEventListener("click", function (event) {
        event.stopPropagation();
        event.preventDefault();

        var nomeSimulacao = prompt("Nome da Simulação?");
        if (nomeSimulacao === null) {
            return;
        }

        var obj = JSON.stringify(SaveToJson.GenerateJson());

        WebService.Init({ json: obj, nome: nomeSimulacao}, toUrlToSave, function (response) {

            if (response.success) {
                $.notify({
                    message: response.menssage
                }, {
                        type: "success",
                        placement: {
                            from: "bottom"
                        },
                        animate: {
                            enter: 'animated fadeInRight',
                            exit: 'animated flipOutX'
                        },
                        allow_dismiss: true
                    });
            } else {
                $.notify({
                    message: response.menssage
                }, {
                        type: "info",
                        placement: {
                            from: "bottom"
                        },
                        animate: {
                            enter: 'animated fadeInRight',
                            exit: 'animated flipOutX'
                        },
                        allow_dismiss: true
                    });
            }

        });
    });
});