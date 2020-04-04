using Newtonsoft.Json;
using System.IO;

namespace Quarentime.Common.Helpers
{
    public static class StreamUtils
    {
        public static T DeserializeStream<T>(this Stream stream) where T : class
        {
            var serializer = new JsonSerializer();

            using (var sr = new StreamReader(stream))
            using (var jsonTextReader = new JsonTextReader(sr))
            {
                return serializer.Deserialize<T>(jsonTextReader);
            }
        }
    }
}
