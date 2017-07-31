

    function error(responseData) {
        var message = responseData.responseJSON && responseData.responseJSON.Message ?
            responseData.responseJSON.Message
            :
            responseData.responseText;
        alert(message);
    };

		function toTimeString(timeString) {
			if (timeString) {
				// This looks goofy, but essentially it creates a new moment (date)
				// and then resets the timezone to server local time by reading the string again
				var date = moment(timeString).zone(timeString);
				if (date.isValid()) {
					return date.format("M/D/YYYY h:mm:ss A");
            }
        }
        return "None";
    }
