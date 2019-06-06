/**
 * Biblioteca para trnasforma a uma tabela em Json
 * */
var SimuladorChart = (function () {    

    var colProduto = 2;
    var colComponente = 3;
    var colTipo = 4;
    var colUnidade = 5;
    var colFormula = 6;
    var colCustoMateriaPrima = 7;
    var colCustoTotal = 8;
    var totalColumns = 9;
    var formatFirst = true;
    var multiplicador = 1000;

    var series = new Array();
    FormulaSerie = new Object();
    CustoSerie = new Object();

    FormulaSerie.name = 'Formula';
    FormulaSerie.type = 'column';
    FormulaSerie.data = new Array();

    CustoSerie.name = 'Custo';
    CustoSerie.type = 'line';
    CustoSerie.data = new Array();

    labels = new Array();

    xaxis = {
        type: 'category'
    };
    var options = {
        chart: {
            height: 350,
            type: 'line',
            stacked: false,
        },
        stroke: {
            width: [0, 2, 5],
            curve: 'smooth'
        },
        plotOptions: {
            bar: {
                columnWidth: '50%'
            }
        },
        colors: ['#3A5794', '#660066', '#368c45'],
        series: [],
        labels: [],
        markers: {
            size: 0
        },
        xaxis: {
            type: 'category'
        },
        yaxis: {
            min: 0,
            show: false
        },
        tooltip: {
            shared: true,
            intersect: false,
            y: {
                formatter: function (y) {
                    if (typeof y !== "undefined") {
                        if (formatFirst) {                            
                            formatFirst = false;
                            return (y / 10).toFixed(2)+'%';
                        } else {
                            formatFirst = true;
                            return (y/1000).toFixed(4);
                        }
                        
                    }
                    return y;

                }
            }
        },
        legend: {
            labels: {
                useSeriesColors: true
            },
            markers: {
                customHTML: [
                    function () {
                        return '';
                    }, function () {
                        return '';
                    }, function () {
                        return '';
                    }
                ]
            }
        }
    }

    function GetSerieData() {
        var tabSimulacao = $("table#tabEstrutura");
        
        for (var tab = 0; tab < tabSimulacao.length; tab++) {
            for (var lin = 1; lin < tabSimulacao[tab].rows.length; lin++) {
                if (tabSimulacao[tab].rows[lin].cells.length === totalColumns) {
                    if ($(tabSimulacao[tab].rows[lin]).attr("total-line") !== "true") {
                        FormulaSerie.data.push(
                            ((superParseFloat(tabSimulacao[tab].rows[lin].cells[colFormula])) / 100) * 1000
                        );
                        CustoSerie.data.push(superParseFloat(tabSimulacao[tab].rows[lin].cells[colCustoTotal]) * 1000);

                        var label = tabSimulacao[tab].rows[lin].cells[colComponente].innerText;
                        if (labels.indexOf(label) ===-1) {
                            labels.push(label);
                        }
                        
                    }
                }
            }
        }
        series.push(FormulaSerie);
        series.push(CustoSerie);    
    }

    /**
     * Adpta a posição das colunas para tabela de vizualização
     * */
    function GetSerieView() {
        
        colComponente = 2;
        colFormula = 5;
        colCustoTotal = 7;    
        totalColumns = 8;          
        GetSerieData();
    }

    /**
     * Cria o gráfico em tela
     * @param {any} table
     */
    function create() {
        GetSerieData();
        options.series = series;
        options.labels = labels;

        _chart = new ApexCharts(
            document.querySelector("#chart"),
            options
        );
        _chart.render();
    }

    /**
     * Função para renderizar apenas as vizualizações
     * */
    function RenderView() {
        GetSerieView();
        options.series = series;
        options.labels = labels;

        _chart = new ApexCharts(
            document.querySelector("#chart"),
            options
        );
        _chart.render();
    }
    /**
     * Função de atualização
     * */
    function Atualiza() {
        series = new Array();
        FormulaSerie.data = new Array();
        CustoSerie.data = new Array();

        GetSerieData();
        options.labels = labels;
        _chart.updateSeries(series, true);    
    }
    /*
     * Métodos publicos 
     */
    return {
        BuildChart: create,
        Update: Atualiza,
        ViewOnly: RenderView
    };

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

})();


