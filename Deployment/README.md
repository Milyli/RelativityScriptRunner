# CLI Build Tools

## Prereqs

This project uses .NET Framework 4.7.

The build process uses [psake](https://github.com/psake/psake).
It has been included as a submodule, before running any build commands run `git submodule update --init --recursive`.

psake 4.7.4 uses the [Visual Studio Setup PowerShell Module](https://github.com/Microsoft/vssetup.powershell) to detect the msbuild executable.
You can read more about [their interactions here](https://github.com/psake/psake/issues/241).

## Running Tests

This project uses NUnit 3.
All test projects included in the Solution should be picked up during a Unit Test Build.
Currently, Integration Tests must be run manually in Visual Studio.

In order to rebuild and run with unit tests pass `UnitTest` as the -TaskList parameter to the build script.
The following are all valid invocations:

```ps1
.\build.ps1 # the default is a unit test build
.\build.ps1 UnitTest
.\build.ps1 -TaskList UnitTest
```

Note: The -Version parameter is ignored for UnitTest builds.

## Creating a RAP file

This project uses the [RelativityDev.RapBuilder nuget package](https://www.nuget.org/packages/RelativityDev.RapBuilder/0.0.0.3-alpha) to build a rap file.

In order to rebuild and package the application as a rap pass `CreateRap` as the -TaskList parameter to the build script.
Additionally, you can pass a version for the rap to the -Version parameter. 
The following are all valid invocations:

```ps1
.\build.ps1 CreateRap # the version defaults to 1.0.0.18
.\build.ps1 CreateRap 9.6.0.0
.\build.ps1 CreateRap -Version 9.6.0.0
.\build.ps1 -TaskList CreateRap 
.\build.ps1 -TaskList CreateRap 9.6.0.0
.\build.ps1 -TaskList CreateRap -Version 9.6.0.0
```

You can find the output rap in the Deployment/Packages folder.
*Note:* the output rap will override any other raps with the same version.

## Creating a Nuget Package

The `Milyli.ScriptRunner.Services.Interfaces` and `Milyli.ScriptRunner.Services.Interfaces.Contracts` projects comprise a consumable package of [Kepler services](../Milyli.ScriptRunner.Services.Interfaces/) that allow other applications to use ScriptRunner.  These can be packed into a .nupkg file using the `CreateNugetPackage` option as a -TaskList parameter, or alternatively the `CreateRapAndNuget` option to create both a Rap and Nupkg.

Like the Rap building process, the -Version parameter can be used to inform the version of the .nupkg.
Also like the rap building process, the output .nupkg file can be found in the Deployment/Packages folder and will override any packages of the same version.
*Note:* When creating both a .rap and .nupkg, any provided -Version value will be used for both.

## Structure and Process

### Relevant files/folders

```
RelativityScriptRunner/
|-- Deployment/
|   |-- Artifacts/ 	 # Intermediate build artifacts for packagin the RAP file, not in Source Control
|   |-- Packages/ 	 # Final output and versioned raps, not in Source Control
|   |-- Tools/
|   |   |-- BuildTools/  # Restored nuget packages required for builds, not in Source Control
|   |   |-- psake/ 	 # Location of the psake git submodule
|   |   \-- nuget.exe 	 # Downloaded nuget executable, not in Source Control
|   |
|   |-- application.xml  # Relativity application manifest
|   |-- build.xml 	 # Input file for the RelativityDev.RapBuilder executable
|   |-- default.ps1 	 # The psake build file
|   |-- packages.config  # Contains the nuget packages required for the build process
|   |-- ScriptRunner.rap # The intermediate rap file output by the RelativityDev.RapBuilder executable, not in Source Control
|   |-- TestResult.xml 	 # Output of the last UnitTest build
|   \-- Version.txt 	 # Used internally by Milyli for versioning the production rap
|
|-- Solutions/ 		 # Visual Studio .sln file
|-- build.ps1		 # The entry point of the build scripts
\-- lbuild		 # The WSFL entry point of the build scripts
```

### The build process

#### Accessing the build tools

When the build runs for the first time it will download the latest nuget executable.
It will use this executable to restore the nuget packages in the `Deployment/packages.config` file.

If you update the packages you will have to delete the `Deployment/Tools/BuildTools` directory in order for them to be re-restored.

#### Running Unit Tests

The solution is built in debug mode using msbuild.
The nunit3 console runner is then used to build only the Unit Tests.

#### Creating the Rap file

The solution is built in release mode and the build artifacts are output to the `Deployment/Artifacts` folder.
After the build the RelativityDev.RapBuilder executable is used to package the assemblies and custom page.
The rap is output to `Deployment/ScriptRunner.rap` and then copied and renamed into the `Deployment/Packages` folder.
