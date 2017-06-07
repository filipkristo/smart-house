
<form enctype='application/json' method="POST" action="http://10.110.166.90:8080/SaveSettings">

	<div class="panel panel-default">
		<div class="panel-heading">Yamaha</div>		
		<div class="panel-body">
			<div class="form-group">
    			<label>
			    IP Address:
				<input name="yamahaSettings[ipAddress]" type="text" class="form-control">
			    </label>    
		  	</div>  			

			<div class="form-group">
    			<label>
			    Port:
				<input name="yamahaSettings[port]" type="number" class="form-control">
			    </label>    
		  	</div>  	

			<div class="form-group">
    			<label>
			    Username:
				<input name="yamahaSettings[username]" type="text" class="form-control">
			    </label>    
		  	</div>  	

			<div class="form-group">
    			<label>
			    Password:
				<input name="yamahaSettings[password]" type="password" class="form-control">
			    </label>    
		  	</div>  	

		</div>

	</div>

	<div class="panel panel-default">
		<div class="panel-heading">Kodi</div>
		<div class="panel-body">
			<div class="form-group">
    			<label>
			    IP Address:
				<input name="kodiSettings[ipAddress]" type="text" class="form-control">
			    </label>    
		  	</div>  			

			<div class="form-group">
    			<label>
			    Port:
				<input name="kodiSettings[port]" type="number" class="form-control">
			    </label>    
		  	</div>  	

			<div class="form-group">
    			<label>
			    Username:
				<input name="kodiSettings[username]" type="text" class="form-control">
			    </label>    
		  	</div>  	

			<div class="form-group">
    			<label>
			    Password:
				<input name="kodiSettings[password]" type="password" class="form-control">
			    </label>    
		  	</div>  	
			
		</div>
	</div>

	<div class="panel panel-default">
		<div class="panel-heading">MPD</div>
		<div class="panel-body">
			<div class="form-group">
    			<label>
			    IP Address:
				<input name="mpdSettings[ipAddress]" type="text" class="form-control">
			    </label>    
		  	</div>  			

			<div class="form-group">
    			<label>
			    Port:
				<input name="mpdSettings[port]" type="number" class="form-control">
			    </label>    
		  	</div>  	
			
		</div>
	</div>

	<div class="panel panel-default">
		<div class="panel-heading">Mode</div>
		<div class="panel-body">
			<div class="form-group">
    			<label>
			    Silent:
				<input name="modeSettings[0][value]" type="number" class="form-control">
			    </label>    
			    <input name="modeSettings[0][name]" type="hidden" value="0" class="form-control">
		  	</div>  	
		  	<div class="form-group">
    			<label>
			    Normal:
				<input name="modeSettings[1][value]" type="number" class="form-control">
			    </label>    
			    <input name="modeSettings[1][name]" type="hidden" value="1" class="form-control">
		  	</div>  	
		  	<div class="form-group">
    			<label>
			    Loud:
				<input name="modeSettings[2][value]" type="number" class="form-control">
			    </label>    
			    <input name="modeSettings[2][name]" type="hidden" value="2" class="form-control">
		  	</div>  	
		  	<div class="form-group">
    			<label>
			    Extra loud:
				<input name="modeSettings[3][value]" type="number" class="form-control">
			    </label>    
			    <input name="modeSettings[3][name]" type="hidden" value="3" class="form-control">
		  	</div>  	
		</div>
	</div>

  	<button type="submit" class="btn btn-default">Submit</button>
</form>