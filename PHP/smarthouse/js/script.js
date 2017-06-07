
var baseUrl = 'http://10.110.166.90:8081/api/';
var lastVolume = 0.0;

function yamahaState(){

	var uri = baseUrl + 'Yamaha/Status';

	return $.ajax({
		method: "GET",
		url: uri,
		cache: false,
		}).done(function(data) {

			var volume = data.main_Zone.basic_Status.volume.lvl.val / 10;			
			lastVolume = volume;

			$('#volumeKnob').val(volume).trigger('change');
			$('#lblVolume').html("Volume: &nbsp <strong>" + volume + " Db</strong>");

			if(data.main_Zone.basic_Status.power_Control.power != "Standby")
			{
				$('.bfunc').removeAttr("disabled");
				enableKnob();
				setInfo('Smart House is turn on');
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

function pandoraThumbUp(){

	var uri = baseUrl + 'Remote/Love';

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

function getNowPlaying(){

	var uri = baseUrl + 'SmartHouse/NowPlaying';
		
	return $.ajax({
		method: "GET",
		url: uri,
		cache: false,
		});
}

function showLyrics(artist, song){
	
	var uri = baseUrl + 'Lyrics/GetMetroLyrics?artist=' + encodeURI(artist) + '&song=' + encodeURI(song);
	
	$('#lyricsBody').empty();
	$('#lyricsBody').html('Please wait....');

	return $.ajax({
		method: "GET",
		url: uri,
		cache: false,
		}).done(function(data){

			$('#lyricsBody').empty();
			$('#lyricsBody').html(data);
			
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
        	$('#smoke').text(Number(data.gasValue).toFixed(0) + ' %');

	        $('#updated').text(new Date(data.measured).toLocaleString());

			
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

function startPandora(){

	var uri = baseUrl + 'Pandora/Start';

	return $.ajax({
		method: "GET",
		url: uri,
		cache: false,
		}).done(function(data) {
			
			$("#response").text(JSON.stringify(data,null,4));		

		});	
}

function stopPandora(){

	var uri = baseUrl + 'Pandora/Stop';

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
	var dsmar = smartHouseState();

	$.when(dyamm, dsmar).done(function (v1, v2) {

	    console.log(v1);
	    console.log(v2);    	

		getNowPlaying().done(function(data){
			
			lastArtist = data.artist;
	        lastSong = data.song;
			lastAlbum = data.album;

		});

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

function TurnOnAC(){

	var uri = baseUrl + 'Sensor/AirCondition?On=1';

	return $.ajax({
		method: "POST",		
		url: uri,
		cache: false,
		}).done(function(data) {
			
			
		});
}

function TurnOffAC(){

	var uri = baseUrl + 'Sensor/AirCondition?On=0';

	return $.ajax({
		method: "POST",		
		url: uri,
		cache: false,
		}).done(function(data) {
			
			
		});
}

function setTemperatureTimer(){

	setInterval(getTemperature, 30000);

}

function enableKnob(){

	$('#volumeKnob').trigger(
        'configure',
        {
            "readonly":false
        }
    );
}

function setInfo(info){

	$('#infoPanel').empty();
	//$('#infoPanel').append('<div class="alert alert-success"><strong>' + info + '</strong></div>');

}

function setError(error){
	
	$('#infoPanel').empty();
	$('#infoPanel').append('<div class="alert alert-danger"><strong>' + error + '</strong></div>');
}

function disableKnob(){
	
	$('#volumeKnob').data('readOnly', 'true');

	$('#volumeKnob').trigger(
        'configure',
        {
            "readonly":true
        }
    );    
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
