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

                        <div class="col-md-3 col-xs-6">                            

                            <img class="sensorIcon centerSmall" alt="Temperature" title="Temperature" src="./img/Temperature.png" />

                            <h4>                                
                                <div id="temperature" class="temperature" style="font-weight:bold;">
                                    --- &deg;C 
                                </div>                                
                            </h4>                                                                           

                        </div>

                        <div class="col-md-3 col-xs-6">

                            <img class="sensorIcon centerSmall" alt="Humidity" title="Humidity" src="./img/Humidity.png" />

                            <h4>                                
                                <div id="humidity" class="humidity" style="font-weight:bold;">
                                    --- %
                                </div>
                            </h4>                            
                           
                        </div>

                    	<div class="col-md-3 col-xs-6">

                            <img class="sensorIcon centerSmall" alt="Smoke" title="Somke" src="./img/Smoke.png" />

                            <h4>                                
                                <div id="smoke" class="smoke" style="font-weight:bold;">
                                    --- 
                                </div>
                            </h4>                            
                           
                        </div>

                        <div class="col-md-3 col-xs-6">

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
		              <a id="turnOnAC" class="list-group-item" href="#">Turn on air condition</a>
		              <a id="turnOffAC" class="list-group-item" href="#">Turn off air condition</a>
		              <a id="pandoraState" class="list-group-item" href="#">Pandora state</a>
		              <a id="yamahaState" class="list-group-item" href="#">Yamaha state</a>
		              <a id="mpdState" class="list-group-item" href="#">MPD state</a>
		              <a id="mpdSong" class="list-group-item" href="#">MPD song</a>
		              <a id="turnOffTimer" data-target="#timer-dialog" data-toggle="modal" class="list-group-item" href="#">Turn off timer</a>
		              <a id="downloadLog" class="list-group-item" href="#">Download log</a>
		              <a id="startPandora" class="list-group-item" href="#">Start pandora</a>
		              <a id="stopPandora" class="list-group-item" href="#">Stop pandora</a>
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
			  		<?php include 'settings.php';?>
			</div>

		<?php include 'player.php';?>
        <?php include 'footer.php';?>
        <?php include 'timerDialog.php';?>
        <?php include 'infoDialog.php';?>
        <?php include 'lyricsDialog.php';?>        
</div> 
 
