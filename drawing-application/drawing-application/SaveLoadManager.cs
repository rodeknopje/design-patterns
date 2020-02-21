using System;
using System.IO;

namespace drawing_application
{
    class SaveLoadManager
    {
        string textfile;

        public SaveLoadManager()
        {         
            textfile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "savedata.txt");
        }

        public void WriteShapeToFile(string text)
        {
            File.AppendAllText(textfile,$"{text}\n");
        }

        public void ClearFile()
        {
            File.Delete(textfile);
        }


    }
}
