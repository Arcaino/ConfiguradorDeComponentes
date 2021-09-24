$(document).ready(function() {
    $('.check-box').prop("disabled", false);
});

$('.switch').change(function(){
    var id = $('.switch').attr('id');
    if ($('.check-box').is(':checked')) {
        statusDoAlarme = true;
    }
    else{
        statusDoAlarme = false;
    }
    ativarAlarme(id, statusDoAlarme);
});


ativarAlarme = function (id, statusDoAlarme) {

    console.log(statusDoAlarme);

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