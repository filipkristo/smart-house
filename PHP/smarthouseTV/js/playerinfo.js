
var timer = null;

function initializeInfoControl(){

	$('#info-player').empty();

	if(timer != null){
		clearInterval(timer);
		timer = null;
	}

	if(lastState == 'Music'){	

		$.ajax({
			method: "GET",
			url: 'mpd.php',							
		}).done(function(data) {
		
			$('#info-player').append(data);

			updatePlayer();
			timer = setInterval(updatePlayer, 2000); 
		});			

	}	
	else if(lastState == 'Pandora'){

		$.ajax({
			method: "GET",
			url: 'pandora.php',							
		}).done(function(data) {
		
			$('#info-player').append(data);

			updatePandora();
			//timer = setInterval(updatePandora, 10000); 

			yamahaState();
		});			

	}				

}

function updatePandora(){

	pandoraState().done(function(result){

		updatePandoraValues(result);

	});

}

function updatePandoraValues(result){

	$('#currentArtist').html(result.artist);
	$('#currentAlbum').html(result.album);
	$('#currentRadio').html(result.radio);
	$('#currentSong').html(result.song);

	$('#imagePic').show();
	$('#imagePic').attr('src', result.albumUri);

	if(result.loved == true){

		$('#loved').show();
		$('#btnThumpUp').hide();

	}
	else{

		$('#loved').hide();
		$('#btnThumpUp').show();
	}

}

function updatePlayer(){

	musicState().done(function(mpdResult){

		mpdCurrentSong().done(function(mpdSong){

			var elapsed = (Number(mpdResult.timeElapsed) / 60).toFixed(2);
			var total = (Number(mpdResult.timeTotal) / 60).toFixed(2);
			var perc = 100 / (mpdResult.timeTotal / mpdResult.timeElapsed);

			console.log('Duration ' + elapsed + '/ ' + total)

			$('#currentArtist').text(mpdSong.artist);
			$('#currentAlbum').text(mpdSong.album);
			$('#currentGenre').text(mpdSong.genre);
			$('#currentSong').text(mpdSong.title);

			$('#lblDuration').html('Duration: ' + elapsed + '/ ' + total);
			$('#lblPosition').html('Position: ' + (Number(mpdSong.pos) + 1) + '/ ' + mpdResult.playlistLength);

			$('#prgDuration').attr('aria-valuemin', 0);
			$('#prgDuration').attr('aria-valuemax', mpdResult.timeTotal);
			$('#prgDuration').attr('aria-valuenow', mpdResult.timeElapsed);
			$('#prgDuration').css('width', perc+'%');				

		});

	});

}