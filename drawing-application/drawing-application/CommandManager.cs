using System.Collections.Generic;
using drawing_application.Commands;

namespace drawing_application
{
    public class CommandManager
    {
        private static CommandManager _instance;
        // list of all commands.
        private readonly List<Command> commands;
        // counter of the current command.
        private int counter = -1;



        private CommandManager()
        {
            _instance = this;

            commands = new List<Command>();
        }

        public  static CommandManager GetInstance()
        {
            return _instance ?? new CommandManager();
        }

        public void InvokeCommand(Command cmd)
        {
            // if the current command is not the last.
            while (commands.Count - 1 > counter)
            {
                // remove al commands after the current command.
                commands.RemoveAt(counter+1);
            }
            // execute the new command.
            cmd.Execute();
            // add it command list.
            commands.Add(cmd);
            // increase the counter, so this is now the current command.
            counter++;
        }

        public void Undo()
        {
            // if we are not at te begin of the command list.
            if (counter >= 0)
            {
                // undo the current command, then decrease it.
                commands[counter--].Undo();               
            }
        }

        public void Redo()
        {
            // if we are not at the end of the command list.
            if (counter < commands.Count - 1)
            {
                // increase the current command then execute it
                commands[++counter].Execute();          
            }
        }


    }
}
