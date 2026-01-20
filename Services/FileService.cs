using System;
using System.IO;

namespace Simple_Screenshot.Services
{
    public class FileService
    {
        public void SaveSummary(string? summaryText)
        {
            if (string.IsNullOrEmpty(summaryText))
                return;

            var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var dir = Path.Combine(documents, "Summary");
            File.WriteAllText(dir, summaryText);
        }

        public void SavePrerequisites(string[] prerequisites)
        {
            if (prerequisites == null || prerequisites.Length == 0)
                return;

            var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var dir = Path.Combine(documents, "Prereqs");
            
            foreach (string prerequisite in prerequisites)
            {
                File.WriteAllText(dir, prerequisite);
            }
        }
    }
}
