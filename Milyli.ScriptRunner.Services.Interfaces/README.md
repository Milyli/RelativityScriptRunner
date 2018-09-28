# Kepler Services

Kepler is a framework internal to Relativity for building HTTP APIs.

To better separate the Custom Page and Agent Projects from the actual ScriptRunner Business Logic we've created 2 Kepler Services.

**Table of Contents**

- [Versioning](#versioning)
- [Usage](#usage)
- [API Documentation](#api-documentation)
  - [Models](#models)
  - [IScriptManager](#iscriptmanager)
  - [IScriptRunManager](#iscriptrunmanager)

## Versioning

The Kepler Services are currently in a "Double Secret Pre-Release" state.
We've defined the Contracts and Interfaces but not implemented them as of 10/2018.

Initially, the Kepler Services live in a V1 namespace.
These services will be valid until, and possibly after, we majorly re-write the UI to be more user friendly.
Any changes to the V1 services will be be backwards compatible.

At the time we need to majorly overhaul the Kepler Services, we will introduce a V2 namespace.
V1 services will receive an `ObsoleteAttribute` and be maintained for as many releases as ~~possible~~ convenient.

## Usage

You can find the Kepler Interfaces for consumption in the Nuget Package contained in the [latest release](https://github.com/Milyli/RelativityScriptRunner/releases/latest).
Include it in your Application using the [`Install-Package` nuget command](https://stackoverflow.com/a/35753968).

Calling Kepler Services is similar to [calling the Services API](https://platform.relativity.com/9.6/Content/RSAPI/Basic_concepts/Best_practices_for_the_Services_API.htm).

```csharp
using (var scriptManager = _helper.GetServicesManager().CreateProxy<IScriptManager>(ExecutionIdentity.System))
{
    var readScriptResponse = await scriptManager.GetScriptAsync(new ReadScriptRequest());
}

```

## API Documentation 

In this documentation the Request/Response format:

Name | Type | Description
--- | --- | ---
ReadScriptRequest | Object | The Request Container.
ReadScriptRequest.CaseId | Integer | The ArtifactID of the Workspace containing the Script.
ReadScriptRequest.ScriptId | Integer | The ArtifactID of the Script in the Workspace.

translates to an HTTP Request of

```json
{
  "ReadScriptRequest": {
    "CaseId": 1265546,
    "ScriptId": 861215
  }
}
```

and a C# Request of

```csharp
new ReadScriptRequest
{
   CaseId = 1265546,
   ScriptId = 1265546
};
```

### Models

There are 4 Models that are shared between the Requests and Responses.

##### Script

Script represents an individual script in the [Relativity Script Library](https://help.relativity.com/9.6/Content/Relativity/Library_scripts/Relativity_Script_Library.htm).

Name | Type | Description
--- | --- | ---
RelativityScriptId | Integer | The ArtifactID of the Script in the Workspace.
WorkspaceId | Integer | The ArtifactID of the Workspace containing the Script.
WorkspaceName | String | The Name of the Workspace containing the Script.
Name | String | The Name of the Script.
Description | String | The Description of the Script.

##### ScriptRun

ScriptRun represents a Script Run Job which pairs an individual script with an execution schedule and history.

Name | Type | Description
--- | --- | ---
Id | Integer | The Id of the Script Run Job.
RelativityScriptId | Integer | The ArtifactID of the Script in the Workspace.
WorkspaceId | Integer | The ArtifactID of the Workspace containing the Script.
Name | String | The Name of the Script Run Job.
LastExecutionTimeUTC | DateTime | The last time the Script Run Job will execution. **Only available on Read.**
NextExecutionTimeUTC | DateTime | The next time the Script Run Job will execution. **Only available on Read.**
JobStatus | Integer | The status of the Script Run Job.
ExecutionSchedule | WeeklySchedule/Integer | The days of the week the Script Run Job will Execute. See the WeeklySchedule section below.
ExecutionTime | Integer | The number of seconds after midnight UTC to execute the script.

##### WeeklySchedule

WeeklySchedule represents the days of the week that a Script Run Job will execute.

Name | Value
--- | ---
Sunday | 1
Monday | 2
Tuesday | 4
Wednesday | 8
Thursday | 16
Friday | 32
Saturday | 64

Multiple days can be selected, for example to execute on Tuesday and Thursday:

When making the request via HTTP you can set multiple days adding the values of the days together, i.e. `"ExecutionSchedule": 20"`

When making the request via C# you can set multiple days using the `|` operator, i.e. `WeeklySchedule.Tuesday | WeeklySchedule.Thursday`

##### Input

Input represents a parameter passed to the script as an Input value.

**WIP**

### IScriptManager

The Script Manager is used to Query for scripts in the Relativity Script Library.

#### Get a single script (`IScriptManager.GetScriptAsync`)

```
POST https://localhost/Relativity.REST/api/Milyli.ScriptRunner/API/V1/Script/ReadSingle
```

##### Request

Name | Type | Description
--- | --- | ---
ReadScriptRequest | Object | The Request Container.
ReadScriptRequest.CaseId | Integer | The ArtifactID of the Workspace containing the Script.
ReadScriptRequest.ScriptId | Integer | The ArtifactID of the Script in the Workspace.

##### Response

Name | Type | Description
--- | --- | ---
ReadScriptResponse | Object | The Response Container.
ReadScriptResponse.Script | Script | Information about the script in the Relativity Script Library.
ReadScriptResponse.ScriptRuns | ScriptRun[] | A list of information about the schedule and history of the script.

#### List all scripts in a workspace (`IScriptManager.GetCaseScriptsAsync`)

```
POST https://localhost/Relativity.REST/api/Milyli.ScriptRunner/API/V1/Script/ReadAll
```

##### Request

Name | Type | Description
--- | --- | ---
ReadCaseScriptsRequest | Object | The Request Container.
ReadCaseScriptsRequest.CaseId | Integer | The ArtifactID of the Workspace containing the Scripts.

##### Response

Name | Type | Description
--- | --- | ---
ReadCaseScriptResponse | Object | The Response Container.
ReadScriptRequest.CaseId | Integer | The ArtifactID of the Workspace containing the Scripts.
ReadScriptRequest.CaseName | Integer | The Name of the Workspace containing the Scripts.
ReadCaseScriptResponse.CaseScripts | Script[] | A list of information about the scripts in the Workspace Relativity Script Library.

### IScriptRunManager

The Script Run Manager is used to Query for information about a script's schedule and history.

#### Get a single script run job (`IScriptRunManager.GetScriptRunAsync`)

```
POST https://localhost/Relativity.REST/api/Milyli.ScriptRunner/API/V1/ScriptRun/Read
```

##### Request

Name | Type | Description
--- | --- | ---
ReadScriptRunRequest | Object | The Request Container.
ReadScriptRunRequest.ScriptRunId | Integer | The Id of the Script Run Job.

##### Response

Name | Type | Description
--- | --- | ---
ReadScriptRunResponse | Object | The Response Container.
ReadScriptRunResponse.ScriptRun | ScriptRun | Information about the schedule and history of a script.
ReadScriptRunResponse.ScriptInputs | Input[] | A list of information about the inputs of a script.

#### Create a script run job (`IScriptRunManager.CreateScriptRunAsync`)

```
POST https://localhost/Relativity.REST/api/Milyli.ScriptRunner/API/V1/ScriptRun/Create
```

##### Request

Name | Type | Description
--- | --- | ---
CreateScriptRunRequest | Object | The Request Container.
CreateScriptRunRequest.ScriptRun | ScriptRun | Information about the script run job to be created.
CreateScriptRunRequest.ScriptInputs | Input[] | A list of script inputs to use when executing the script run job.

##### Response

Name | Type | Description
--- | --- | ---
CreateScriptRunResponse | Object | The Response Container.
CreateScriptRunResponse.ScriptRun | ScriptRun | Information about the schedule and history of a script.
CreateScriptRunResponse.ScriptInputs | ScriptInput[] | A list of information about the inputs of a script.

#### Get history for a single script run job (`IScriptRunManager.GetRunHistoryAsync`)

```
POST https://localhost/Relativity.REST/api/Milyli.ScriptRunner/API/V1/ScriptRun/History
```

##### Request

Name | Type | Description
--- | --- | ---
ReadHistoryRequest | Object | The Request Container.
ReadHistoryRequest.ScriptRunId | Integer | The Id of the script run job to read history.

##### Response

Name | Type | Description
--- | --- | ---
ReadRunHistoryResponse | Object | The Response Container.
ReadRunHistoryResponse.Id | Integer | The Id of the script run job history.
ReadRunHistoryResponse.ScriptRunId | Integer | The Id of the script run job.
ReadRunHistoryResponse.StartTimeUTC | DateTime | The time the script run job started executing.
ReadRunHistoryResponse.Runtime | Integer | The length of a script run job execution in seconds.
ReadRunHistoryResponse.HasError | Boolean | A value indicating whether the script run job execution encountered errors.
ReadRunHistoryResponse.ResultText | String | The textual representation of the script run job execution.

#### Update a script run job (`IScriptRunManager.UpdateScriptRunAsync`)

```
POST https://localhost/Relativity.REST/api/Milyli.ScriptRunner/API/V1/ScriptRun/Update
```

##### Request

Name | Type | Description
--- | --- | ---
UpdateScriptRunRequest | Object | The Request Container.
UpdateScriptRunRequest.ScriptRun | ScriptRun | Information about the script run job to be updated.
UpdateScriptRunRequest.ScriptInputs | Input[] | A list of script inputs to use when executing the script run job.

##### Response

Name | Type | Description
--- | --- | ---
UpdateScriptRunResponse | Object | The Response Container.
UpdateScriptRunResponse.ScriptRun | ScriptRun | Information about the schedule and history of a script.
UpdateScriptRunResponse.ScriptInputs | ScriptInput[] | A list of information about the inputs of a script.

#### Run a single script run job (`IScriptRunManager.RunScriptRunRequest`)

```
POST https://localhost/Relativity.REST/api/Milyli.ScriptRunner/API/V1/ScriptRun/Run
```

##### Request

Name | Type | Description
--- | --- | ---
RunScriptRunRequest | Object | The Request Container.
RunScriptRunRequest.ScriptRunId | Integer | The Id of the script run job to execute.

##### Response

WIP

#### Run many script run jobs (`IScriptRunManager.RunAllAsync`)

```
POST https://localhost/Relativity.REST/api/Milyli.ScriptRunner/API/V1/ScriptRun/RunAll
```

##### Request

Name | Type | Description
--- | --- | ---
RunAllRequest | Object | The Request Container.
RunAllRequest.RunTimeUTC | DateTime | The cutoff point for selecting scheduled and unrun script run jobs to execute.

##### Response

WIP
