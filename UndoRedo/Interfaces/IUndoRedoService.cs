namespace UndoRedo.Interfaces
{
    public interface IUndoRedoService
    {
        void Do(IUndoRedoCommand command);

        void Undo();
        void Redo();
        
        bool CanUndo();
        bool CanRedo();

        IUndoRedoCommand GetNextCommandToBeUndone();
        IUndoRedoCommand GetNextCommandToBeRedone();
    }
}
