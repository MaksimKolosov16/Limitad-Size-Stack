using System.Collections.Generic;

namespace LimitedSizeStack
{
    public class LimitedSizeStack<T>
    {
        private readonly int _limit;
        private readonly LinkedList<T> _linkedList = new();
        public int Count => _linkedList.Count; 

        public LimitedSizeStack(int limit)
        {
            _limit = limit;
        }
        
        public void Push(T item)
        {
            if (_limit == 0) return;
            if (_linkedList.Count == _limit)
                _linkedList.RemoveFirst();
            _linkedList.AddLast(item);
        }

        public T Pop()
        {
            var result = _linkedList.Last.Value;
            _linkedList.RemoveLast();
            return result;
        }
    }
}