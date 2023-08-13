# GenericPluginInvoker
Most of the time we need a command implementation and its parameters to carry out a task. If we give these in a configuration file we can extend our program as much as we want.

In this application we have a configuration file(called actionsConfiguration.json). Get the type of the action we want to execute, get its parameters and get the assemblies that store these types.

All the action types implement `IPluginAction` interface and all the parameter types implement `IActionParameters` interface. Then we pass this parameter to the command implementor's `Perform` method. Please see [DefaultInvokerAction.cs](https://github.com/burak3000/GenericPluginInvoker/blob/main/GenericPluginInvoker.Core/DefaultInvokerAction.cs "Implementation class that calls the actions in the actionsConfiguration file we have.").

Below you can see a configuration file that includes only one action. Its type is SamplePlugin.SampleAction and it is available at SamplePlugin.dll. To carry out this task it needs SamplePlugin.SampleActionParameters which is available SamplePlugin.dll. 

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

This implementation needs Windows' MessageBox API. If you directly clone the code and run it on a non-Windows machine, it will throw an exception as expected.

```
using GenericPluginInvoker.Core;
using System.Runtime.InteropServices;

namespace SamplePlugin
{
    internal class Win32
    {
        // Use DllImportAttribute to inport the Win32 MessageBox
        // function.  Set the SetLastError flag to true to allow
        // the function to set the Win32 error.
        [DllImportAttribute("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern int MessageBox(IntPtr hwnd, String text, String caption, uint type);
    }
    public class SampleAction : IPluginAction
    {
        public void Perform(IActionParameters parameters)
        {
            SampleActionParameters? actionParams = parameters as SampleActionParameters;
            Console.WriteLine("Calling Win32 MessageBox without error...");

            Win32.MessageBox(new IntPtr(0), actionParams.MessageToShow, "Press OK Dialog", 0);

            // Get the last error and display it.
            int error = Marshal.GetLastWin32Error();

            Console.WriteLine("The last Win32 Error was: " + error);

            // Call the MessageBox with an invalid window handle to
            // produce a Win32 error.

            Console.WriteLine("Calling Win32 MessageBox with error...");

            Win32.MessageBox(new IntPtr(123132), "Press OK...", "Press OK Dialog", 0);

            // Get the last error and display it.

            error = Marshal.GetLastWin32Error();

            Console.WriteLine("The last Win32 Error was: " + error);
        }
    }

    public class SampleActionParameters : IActionParameters
    {
        public string MessageToShow { get; set; }
    }
}`


