Properties {
	$Deployment = Split-Path $psake.build_script_file
	$Artifacts = "$Deployment\Artifacts"
	$sln = "$Deployment\..\Solutions\ScriptRunner.sln"
}

Task Default -Depends Rebuild

Task Rebuild -Depends Clean, Build

Task Build {
	Exec { 
		msbuild "$sln" `
			/t:Build `
			/p:Configuration=Release `
			/v:quiet `
			/p:OutDir=$Artifacts }
}

Task Clean {

	if(Test-Path $Artifacts) {
		rd $Artifacts -rec -force | out-null
	}
	mkdir $Artifacts | out-null

	Write-Host "$sln"

	Exec { 
		msbuild "$sln" `
			/t:Clean `
			/p:Configuration=Release `
			/v:quiet
	}
}
