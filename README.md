# UndoRedo

Easy to use .NET Standard library for adding history tracking to an application. 
Implement your data editing code as a undoable command using the IUndoRedoCommand and execute them through an instance of LinearUndoRedoSystem.

## Interfaces
| Name | Purpose | Implementations |
|--|--|--|
|IUndoRedoCommand| Your commands that should be able to be undone/redone should implement this interface | N/A |
|IUndoRedoSystem| An instance is to be used to execute 'IUndoRedoCommand's and this will manage what to Undo/Redo when prompted | LinearUndoRedoSystem |

## Licence
Copyright (c) 2022 Alex Hesketh

Permission is hereby granted, free of charge, to any person obtaining
a copy of this software and associated documentation files (the
"Software"), to deal in the Software without restriction, including
without limitation the rights to use, copy, modify, merge, publish,
distribute, sublicense, and/or sell copies of the Software, and to
permit persons to whom the Software is furnished to do so, subject to
the following conditions:

The above copyright notice and this permission notice shall be
included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
