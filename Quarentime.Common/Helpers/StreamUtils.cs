using System.IO;
using System.Text;

namespace Quarentime.Common.Helpers
{
    public static class StreamUtils
    {
        public static string StreamToString(this Stream stream)
        {
            stream.Position = 0;
            using var reader = new StreamReader(stream, Encoding.UTF8);

            return reader.ReadToEnd();
        }
    }
}
