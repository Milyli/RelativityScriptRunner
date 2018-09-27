# Kepler Services

Kepler is a framework internal to Relativity for building HTTP APIs.

To better separate the Custom Page and Agent Projects from the actual ScriptRunner Business Logic we've created 2 Kepler Services.

**Table of Contents**

- [Versioning](#Versioning)
- [Usage](#Usage)
- [API Documentation](#API-Documentation)

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

### Models

### IScriptManager

### IScriptRunManager
