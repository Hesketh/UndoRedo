using System;
using System.Collections.Generic;
using System.Linq;
using UndoRedo.Events;
using UndoRedo.Interfaces;

namespace UndoRedo
{
    public partial class LinearUndoRedoService : IUndoRedoService
    {
        public event EventHandler<CommandPerformedEventArgs> OnCommandDone;
        public event EventHandler<CommandPerformedEventArgs> OnCommandUndone;
        public event EventHandler<CommandPerformedEventArgs> OnCommandRedone;

        private readonly Stack<IUndoRedoCommand> _undoStack = new Stack<IUndoRedoCommand>();
        private readonly Stack<IUndoRedoCommand> _redoStack = new Stack<IUndoRedoCommand>();

        public IEnumerable<IUndoRedoCommand> Timeline
        {
            get
            {
                List<IUndoRedoCommand> timeline = new List<IUndoRedoCommand>();
                timeline.AddRange(_redoStack);
                timeline.AddRange(_undoStack.Reverse());
                return timeline;
            }
        }

        public void Do(IUndoRedoCommand command)
        {
            if (command.CanDo())
            {
                command.Do();

                _undoStack.Push(command);
                _redoStack.Clear();
                
                OnCommandDone?.Invoke(this, new CommandPerformedEventArgs(command));
            }
        }

        public void Undo()
        {
            if (_undoStack.Count > 0)
            {
                IUndoRedoCommand topOfStack = _undoStack.Peek();
                if (topOfStack.CanUndo())
                {
                    topOfStack.Undo();

                    _undoStack.Pop();
                    _redoStack.Push(topOfStack);
                    
                    OnCommandUndone?.Invoke(this, new CommandPerformedEventArgs(topOfStack));
                }
            }
        }

        public bool CanUndo()
        {
            if (_undoStack.Count > 0)
            {
                IUndoRedoCommand topOfStack = _undoStack.Peek();
                return topOfStack.CanUndo();
            }
            return false;
        }

        public void Redo()
        {
            if (_redoStack.Count > 0)
            {
                IUndoRedoCommand topOfStack = _redoStack.Peek();
                if (topOfStack.CanDo())
                {
                    topOfStack.Do();

                    _redoStack.Pop();
                    _undoStack.Push(topOfStack);

                    OnCommandRedone?.Invoke(this, new CommandPerformedEventArgs(topOfStack));
                }
            }
        }

        public bool CanRedo()
        {
            if (_redoStack.Count > 0)
            {
                IUndoRedoCommand topOfStack = _redoStack.Peek();
                return topOfStack.CanDo();
            }
            return false;
        }

        public IUndoRedoCommand GetNextCommandToBeUndone()
        {
            return _undoStack.Count > 0 ? _undoStack.Peek() : null;
        }

        public IUndoRedoCommand GetNextCommandToBeRedone()
        {
            return _redoStack.Count > 0 ?_redoStack.Peek() : null;
        }
    }
}
