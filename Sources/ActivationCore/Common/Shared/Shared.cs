﻿using System;

namespace Activator
{
    public class Shared
    {
        public static string Cscript { get; private set; } = "cscript.exe";

        public static string Dism { get; private set; } = "dism.exe";

        public static string AnawaertServerAddress { get; private set; } = "www.anawaert.tech";

        public static string? ApplicationCurrentDirectory { get; private set; }

        public static string? System32Path { get; private set; }

        public static string? UserDocumentsPath { get; private set; }

        public static string? UserDocumentsActivatorPath { get; private set; }

        public static string? JsonFilePath { get; private set; }

        public static void InitializeSharedInfo()
        {
            ApplicationCurrentDirectory = AppDomain.CurrentDomain.BaseDirectory + "\\";
            System32Path = Environment.GetFolderPath(Environment.SpecialFolder.System) + "\\";
            UserDocumentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\";
            UserDocumentsActivatorPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Anawaert KMS Activator\\";
            JsonFilePath = UserDocumentsActivatorPath + "Activator Configuration.json";
        }
    }
}
