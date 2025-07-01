using System;
using System.IO;

namespace Activator
{
    /// <summary>
    /// <para>本类主要实现了一些工具函数，以辅助激活工具与激活器内核的工作</para>
    /// <para>This class mainly implements some utility functions to assist the work of activation tool and activator core</para>
    /// </summary>
    public partial class Utility
    {
        internal static DirectoryInfo CreateActivatorDirectory()
        {
            if (Shared.UserDocumentsPath is null)
            {
                return Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Anawaert KMS Activator");
            }
            else
            {
                return Directory.CreateDirectory(Shared.UserDocumentsPath + "Anawaert KMS Activator");
            }
        }
    }
}
