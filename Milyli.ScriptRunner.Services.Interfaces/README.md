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
ReadScriptRequest | Object | The Request Container
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

#### Get a single script (IScriptManager.GetScriptAsync)

This gets a single Workspace Level Script and a list of associated Milyli.ScriptRunner Script Run Jobs.

```
POST https://localhost/Relativity.REST/api/Milyli.ScriptRunner/API/V1/Script/ReadSingle
```

##### Request

Name | Type | Description
--- | --- | ---
ReadScriptRequest | Object | The Request Container
ReadScriptRequest.CaseId | Integer | The ArtifactID of the Workspace containing the Script.
ReadScriptRequest.ScriptId | Integer | The ArtifactID of the Script in the Workspace.

##### Response


