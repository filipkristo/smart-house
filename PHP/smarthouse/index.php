<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no">
	<meta name="author" content="Filip Krišto">
  	<title>Smart House</title>

  	<link href="css/bootstrap.min.css" rel="stylesheet">	
	<link href="css/Site.css" rel="stylesheet">
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
				 			<img src = "/smarthouse/img/pandora.png" alt = "Pandora input">
						</a>
					</div>

					<div class="col-md-3 col-xs-3">
						<a href = "#" class = "bfunc thumbnail smartInput prog" data-command="Music">
				 			<img src = "/smarthouse/img/music.png" alt = "Music input">
						</a>
					</div>

					<div class="col-md-3 col-xs-3">
						<a href = "#" class = "bfunc thumbnail smartInput prog" data-command="XBox">
				 			<img src = "/smarthouse/img/xbox.png" alt = "XBox input">
						</a>
					</div>

					<div class="col-md-3 col-xs-3">
						<a href = "#" class = "bfunc thumbnail smartInput prog" data-command="TV">
				 			<img src = "/smarthouse/img/TV.png" alt = "TV input">
						</a>
					</div>

				</div>

				<br />
				<br />								

				<br />

				<div class="row" style="text-align: center;">
					<input id="volumeKnob" class="knob" data-min="-50" data-max="-10" data-step="0.5" data-val="0" data-displayPrevious=true data-fgColor="#0ce3ac" />
				</div>	

		  	</div>  

			<div role="tabpanel" class="tab-pane" id="info">
			
				<h3>Temperature </h3>
		  		<div class="row">

                        <br />

                        <div class="col-md-4 col-xs-6">                            

                            <img class="sensorIcon centerSmall" alt="Temperature" title="Temperature" src="./img/Temperature.png" />

                            <h4>                                
                                <div id="temperature" style="font-weight:bold;">
                                    --- &deg;C 
                                </div>                                
                            </h4>                                                                           

                        </div>

                        <div class="col-md-4 col-xs-6">

                            <img class="sensorIcon centerSmall" alt="Humidity" title="Humidity" src="./img/Humidity.png" />

                            <h4>                                
                                <div id="humidity" style="font-weight:bold;">
                                    --- %
                                </div>
                            </h4>                            
                           
                        </div>


                        <div class="col-md-4 col-xs-12">

                            <img class="sensorIcon centerSmall" alt="Last updated" title="Clock" src="./img/Clock.png" />

                            <h4>                                
                                <div id="updated" style="font-weight:bold;">
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
        <?php include 'infoDialog.php';?>
</div> 
 
<script src="js/jquery-2.1.3.min.js"></script>
<script src="js/bootstrap.min.js"></script>
<script src="js/jquery.knob.min.js"></script>
<script src="js/script.js"></script>
<script src="js/dialog.js"></script>

