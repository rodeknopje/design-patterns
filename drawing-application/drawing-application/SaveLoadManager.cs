using System;
using System.IO;
using System.Linq;
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
            if (File.Exists(textfile) == false)
            {
                return;
            }

            foreach (string line in File.ReadAllLines(textfile))
            {
                // split the string with each spacee
                var data = line.Split(" ");
                // check if the first word is a rectangle or a ellipse, then convert the rest of the data to ints.
                new StopDrawCommand((data[0][0]=='R'?1:0), data.Skip(1).Select(x=>Convert.ToInt32(x)).ToArray()).Execute();
            }
        }

        public void SaveProgramState()
        {
            File.WriteAllText(textfile, "");

            MainWindow.ins.DeleteSelectionItems();

            foreach (Shape shape in MainWindow.ins.draw_canvas.Children)
            {
                File.AppendAllText(textfile, $"{shape.GetType().Name} {(int)Canvas.GetLeft(shape)} {(int)Canvas.GetTop(shape)} {(int)shape.Width} {(int)shape.Height}\n");
            }
        }

        public void ClearFile()
        {
            File.Delete(textfile);
        }


    }
}
