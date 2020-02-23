using System;
using System.IO;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace drawing_application
{
    public class SaveLoadManager
    {
        string textfile;

        public SaveLoadManager()
        {         
            textfile = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "savedata.txt");
        }

        public void WriteShapeToFile(Shape shape)
        {
            File.AppendAllText(textfile,$"{shape.GetType().Name} {Canvas.GetLeft(shape)} {Canvas.GetTop(shape)} {shape.Width} {shape.Height}\n");
        }

        public void ClearFile()
        {
            File.Delete(textfile);
        }


    }
}
