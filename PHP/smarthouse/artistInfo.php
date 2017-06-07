<?php 
	
	$artist = $_REQUEST["artist"];	
	$similarArtist = isset($_REQUEST["similarArtist"]);	

	if($similarArtist == true)
	{
		$json = file_get_contents("http://10.110.166.90:8081/api/LastFM/GetSimilarArtist?artist=" . urlencode($artist) . "&limit=100");
		$similarObject = json_decode($json);

		echo '<div class="row">';	
			foreach ($similarObject as $value)
			{
				echo '<div class="col-md-3 col-xs-6">';	
					echo '<h5>' . $value->name . '</h5>';;
					echo '<a href=' . $value->url . ' target="_blank">';
						echo '<img src=' . $value->mainImage[2] . ' class="img-thumbnail"</img>';
					echo '</a>';
				echo '</div>';
			}
		echo '</div>';

		die();		
	}
?>
<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no">
	<meta name="author" content="Filip Krišto">
  	<title>Artist info</title>

  	<link href="css/bootstrap.min.css" rel="stylesheet">	
	<link href="css/Site.css" rel="stylesheet">
	<link href="css/toastr.min.css" rel="stylesheet" type="text/css" />
	
	<link rel="icon" href="favicon.ico">

</head>
<body>

<br />						

<div class="container body-content">	
		
		
		<div class="">
		  <ul class="nav nav-pills" role="tablist" id="myPill">
		    <li class="active"><a href="#info" aria-controls="home" role="tab" data-toggle="tab">Info</a></li>
		    <li><a href="#similarArtist" aria-controls="home" role="tab" data-toggle="tab">Similar artist</a></li>		   
		  </ul>
		</div>	

		<br />

		<div class="tab-content">

		  	<div role="tabpanel" class="tab-pane active" id="info">

				<?php 

					$artist = $_REQUEST["artist"];
					$album = $_REQUEST["album"];
					$song = $_REQUEST["song"];

					echo '<input type="hidden" id="artist" value="' . $artist . '" />';
					echo '<input type="hidden" id="album" value="' . $album . '" />';
					echo '<input type="hidden" id="song" value="' . $song . '" />';

					$json = file_get_contents("http://10.110.166.90:8081/api/LastFM/GetArtistInfo?artist=" . urlencode($artist));
					$artistObject = json_decode($json);

					echo '<div style="text-align:center;">';	
						echo '<h1>';
						echo '<a href=' . $artistObject->url . ' target="_blank">';
						echo $artistObject->name;
						echo '</a>';
						echo '</h1>';
					echo '</div>';				

					echo '<hr/>';

					echo '<div class="row">';	
						echo '<div class="col-md-12">';	

							echo '<div class="col-md-6">';	
								echo '<a href=' . $artistObject->url . ' target="_blank">';
									echo '<img src=' . $artistObject->mainImage[3] . '</img>';			
								echo '</a>';				
							echo '</div>';

							echo '<div class="col-md-6">';	
								echo '<div class="row">';	
									echo $artistObject->bio->summary;
								echo '</div>';

								echo '<br />';

								echo '<div class="row">';	
								foreach ($artistObject->tags as $value)
								{
									echo '<a href=' . $value->url . ' target="_blank">' . $value->name . '</a>';
									echo '<br />';
								}
								echo '</div>';
								
								echo '<br />';
								echo '<div class="row">';
									echo 'Listeners: ' . number_format($artistObject->stats->listeners, 0);
								echo '</div>';

							echo '</div>';						
						echo '</div>';
					echo '</div>';

					echo '<br />';
					echo '<h4>Similar artist:</h4>';
					echo '<hr />';

					echo '<div class="row">';	
						foreach ($artistObject->similar as $value)
						{
							echo '<div class="col-md-4 col-xs-6">';	
								echo '<h4>' . $value->name . '</h4>';;
								echo '<a href=' . $value->url . '>';
									echo '<img src=' . $value->mainImage[2] . '</img>';
								echo '</a>';
							echo '</div>';
						}
					echo '</div>';

					echo '<hr />';

					$json = file_get_contents("http://10.110.166.90:8081/api/LastFM/GetAlbumInfo?artist=" . urlencode($artist) . "&album=" . urlencode($album));
					$albumObject = json_decode($json);

					echo '<div style="text-align:center;">';	
					echo '<h1>';
					echo '<a href=' . $albumObject->url . ' target="_blank">';
					echo $albumObject->name;
					echo '</a>';
					echo '</h1>';
					echo '</div>';								

					echo '<div class="row">';	
						echo '<div class="col-md-12">';	
							echo '<div class="col-md-6">';	
								echo '<a href=' . $albumObject->url . ' target="_blank">';
									echo '<img src=' . $albumObject->images[3] . ' class="img-thumbnail"</img>';			
								echo '</a>';				
							echo '</div>';

							echo '<div class="col-md-6">';	
				
								echo '<div class="row">';	
								foreach ($albumObject->tracks as $value)
								{
									echo '<a href=' . $value->url . '>' . $value->name . '</a>';
									echo '<br />';
								}
								echo '</div>';
								
								echo '<br />';
								echo '<div class="row">';
									echo 'Listeners: ' . number_format($albumObject->listenerCount, 0);
								echo '</div>';
								echo '<div class="row">';
									echo 'Play count: ' . number_format($albumObject->playCount, 0);
								echo '</div>';

							echo '</div>';						
						echo '</div>';				
					echo '</div>';				

				?>

			</div>  

		  	<div role="tabpanel" class="tab-pane" id="similarArtist">
				<div id="similarContent">
				</div>
			</div>		  				

		</div>	

        <?php include 'footer.php';?>
   
</div> 


<script type="text/javascript" src="js/jquery.min.js"></script>
<script type="text/javascript" src="js/bootstrap.min.js"></script>
<script type="text/javascript" src="js/jquery.knob.min.js"></script>
<script type="text/javascript" src="js/util.js"></script>
<script type="text/javascript" src="js/dialog.js"></script>
<script type="text/javascript" src="js/winapi.js"></script>
<script type="text/javascript" src="js/jquery.signalR-2.2.1.min.js"></script>
<script src='http://10.110.166.90:8081/signalr/js'></script>
<script type="text/javascript" src="js/hub.js"></script>
<script type="text/javascript" src="js/toastr.min.js"></script>
<script type="text/javascript">for (var i = 0; i < document.links.length; i++) { document.links[i].onclick = function() { window.external.notify('LaunchLink:' + this.href); return false; } }</script>
<script type="text/javascript">

	$(function () {

		var uri = "artistInfo.php?artist=" + encodeURI($('#artist').val()) + "&album=" + encodeURI($('#album').val()) + "&similarArtist=true";

		$.ajax({
		method: "GET",
		url: uri,		
		}).done(function(data){
			
			$('#similarContent').empty();
			$('#similarContent').html(data);
			
		});

	});

</script>

</body>
</html> 