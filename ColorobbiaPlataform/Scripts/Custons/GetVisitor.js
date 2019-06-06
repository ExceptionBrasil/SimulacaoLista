/**
 * Data: 10/10/18
 * Autor; Daniel P. Silveira
 * função para recuperar o cadastro do visitante após digitar o CPF
 */

$(document).ready(function () {
    var documento = document.querySelector("#Documento");

    documento.addEventListener("keypress", TestKey);
    documento.addEventListener("blur", TestKey);

});

var TestKey = function (event) {
    event.stopPropagation();
    var documento = document.querySelector("#Documento");
    var documentoDigitado = documento.value.trim();

    if (documentoDigitado.length >= 0) {
        if (!isNumber(documentoDigitado)) {
            documento.value = "";
            return ShowToast("CPF deve ter apenas números");
        }
    }


    if (documentoDigitado.length >= 10) {
        documento.value = documentoDigitado.substring(0, 11);
    }

    if (documentoDigitado.length == 11) {
        var visitante = GetVisitante(documento.value);
    }

};

var GetVisitante = function (documento) {

    WebService.Init({ document: documento },
        urlWebService,
        function (response) {
            if (response.success) {

                var canvas = document.getElementById('canvas');
                var context = canvas.getContext('2d');
              
                var imagem = new Image();

               imagem.onload = function () {
                    context.drawImage(this, 0, 0, canvas.width, canvas.height);
                };

                imagem.src = response.visitante.FotoBase64;
                context.drawImage(imagem, 0, 0, 320, 240);
                
                var Documento = document.querySelector("#Documento");
                var Empresa = document.querySelector("#Empresa");
                var Nome = document.querySelector("#Nome");
                var Rg = document.querySelector("#Rg");
                var Telefone = document.querySelector("#Telefone");
                var IdVisitante = document.querySelector("#IdVisitante");

                //atualiza os dados do visitante

                Documento.value = response.visitante.Documento;
                Empresa.value = response.visitante.Empresa;
                Nome.value = response.visitante.Nome;
                Rg.value = response.visitante.Rg;
                Telefone.value = response.visitante.Telefone;
                IdVisitante.value = response.visitante.Id;

               

               

            } else {
                //console.log("Incluir.cshtml | Documento não encontrado, " + documento);
                if (response.block) {
                    ShowToast(response.mensagem);
                }
            }
        });
};

function isNumber(n) {
    if (n == "") {
        return true;
    }
    return !isNaN(parseFloat(n)) && isFinite(n);
}