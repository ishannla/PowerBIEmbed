(function () {
    "use strict";

	Office.initialize = function (reason) {
		
		$(document).ready(function () {

			if (Office.context.document.settings.get("reportLoaded")) {
				switchLocation("Report.html")
			} else if (localStorage.getItem("accessToken") != null) {
				switchLocation("Navigation.html")
			} else {
				$('#logIn').click(logIn)
			}
		});
	};

	function logIn() {
		$.ajax({
			url: "/api/authentication/getaccesstoken/generate_new_token", success: function (result) {

				if (result != "user_cancelled_login") {
					localStorage.setItem("accessToken", result)
					switchLocation("Navigation.html")
				}
			}, error: function (xhr, status, error) {
				console.log(error)
			}
		});
	}

	function switchLocation(destination) {
		// sample window.location.origin - "https://localhost:44359"
		// sample window.location.pathname - "/Templates/Navigation.html"
		// sample window.location.search - "?_host_Info=Powerpoint$Win32$16.01$en-US"

		window.location = window.location.origin + "/Templates/" + destination + window.location.search
	}

})();