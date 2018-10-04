Properties {
	$Deployment = Split-Path $psake.build_script_file
	$sln = "$Deployment\..\Solutions\ScriptRunner.sln"
	$Artifacts = "$Deployment\Artifacts"
	$Tools = "$Deployment\Tools"
	$TestResult_xml = "$Deployment\TestResult.xml"
	$BuildTools = "$Tools\BuildTools"
	$nuget_exe = "$Tools\nuget.exe"
	$packages_config = "$Deployment\packages.config"
	$build_xml = "$Deployment\build.xml"
	$nunit_exe = "$BuildTools\NUnit.ConsoleRunner.3.8.0\tools\nunit3-console.exe"
	$rapbuilder_exe = "$BuildTools\RelativityDev.RapBuilder.0.0.0.3-alpha\lib\kCura.RAPBuilder.exe"
	$Packages = "$Deployment\Packages"
	$scriptrunner_rap = "$Deployment\ScriptRunner.rap"
	$nuspec = "$Deployment\Milyli.ScriptRunner.Services.Interfaces.nuspec"
	$Version = $null
}

Task Default -Depends UnitTest

Task PackageBuild -Depends RestorePackages {
	Exec { 
		msbuild "$sln" `
			/t:Clean,Rebuild `
			/p:Configuration=Release `
			/v:quiet `
			/p:OutDir=$Artifacts 
	}
}

Task TestBuild -Depends RestorePackages {
	Exec { 
		msbuild "$sln" `
			/t:Clean,Rebuild `
			/p:Configuration=Debug `
			/v:quiet `
	}
}

Task RestorePackages -Depends InstallNuget {
	Exec { & $nuget_exe restore $sln }
}

Task RestoreBuildTools -Depends InstallNuget -precondition { -Not (Test-Path $BuildTools) } {
	Exec { & $nuget_exe install $packages_config -o $BuildTools }
}

Task InstallNuget -precondition { -Not (Test-Path $nuget_exe) } {
	Invoke-WebRequest -Uri "https://dist.nuget.org/win-x86-commandline/latest/nuget.exe" -OutFile $nuget_exe -ErrorAction Stop
}

Task UnitTest -Depends RestoreBuildTools, TestBuild {
	Exec { & $nunit_exe $sln --result="$TestResult_xml" --where="cat == Unit && cat != Integration && cat != PlatformUnitTest && cat != Explicit && cat != Ignore" --skipnontestassemblies }
}

Task CreateRap -Depends RestoreBuildTools, PackageBuild {
	Exec { & $rapbuilder_exe /source:"$Deployment" /input:"$build_xml" /version:"$Version" }
	if(-Not (Test-Path "$Packages\$Version")) {
		New-Item "$Packages\$Version" -ItemType Directory
	}
	Copy-Item $scriptrunner_rap -Destination "$Packages\$Version\Milyli.ScriptRunner-$Version.rap" -Force | Out-Null
}

Task CreateNugetPkg -Depends PackageBuild {
	if(-Not (Test-Path "$Packages\$Version")) {
		New-Item "$Packages\$Version" -ItemType Directory
	}
	else {
		Remove-Item (Join-Path "$Packages\$Version" "Milyli.ScriptRunner.Services*")
	}
	Exec { & $nuget_exe pack $nuspec
		-outputdirectory "$Packages\$Version\"
		-version $Version }
}

Task CreateRapAndNuget -Depends CreateRap, CreateNugetPkg
