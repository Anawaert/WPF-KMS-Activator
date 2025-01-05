using System.IO;

namespace Activator
{
    public partial class Utility
    {
        internal static DirectoryInfo CreateActivatorDirectory() => Directory.CreateDirectory(Shared.UserDocumentsActivatorPath + "Anawaert KMS Activator");
    }
}
