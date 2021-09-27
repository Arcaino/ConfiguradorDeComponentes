$(document).ready(function() {
    $('.check-box').prop("disabled", false);
});

$('.tabela__corpo__items__item__switch').change(function(){

    var id = $(this).attr('id');

    if ($(this).children('.check-box').is(':checked')) {
        statusDoAlarme = true;
    }
    else{
        statusDoAlarme = false;
    }
    
    ativarAlarme(id, statusDoAlarme);

});

capturarHorarioDaUltimaAtuacao = function (){
    var dataAtual = new Date();
    var dia = dataAtual.getDate();
    var mes = (dataAtual.getMonth() + 1);
    var ano = dataAtual.getFullYear();
    var horas = dataAtual.getHours();
    var minutos = dataAtual.getMinutes();
    var segundos = dataAtual.getSeconds();
    var mesFormatadoComCasaDecimal = mes < 10 ? `0${mes}` : mes.toString();
    var minutosFormatadoComCasaDecimal = minutos < 10 ? `0${minutos}` : minutos.toString();
    var segundosFormatadoComCasaDecimal = segundos < 10 ? `0${segundos}` : segundos.toString();
    return ano + "-" + mesFormatadoComCasaDecimal + "-" + dia + " " + horas + ":" + minutosFormatadoComCasaDecimal + ":" + segundosFormatadoComCasaDecimal;
}

colocarCasaDecimal = function (minutos) {
    return value < 10 ? `0${value}` : value.toString();
}

ativarAlarme = function (id, statusDoAlarme) {

    $.ajax({
        type: 'POST',
        url: 'AtuarAlarme',
        data: {id, statusDoAlarme},
        success: () => {
            dataUltimaAcao = capturarHorarioDaUltimaAtuacao();
            $( "#dataSaida-"+id ).html( $( "#dataEntrada-"+id ).text() );
            $( "#dataEntrada-"+id ).html( dataUltimaAcao );
        },

        error: (a) => {
        }
    });

}