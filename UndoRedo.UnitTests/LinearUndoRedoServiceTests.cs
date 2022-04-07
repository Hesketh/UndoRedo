using Microsoft.VisualStudio.TestTools.UnitTesting;
using UndoRedo.Tests.Mocks;

namespace UndoRedo.Tests
{
    [TestClass]
    public class LinearUndoRedoServiceTests
    {
        [TestMethod]
        public void CommandExecutedWhenAble()
        {
            const int MaximumValue = 5;
            const int MinimumValue = 0;
            const int InitialValue = 0;
            const int ExpectedValue = InitialValue + 1;

            int value = InitialValue;

            LinearUndoRedoService linearUndoRedoService = new LinearUndoRedoService();

            MockUndoRedoCommand incrementByOneCommand = new MockUndoRedoCommand(nameof(incrementByOneCommand), nameof(incrementByOneCommand), 
                () => { value++; }, () => { return value < MaximumValue; },
                () => { value--; }, () => { return value > MinimumValue; });

            linearUndoRedoService.Do(incrementByOneCommand);

            Assert.AreEqual(ExpectedValue, value);
        }

        [TestMethod]
        public void CommandNotExecutedWhenNotAble()
        {
            const int MaximumValue = 0;
            const int MinimumValue = 0;
            const int InitialValue = 0;
            const int ExpectedValue = 0;

            int value = InitialValue;

            LinearUndoRedoService linearUndoRedoService = new LinearUndoRedoService();

            MockUndoRedoCommand incrementByOneCommand = new MockUndoRedoCommand(nameof(incrementByOneCommand), nameof(incrementByOneCommand),
                () => { value++; }, () => { return value < MaximumValue; },
                () => { value--; }, () => { return value > MinimumValue; });

            linearUndoRedoService.Do(incrementByOneCommand);

            Assert.AreEqual(ExpectedValue, value);
        }

        [TestMethod]
        public void CommandNotUndoneWhenNotAble()
        {
            const int MaximumValue = 5;
            const int MinimumValue = 1;
            const int InitialValue = 0;
            const int ExpectedValue = InitialValue + 1;

            int value = InitialValue;

            LinearUndoRedoService linearUndoRedoService = new LinearUndoRedoService();

            MockUndoRedoCommand incrementByOneCommand = new MockUndoRedoCommand(nameof(incrementByOneCommand), nameof(incrementByOneCommand),
                () => { value++; }, () => { return value < MaximumValue; },
                () => { value--; }, () => { return value > MinimumValue; });

            linearUndoRedoService.Do(incrementByOneCommand);
            linearUndoRedoService.Undo();

            Assert.AreEqual(ExpectedValue, value);
        }

        [TestMethod]
        public void CommandUndoWhenAble()
        {
            const int MaximumValue = 5;
            const int MinimumValue = 0;
            const int InitialValue = 0;
            const int ExpectedValue = InitialValue;

            int value = InitialValue;

            LinearUndoRedoService linearUndoRedoService = new LinearUndoRedoService();

            MockUndoRedoCommand incrementByOneCommand = new MockUndoRedoCommand(nameof(incrementByOneCommand), nameof(incrementByOneCommand),
                () => { value++; }, () => { return value < MaximumValue; },
                () => { value--; }, () => { return value > MinimumValue; });

            linearUndoRedoService.Do(incrementByOneCommand);
            linearUndoRedoService.Undo();

            Assert.AreEqual(ExpectedValue, value);
        }

        [TestMethod]
        public void CommandAddToUndoAfterExecution()
        {
            LinearUndoRedoService linearUndoRedoService = new LinearUndoRedoService();

            MockUndoRedoCommand emptyCommand = new MockUndoRedoCommand(nameof(emptyCommand), nameof(emptyCommand),
                () => { }, () => { return true; },
                () => { }, () => { return true; });

            linearUndoRedoService.Do(emptyCommand);

            Assert.AreEqual(emptyCommand, linearUndoRedoService.GetNextCommandToBeUndone());
        }

        [TestMethod]
        public void CommandRemovedFromNextUndoAfterUndo()
        {
            LinearUndoRedoService linearUndoRedoService = new LinearUndoRedoService();

            MockUndoRedoCommand emptyCommand = new MockUndoRedoCommand(nameof(emptyCommand), nameof(emptyCommand),
                () => { }, () => { return true; },
                () => { }, () => { return true; });

            linearUndoRedoService.Do(emptyCommand);
            linearUndoRedoService.Undo();

            Assert.AreNotEqual(emptyCommand, linearUndoRedoService.GetNextCommandToBeUndone());
        }

        [TestMethod]
        public void CommandAddToRedoAfterUndo()
        {
            LinearUndoRedoService linearUndoRedoService = new LinearUndoRedoService();

            MockUndoRedoCommand emptyCommand = new MockUndoRedoCommand(nameof(emptyCommand), nameof(emptyCommand),
                () => { }, () => { return true; },
                () => { }, () => { return true; });

            linearUndoRedoService.Do(emptyCommand);
            linearUndoRedoService.Undo();

            Assert.AreEqual(emptyCommand, linearUndoRedoService.GetNextCommandToBeRedone());
        }

        [TestMethod]
        public void CommandRemovedFromNextRedoAfterRedo()
        {
            LinearUndoRedoService linearUndoRedoService = new LinearUndoRedoService();

            MockUndoRedoCommand emptyCommand = new MockUndoRedoCommand(nameof(emptyCommand), nameof(emptyCommand),
                () => { }, () => { return true; },
                () => { }, () => { return true; });

            linearUndoRedoService.Do(emptyCommand);
            linearUndoRedoService.Undo();
            linearUndoRedoService.Redo();

            Assert.AreNotEqual(emptyCommand, linearUndoRedoService.GetNextCommandToBeRedone());
        }

        [TestMethod]
        public void NoRedoToBeDoneAfterDo()
        {
            LinearUndoRedoService linearUndoRedoService = new LinearUndoRedoService();

            MockUndoRedoCommand emptyCommand = new MockUndoRedoCommand(nameof(emptyCommand), nameof(emptyCommand),
                () => { }, () => { return true; },
                () => { }, () => { return true; });

            linearUndoRedoService.Do(emptyCommand);
            linearUndoRedoService.Undo();
            linearUndoRedoService.Do(emptyCommand);

            Assert.IsNull(linearUndoRedoService.GetNextCommandToBeRedone());
        }
    }
}
