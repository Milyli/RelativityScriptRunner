
    function error(responseData) {
        var message = responseData.responseJSON && responseData.responseJSON.Message ?
            responseData.responseJSON.Message
            :
            responseData.responseText;
        alert(message);
    };

    function toTimeString (timeString) {
        if (timeString) {
            var date = new Date(timeString.replace('T', ' '));
            if (!isNaN(date.getDay())) {
                return date.toLocaleDateString() + ' ' + date.toLocaleTimeString();
            }
        }
        return "None";
    }
