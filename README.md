# Naxtor Games Debug Plus

## Usage
### Conventional:

Parameter:

```csharp
Debug.Log(string message,UnityEngine.Object context);
```

Example:

```csharp
using UnityEngine;

Debug.Log("My info message.",objectReference);
Debug.LogWarning("My warning message.",objectReference);
Debug.LogError("My error message.",objectReference);
```

### Debug Plus:

Parameter:

```csharp
DebugPlus.LogInfo(string message,UnityEngine.Object context,string logTag,System.Type classType);
```

Example:

```csharp
using NaxtorGames.Debugging;

DebugPlus.LogInfo("My info message.",objectReference,"UI",typeof(MyClass));
DebugPlus.LogWarning("My warning message.",objectReference,"UI",typeof(MyClass));
DebugPlus.LogError("My error message.",objectReference,"UI",typeof(MyClass));
```

## Notes

### Define 'DEBUG_PLUS'
When the 'DebugPlus' package is installed it automatically sets a define for all default platforms so other code that uses this debugger can exclude those logs without any error.

```csharp
#if DEBUG_PLUS
    DebugPlus.LogInfo("My info message.",objectReference,"UI",typeof(MyClass));
#else
    Debug.Log("My info message.",objectReference);
#endif
```

### Define 'DEBUG_PLUS_NO_LOGGING'
When the define 'DEBUG_PLUS_NO_LOGGING' is set in project settings, none of the DebugPlus method calls do anything anymore. This is an easy way to exclude all debug logs at once.