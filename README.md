# dnSpyDLLHelper
Simplifying dnSpy Debugging for DLLs

## Why do you need this?
You don't! In my case, it is too annoying when I dump a **DLL** in **.NET** and cannot debug it directly. After experiencing this problem, I decided to create a kind of _helper_ for display.

It means that you can simply tell what should be called, and the _helper_ will do it for you. Let's see an example.

### Config File
The ``config.json``.

```JSON
{
    "DllPath": "C:\\myFolder\\DLLTest.dll",
    "Methods": [
        { "Name": "PrintHelloWorld", "Parameters": [] },
        { "Name": "ShowMessage", "Parameters": ["Hello from config!", "My new title"] },
        { "Name": "AddTwoValues", "Parameters": [2, 5] }
    ]
}

```

There is a need to add the ``Name``, the MethodName, and the ``Parameters`` parameter if needed.

In my **DLLTest** example I have three methods:
```C#
public static void ShowMessage(string message, string title = "Message")
{
    MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
}

public static void PrintHelloWorld()
{
    Console.WriteLine("Hello, World!");
}

public static int AddTwoValues(int val1, int val2)
{
    return val1 + val2;
}
```

That's the reason my ``config.json`` is in that way.

## Using dnSpyDLLHelper
We just need to load both files in **[dnSpy](https://github.com/dnSpyEx)**. The fastest way is to add a breakpoint on the **DLL** you want to debug and run the _helper_.

It should be something such as this image below:

![dnSpyDLLHelper](/imgs/dnSpyDLLHelper.png)