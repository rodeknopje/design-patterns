using System;
using System.IO;
using System.Windows.Controls;
using System.Windows.Shapes;
using drawing_application.Commands;

namespace drawing_application
{
    public class SaveLoadManager
    {
        string textfile;

        public SaveLoadManager()
        {         
            textfile = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "savedata.txt");
        }

        public void LoadProgramState()
        {
            foreach (string line in File.ReadAllLines(textfile))
            {
                // create shape here.
                
            }        
        }

        public void SaveProgramState()
        {
            File.WriteAllText(textfile,"");

            foreach (Shape shape in MainWindow.ins.draw_canvas.Children)
            { 
                File.AppendAllText(textfile,$"{shape.GetType().Name} {Canvas.GetLeft(shape)} {Canvas.GetTop(shape)} {shape.Width} {shape.Height}\n");               
            }
        }

        public void ClearFile()
        {
            File.Delete(textfile);
        }


    }
}
