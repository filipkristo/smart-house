
var baseUrl = 'http://10.110.166.90:8081/api/';

function yamahaState(){

	var uri = baseUrl + 'Yamaha/Status';

	return $.ajax({
		method: "GET",
		url: uri,
		cache: false,
		}).done(function(data) {

			var volume = data.main_Zone.basic_Status.volume.lvl.val / 10;			
			$('#volumeKnob').val(volume).trigger('change');

			if(data.main_Zone.basic_Status.power_Control.power != "Standby")
			{
				$('.bfunc').removeAttr("disabled");
			}
		});
}

function musicState(){

	var uri = baseUrl + 'MPD/GetStatus';

	return $.ajax({
		method: "GET",
		url: uri,
		cache: false,
		}).done(function(data) {
						
						
		});
}

function pandoraState(){

	var uri = baseUrl + 'Pandora/CurrentSongInfo';

	return $.ajax({
		method: "GET",
		url: uri,
		cache: false,
		}).done(function(data) {
			
					
		});
}

function smartHouseState(){

	var uri = baseUrl + 'SmartHouse/GetCurrentState';

	return $.ajax({
		method: "GET",
		url: uri,
		cache: false,
		}).done(function(data) {
			
			$('.smartInput').removeClass('active');
			$('*[data-command="' + data + '"]').addClass('active');

		});
}

function setVolume(volume){

	var uri = baseUrl + 'Yamaha/SetVolume?volume=' + volume;

	return $.ajax({
		method: "GET",
		url: uri,
		cache: false,
		}).done(function(data) {
						
			$('#output').text(data.Message);
			
		});	
}



function runSmartHouseCommand(name){
	
	var uri = baseUrl + 'SmartHouse/' + name + '?id=' + guid();
		
	return $.ajax({
		method: "GET",
		url: uri,
		cache: false,
		}).done(function(msg) {
			
			$('#output').text(msg.Message);
			
		});

}

function getTemperature(){

	var uri = baseUrl + 'Sensor/GetCurrentTemperature';

	return $.ajax({
		method: "GET",
		url: uri,
		cache: false,
		}).done(function(data) {
			
			$('#temperature').html(data.temperature + ' &deg;C');
	        $('#humidity').text(data.humidity + ' %');

	        $('#updated').text(new Date(data.measured).toLocaleString());

			console.log(JSON.stringify(data));	
		});
}

function refreshAll(){

	var dtemp = getTemperature();
	var dyamm = yamahaState();	
	var dpand = pandoraState();
	var dsmar = smartHouseState();

	$.when(dtemp, dyamm, dpand, dsmar).done(function (v1, v2, v3, v4) {

	    console.log(v1);
	    console.log(v2);
    	console.log(v3);
	    console.log(v4);

	});
}

function setTemperatureTimer(){

	setInterval(getTemperature, 15000);

}

function guid() {
  function s4() {
    return Math.floor((1 + Math.random()) * 0x10000)
      .toString(16)
      .substring(1);
  }
  return s4() + s4() + '-' + s4() + '-' + s4() + '-' +
    s4() + '-' + s4() + s4() + s4();
}
