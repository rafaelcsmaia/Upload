google.load('visualization', '1', { packages: ['corechart'] });

function drawVisualization(dat, titulo) {
    
    var dados = [];
    // Some raw data (not necessarily accurate)   
    for (i = 0; i < dat.length; i++) {
        var arr = [];
        for (j = 0; j < dat[i].length; j++) {            
            arr.push(dat[i][j]);
        }
        dados.push(arr);
        //if (i == 0) {
        //    var dados = [[dat[i].Data, dat[i].Valor1, dat[i].Valor2]];
        //} else {
        //    dados.push([dat[i].Data, dat[i].Valor1, dat[i].Valor2]);
        //}
    }
    var data = google.visualization.arrayToDataTable(dados);
    //var zica = google.visualization.arrayToDataTable([
    //  ['Mês', 'Picking', 'Packing'],
    //  ['Janeiro', 75689, 89754],
    //  ['Fevereiro', 34232, 44655],
    //  ['Março', 86755, 63345],
    //  ['Abril', 24322, 26575],
    //  ['Maio', 75689, 98766],
    //  ['Junho', 56766, 75658],
    //  ['Julho', 53434, 56797],
    //  ['Agosto', 23432, 23444],
    //  ['Setembro', 75689, 64554],
    //  ['Outubro', 76575, 67986],
    //  ['Novembro', 98777, 67867],
    //  ['Dezembro', 34534, 75675]
    //]);

    var options = {
        title: titulo,
        vAxis: { title: "Itens" },
        hAxis: { title: "Mês" },
        seriesType: "bars",
        backgroundColor: '#DEDEDE'
        //series: { 5: { type: "line" } }
    };

    var chart = new google.visualization.ComboChart(document.getElementById('grafico'));
    chart.draw(data, options);
}