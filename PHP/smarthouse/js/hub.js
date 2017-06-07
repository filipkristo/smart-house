	
var hub;

var lastArtist = '';
var lastAlbum = '';
var lastSong = '';

function connectionStateChanged(state) {
    var stateConversion = {0: 'connecting', 1: 'connected', 2: 'reconnecting', 4: 'disconnected'};
    console.log('SignalR state changed from: ' + stateConversion[state.oldState]
     + ' to: ' + stateConversion[state.newState]);
}

 $(function () {

 		var connection = $.connection();
	    hub = $.connection.serverHub;
	    $.connection.hub.url = 'http://10.110.166.90:8081/signalr';
	    
	    $.connection.hub.start()
    		.done(function(){ console.log('Now connected, connection ID=' + $.connection.hub.id); })
    		.fail(function(){ console.log('Could not Connect!'); });		

		connection.stateChanged(connectionStateChanged);

	    hub.client.hello = function (message) {
	        toastr["success"]('Hello ' + message);
	    };	    

    	hub.client.notification = function (message) {
	        toastr["success"](message);
	    };	    


	    hub.client.refreshAll = function () {	        
	        refreshAll();	        
	    };

	    hub.client.volumeChange = function (volume) {
	        
	        var vol = Number(volume) / 10;		

			$('#lblVolume').html("Volume: &nbsp <strong>" + vol + " Db</strong>");
		  	$('#volumeKnob').val(vol);

		  	console.log("Volume: " + vol);
	    };	    

	    hub.client.temperature = function (data) {	        	        

	        $('.temperature').html(Number(data.temperature).toFixed(2) + ' &deg;C');
	        $('.humidity').text(Number(data.humidity).toFixed(2) + ' %');
	        $('.smoke').text(Number(data.gasValue).toFixed(0) + ' %');

	        var now = new Date(data.measured);
			var formatted = now.getHours() + ":" + now.getMinutes() + ":" + now.getSeconds();
	        $('.updated').text(formatted);
	        
	    };	    

	    hub.client.pandoraRefresh = function (data) {	        	        	    	

	        lastArtist = data.artist;
	        lastSong = data.song;
			lastAlbum = data.album;
			
	        if(!$('#info-dialog').is(':visible'))
	        	return;	        

	        $('#currentArtist').html('Artist: &nbsp&nbsp&nbsp&nbsp&nbsp <strong>' + data.artist + '</strong>');
			$('#currentAlbum').html('Album: &nbsp&nbsp&nbsp&nbsp <strong>' + data.album + '</strong>');
			$('#currentRadio').html('Radio: &nbsp&nbsp&nbsp&nbsp&nbsp <strong>' + data.radio + '</strong>');
			$('#currentSong').html('Song: &nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp <strong>' + data.song + '</strong>');

			$('#imagePic').show();
			$('#imagePic').attr('src', data.albumUri);

			if(data.loved)
			{
				$('#btnThumpUp').attr('disabled', 'disabled');				
				$('#btnThumpUp').hide();
				$('#loved').show();
			}
			else
			{
				$('#btnThumpUp').removeAttr('disabled', 'disabled');
				$('#btnThumpUp').show();
				$('#loved').hide();
			}			

	    };
});