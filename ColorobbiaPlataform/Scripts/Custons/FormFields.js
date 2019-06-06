// ***********************************************************************
// Assembly         : Inhuman
// Author           : Daniel
// Created          : 06-08-2018
//
// Last Modified By : Daniel
// Last Modified On : 06-08-2018
// ***********************************************************************
// <copyright file="DisableFormFields.js" company="DPS">
//     Copyright ©  2018
// </copyright>
// <summary>Desabilita/Habilita todos os campos do form exceto o Submit</summary>
// ***********************************************************************



    /// <summary>
    /// Disabilita todos os form fields com exeção do indicado no parâmetro <<exclude>>
    /// Se o parametro for deixado em braco desabilita todos os campos 
    /// Informe os Ids separados por vírgula, para habilitar campos específicos
    /// </summary>
    /// <param name="exclude">The exclude.</param>
var DisableFormFields = function (exclude) {
    
    var inputs = document.querySelectorAll("input");
    var excludes = "";

    if (typeof (exclude) !== "undefined") {
        excludes = exclude;
    }


    for (var i = 0; i < inputs.length; i++) {

        if (inputs[i].type !== "submit" ) {
            if (excludes.length > 0) {

                if (excludes==inputs[i].id) {
                    inputs[i].disabled = true;
                } else {
                    inputs[i].disabled = false;
                }

            } else {
                inputs[i].disabled = true;
            }

        }
    }
}


    /// <summary>
    /// Habilita todos os form fields com exeção do indicado no parâmetro <<exclude>>
    /// Se o parametro for deixado em braco habilita todos os campos 
    /// Informe os Ids separados por vírgula, para habilitar campos específicos
    /// </summary>
    /// <param name="exclude">The exclude.</param>
var EnableFormFields = function (exclude) {
    var inputs = document.querySelectorAll("input");
    var excludes = "";

    if (typeof (exclude) !== "undefined") {
        excludes = exclude;
    }


    for (var i = 0; i < inputs.length; i++) {

        if (inputs[i].type !== "submit") {
            if (excludes.length > 0) {

                if (excludes==inputs[i].id ) {
                    inputs[i].disabled = false;
                } else {
                    inputs[i].disabled = true;
                }

            } else {
                inputs[i].disabled = false;
            }

        }
    }
}

