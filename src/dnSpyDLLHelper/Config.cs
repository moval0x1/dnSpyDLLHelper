using System.Collections.Generic;

namespace dnSpyDLLHelper
{
    public class JsonConfig
    {
        public string DllPath { get; set; }
        public List<MethodConfig> Methods { get; set; }
    }

    public class MethodConfig
    {
        public string Name { get; set; }
        public List<string> Parameters { get; set; }
    }
}