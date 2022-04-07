using System;
using UndoRedo.Interfaces;

namespace UndoRedo.Tests.Mocks
{
    internal class MockUndoRedoCommand : IUndoRedoCommand
    {
        private readonly Action _doAction;
        private readonly Action _undoAction;
        private readonly Func<bool> _canDoAction;
        private readonly Func<bool> _canUndoAction;

        public string Name { get; private set; }
        public string Description { get; private set; }

        public MockUndoRedoCommand(string name, string description, Action doAction, Func<bool> canDoAction, Action undoAction, Func<bool> canUndoAction)
        {
            Name = name;
            Description = description;

            _doAction = doAction;
            _canDoAction = canDoAction;
            _undoAction = undoAction;
            _canUndoAction = canUndoAction;
        }


        public void Do()
        {
            if (CanDo())
            {
                _doAction();
            }
        }

        public bool CanDo()
        {
            return _canDoAction();
        }

        public void Undo()
        {
            if (CanUndo())
            {
                _undoAction();
            }
        }

        public bool CanUndo()
        {
            return _canUndoAction();
        }
    }
}
