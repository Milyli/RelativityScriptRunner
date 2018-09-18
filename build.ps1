[cmdletbinding()]
param(
	[Parameter(Position = 0, Mandatory = $false)]
	[string]$TaskList = "Default",


	[Parameter(Position = 1, Mandatory = $false)]
	[string]$Version = "1.0.0.18"
)

./Deployment/Tools/psake/src/psake.ps1 ./Deployment/default.ps1 `
	-taskList "$TaskList" `
	-framework "4.7" `
	-properties @{ "version"="$Version" }
