using System.Collections.Generic;
using UndoRedo.Interfaces;

namespace UndoRedo
{
    public class LinearUndoRedoService : IUndoRedoService
    {
        private readonly Stack<IUndoRedoCommand> _undoStack = new Stack<IUndoRedoCommand>();
        private readonly Stack<IUndoRedoCommand> _redoStack = new Stack<IUndoRedoCommand>();

        public void Do(IUndoRedoCommand command)
        {
            if (command.CanDo())
            {
                command.Do();

                _undoStack.Push(command);
                _redoStack.Clear();
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
