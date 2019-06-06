// ***********************************************************************
// Assembly         : Inhuman
// Author           : Daniel
// Created          : 05-20-2018
//
// Last Modified By : Daniel
// Last Modified On : 05-29-2018
// ***********************************************************************
// <copyright file="Toast.js" company="DPSYS">
//     Copyright ©  2018
// </copyright>
// <summary>Rotinas auxiliares de interface para exibição do Toast em tela</summary>
// ***********************************************************************






/// <summary>
/// Aplica o Toast quando há retorno de HTML.ValidationMessage
/// para isso é necessário carregar o script:
/// <script type="text/javascript" src="~/Scripts/Custom/Toast.js"></script>
/// E colar HTML.ValidationMessage dentro da <div class="toaster"></div>
/// </summary>
/// <returns></returns>
$(document).ready(function () {
    var t = document.querySelectorAll(".toaster");
   

    //Run Toasts
    for (var i = 0; i < t.length; i++) {
        if (t[i].innerText !== "") {
            ShowToast(t[i].innerText);
        }

        //Don`t show the original menssage
        t[i].innerHTML = "";
        t[i].outerHTML = "";
    }
})


//Exibe o Toast
var ShowToast = (m) => {
    M.toast({ html: m, classes: 'rounded' });
}

