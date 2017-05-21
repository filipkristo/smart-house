$(function () {

    moment.locale('hr-HR');

    $.connection.hub.logging = true;
    if (typeof ($.connection.telemetryHub) !== "undefined") {

        var hub = $.connection.telemetryHub;

        hub.client.receiveTelemetry = function (data) {

            console.log(data);

            $('#temperature').html(data.Temperature.toFixed(2) + ' &deg;C');
            $('#humidity').text(data.Humidity.toFixed(2) + ' %');
            $('#smoke').text(data.GasValue.toFixed(0) + ' %');

            $('#updated').text(moment(new Date(data.CreatedUtc).toLocaleString()).format("LLL"));

        };        

        $.connection.hub.start()
            .done(function () { console.log('Now connected, connection ID=' + $.connection.hub.id); })
            .fail(function () { console.log('Could not Connect!'); });		

    }

    $.getJSON('/api/Telemetry/GetLastTelemetry', function (data, status) {

        $('#temperature').html(data.Temperature.toFixed(2) + ' &deg;C');
        $('#humidity').text(data.Humidity.toFixed(2) + ' %');
        $('#smoke').text(data.GasValue.toFixed(0) + ' %');

        $('#updated').text(moment(new Date(data.CreatedUtc).toLocaleString()).format("LLL"));

    });
});