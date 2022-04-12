using System;
using UndoRedo.Interfaces;

namespace UndoRedo.Events
{
    public class CommandPerformedEventArgs : EventArgs
    {
        public IUndoRedoCommand Command { get; private set; }

        public CommandPerformedEventArgs(IUndoRedoCommand command)
        {
            Command = command;
        }
    }
}
