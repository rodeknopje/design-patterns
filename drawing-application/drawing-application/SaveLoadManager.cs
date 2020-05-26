using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using drawing_application.Buttons;
using drawing_application.CustomShapes;
using drawing_application.Visitors;

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


            //File.AppendAllText(textFile, $"{Hierarchy.GetInstance().GetTopGroup().ToString(0)}");
            File.AppendAllText(textFile, $"{Hierarchy.GetInstance().GetTopGroup().Accept(new WriteVisitor(0))}");
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
            var currentOrnaments = new string[4];
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

                if (currentLine[1].Contains('\"'))
                {
                    i--;

                    var ornament = currentLine[1].Replace('\"', ' ').Trim();

                    switch (currentLine[0])
                    {
                        case "left":   currentOrnaments[0] = ornament; break;
                        case "right":  currentOrnaments[1] = ornament; break;
                        case "top":    currentOrnaments[2] = ornament; break;
                        case "bottom": currentOrnaments[3] = ornament; break;
                    }

                }
                // check if this line is a group.
                else if (currentLine.Count == 2)
                {
                    // recursively add the group to this group.
                    group.AddChild(LoadGroup());
                    // lower the index by one otherwise the recursive call would add one to many.
                    index--;
                }
                else if (currentLine.Count ==5)
                {
                    // if its not a group create a shape and add it to this group.
                    group.AddChild(CreateShape(currentLine,currentOrnaments));

                    currentOrnaments = new string[4];
                }

                index++;
                // add one to the current line index
            }
            // return the group.
            return group;
        }

        private CustomShape CreateShape(IReadOnlyList<string> line, IReadOnlyList<string> ornaments)
        {
            // initialize a shape based on their type.
            var shape = Utility.GetInstance().CreateShape(line.First(),ornaments);
            // convert the text data to integers to assign the transform of the shape.
            var transformData = line.Skip(1).Select(x=>Convert.ToInt32(x)).ToList();
            // set the position of the shape.
            shape.SetLeft(transformData[0]);
            shape.SetTop (transformData[1]);
            // set the transform of the shape.
            shape.SetWidth (transformData[2]);
            shape.SetHeight(transformData[3]);
            // return the shape.
            return shape;
        }
    }
}
