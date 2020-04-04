using System;

namespace Quarentime.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class EntityPathAttribute : Attribute
    {
        public string Path { get; }

        public EntityPathAttribute(string path)
        {
            Path = path;
        }
    }
}
