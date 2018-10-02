# Kepler Services

Kepler is a framework internal to Relativity for building HTTP APIs.

To better separate the Custom Page and Agent Projects from the actual ScriptRunner Business Logic we've created 2 Kepler Services.

**Table of Contents**

- [Versioning](#versioning)
- [Usage](#usage)
- [API Documentation](#api-documentation)
  - [Models](#models)
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
using (var scriptRunManager = _helper.GetServicesManager().CreateProxy<IScriptRunManager>(ExecutionIdentity.System))
{
    var readScriptResponse = await scriptRunManager.GetScriptAsync(new ReadScriptRequest());
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

There are 5 Models that are shared between the Requests and Responses.

##### ScriptRun

ScriptRun represents a Script Run Job which pairs an individual script with an execution schedule and history.

You can get the Relativity Ids from the RSAPI.

Name | Type | Description
--- | --- | ---
Id | Integer | The Id of the Script Run Job.
RelativityScriptId | Integer | The ArtifactID of the Script in the Workspace.
WorkspaceId | Integer | The ArtifactID of the Workspace containing the Script.
Name | String | The Name of the Script Run Job.
ExecutionSchedule | WeeklySchedule/Integer | The days of the week the Script Run Job will Execute. See the WeeklySchedule section below.
ExecutionTime | Integer | The number of seconds after midnight UTC to execute the script.
LastExecutionTimeUTC | DateTime | The last time the Script Run Job will execution. **Only available on Read.**
NextExecutionTimeUTC | DateTime | The next time the Script Run Job will execution. **Only available on Read.**
JobStatus | JobStatus/Integer | The status of the Script Run Job. **Only available on Read.**

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

##### JobStatus

JobStatus represents what state the ScriptRun is currently in.

Name | Value
--- | ---
Idle | 0
Waiting | 1
Running | 2

##### Input

Input represents a parameter passed to the script as an Input value.

You can get the Relativity Ids from the RSAPI.

Name | Type | Description
--- | --- | ---
Id | Integer | The Id of the Script Run Job.
RelativityInputId | Integer | The ArtifactID of the Input in the Workspace. The Workspace is assumed to be the same as the associated ScriptRun.
InputName | String | The Name of the Input. **Only available on Read.**
InputValue | String | The value of the input to be used during the Script Run.

##### ScriptRunHistory

ScriptRunHistory represents the results of a single execution of a Script Run Job.

Name | Type | Description
--- | --- | ---
Id | Integer | The Id of the Script Run Job History.
StartTimeUTC | DateTime | The time the script run job started executing.
Runtime | Integer | The length of a script run job execution in seconds.
HasError | Boolean | A value indicating whether the script run job execution encountered errors.
ResultText | String | The textual representation of the script run job execution.

### IScriptRunManager

The Script Run Manager is used to Query for information about a script's schedule and history.

#### Get a single script run job (`IScriptRunManager.ReadSingleAsync`)

```
POST https://localhost/Relativity.REST/api/Milyli.ScriptRunner/V1/ScriptRun/Read
```

##### Request

Name | Type | Description
--- | --- | ---
scriptRunId | Integer | The Id of the Script Run Job.

##### Response

Name | Type | Description
--- | --- | ---
ScriptRunResponse | Object | The Response Container.
ScriptRunResponse.ScriptRun | ScriptRun | Information about the schedule and history of a script.
ScriptRunResponse.ScriptInputs | Input[] | A list of information about the inputs of a script.

#### Create a script run job (`IScriptRunManager.CreateSingleAsync`)

```
POST https://localhost/Relativity.REST/api/Milyli.ScriptRunner/V1/ScriptRun/Create
```

##### Request

Name | Type | Description
--- | --- | ---
ScriptRunRequest | Object | The Request Container.
ScriptRunRequest.ScriptRun | ScriptRun | Information about the script run job to be created.
ScriptRunRequest.ScriptInputs | Input[] | A list of script inputs to use when executing the script run job.

##### Response

Name | Type | Description
--- | --- | ---
ScriptRunResponse | Object | The Response Container.
ScriptRunResponse.ScriptRun | ScriptRun | Information about the schedule and history of a script.
ScriptRunResponse.ScriptInputs | Input[] | A list of information about the inputs of a script.

#### Get history for a single script run job (`IScriptRunManager.GetRunHistoryAsync`)

```
POST https://localhost/Relativity.REST/api/Milyli.ScriptRunner/V1/ScriptRun/ReadHistory
```

##### Request

Name | Type | Description
--- | --- | ---
scriptRunId | Integer | The Id of the script run job to read history.

##### Response

Name | Type | Description
--- | --- | ---
ReadRunHistoryResponse | Object | The Response Container.
ReadRunHistoryResponse.RunHistory | ScriptRunHistory[] | All the execution history for the requested Script Run Job.

#### Update a script run job (`IScriptRunManager.UpdateSingleAsync`)

```
POST https://localhost/Relativity.REST/api/Milyli.ScriptRunner/V1/ScriptRun/Update
```

##### Request

Name | Type | Description
--- | --- | ---
ScriptRunRequest | Object | The Request Container.
ScriptRunRequest.ScriptRun | ScriptRun | Information about the script run job to be created.
ScriptRunRequest.ScriptInputs | Input[] | A list of script inputs to use when executing the script run job.

##### Response

Name | Type | Description
--- | --- | ---
ScriptRunResponse | Object | The Response Container.
ScriptRunResponse.ScriptRun | ScriptRun | Information about the schedule and history of a script.
ScriptRunResponse.ScriptInputs | Input[] | A list of information about the inputs of a script.

#### Run a single script run job (`IScriptRunManager.RunSingleAsync`)

```
POST https://localhost/Relativity.REST/api/Milyli.ScriptRunner/V1/ScriptRun/Run
```

##### Request

Name | Type | Description
--- | --- | ---
scriptRunId | Integer | The Id of the script run job to execute.

##### Response

WIP

#### Run many script run jobs (`IScriptRunManager.RunAllAsync`)

```
POST https://localhost/Relativity.REST/api/Milyli.ScriptRunner/V1/ScriptRun/RunAll
```

##### Request

Name | Type | Description
--- | --- | ---
runTimeUTC | DateTime | The cutoff point for selecting scheduled and unrun script run jobs to execute.

##### Response

WIP
