# GenericPluginInvoker
Most of the time we need a command implementation and its parameters to carry out a task. If we give these in a configuration file we can extend our program much as we want.

In this application we have a configuration file(called actionsConfiguration.json). Get the type of the action we want to execute, get its parameters and get the assemblies that store these types.

All the action types implement `IPluginAction` interface and all the parameter types implement `IActionParameters` interface. Then we pass this parameter to the command implementor's `Perform` method.



`{
  "Actions": [
    {
      "ActionType": "SamplePlugin.SampleAction",
      "ActionAssembly": "SamplePlugin.dll",
      "ActionParametersType": "SamplePlugin.SampleActionParameters",
      "ActionParametersAssembly": "SamplePlugin.dll",
      "ActionParametersJson": {
        "MessageToShow": "This is the best message to show!"
      }
    }
  ]
}`
