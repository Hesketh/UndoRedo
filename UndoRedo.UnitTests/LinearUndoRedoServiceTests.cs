using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using UndoRedo.Interfaces;
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
        public void EventFiresWhenDo()
        {
            bool eventFired = false;

            LinearUndoRedoService linearUndoRedoService = new LinearUndoRedoService();
            linearUndoRedoService.OnCommandDone += (o, e) => { eventFired = true; };

            MockUndoRedoCommand mockCommand = new MockUndoRedoCommand(nameof(mockCommand), nameof(mockCommand),
                () => {  }, () => { return true; },
                () => {  }, () => { return true; });

            linearUndoRedoService.Do(mockCommand);

            Assert.IsTrue(eventFired);
        }

        [TestMethod]
        public void EventFiresWhenUndo()
        {
            bool eventFired = false;

            LinearUndoRedoService linearUndoRedoService = new LinearUndoRedoService();

            MockUndoRedoCommand mockCommand = new MockUndoRedoCommand(nameof(mockCommand), nameof(mockCommand),
                () => { }, () => { return true; },
                () => { }, () => { return true; });

            linearUndoRedoService.Do(mockCommand);
            
            linearUndoRedoService.OnCommandUndone += (o, e) => { eventFired = true; };
            linearUndoRedoService.Undo();

            Assert.IsTrue(eventFired);
        }

        [TestMethod]
        public void EventFiresWhenRedo()
        {
            bool eventFired = false;

            LinearUndoRedoService linearUndoRedoService = new LinearUndoRedoService();

            MockUndoRedoCommand mockCommand = new MockUndoRedoCommand(nameof(mockCommand), nameof(mockCommand),
                () => { }, () => { return true; },
                () => { }, () => { return true; });

            linearUndoRedoService.Do(mockCommand);
            linearUndoRedoService.Undo();

            linearUndoRedoService.OnCommandRedone += (o, e) => { eventFired = true; };
            linearUndoRedoService.Redo();

            Assert.IsTrue(eventFired);
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

        [TestMethod]
        public void TimelineSequencedCorrectly()
        {
            // It can be argued that this test should be reduced in size
            LinearUndoRedoService linearUndoRedoService = new LinearUndoRedoService();

            Action doNothing = new Action(() => { });
            Func<bool> alwaysTrue = new Func<bool>(() => { return true; });

            MockUndoRedoCommand mockCommand1 = new MockUndoRedoCommand("1", "", doNothing, alwaysTrue, doNothing, alwaysTrue);
            MockUndoRedoCommand mockCommand2 = new MockUndoRedoCommand("2", "", doNothing, alwaysTrue, doNothing, alwaysTrue);
            MockUndoRedoCommand mockCommand3 = new MockUndoRedoCommand("3", "", doNothing, alwaysTrue, doNothing, alwaysTrue);
            MockUndoRedoCommand mockCommand4 = new MockUndoRedoCommand("4", "", doNothing, alwaysTrue, doNothing, alwaysTrue);
            MockUndoRedoCommand mockCommand5 = new MockUndoRedoCommand("5", "", doNothing, alwaysTrue, doNothing, alwaysTrue);
            MockUndoRedoCommand mockCommand6 = new MockUndoRedoCommand("6", "", doNothing, alwaysTrue, doNothing, alwaysTrue);

            // 1 to 6 are executed. Therefore 6 will be next to be undo (the top) and 1 will be the bottom
            linearUndoRedoService.Do(mockCommand1);
            linearUndoRedoService.Do(mockCommand2);
            linearUndoRedoService.Do(mockCommand3);
            linearUndoRedoService.Do(mockCommand4);
            linearUndoRedoService.Do(mockCommand5);
            linearUndoRedoService.Do(mockCommand6);

            /// Expected Result
            // 6 - top of undo stack
            // 5
            // 4
            // 3
            // 2
            // 1 - bottom of undo stack
            ///

            var historyAfterDos = new List<IUndoRedoCommand>(linearUndoRedoService.Timeline);
            for (int i = 5; i >= 0; i--)
            {
                string expectedName = (i + 1).ToString();
                Assert.AreEqual(expectedName, historyAfterDos[i].Name);
            }

            // Next we try Undoing some of the history, this technically should not change the time line
            linearUndoRedoService.Undo();
            linearUndoRedoService.Undo();
            linearUndoRedoService.Undo();

            /// Expected Result
            // 6 -- undone, therefore at bottom of redo stack
            // 5 -- undone, therefore in the middle of redo stack
            // 4 -- undone, therefore at the top of the redo stack
            // 3 -- at the top of the undo stack
            // 2
            // 1 -- at the bottom of the undo stack
            ///

            var historyAfterUndos = new List<IUndoRedoCommand>(linearUndoRedoService.Timeline);
            for (int i = 5; i >= 0; i--)
            {
                string expectedName = (i + 1).ToString();
                Assert.AreEqual(expectedName, historyAfterDos[i].Name);
            }

            // Next we try redoing. Again nothing should change
            linearUndoRedoService.Redo();

            /// Expected Result
            // 6 -- undone earlier, therefore at bottom of redo stack
            // 5 -- undone earlier, therefore at the top of the redo stack
            // 4 -- redone, therefore at the top of the undo stack
            // 3
            // 2
            // 1 -- at the bottom of the undo stack
            ///

            var historyAfterRedos = new List<IUndoRedoCommand>(linearUndoRedoService.Timeline);
            for (int i = 5; i >= 0; i--)
            {
                string expectedName = (i + 1).ToString();
                Assert.AreEqual(expectedName, historyAfterDos[i].Name);
            }

            // Next check that doing an action clears the redo history
            while (linearUndoRedoService.CanRedo())
            {
                linearUndoRedoService.Redo();
            }


            linearUndoRedoService.Undo();
            linearUndoRedoService.Undo();
            linearUndoRedoService.Undo();
            linearUndoRedoService.Undo();
            linearUndoRedoService.Undo();

            linearUndoRedoService.Do(mockCommand4);

            /// Expected Result
            // 4
            // 1
            ///

            var historyFinal = new List<IUndoRedoCommand>(linearUndoRedoService.Timeline);
            Assert.AreEqual(2, historyFinal.Count);
            Assert.AreEqual("4", historyFinal[1].Name);
            Assert.AreEqual("1", historyFinal[0].Name);
        }
    }
}
