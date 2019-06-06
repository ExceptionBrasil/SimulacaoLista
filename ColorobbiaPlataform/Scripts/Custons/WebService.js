// ***********************************************************************
// Assembly         : Inhuman
// Author           : Daniel
// Created          : 06-14-2018
//
// Last Modified By : Daniel
// Last Modified On : 06-15-2018
// ***********************************************************************
// <copyright file="WebService.js" company="DPSYS">
//     Copyright ©  2018
// </copyright>
// <summary> WebService de envio de dados ao servidor 
// Ao fim do processamento  
// </summary >
// ***********************************************************************

var WebService = (
    function () {


        //Chamada do Ajax
        //Faz o envio da informação ao Server e trata o retorno fazendo o call da promisse
        var SendData = (obj, toUrl) => {
            return (new Promise((resolve, reject) => {

                $.ajax({
                    url: toUrl,
                    data: obj,
                    dataType: "json",
                    method: "POST",
                    error: function (response) {
                        console.log("Error on server. Epic Fail! | WebService.js");                        
                        reject();
                    },
                    success: function (response) {
                        if (typeof (response) == "undefined") {
                            reject();
                        }

                        console.log("Success on request response to server! | WebService.js");
                        if (response.success) {                            
                            resolve(response);
                        } else {                            
                            resolve(response);
                        }
                    }

                });

            })
            );
        }

        //Faz o envio para o servidor, tratando o retorno pelo callback, caso exista
        var Send = (obj, toUrl, callBack) => {
            SendData(obj, toUrl).then((response) => {     

                if (typeof (response.success) == "undefined") {
                    return console.log("Response object is missim the property [bool:success]");
                }

                if (response.success) {
                    if (typeof (response.menssage) != "undefined"){
                        console.log(response.menssage);
                    }

                    if (typeof (callBack != "undefined")) {
                        callBack(response);
                    }

                    
                } else {
                    if (typeof (response.menssage) != "undefined") {
                        console.log(response.menssage);
                    }
                    if (typeof (callBack != "undefined")) {
                        callBack(response);
                    }
                }
            }).catch((e) => {
                console.log("Invalid return from server, or the server is missing, way, gone, caputs, GAME OVER!!!!!.");
            });            
        }

        //***********************************
        //Métodos públicos
        //***********************************
        return ({
            Init: Send            
        });
    }
)();

