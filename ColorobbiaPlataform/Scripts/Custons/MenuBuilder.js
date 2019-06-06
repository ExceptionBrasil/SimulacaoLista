/**
 * Menubuilder.js - Version Materializer.css
 * Autor: Daniel P Silveira
 * Description: 
 * Request the menu options to the controller by the location. 
 * After that built the menu based on framework materializer.css 
 * and sho to the user
 * 
 * The MIT License (MIT)
 * 
 * Copyright (c) [year] [fullname]
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy of
 * this software and associated documentation files (the "Software"), to deal in
 * the Software without restriction, including without limitation the rights to
 * use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
 * the Software, and to permit persons to whom the Software is furnished to do so,
 * subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
 * FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
 * COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
 * IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
 * CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 * 
 * */


let MenuBuilder2 = (function () {

	//Objeto Menu
	let MenuObj = function () {

		/**
		 * Controller que armazena a localização dos menus que serão carregados
		 * */
		this.Controller = {
			set: function (value) { Controller = value; },
			get: function () { return Controller; },
			SetLocation: function (location) {
				this.set({ "Location": location });
			}
		};

		/**
		 * Var que possui a URL do Controller que responde pela requisição dos menus
		 * */
		this.UrlDestino = {
			set: function (value) { UrlDestino = value; },
			get: function () { return UrlDestino; }
		};


		/**
		 * Endereço default
		 * 
		 * */
		this.controllerDefault = {
			get: "Home"
		};

		/**
		 * Verifica qual é o controller que estou trabalhando e recupera ele
		 * @returns {string} contexto
		 * */
		this.GetContex = function () {
			let contexto = window.location.pathname;

			while (contexto.indexOf("/") >= 0) {

				if (contexto.indexOf("/") === 0) {
					contexto = contexto.slice(1, contexto.length);
				}

				if (contexto.indexOf("/") > 0) {

					contexto = contexto.slice(0, contexto.indexOf("/"));
				}

			}

			//Valor default
			if (contexto === "") {
				contexto = this.controllerDefault.get;
			}

			return contexto;
		};

		/**
		 * Faz o request do Menu
		 * */
		this.Load = function () {
			let promise = new Promise((resolve, reject) => {

				//Ajax Object 
				let ajaxObj = {
					url: this.UrlDestino.get(),
					data: this.Controller.get(),
					dataType: "json",
					method: "POST",
					error: function (response) {
						reject("Erro to get Menu on Ajax request on server!");
					},
					success: function (response) {
						if (response !== null && response !== "undefined") {
							if (response.success) {
								resolve(response.menu);
							} else {
								reject("Menu don't have itens to continue | MenuBuilder_Module.js");
							}
						}
					}
				};

				//Faz o call do Webservice
				$.ajax(ajaxObj);

			});

			//Resolve a promisse 
			promise
				.then((data) => WriteHtml(data))
				.catch((data) => console.log(data));

		};

	};

	//Cria o Menu
	let Menu = new MenuObj();

	//Obtem a Area que estou trabalhando, para recuperar o menu funcional dessa área
	Menu.Controller.SetLocation(Menu.GetContex());

	//Obtem da variável publica a URL do controller que dará a resposta do menu público 
	Menu.UrlDestino.set(urlGetMenu);

	//Faz o request do Menu e escreve ele no DOM
	Menu.Load();



})();

/**
 *  Escreve o Menu no DOM, utilizando o CSS do Materialize
 * @param {Array<string>} menu
 */
var WriteHtml = function (menu) {

	var LoadMenu = document.querySelector("#LoadMenu");

	//Validate if the Id is present at Document
	if (LoadMenu === null || LoadMenu === "undefined") {
		return Console.log("Id #LoadMenu not present at Document, written the Id there");
		document.write('<div id="LoadMenu"></div>');
	}


	var htmlScript = "";
	//Sempre fixo - Topo do Menu



	if (menu.length > 0) {

		var i = 0;
		while (i < menu.length) {

			var MenuGroup = menu[i].Grupo.trim();
            var grupoAtu = menu[i].Grupo.trim();
            var icon = "";
			while (grupoAtu === MenuGroup) {

                if (menu[i].Area.trim() !== "") {
                    if (menu[i].Icon === null) {
                        icon = "chevron_right";
                    } else {
                        icon = menu[i].Icon;
                    }
                    htmlScript += `<a class="dropdown-item" href="/` + menu[i].Area.trim() + `/` + menu[i].Controller.trim() + `/` + menu[i].Action.trim() + `"> <i class="material-icons">` + icon+`</i>`+ menu[i].Descricao.trim() + `</a>`;
				} else {
                    htmlScript += `<a class="dropdown-item" href="/` + menu[i].Controller.trim() + `/` + menu[i].Action.trim() + `"> <i class="material-icons">` + icon + `</i>` + menu[i].Descricao.trim() + `</a>`;
				}
				i++;

				if (typeof (menu[i]) === "undefined") {
					MenuGroup = "";
					grupoAtu = "*";
				} else {
					grupoAtu = menu[i].Grupo.trim();
				}

			}
			htmlScript += ' <div class="dropdown-divider"></div>';
			
		}
	}

	//sempre fixo Home 
	htmlScript += ' <div class="dropdown-divider"></div>';
	htmlScript += '<a class="dropdown-item"  href="/Home"><i class="material-icons">home</i>Home</a>';
	htmlScript += '<a class="dropdown-item"  href="/Admin/Login/Logout"><i class="material-icons">exit_to_app</i>Sair</a>';
	//Escreve o Menu no DOM
	LoadMenu.innerHTML = htmlScript;
};