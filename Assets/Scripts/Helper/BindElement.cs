using System;
using JetBrains.Annotations;

namespace Helper
{
    [AttributeUsage(AttributeTargets.Field)]
    public class BindElement: Attribute
    {
        [CanBeNull] // OH NO OLD STYLE CSharp...
        public readonly string ElementName;
        
        public BindElement(string elementName)
        {
            ElementName = elementName;
        }
        
        public BindElement()
        {
            ElementName = null;
        }
    }
}