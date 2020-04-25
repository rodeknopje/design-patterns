﻿using System;
using System.Windows;
using System.Windows.Controls;

namespace drawing_application.Commands
{
    public class StartMoveCommand : Command
    {
        // the current mouse position.
        private readonly Point mousePos;

        public StartMoveCommand(Point mousePos)
        {
            // assign the mouse position.
            this.mousePos = mousePos;
        }

        public override void Execute()
        {
            // switch to the move state.
            M.SwitchState(states.move);
            // set the mouse origin.
            M.orgin_mouse = mousePos;
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }
    }
}
