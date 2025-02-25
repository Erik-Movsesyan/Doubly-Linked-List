﻿using System;
using System.Collections;
using System.Collections.Generic;
using MyCollections.DoublyLinkedList;

namespace DoublyLinkedList
{
    public class DoublyLinkedListEnumerator<T>(DoublyLinkedList<T> list) : IEnumerator<T>
    {
        private DoublyLinkedListNode<T>? _node = list.First;
        private T? _current ;
        private int _index;

        public T Current => GetCurrent()!;
        object? IEnumerator.Current => GetCurrent();

        public bool MoveNext()
        {
            if (_node == null)
            {
                _index++;
                return false;
            }

            ++_index;
            _current = _node.Value;
            _node = _node == list.Last ? null : _node!.Next;
            return true;
        }

        public void Reset()
        {
            _current = default;
            _node = list.First;
            _index = 0;
        }

        public void Dispose() { }

        private T? GetCurrent()
        {
            if (_index == 0 || _index == list.Length + 1)
            {
                throw new InvalidOperationException("Enumeration has either not started or has already finished");
            }

            return _current;
        }
    }
}
