namespace UndoRedo.Interfaces
{
    public interface IUndoRedoCommand
    {
        string Name { get; }
        string Description { get; }

        void Do();
        bool CanDo();

        void Undo();
        bool CanUndo();
    }
}
