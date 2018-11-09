using System;
using System.Collections.Generic;
using System.Text;

namespace reportsApp
{
    public class Utils
    {
        public static string GetEnvironmentVariable(string name)
        {
            return System.Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Process);
        }
    }
}
