using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Controls;
using drawing_application.CustomShapes;

namespace drawing_application
{
    public class Controller
    {
        private readonly Hierarchy hierarchy;

        private readonly Canvas drawCanvas;

        private readonly SaveLoadManager saveLoadManager;

        private readonly CommandManager commandManager;

        private Point mouseOrigin;

        private States state;


        public Controller(StackPanel buttonPanel, StackPanel stylePanel, Canvas canvas, Button clearButton, Button styleButton , Button redoButton, Button undoButton, Action onClose)
        {
            InitializeMouseActions();


            onClose = () => saveLoadManager.SaveProgramState();
        }

        private void InitializeMouseActions()
        {


            drawCanvas.MouseLeftButtonDown += (a, b) =>
            {



            };

            drawCanvas.MouseLeftButtonUp += (a, b) =>
            {



            };

            drawCanvas.MouseMove += (a, b) =>
            {



            };


        }




    }


    public enum States
    {
        None,
        Draw,
        Select,
        Move,
        Resize,
    }
}
