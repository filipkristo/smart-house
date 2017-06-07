<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no">
	<meta name="author" content="Filip Krišto">
  	<title>Smart House</title>

  	<link href="css/bootstrap.min.css" rel="stylesheet">	
	<link href="css/Site.css" rel="stylesheet">
	<link href="css/toastr.min.css" rel="stylesheet" type="text/css" />
	<link rel="icon" href="favicon.ico">

</head>
<body>

<br />						

<div class="container body-content">			
		
		<?php
			include 'header.php';	
		?>

		<br />		

			<div id="infoPanel">

			</div>			

		<br />

		<div class="tab-content">
		  	<div role="tabpanel" class="tab-pane active" id="main">

				<div class="row">

					<div class="col-md-3 col-xs-3">
						<a href = "#" class = "bfunc thumbnail smartInput prog" data-command="Pandora">
				 			<img src = "/smarthouse/img/pandora.png" class="inputImage" alt = "Pandora input">
						</a>
					</div>

					<div class="col-md-3 col-xs-3">
						<a href = "#" class = "bfunc thumbnail smartInput prog" data-command="Music">
				 			<img src = "/smarthouse/img/music.png" class="inputImage" alt = "Music input">
						</a>
					</div>

					<div class="col-md-3 col-xs-3">
						<a href = "#" class = "bfunc thumbnail smartInput prog" data-command="XBox">
				 			<img src = "/smarthouse/img/xbox.png" class="inputImage" alt = "XBox input">
						</a>
					</div>

					<div class="col-md-3 col-xs-3">
						<a href = "#" class = "bfunc thumbnail smartInput prog" data-command="TV">
				 			<img src = "/smarthouse/img/TV.png" class="inputImage" alt = "TV input">
						</a>
					</div>

				</div>

				<br />
				<br />								

				<?php
					include 'info.php';	
				?>

		  	</div>  

			<div role="tabpanel" class="tab-pane" id="info">
			
				<h3>Temperature </h3>
		  		<div class="row">

                        <br />

                        <div class="col-md-4 col-xs-6">                            

                            <img class="sensorIcon centerSmall" alt="Temperature" title="Temperature" src="./img/Temperature.png" />

                            <h4>                                
                                <div id="temperature" class="temperature" style="font-weight:bold;">
                                    --- &deg;C 
                                </div>                                
                            </h4>                                                                           

                        </div>

                        <div class="col-md-4 col-xs-6">

                            <img class="sensorIcon centerSmall" alt="Humidity" title="Humidity" src="./img/Humidity.png" />

                            <h4>                                
                                <div id="humidity" class="humidity" style="font-weight:bold;">
                                    --- %
                                </div>
                            </h4>                            
                           
                        </div>


                        <div class="col-md-4 col-xs-12">

                            <img class="sensorIcon centerSmall" alt="Last updated" title="Clock" src="./img/Clock.png" />

                            <h4>                                
                                <div id="updated" class="updated" style="font-weight:bold;">
                                    --- 
                                </div>
                            </h4>                            
                           
                        </div>

				</div>

			</div>

			<div role="tabpanel" class="tab-pane" id="action">
		  		<div class="row">
		            <div class="list-group table-of-contents">
		              <a id="restartVPN" class="list-group-item" href="#">Restart VPN</a>
		              <a id="playAlarm" class="list-group-item" href="#">Play alarm sound</a>
		              <a id="pandoraState" class="list-group-item" href="#">Pandora state</a>
		              <a id="yamahaState" class="list-group-item" href="#">Yamaha state</a>
		              <a id="mpdState" class="list-group-item" href="#">MPD state</a>
		              <a id="mpdSong" class="list-group-item" href="#">MPD song</a>
		              <a id="turnOffTimer" data-target="#timer-dialog" data-toggle="modal" class="list-group-item" href="#">Turn off timer</a>
		            </div>
      			</div>

  				<div class="row">
  					<h4>Result:</h4>
  				</div>				

      			<div class="row">
					<div class="col-md-12">
						<textarea class="form-control shadow" id="response" rows="30"></textarea>
					</div>
				</div>

			</div>			

			<div role="tabpanel" class="tab-pane" id="settings">
			  		Sample for Settings
			</div>

		<?php include 'player.php';?>
        <?php include 'footer.php';?>
        <?php include 'timerDialog.php';?>        
</div> 
 
