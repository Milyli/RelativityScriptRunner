﻿
    function error(responseData) {
        var message = responseData.responseJSON && responseData.responseJSON.Message ?
            responseData.responseJSON.Message
            :
            responseData.responseText;
        alert(message);
    };

    function toTimeString (timeString) {
    	if (timeString) {
				// it's really UTC
		    timeString = timeString + 'Z';
        	var date = new Date(timeString);
            if (!isNaN(date.getDay())) {
                return date.toLocaleDateString() + ' ' + date.toLocaleTimeString();
            }
        }
        return "None";
    }
