	
var hub;

 $(function () {

	    hub = $.connection.serverHub;
	    $.connection.hub.url = 'http://10.110.166.90:8081/signalr';
	    
	    $.connection.hub.start()
    		.done(function(){ console.log('Now connected, connection ID=' + $.connection.hub.id); toastr["success"]('SignalR Connected'); })
    		.fail(function(){ console.log('Could not Connect!'); });		

    	$.connection.hub.disconnected(function() {
   			setTimeout(function() {
       			$.connection.hub.start();
   			}, 8000); // Restart connection after 8 seconds.
		});

	    hub.client.hello = function (message) {
	        toastr["success"]('Hello ' + message);
	    };	    

    	hub.client.notification = function (message) {
	        toastr["success"](message);
	    };	    

	    hub.client.temperature = function (data) {
	        
	        console.log(data);

	        $('.temperature').html(Number(data.temperature).toFixed(2) + ' &deg;C');
	        $('.humidity').text(Number(data.humidity).toFixed(2) + ' %');
        	$('.gas').text(Number(data.gasValue).toFixed(0) + ' G');

	        var now = new Date(data.measured);
			var formatted = now.getHours() + ":" + now.getMinutes() + ":" + now.getSeconds();
	        $('.updated').text(formatted);
	        
	    };	    

	    hub.client.refreshAll = function () {
	        
	        refreshAll();
	        
	        if(lastState == 'Pandora'){
	        	updatePandora();
	        }
	        else if(lastState == 'Music'){
	        	updatePlayer();
	        }
	    };	    

		hub.client.volumeChange = function (volume) {
	        
	        var vol = Number(volume) / 10;			
			$('#lblVolume').text(vol);
				        
	    };	    

	    hub.client.pandoraRefresh = function (data) {
	        
	        updatePandoraValues(data);

	    };	    
});