<script type="text/javascript" src="js/jquery.min.js"></script>
<script type="text/javascript" src="js/bootstrap.min.js"></script>
<script type="text/javascript" src="js/jquery.knob.min.js"></script>
<script type="text/javascript" src="js/util.js"></script>
<script type="text/javascript" src="js/script.js"></script>
<script type="text/javascript" src="js/settings.js"></script>
<script type="text/javascript" src="js/dialog.js"></script>
<script type="text/javascript" src="js/winapi.js"></script>
<script type="text/javascript" src="js/jquery.signalR-2.2.1.min.js"></script>
<script src='http://10.110.166.90:8081/signalr/js'></script>
<script type="text/javascript" src="js/hub.js"></script>
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

	function volumeKnowInitialization(){

		$('#volumeKnob').knob({

			change : function (value) {               
                console.log("change : " + value);
            },
            release : function (value) {                
                console.log("release : " + value);
				setVolume(value);
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

	}

 	$(document).ready(function(){
		
		bfuncEventHandler();
		volumeKnowInitialization();		
		
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

		$('#startPandora').click(function(){

			waitingDialog.show('Please wait...');

			startPandora().done(function(data){
				waitingDialog.hide();
			});

		});

		$('#stopPandora').click(function(){
			
			stopPandora();

		});

		$('#turnOnAC').click(function(){
			TurnOnAC().done(function(data){
				$("#response").text(JSON.stringify(data,null,4));
			});
		});		

		$('#turnOffAC').click(function(){
			TurnOffAC().done(function(data){
				$("#response").text(JSON.stringify(data,null,4));
			});
		});		

		$('#downloadLog').click(function(){

			var uri = baseUrl + 'Util/DownloadLog';

			if(isMicrosoftApi())
				downloadLogWin(uri);
			else
				window.location.href = uri;

		});		

		$('#btnMusicInfo').click(function(){

			smartHouseState().done(function(result){
				
				var state = result;	

				$('#loved').hide();
				$('#imagePic').hide();
				$('#extrabutton').empty();

				if(state == 'Music'){	

					var delegateMusic = function(){

						musicState().done(function(mpdResult){

							mpdCurrentSong().done(function(mpdSong){

								var elapsed = (Number(mpdResult.timeElapsed) / 60).toFixed(2);
								var total = (Number(mpdResult.timeTotal) / 60).toFixed(2);

								console.log('Duration ' + elapsed + '/ ' + total)

								$('#currentArtist').html('Artist: &nbsp&nbsp&nbsp&nbsp&nbsp <strong>' + mpdSong.artist + '</strong>');
								$('#currentAlbum').html('Album: &nbsp&nbsp&nbsp&nbsp <strong>' + mpdSong.album + '</strong>');
								$('#currentRadio').html('Genre: &nbsp&nbsp&nbsp&nbsp&nbsp <strong>' + mpdSong.genre + '</strong>');
								$('#currentSong').html('Song: &nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp <strong>' + mpdSong.title + '</strong>');
								
								lastArtist = mpdSong.artist;
	        					lastSong = mpdSong.title;
								lastAlbum = mpdSong.album;

								if($('#lblDuration').length == 0){

									$('#extrabutton').append('<div class="row"><div class="progress"><div role="progressbar" class="progress-bar progress-bar-success"></div></div></div>')
									$('#extrabutton').append('<div class="row"><label id="lblDuration">Duration ' + elapsed + '/ ' + total +' </label></div>');
									$('#extrabutton').append('<div class="row"><label id="lblPosition">Position: ' + (Number(mpdSong.pos) + 1) + '/ ' + mpdResult.playlistLength +' </label></div>');		
									$('#extrabutton').append('<div class="row" id="extraRow"></div>');		

									$.ajax({
										method: "GET",
										url: 'buttonplay.php',							
									}).done(function(data) {
									
										$('#extraRow').append(data);
										bfuncEventHandler();


									});					
								}							
								else{
									$('#lblDuration').html('Duration ' + elapsed + '/ ' + total);
									$('#lblPosition').html('Position: ' + (Number(mpdSong.pos) + 1) + '/ ' + mpdResult.playlistLength);

									var perc = 100 / (mpdResult.timeTotal / mpdResult.timeElapsed);

									$('.progress-bar').attr('aria-valuemin', 0);
									$('.progress-bar').attr('aria-valuemax', mpdResult.timeTotal);
									$('.progress-bar').attr('aria-valuenow', mpdResult.timeElapsed);
									$('.progress-bar').css('width', perc+'%');									
								}

								if(!$('#infoModalLabel').is(':visible') && !dialogShow){

									console.log('show dialog');
									
									dialogShow = true;
									$('#infoModalLabel').text(state);
									$('#info-dialog').modal();
								}

							});

						});

					};

					delegateMusic();
					timer = setInterval(delegateMusic, 4000); 
				}	

				else if(state == 'Pandora'){

					var delegateMusic = function(){					

					pandoraState().done(function(result){

						$('#currentArtist').html('Artist: &nbsp&nbsp&nbsp&nbsp&nbsp <strong>' + result.artist + '</strong>');
						$('#currentAlbum').html('Album: &nbsp&nbsp&nbsp&nbsp <strong>' + result.album + '</strong>');
						$('#currentRadio').html('Radio: &nbsp&nbsp&nbsp&nbsp&nbsp <strong>' + result.radio + '</strong>');
						$('#currentSong').html('Song: &nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp <strong>' + result.song + '</strong>');

						$('#imagePic').show();
						$('#imagePic').attr('src', result.albumUri);
						
						lastArtist = result.artist;
						lastSong = result.song;
						lastAlbum = result.album;

						if($('#buttonplay').length == 0){								

								$.ajax({
									method: "GET",
									url: 'buttonplay.php',							
								}).done(function(html) {
																		
									$('#extrabutton').addClass('row');
									$('#extrabutton').append(html);

									bfuncEventHandler();

									$('#extrabutton').append('<div class="btn-group" id="extraRow"><button title="Thumb up" class="btn btn-default button-down" id="btnThumpUp"><span class="glyphicon glyphicon-thumbs-up"</span></button></div>');							

									$('#btnThumpUp').click(function(){

										var btnThumpUp = this;
										$(btnThumpUp).attr('disabled', 'disabled');

										pandoraThumbUp().done(function(thumbResult){

											if(thumbResult.ok == true)
												$(btnThumpUp).hide();

										}).fail(function(){

											$(btnThumpUp).removeAttr('disabled', 'disabled');

										});

									});

									$('#extrabutton').append('<div class="btn-group" id="extraRow2"><button title="Refresh" class="btn btn-default button-down" id="btnRefreshPandora"><span class="glyphicon glyphicon-refresh"</span></button></div>');							

									$('#btnRefreshPandora').click(function(){

										console.log('Refresh pandora');
										delegateMusic();

									});

									if(result.loved == true){

										$('#loved').show();
										$('#btnThumpUp').hide();

									}
									else{

										$('#loved').hide();
										$('#btnThumpUp').show();
									}

								});
								
						}						
						else{
							
							if(result.loved == true){

								$('#loved').show();
								$('#btnThumpUp').hide();

							}
							else{

								$('#loved').hide();
								$('#btnThumpUp').show();
							}

						}	

						if(!$('#infoModalLabel').is(':visible') && !dialogShow){

							console.log('show dialog');
							
							dialogShow = true;
							$('#infoModalLabel').text(state);
							$('#info-dialog').modal();
						}


						});

					};

					delegateMusic();										

				}			

			});

		});

		$('.bfunc').attr('disabled', 'disabled');
		disableKnob();

		refreshAll();

		loadSettings();

		$('#info-dialog').on('hidden.bs.modal', function () {

    		console.log('dialog closed ' + new Date());

    		clearInterval(timer);

      		timer = null;
      		dialogShow = false;
      		$('#extrabutton').empty();
		});	

		$('#btnLyrics').click(function(){

			if(lastSong == '' || lastArtist == '')
			{
				getNowPlaying().done(function(data){

					lastArtist = data.artist;
					lastSong = data.song;
					
					$('#lyricsModalText').val(lastArtist + ' - ' + lastSong);
					$('#lyricsModalLabel').text(lastArtist + ' - ' + lastSong);
					$('#lyrics-dialog').modal();

					showLyrics(lastArtist, lastSong);
				});
			}
			else
			{
				$('#lyricsModalText').val(lastArtist + ' - ' + lastSong);
				$('#lyricsModalLabel').text(lastArtist + ' - ' + lastSong);
				$('#lyrics-dialog').modal();

				showLyrics(lastArtist, lastSong);
			}			

		});

		$('#lyrics-dialog').on('hidden.bs.modal', function () {

    		$('#lyricsBody').empty();
			$('#lyricsBody').html('Please wait....');

		});	

		$('#btnArtist').click(function(){

			if(lastArtist == '' || lastAlbum == '' || lastSong == '')
			{
				showAlert('Need more data');
				return;
			}

			window.location.href = 'artistInfo.php?artist=' + lastArtist  + '&album=' + lastAlbum + '&song=' + lastSong;

		});

		$('#btnEditLyrics').click(function(){

			if($('#lyricsModalText').is(':visible')){				

				$('#lyricsModalText').hide();
				$("#foo").unbind("change");

				showLyrics(lastArtist, lastSong);
			}
			else{

				$('#lyricsModalText').show();

				$('#lyricsModalText').change(function(){

					var value = $(this).val();
					var values = value.split('-');

					if(values.length < 2)
						return;
					
					showLyrics(values[0], values[1]);

				});				

			}			

		});		

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

function toggleDialog(){

	if(!dialogShow)
		$('#btnMusicInfo').click();
}


</script>

</body>
</html> 