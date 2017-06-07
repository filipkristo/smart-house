
function loadSettings(){

	var uri = "http://10.110.166.90:8081/GetSettings";

	return $.ajax({
		method: "GET",
		url: uri,
		cache: false,
		}).done(function(data) {

			$('[name="yamahaSettings[ipAddress]"]').val(data.yamahaSettings.ipAddress);
			$('[name="yamahaSettings[port]"]').val(data.yamahaSettings.port);
			$('[name="yamahaSettings[username]"]').val(data.yamahaSettings.username);
			$('[name="yamahaSettings[password]"]').val(data.yamahaSettings.password);

			$('[name="kodiSettings[ipAddress]"]').val(data.kodiSettings.ipAddress);
			$('[name="kodiSettings[port]"]').val(data.kodiSettings.port);
			$('[name="kodiSettings[username]"]').val(data.kodiSettings.username);
			$('[name="kodiSettings[password]"]').val(data.kodiSettings.password);

			$('[name="mpdSettings[ipAddress]"]').val(data.mpdSettings.ipAddress);
			$('[name="mpdSettings[port]"]').val(data.mpdSettings.port);

			$('[name="modeSettings[0][value]"]').val(data.modeSettings[0].value);
			$('[name="modeSettings[1][value]"]').val(data.modeSettings[1].value);
			$('[name="modeSettings[2][value]"]').val(data.modeSettings[2].value);
			$('[name="modeSettings[3][value]"]').val(data.modeSettings[3].value);
		});

}