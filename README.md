# Features

Users **MUST** have a Power BI Pro license to use this tool and have an Azure Active Directory set up for authentication.

My tool solves multiple problems with its incorporated feature set: 
-	**Refresh:** allows user to refresh chart real-time to reflect any updated changes in data rather than having to re-export report into PowerPoint 
-	**Toggle Filters:** allows user to enable/disable filters pane, which provides functionality to apply filters on the data within a selected visual
-	**Toggle View Mode:** allows user to toggle edit/view mode, which enables them to make changes to the report directly from the add-in rather than having to use the Power BI app  

\s\s

![Options pane](PowerBIEmbedWeb/options.jpg?raw=true)

The following options are enabled on hover: get rid of options, go back to navigation, toggle filters, toggle edit/view mode, refresh report. 

![My Workspace](PowerBIEmbedWeb/myworkspace.jpg?raw=true)

![Shared Workspaces](PowerBIEmbedWeb/sharedworkspace.jpg?raw=true)  

The user can not only open reports that are stored within “My Workspace,” but can also browse through any shared workspaces that they are members of and open reports within them. 

To embed a report, we need 3 parameters: the report id, the report embed URL, and a token.
For embedding reports in “My Workspace,” we use the access token generated during our log-in for our token while for reports in shared workspaces, we have to generate a specific token for each report we want to embed.   

\s\s

# Set-up Instructions

Instructions for creating Azure AD tenant 
1.	Sign into Azure web portal with an account that has an Azure subscription
2.	Within Azure web portal, create an Azure Active Directory and provide organization name and initial domain name
3.	Create a global administrator for the tenant and also a user, whose log-in will be used to sign up for a Power BI pro account
 
Instructions for registering application
1.	Locate "App Registration" service -> New application registration 
2.	Specify Name, Native app, and Sign-on URL (aka redirect URL), also add logout URL (can be same as redirect URL) 
3.	From within Settings -> Required Permissions, delegate all permissions from Power BI Service and Windows Azure AD
4.	Remember to Grant Permissions from a Global Admin (to ensure all users within app are granted permissions) 
5.	Obtain the Application ID (aka client ID) 
