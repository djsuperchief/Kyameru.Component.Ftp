using System;
using System.Collections.Generic;
using System.Text;

namespace Kyameru.Component.Ftp
{
    /// <summary>
    /// Configuration extensions.
    /// </summary>
    public static class ConfigSetup
    {
        /// <summary>
        /// Valid from headers
        /// </summary>
        private static string[] fromHeaders = new string[] { "Target", "Host", "UserName", "Password", "Recursive", "PollTime", "Delete", "Port", "Filter" };

        /// <summary>
        /// Converts incoming headers to valid processing headers.
        /// </summary>
        /// <param name="incoming">Incoming dictionary.</param>
        /// <returns>Returns a dictionary of valid headers.</returns>
        public static Dictionary<string, string> ToFromConfig(this Dictionary<string, string> incoming)
        {
            Dictionary<string, string> response = new Dictionary<string, string>();
            for (int i = 0; i < fromHeaders.Length; i++)
            {
                if (incoming.ContainsKey(fromHeaders[i]))
                {
                    response.Add(fromHeaders[i], incoming[fromHeaders[i]]);
                }
            }

            CheckDefaultHeaders(response);

            return response;
        }

        private static void CheckDefaultHeaders(Dictionary<string, string> response)
        {
            response.SetDefault("PollTime", "60000");
            response.SetDefault("Filter", "*.*");
            response.SetDefault("Delete", "false");
            response.SetDefault("Recursive", "false");
            response.SetDefault("Port", "21");
        }

        public static string GetKeyValue(this Dictionary<string, string> incoming, string key)
        {
            string response = string.Empty;
            if (incoming.ContainsKey(key))
            {
                response = incoming[key];
            }

            return response;
        }

        public static void SetDefault(this Dictionary<string, string> incoming, string key, string value)
        {
            if (!incoming.ContainsKey(key))
            {
                incoming.Add(key, value);
            }
        }
    }
}