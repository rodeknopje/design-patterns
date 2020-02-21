using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.IO;
using System.Windows;
namespace drawing_application
{
    class SaveLoadManager
    {
        string textfile;

        public SaveLoadManager()
        {         
            textfile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "savedata.txt");
        }

        public void WriteShapeToFIle(string text)
        {
            File.AppendAllText(textfile,$"{text}\n");
        }

        public void ClearFile()
        {
            File.Delete(textfile);
        }


    }
}
