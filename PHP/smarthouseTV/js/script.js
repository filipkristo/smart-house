
var baseUrl = 'http://10.110.166.90:8081/api/';
var lastState = '';

function yamahaState(){

	var uri = baseUrl + 'Yamaha/Status';

	return $.ajax({
		method: "GET",
		url: uri,
		cache: false,
		}).done(function(data) {

			var volume = data.main_Zone.basic_Status.volume.lvl.val / 10;			
			$('#lblVolume').text(volume);

			if(data.main_Zone.basic_Status.power_Control.power != "Standby")
			{
				$('.bfunc').removeAttr("disabled");
				$('#infoPanel').empty();
			}
			else
			{
				setError('Smart House is turn off');
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

function mpdCurrentSong(){

	var uri = baseUrl + 'MPD/GetCurrentSong';

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

function pandoraThumbUp(){

	var uri = baseUrl + 'Pandora/ThumbUp';

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

			if(lastState != data){

				lastState = data;	
				initializeInfoControl();

			}		

		});
}

function setVolume(volume){

	var uri = baseUrl + 'Yamaha/SetVolume?volume=' + volume;

	return $.ajax({
		method: "GET",
		url: uri,
		cache: false,
		}).done(function(data) {
						
			
			
		});	
}



function runSmartHouseCommand(name){
	
	var uri = baseUrl + 'SmartHouse/' + name + '?id=' + guid();
		
	return $.ajax({
		method: "GET",
		url: uri,
		cache: false,
		}).done(function(msg) {
			
			
			
		});

}

function getTemperature(){

	var uri = baseUrl + 'Sensor/GetCurrentTemperature';

	return $.ajax({
		method: "GET",
		url: uri,
		cache: false,
		}).done(function(data) {
			
			$('.temperature').html(data.temperature + ' &deg;C');
	        $('.humidity').text(data.humidity + ' %');

	        $('.updated').text(new Date(data.measured).toLocaleString());

			
		});
}

function restartVPN(){

	var uri = baseUrl + 'SmartHouse/RestartOpenVPN';

	return $.ajax({
		method: "GET",
		url: uri,
		cache: false,
		}).done(function(data) {
			
			$("#response").text(JSON.stringify(data,null,4));		

		});

}

function playAlarm(){

	var uri = baseUrl + 'SmartHouse/PlayAlarm';

	return $.ajax({
		method: "GET",
		url: uri,
		cache: false,
		}).done(function(data) {
			
			$("#response").text(JSON.stringify(data,null,4));		

		});

}

function turnOffTimer(value){

	var uri = baseUrl + 'SmartHouse/TurnOfTimer?minutes=' + value;

	return $.ajax({
		method: "GET",
		url: uri,
		cache: false,
		}).done(function(data) {
			
			$("#response").text(JSON.stringify(data,null,4));		

		});	
}

function refreshAll(){

	var dyamm = yamahaState();	
	var dpand = pandoraState();
	var dsmar = smartHouseState();

	$.when(dyamm, dpand, dsmar).done(function (v1, v2, v3) {

	    console.log(v1);
	    console.log(v2);
    	console.log(v3);	    

	});
}

function setTemperatureTimer(){

	setInterval(getTemperature, 30000);

}

function setInfo(info){

	$('#infoPanel').empty();
	$('#infoPanel').append('<div class="alert alert-success"><strong>' + info + '</strong></div>');

}

function setError(error){
	
	$('#infoPanel').empty();
	$('#infoPanel').append('<div class="alert alert-danger"><strong>' + error + '</strong></div>');
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
