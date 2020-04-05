using Newtonsoft.Json;
using System.Collections.Generic;

namespace Quarentime.Common.Helpers
{
    public static class Extensions
    {
        public static Dictionary<string, string> ToDictionary<T>(this T obj) where T: class
        {
            var jsonString = JsonConvert.SerializeObject(obj);
            var dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonString);

            return dictionary;
        }
    }
}
