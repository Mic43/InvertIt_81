using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Infrastructure
{
    public class UndoRedoCollection<T>
    {
        private readonly Stack<T> _undoStack;
        private readonly Stack<T> _redoStack;

        public UndoRedoCollection(IEnumerable<T> undoStack, IEnumerable<T> redoStack)
        {
            if (undoStack == null) throw new ArgumentNullException("undoStack");
            if (redoStack == null) throw new ArgumentNullException("redoStack");
            _undoStack = new Stack<T>(undoStack);
            _redoStack = new Stack<T>(redoStack);
        }

        public UndoRedoCollection(): this(new Stack<T>(),new Stack<T>() )
        {
            
        }

        public void Add(T element)
        {
            UndoStack.Push(element);
            RedoStack.Clear();
        }
        public T Redo()
        {
            if (!CanRedo())
                throw new InvalidOperationException("Cannot redo -collection is empty");
            var redoMoveCoord = RedoStack.Pop();
            UndoStack.Push(redoMoveCoord);
            return redoMoveCoord;
        }
        public T Undo()
        {
            if (!CanUndo())
                throw new InvalidOperationException("Cannot undo -collection is empty");
            var lastMoveCoord = UndoStack.Pop();
            RedoStack.Push(lastMoveCoord);

            return lastMoveCoord;
        }

        public bool CanUndo()
        {
            return UndoStack.Count > 0;
        }
        public bool CanRedo()
        {
            return RedoStack.Count > 0;
        }
        public Collection<T> Elements
        {
            get
            {
                return new Collection<T>(UndoStack.AsEnumerable().Reverse().ToList());
            }
        }

        public Stack<T> UndoStack
        {
            get { return _undoStack; }
        }

        public Stack<T> RedoStack
        {
            get { return _redoStack; }
        }

        public void Clear()
        {
            RedoStack.Clear();
            UndoStack.Clear();
        }
    }
}