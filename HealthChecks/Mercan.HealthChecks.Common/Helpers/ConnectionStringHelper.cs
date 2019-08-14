using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Mercan.Common.HealthChecks
{
    public static class ConnectionStringHelper
    {
        public static Dictionary<string, string> ConnectionStringToDict(string connectionString)
        {
            Dictionary<string, string> dict = Regex.Matches(connectionString, @"\s*(?<key>[^;=]+)\s*=\s*((?<value>[^'][^;]*)|'(?<value>[^']*)')").Cast<Match>()
            .ToDictionary(m => m.Groups["key"].Value, m => m.Groups["value"].Value);
            return dict;
        }

        public static string GetValueFromconnectionString(string connectionString, string Key)
        {
            var dict = ConnectionStringToDict(connectionString);
            if (dict[Key] != null)
            {
                return dict[Key];
            }
            else
            {
                return "";
            }
        }
    }
}