<script type="text/javascript" src="js/jquery.min.js"></script>
<script type="text/javascript" src="js/bootstrap.min.js"></script>
<script type="text/javascript" src="js/jquery.knob.min.js"></script>
<script type="text/javascript" src="js/script.js"></script>
<script type="text/javascript" src="js/dialog.js"></script>
<script type="text/javascript" src="js/winapi.js"></script>
<script type="text/javascript" src="js/jquery.signalR-2.2.1.min.js"></script>
<script src='http://10.110.166.90:8081/signalr/js'></script>
<script type="text/javascript" src="js/hub.js"></script>
<script type="text/javascript" src="js/playerinfo.js"></script>
<script type="text/javascript" src="js/toastr.min.js"></script>

<script>

	//http://anthonyterrien.com/demo/knob/
	//http://bootswatch.com/darkly/#buttons

	var timer = null;
	var dialogShow = false;

	function runCommand(command){

		runSmartHouseCommand(command).done(function(){
			
			console.log('command ' + command + new Date());			
			
		});

	}

	function handleFuncClick(){

		var button = $(this);
		var command = $(this).data("command");

		button.attr('disabled', 'disabled');

		if(button.hasClass('prog'))
			waitingDialog.show('Please wait...');

		runSmartHouseCommand(command).done(function(){
			
			console.log('command ' + command + new Date());

			button.removeAttr('disabled', 'disabled');
			
			if(button.hasClass('prog'))
				waitingDialog.hide();
			
		});

	}

	function bfuncEventHandler(){
		
		$('.bfunc').unbind('click', handleFuncClick);		
		$('.bfunc').bind('click', handleFuncClick);

	}

 	$(document).ready(function(){
		
		bfuncEventHandler();
		
		$('#turnon').click(function(){

			var command = $(this).data("command");

			$(this).attr('disabled', 'disabled');
			waitingDialog.show('Please wait...');

			runSmartHouseCommand(command).done(function(){
				
				console.log('command ' + command + new Date());

				$(this).removeAttr('disabled', 'disabled');
				waitingDialog.hide();

			});

		});

		$('#restartVPN').click(function(){

			restartVPN();
		});

		$('#playAlarm').click(function(){
			
			playAlarm();
		});

		$('#pandoraState').click(function(){

			pandoraState().done(function(data){
				$("#response").text(JSON.stringify(data,null,4));
			});
		});

		$('#yamahaState').click(function(){

			yamahaState().done(function(data){
				$("#response").text(JSON.stringify(data,null,4));
			});
		});

		$('#mpdState').click(function(){

			musicState().done(function(data){
				$("#response").text(JSON.stringify(data,null,4));
			});
		});

		$('#mpdSong').click(function(){

			mpdCurrentSong().done(function(data){
				$("#response").text(JSON.stringify(data,null,4));
			});
		});

		$('#btnTimerOk').click(function(){

			var value = Number($('#timerSeconds').val());
			turnOffTimer(value).done(function(data){
				$("#response").text(JSON.stringify(data,null,4));
			});

		});

		$('#btnRefresh').click(function(){
			
			refreshAll();

			if(lastState == 'Pandora'){
	        	updatePandora();
	        }
	        else if(lastState == 'Music'){
	        	updatePlayer();
	        }

		});

		$('.bfunc').attr('disabled', 'disabled');

		refreshAll();

		toastr.options = {
		  "closeButton": true,
		  "debug": false,
		  "newestOnTop": false,
		  "progressBar": false,
		  "positionClass": "toast-top-right",
		  "preventDuplicates": false,
		  "onclick": null,
		  "showDuration": "300",
		  "hideDuration": "1000",
		  "timeOut": "5000",
		  "extendedTimeOut": "1000",
		  "showEasing": "swing",
		  "hideEasing": "linear",
		  "showMethod": "fadeIn",
		  "hideMethod": "fadeOut"
		};		

	});
	
</script>

<script type="text/javascript">

function love(){

	if($('#btnThumpUp').length > 0)
        	{
        		pandoraThumbUp().done(function(thumbResult){

					if(thumbResult.ok == true)
						$('#btnThumpUp').hide();

				});
        	}        	
}

function toogleDialog(){

	if(!dialogShow)
		$('#btnMusicInfo').click();
}

$(document).keydown(function(e) { e.preventDefault(); });

</script>

</body>
</html> 