<script>

	var smarthouseState;
	var pandoraState;
	var yamahaState;

 	$(document).ready(function(){
		
		//http://anthonyterrien.com/demo/knob/
		//http://bootswatch.com/darkly/#buttons
		$('#volumeKnob').knob({

			change : function (value) {               
                console.log("change : " + value);
                setVolume(value);
            },
            release : function (value) {                
                console.log("release : " + value);
            },
            cancel : function () {
                console.log("cancel : ", this);
            },
            format : function (value) {
                 return value + '';
                 },
             draw : function () {

                    // "tron" case
                    if(this.$.data('skin') == 'tron') {

                        this.cursorExt = 0.3;

                        var a = this.arc(this.cv)  // Arc
                                , pa                   // Previous arc
                                , r = 1;

                        this.g.lineWidth = this.lineWidth;

                        if (this.o.displayPrevious) {
                            pa = this.arc(this.v);
                            this.g.beginPath();
                            this.g.strokeStyle = this.pColor;
                            this.g.arc(this.xy, this.xy, this.radius - this.lineWidth, pa.s, pa.e, pa.d);
                            this.g.stroke();
                        }

                        this.g.beginPath();
                        this.g.strokeStyle = r ? this.o.fgColor : this.fgColor ;
                        this.g.arc(this.xy, this.xy, this.radius - this.lineWidth, a.s, a.e, a.d);
                        this.g.stroke();

                        this.g.lineWidth = 2;
                        this.g.beginPath();
                        this.g.strokeStyle = this.o.fgColor;
                        this.g.arc( this.xy, this.xy, this.radius - this.lineWidth + 1 + this.lineWidth * 2 / 3, 0, 2 * Math.PI, false);
                        this.g.stroke();

                        return false;
                    }
                }
		});


		$('.bfunc').click(function() {
			
			var button = $(this);
			var command = $(this).data("command");

			button.attr('disabled', 'disabled');

			if(button.hasClass('prog'))
				waitingDialog.show('Please wait...');

			runSmartHouseCommand(command).done(function(){
				
				button.removeAttr('disabled', 'disabled');
				
				if(button.hasClass('prog'))
					waitingDialog.hide();

				yamahaState();
				smartHouseState();
				pandoraState();
				
			});
			
		});
		
		$('#turnon').click(function(){

			var command = $(this).data("command");

			$(this).attr('disabled', 'disabled');
			waitingDialog.show('Please wait...');

			runSmartHouseCommand(command).done(function(){
				
				$(this).removeAttr('disabled', 'disabled');
				waitingDialog.hide();

				yamahaState();
				smartHouseState();
				pandoraState();

			});

		});

		$('#restartVPN').click(function(){

			restartVPN();
		});

		$('#playAlarm').click(function(){
			
			playAlarm();
		});

		$('#pandoraState').click(function(){

			pandoraState();
		});

		$('#yamahaState').click(function(){

			yamahaState();
		});

		$('#mpdState').click(function(){

			musicState();
		});

		$('#mpdSong').click(function(){

			mpdCurrentSong();
		});

		$('#btnTimerOk').click(function(){

			var value = Number($('#timerSeconds').val());
			turnOffTimer(value);

		});

		$('#btnMusicInfo').click(function(){

			smartHouseState().done(function(result){

				var state = result;	

				$('#loved').hide();
				$('#imagePic').hide();
				$('#extrabutton').empty();

				if(state == 'Music'){					
					musicState().done(function(mpdResult){

						mpdCurrentSong().done(function(mpdSong){

							$('#currentArtist').html('Artist: &nbsp&nbsp&nbsp&nbsp&nbsp <strong>' + mpdSong.artist + '</strong>');
							$('#currentAlbum').html('Album: &nbsp&nbsp&nbsp&nbsp <strong>' + mpdSong.album + '</strong>');
							$('#currentRadio').html('Genre: &nbsp&nbsp&nbsp&nbsp&nbsp <strong>' + mpdSong.genre + '</strong>');
							$('#currentSong').html('Song: &nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp <strong>' + mpdSong.title + '</strong>');

							$('#extrabutton').append('<div class="row"><label>Position:  ' + mpdSong.track + '/ ' + mpdResult.playlistLength +' </label></div>');

							$('#infoModalLabel').text(state);
							$('#info-dialog').modal();

						});

					});
				}	

				else if(state == 'Pandora'){

					pandoraState().done(function(result){

						$('#currentArtist').html('Artist: &nbsp&nbsp&nbsp&nbsp&nbsp <strong>' + result.artist + '</strong>');
						$('#currentAlbum').html('Album: &nbsp&nbsp&nbsp&nbsp <strong>' + result.album + '</strong>');
						$('#currentRadio').html('Radio: &nbsp&nbsp&nbsp&nbsp&nbsp <strong>' + result.radio + '</strong>');
						$('#currentSong').html('Song: &nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp <strong>' + result.song + '</strong>');

						$('#imagePic').show();
						$('#imagePic').attr('src', result.albumUri);

						if(result.loved == true){

							$('#loved').show();

						}
						else{

							$('#extrabutton').append('<br/><div class="row"><button title="Thumb up" class="btn btn-default button-down" id="btnThumpUp"><span class="glyphicon glyphicon-thumbs-up"</span></button></div>');

							$('#btnThumpUp').click(function(){

								pandoraThumbUp().done(function(thumbResult){

									if(thumbResult.ok == true)
										$('#extrabutton').empty();

								});

							});
						}

						$('#infoModalLabel').text(state);
						$('#info-dialog').modal();

					});

				}			

			});

		});

		$('.bfunc').attr('disabled', 'disabled');
		disableKnob();

		refreshAll();
		setTemperatureTimer();

	});
	
</script>

</body>
</html> 