$(function () {

    //moment.locale($('$cultureInfo').val());
    moment.locale('hr-HR');

    $('[data-utcdate]').each(function () {
        var date = new Date($(this).attr('data-utcdate')).toLocaleString();
        var d = moment(date);
        $(this).html(d.format("LLL"));
    });
});