using System;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using drawing_application.Commands;
using drawing_application.CustomShapes;

namespace drawing_application
{
    public class SaveLoadManager
    {
        // path to the text file.
        private readonly string textFile;

        public SaveLoadManager()
        {         
            // get the save file on the desktop.
            textFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "data.txt");
        }

        public void LoadProgramState()
        {
            // if the text file already exist, return.
            if (File.Exists(textFile) == false)
            {
                return;
            }

            // loop through all the lines in the textFile.
            foreach (var line in File.ReadAllLines(textFile))
            {
                // split the string with each space
                var data = line.Split(" ");
                // check if the first word is a rectangle or a ellipse, then convert the rest of the data to integers.
                new StopDrawCommand(GetStyleIndex(data[0]), data.Skip(1).Select(x=>Convert.ToInt32(x)).ToArray()).Execute();
            }
        }

        public void SaveProgramState()
        {
            // clear the file.
            File.WriteAllText(textFile, "");
            // disable the outline.
            Selection.GetInstance().ToggleOutline(false);

            // loop through all the custom shapes
            foreach (var shape in Hierarchy.GetInstance().GetTopGroup().GetChildren())
            {
                // get their type and transform and write it to the file.
                File.AppendAllText(textFile,$"{shape.ToString(0)}\n");
            }
        }

        public void ClearFile()
        {
            // delete the safe file.
            File.Delete(textFile);
        }

        private static int GetStyleIndex(string style)
        {
            for(var i=0;i<MainWindow.ins.styles.Length;i++)
            {
                if (MainWindow.ins.styles[i].Name == style)
                {
                    return i;             
                }        
            }
            return 0;
        }


    }
}
