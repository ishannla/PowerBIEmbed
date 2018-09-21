(function () {
	"use strict";

	Office.initialize = function (reason) {

		// can assume that at this point, accessToken stored in localStorage cache and in Office Authentication cache
		$(document).ready(function () {

			$('.tabs').tabs()
			$('#logOut1').click(logOut)
			$('#logOut2').click(logOut)
			$('#backButton').click(returnToWorkspacesList)
			$('#backButton').hide();	

			reauthenticate() // access token may have expired, confirm valid
		});
	};

	function reauthenticate() {
		$.ajax({
			url: "/api/authentication/getaccesstoken/use_cached_token", success: function (result) {
				if (result != "user_cancelled_login") {
					localStorage.setItem("accessToken", result)
					viewMyWorkspaceReports()
					viewAllWorkspaces();
				}
			}, error: function (xhr, status, error) {
				console.log(error)
			}
		});
	}

	function logOut() {
		localStorage.removeItem("accessToken")
		switchLocation("Home.html")
	}

	function returnToWorkspacesList() {
		Office.context.document.settings.remove("workspaceId")
		Office.context.document.settings.saveAsync(function (asyncResult){ });

		document.getElementById("workspacesList").innerHTML = ""		
		$('#backButton').hide()
		viewAllWorkspaces()	
	}

	// first tab - viewing reports in my workspace
	function viewMyWorkspaceReports() {
		$.ajax({
			url: "/api/authentication/getreportsfromgroup/myworkspace", success: function (result) {
				var reportTemplateLink = window.location.origin + "/Templates/Report.html" + window.location.search

				for (var key in result) {
					var report = result[key]
					var listElement = ""

					// generating html for each report in list of my workspace reports
					listElement += "<a "
					listElement += "class=\"collection-item\" " 
					listElement += "onclick=\"cacheReportParameters('" + report.id + "', '" + report.embedUrl + "', 'user_owns_data')\" "
					listElement += "href =\"" + reportTemplateLink + "\">" + report.name
					listElement += "</a>"

					document.getElementById("reportsList").innerHTML += listElement							
				}
			}, error: function (xhr, status, error) {
				console.log(error);
			}
		});
	}

	// second tab - viewing all shared workspaces
	function viewAllWorkspaces() {
		$.ajax({
			url: "/api/authentication/getworkspaces", success: function (result) {

				for (var key in result) {
					var workspace = result[key]
					var listElement = ""

					// generating html for each workspace in list of all shared workspaces
					listElement += "<a "
					listElement += "class=\"collection-item\" "
					listElement += "onclick=\"showReports('" + workspace.id + "')\"> "
					listElement += workspace.name + "</a>"

					document.getElementById("workspacesList").innerHTML += listElement
				}

			}, error: function (xhr, status, error) {
				console.log(error);
			}
		});
	}

	function switchLocation(destination) {
		window.location = window.location.origin + "/Templates/" + destination + window.location.search
	}

})();