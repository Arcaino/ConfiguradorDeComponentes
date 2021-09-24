$(document).ready(function() {
    $('.check-box').prop("disabled", false);
});

$('.switch').change(function(){

    var id = $(this).attr('id');

    if ($(this).children('.check-box').is(':checked')) {
        statusDoAlarme = true;
    }
    else{
        statusDoAlarme = false;
    }

    console.log(id);
    
    ativarAlarme(id, statusDoAlarme);

});

ativarAlarme = function (id, statusDoAlarme) {

    $.ajax({
        type: 'POST',
        url: 'AtuarAlarme',
        data: {id, statusDoAlarme},
        success: () => {
        },

        error: (a) => {
        }
    });

}