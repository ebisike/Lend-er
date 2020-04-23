using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Lend_er.Services.Services
{
    public class MyLogger
    {
        public string Folder { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }

        public MyLogger(string folder, string filename)
        {
            this.Folder = folder;
            this.FileName = filename;
            this.FilePath = Path.Combine(this.Folder, this.FileName);
        }

        public void LogToFile(string url)
        {
            using (StreamWriter streamWriter = new StreamWriter(FilePath, true))
            {
                streamWriter.Write(url);
            }
        }
    }
}
