namespace Idealde.Framework.Projects
{
    public class ExtensionType
    {
        public string Name { get; }
        public string Extension { get; }

        public ExtensionType(string name, string extension)
        {
            Name = name;
            Extension = extension;
        }
    }
}