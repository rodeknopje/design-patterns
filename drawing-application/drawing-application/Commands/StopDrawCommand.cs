﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace drawing_application.Commands
{
    class StopDrawCommand : Command
    {
        public override void Execute()
        {
            m.saveload.WriteShapeToFile(m.shape_drawn);
            // add it to the selection row.
            m.AddToSelectionRow(m.shape_drawn);
            // set the shape to null, so the mousemove event will stop, and the shape wil stay childed to the canvas.
            m.shape_drawn = null;

            m.SwitchState(states.none);
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }
    }
}
