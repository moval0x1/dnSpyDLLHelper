using dnSpyDLLHelper;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Reflection;

internal class Program
{
    private static void Main(string[] args)
    {
        // Get the path to the directory where the .exe is located
        var exeDirectory = AppContext.BaseDirectory;

        // Combine it with the config file name
        var configPath = Path.Combine(exeDirectory, "config.json");

        if (!File.Exists(configPath) && args.Length < 1)
        {
            Console.WriteLine("Usage: <dnSpyDLLHelper.exe> <config_path>");
            return;
        }

        try
        {
            if (args.Length >= 1)
            {
                configPath = args[0];
            }

            if (!File.Exists(configPath))
            {
                Console.WriteLine("Configuration file not found.");
                return;
            }

            // Read and deserialize the JSON configuration
            var jsonConfig = File.ReadAllText(configPath);
            var config = JsonConvert.DeserializeObject<JsonConfig>(jsonConfig);

            if (config == null || string.IsNullOrEmpty(config.DllPath) || config.Methods == null)
            {
                Console.WriteLine("Invalid configuration.");
                return;
            }

            // Load the DLL and invoke the method using Reflection
            var assembly = Assembly.LoadFrom(config.DllPath);
            var types = assembly.GetExportedTypes();

            if (types.Length == 0)
            {
                Console.WriteLine("No exported types found in the assembly.");
                return;
            }

            var type = types[0]; // Assume the first exported type

            foreach (var item in config.Methods)
            {
                // Get the method info by name
                var method = type.GetMethod(item.Name, BindingFlags.Public | BindingFlags.Static);

                if (method == null)
                {
                    Console.WriteLine($"Method '{item.Name}' not found in type '{type.FullName}'.");
                    continue;
                }

                // Validate parameters match method signature
                var parameterInfos = method.GetParameters();

                // Dynamically create the parameters array to match the method's parameter count
                object[] parameters = new object[parameterInfos.Length];

                // Populate the parameters array with converted or default values
                for (int i = 0; i < parameterInfos.Length; i++)
                {
                    var expectedType = parameterInfos[i].ParameterType;

                    // Check if parameters are provided in the config
                    if (i < item.Parameters.Count)
                    {
                        // Attempt to convert the parameter to the expected type
                        parameters[i] = Convert.ChangeType(item.Parameters[i], expectedType);
                    }
                    else
                    {
                        // Assign default value for missing parameters
                        parameters[i] = expectedType.IsValueType ? Activator.CreateInstance(expectedType) : null;
                    }
                }

                try
                {
                    if (parameters.Length != parameterInfos.Length)
                    {
                        throw new ArgumentException($"Method '{item.Name}' expects {parameterInfos.Length} parameters, but {parameters.Length} were provided.");
                    }

                    // Invoke the method with the converted parameters
                    var result = method.Invoke(null, parameters);

                    Console.WriteLine($"Successfully invoked '{item.Name}'.");

                    // Handle return value
                    if (result != null)
                    {
                        Console.WriteLine($"Method returned: {result}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error invoking method '{item.Name}': {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}