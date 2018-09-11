Properties {
	$Deployment = Split-Path $psake.build_script_file
	$Artifacts = "$Deployment\Artifacts"
	$nuget_exe = "$Deployment\Tools\nuget.exe"
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

	Exec { 
		msbuild "$sln" `
			/t:Clean `
			/p:Configuration=Release `
			/v:quiet
	}
}

Task InstallNuget -precondition { -Not (Test-Path $nuget_exe) } {

	Invoke-WebRequest -Uri "https://dist.nuget.org/win-x86-commandline/latest/nuget.exe" -OutFile $nuget_exe -ErrorAction Stop
}
