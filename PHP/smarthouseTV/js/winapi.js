
function showToast(message) {
        
    if(typeof Windows != 'undefined') {
        //Error detection
        // var text = document.createTextNode("Calling the notifications")
        // document.body.appendChild(text);
        // Log to the console
        var notifications = Windows.UI.Notifications;
        //Get the XML template where the notification content will be suplied
        var template = notifications.ToastTemplateType.toastImageAndText01;
        var toastXml = notifications.ToastNotificationManager.getTemplateContent(template);
        //Supply the text to the XML content
        var toastTextElements = toastXml.getElementsByTagName("text");
        toastTextElements[0].appendChild(toastXml.createTextNode(message));
        //Supply an image for the notification
        var toastImageElements = toastXml.getElementsByTagName("image");
        //Set the image this could be the background of the note, get the image from the web
        toastImageElements[0].setAttribute("src", "https://raw.githubusercontent.com/seksenov/grouppost/master/images/logo.png");
        toastImageElements[0].setAttribute("alt", "red graphic");
        //Specify a long duration
        var toastNode = toastXml.selectSingleNode("/toast");
        toastNode.setAttribute("duration", "long");
        //Specify the audio for the toast notification
        var toastNode = toastXml.selectSingleNode("/toast");                        
        var audio = toastXml.createElement("audio");
        audio.setAttribute("src", "ms-winsoundevent:Notification.IM");
        //Specify launch paramater
        toastXml.selectSingleNode("/toast").setAttribute("launch", '');
        //Create a toast notification based on the specified XML
        var toast = new notifications.ToastNotification(toastXml);
        //Send the toast notification
        var toastNotifier = notifications.ToastNotificationManager.createToastNotifier();
        toastNotifier.show(toast);
    
    } else {
        //TODO: Fallback to website functionality
        console.log("ERROR: No Windows namespace was detected");
    }
};


function showAlert(info) {

    if (typeof Windows == 'undefined')
        alert(info);
    else {
        var msg = new Windows.UI.Popups.MessageDialog(info);

        msg.commands.append(new Windows.UI.Popups.UICommand(
        "U redu",
        function () {
        }));

        msg.defaultCommandIndex = 0;
        msg.showAsync();
    }
};

function createNewTile() {

    var funkcija = function () {

        var appbarTileId = getPageName();
        var newTileDisplayName = document.title;
        var TileActivationArguments = document.location.href;
        var square150x150Logo = new Windows.Foundation.Uri("ms-appx:///Images/Square150x150Logo.png");
        var newTileDesiredSize = Windows.UI.StartScreen.TileSize.square150x150;

        var tile = new Windows.UI.StartScreen.SecondaryTile(appbarTileId,
                                                        newTileDisplayName,
                                                        TileActivationArguments,
                                                        square150x150Logo,
                                                        newTileDesiredSize);

        tile.visualElements.showNameOnSquare150x150Logo = true;
        tile.visualElements.foregroundText = Windows.UI.StartScreen.ForegroundText.light;

        return new WinJS.Promise(function (complete, error, progress) {
            tile.requestCreateForSelectionAsync({ x: 300, y: 300, width: 600, height: 600 }, Windows.UI.Popups.Placement.above).done(function (isCreated) {
                if (isCreated) {
                    showAlert('Uspješno postavljeno.');
                } else {
                    showAlert('Trenutno nismo u mogućnosti postaviti prečac. Možda već postoji');
                }
            });
        });

    };

    var msg = new Windows.UI.Popups.MessageDialog(
                "Jeste li sigurni da želite postavit prečac na start?");

    // Add commands and set their command handlers
    msg.commands.append(new Windows.UI.Popups.UICommand(
        "Da",
        function (command) {
            funkcija();
        }));
    msg.commands.append(
        new Windows.UI.Popups.UICommand("Ne", function () { }));

    msg.defaultCommandIndex = 0;
    msg.cancelCommandIndex = 1;

    msg.showAsync();
};

function cameraCapture(divID) {

    if (typeof Windows != 'undefined') {

        var Exists = Windows.Foundation.Metadata.ApiInformation.isTypePresent('Windows.Media.Capture.CameraCaptureUI');
        if (!Exists)
            return;

        //Initialize windows media camera capture
        var captureUI = new Windows.Media.Capture.CameraCaptureUI();
        //Set the format of the picture that's going to be captured (.png or .jpg)
        captureUI.photoSettings.format = Windows.Media.Capture.CameraCaptureUIPhotoFormat.png;

        //Pop up the camera UI to take a picture
        captureUI.captureFileAsync(Windows.Media.Capture.CameraCaptureUIMode.photo).then(function (capturedItem) {
            if (capturedItem) {
                // This is where the picture can be taken and shown on the webpage if we want to add this functionality

                 var reader = new window.FileReader();
                 reader.readAsDataURL(capturedItem); 
                 reader.onloadend = function() {
                   var base64pic = reader.result;                
                   //Resize the base64 image
                   var photo = document.createElement("img");
                   photo.id = 'imgCapture';                   
                   photo.setAttribute('src', base64pic);
                   photo.setAttribute('class', 'img-responsive');

                   var div = document.getElementById(divID);
                   div.appendChild(photo);
                   //var resizedImage = imageToDataUri(photo, 300, 300);
                   ////Set the picture as the background of the div
                   //$("#"+divID).css("background-image", "url(" + resizedImage + ")");
                   ////Store the image in the DB
                   //storeImage(divID, resizedImage);
                 }
            }
            else {
                //Taking a picture has failed
                console.log("Taking the picture with WinRT failed");
            }
        });
    } else {
        //TODO: Fallback to website functionality
        console.log("ERROR: No Windows namespace was detected");
    }
};

function isMicrosoftApi() {

    try {

        if (typeof Windows == 'undefined')
            return false;
        else
            return true;

    } catch (e) {
        return false;
    }    

}

function addAppointment(date, Subject, Description) {
    // Check to see if the Windows namespace is defined
    if (typeof Windows != 'undefined') {

        var Exists = Windows.Foundation.Metadata.ApiInformation.isTypePresent('Windows.ApplicationModel.Appointments.Appointment');
        if (!Exists)
            return;

        var appointment = new Windows.ApplicationModel.Appointments.Appointment();        

        appointment.allDay = true;
        appointment.startTime = date;
        appointment.subject = Subject;                
        appointment.details = Description;
        appointment.location = 'Vinodolska 52, 21000 Split';

        Windows.ApplicationModel.Appointments.AppointmentManager.showAddAppointmentAsync(appointment, { x: 300, y: 0, width: 600, height: 100 })
        .done(function (appointmentId) {
                if (appointmentId) {
                    showAlert('Uspješno dodano u kalendar')
                } else {
                    console.log('Greška');
                }
            });
    }
}

function getDeviceId() {
    
    var Id = localStorage.getItem('DeviceId');

    if(Id == null){

        Id = guid();
        localStorage.setItem('DeviceId', Id);
    }

    return Id;
}