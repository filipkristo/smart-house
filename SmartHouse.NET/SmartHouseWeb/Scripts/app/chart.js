$(function () {

    moment.locale('hr-HR');
    var url = $("#charUrl").data("url");

    $.get(url, function (data) {

        for (var i = 0; i < data.length; i++) {
            data[i].time = moment(data[i].time).format("YYYY-MM-DD HH:mm");
        }

        Morris.Area({
            element: 'temperatureChart',
            data: data,
            xkey: 'time',
            ykeys: ['temperature', 'humidity'],
            labels: ['Temperature', 'Humidity'],
            pointSize: 2,
            hideHover: 'auto'
        });

    });

});