using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using drawing_application.CustomShapes;

namespace drawing_application
{
    public class SaveLoadManager
    {
        // path to the text file.
        private readonly string textFile;

        private List<List<string>> lines;

        private int index = 0;

        public SaveLoadManager()
        {         
            // get the save file on the desktop.
            textFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data.txt");
        }



        public void SaveProgramState()
        {

            if (!Hierarchy.GetInstance().GetTopGroup().GetChildren().Any())
            {
                File.Delete(textFile);
                return;
            }

                
            File.WriteAllText(textFile, "");
            // disable the outline.
            Selection.GetInstance().ToggleOutline(false);
            // get their type and transform and write it to the file.
            File.AppendAllText(textFile, $"{Hierarchy.GetInstance().GetTopGroup().ToString(0)}");

        }

        public Group LoadProgramState()
        {
            // if the text file already exist, return.
            if (File.Exists(textFile) == false)
            {
                return new Group();
            }

            lines = File.ReadAllLines(textFile).Select(x => x.Trim().Split(" ").ToList()).ToList();

            return LoadGroup();
        }

        private Group LoadGroup()
        {
            // initialize a new group
            var group = new Group();
            // look in the file how many children it has.
            var count = Convert.ToInt32(lines[index].Last());
            // jump to the next line in the file.
            index++;
            // loop through the amount of children it has.
            for (var i = 0; i < count; i++)
            {
                
                // retrieve the line.
                var currentLine = lines[index];

                // check if this line is a group.
                if (currentLine.Count == 2)
                {
                    // recursively add the group to this group.
                    group.AddChild(LoadGroup());
                }
                else
                {
                    // if its not a group create a shape and add it to this group.
                    group.AddChild(CreateShape(currentLine));
                }
                // add one to the current line index
                index++;
            }
            // return the group.
            return group;
        }




        private CustomShape CreateShape(IReadOnlyList<string> line)
        {
            // initialize a shape based on their type.
            var shape = (CustomShape)Activator.CreateInstance(MainWindow.ins.styles[GetStyleIndex(line.First())]);
            // convert the text data to integers to assign the transform of the shape.
            var transformData = line.Skip(1).Select(x=>Convert.ToInt32(x)).ToList();
            // set the position of the shape.
            Canvas.SetLeft(shape, transformData[0]);
            Canvas.SetTop (shape, transformData[1]);
            // set the transform of the shape.
            shape.Width  = transformData[2];
            shape.Height = transformData[3];
            // return the shape.
            return shape;
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
