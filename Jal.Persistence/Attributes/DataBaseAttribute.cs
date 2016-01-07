using System;

namespace Jal.Persistence.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class DataBaseAttribute : Attribute
    {
        public string Name { get; set; }

        public DataBaseAttribute(string name)
        {
            Name = name;
        }
    }
}
