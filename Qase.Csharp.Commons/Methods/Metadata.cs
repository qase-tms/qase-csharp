using System.Collections.Generic;

namespace Qase.Csharp.Commons.Methods
{
    public static class Metadata
    {
        public static void Comment(string comment)
        {
            ContextManager.AddComment(comment);
        }

        public static void Attach(string path)
        {
            ContextManager.AddAttachment(new List<string> { path });
        }

        public static void Attach(List<string> paths)
        {
            ContextManager.AddAttachment(paths);
        }

        public static void Attach(byte[] data, string fileName)
        {
            ContextManager.AddAttachment(data, fileName);
        }
    }
}
