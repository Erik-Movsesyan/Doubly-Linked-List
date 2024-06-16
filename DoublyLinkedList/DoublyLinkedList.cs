using DoublyLinkedList;
using System;
using System.Collections;
using System.Collections.Generic;

namespace MyCollections.DoublyLinkedList
{
    public class DoublyLinkedList<T>: IEnumerable<T>
    {
        public DoublyLinkedListNode<T>? First { get; private set; }
        public DoublyLinkedListNode<T>? Last { get; private set; }
        public int Length { get; private set; }

        public DoublyLinkedList() { }

        public DoublyLinkedList(IEnumerable<T> collection)
        {
            ArgumentNullException.ThrowIfNull(collection);

            foreach (T item in collection)
            {
                AddLast(item);
            }
        }

        public DoublyLinkedListNode<T> AddLast(T value)
        {
            var newNode = new DoublyLinkedListNode<T>(this, value);
            AddLastInternal(newNode);
            return newNode;
        }

        public DoublyLinkedListNode<T> AddLast(DoublyLinkedListNode<T>? newNode)
        {
            ValidateNewNode(newNode);

            AddLastInternal(newNode!);
            newNode!.List = this;
            return newNode;
        }

        public DoublyLinkedListNode<T> AddFirst(T? value)
        {
            var newNode = new DoublyLinkedListNode<T>(this, value);
            AddFirstInternal(newNode);
            return newNode;
        }

        public DoublyLinkedListNode<T> AddFirst(DoublyLinkedListNode<T>? newNode)
        {
            ValidateNewNode(newNode);

            AddFirstInternal(newNode!);
            newNode!.List = this;
            return newNode;
        }

        public DoublyLinkedListNode<T> AddAfter(DoublyLinkedListNode<T>? node, T? value)
        {
            ValidateNode(node);
            var newNode = new DoublyLinkedListNode<T>(this, value);

            AddAfterInternal(node!, newNode);
            return newNode;
        }

        public DoublyLinkedListNode<T> AddAfter(DoublyLinkedListNode<T>? node, DoublyLinkedListNode<T>? newNode)
        {
            ValidateNode(node);
            ValidateNewNode(newNode);

            AddAfterInternal(node!, newNode!);
            newNode!.List = this;
            return newNode;
        }

        public DoublyLinkedListNode<T> AddBefore(DoublyLinkedListNode<T>? node, T? value)
        {
            ValidateNode(node);
            var newNode = new DoublyLinkedListNode<T>(this, value);

            if(node!.Previous == null)
            {
                AddFirstInternal(newNode);
            }
            else
            {
                AddAfterInternal(node.Previous, newNode);
            }

            return newNode;
        }

        public DoublyLinkedListNode<T> AddBefore(DoublyLinkedListNode<T>? node, DoublyLinkedListNode<T>? newNode)
        {
            ValidateNode(node);
            ValidateNewNode(newNode);

            if (node!.Previous == null)
            {
                AddFirstInternal(newNode);
            }
            else
            {
                AddAfterInternal(node.Previous, newNode);
            }
            newNode.List = this;

            return newNode;
        }

        public bool Remove(T? value)
        {
            var nodeToRemove = Find(value);
            if(nodeToRemove != null)
            {
                RemoveNodeInternal(nodeToRemove);
                return true;
            }

            return false;
        }

        public void Remove(DoublyLinkedListNode<T>? nodeToRemove)
        {
            ValidateNode(nodeToRemove);
            RemoveNodeInternal(nodeToRemove);
        }

        public void RemoveFirst()
        {
            if (First == null)
                throw new InvalidOperationException("The SinglyLinkedList is empty");

            RemoveNodeInternal(First);
        }

        public void RemoveLast()
        {
            if (First == null)
                throw new InvalidOperationException("The SinglyLinkedList is empty");

            RemoveNodeInternal(Last);
        }

        public void Clear()
        {
            DoublyLinkedListNode<T>? current = First;
            while (current != null)
            {
                DoublyLinkedListNode<T> temp = current;
                current = current.Next;
                temp.Invalidate();
            }

            First = null;
            Last = null;
            Length = 0;
        }

        public bool Contains(T? value) => Find(value) != null;

        public DoublyLinkedListNode<T>? Find(T? value)
        {
            var node = First;
            var comparer = EqualityComparer<T>.Default;

            if (node != null)
            {
                if (value != null)
                {
                    DoublyLinkedListNode<T>? nextNode;
                    do
                    {
                        if (comparer.Equals(node!.Value, value))
                        {
                            return node;
                        }
                        nextNode = node.Next;
                        node = nextNode;

                    } while (nextNode != null);
                }
                else
                {
                    do
                    {
                        if (node.Value == null)
                        {
                            return node;
                        }
                        node = node.Next;

                    } while (node != null);
                }
            }
            return null;
        }

        public DoublyLinkedListNode<T>? FindLast(T? value)
        {
            var node = First;
            var comparer = EqualityComparer<T>.Default;
            DoublyLinkedListNode<T>? resultNode = null;

            if (node != null)
            {
                if (value != null)
                {
                    DoublyLinkedListNode<T>? nextNode;
                    do
                    {
                        if (comparer.Equals(node!.Value, value))
                        {
                            resultNode = node;
                        }
                        nextNode = node.Next;
                        node = nextNode;

                    } while (nextNode != null);

                    return resultNode;
                }
                else
                {
                    do
                    {
                        if (node.Value == null)
                        {
                            resultNode = node;
                        }
                        node = node.Next;

                    } while (node != null);

                    return resultNode;
                }
            }
            return null;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new DoublyLinkedListEnumerator<T>(this);
        }

        private void AddFirstInternal(DoublyLinkedListNode<T> newNode)
        {
            if (First == null)
            {
                AddToEmptyList(newNode);
            }
            else
            {
                newNode.Next = First;
                First.Previous = newNode;
                First = newNode;
                Length++;
            }
        }

        private void AddLastInternal(DoublyLinkedListNode<T> newNode)
        {
            if (First == null)
            {
                AddToEmptyList(newNode);
            }
            else
            {
                newNode.Previous = Last;
                Last!.Next = newNode;
                Last = newNode;
                Length++;
            }
        }

        private void AddAfterInternal(DoublyLinkedListNode<T> node, DoublyLinkedListNode<T> newNode)
        {
            if (Last == node)
            {
                AddLastInternal(newNode);
            }
            else
            {
                newNode.Next = node.Next;
                newNode.Previous = node;
                node.Next!.Previous = newNode;
                node.Next = newNode;
                Length++;
            }
        }

        private void AddToEmptyList(DoublyLinkedListNode<T> newNode)
        {
            First = newNode;
            Last = newNode;
            Length++;
        }

        private void RemoveNodeInternal(DoublyLinkedListNode<T>? nodeToRemove)
        {
            if (nodeToRemove!.Previous != null)
            {
                nodeToRemove.Previous.Next = nodeToRemove.Next;

                if (nodeToRemove == Last)
                {
                    Last = nodeToRemove.Previous;
                }
                else
                {
                    nodeToRemove.Next!.Previous = nodeToRemove.Previous;
                }
            }
            else
            {
                if(nodeToRemove.Next != null)
                {
                    nodeToRemove.Next.Previous = null;
                    First = nodeToRemove.Next;
                }
                else
                {
                    First = null;
                    Last = null;
                }
            }

            nodeToRemove.Invalidate();
            Length--;
        }

        private static void ValidateNewNode(DoublyLinkedListNode<T>? node)
        {
            ArgumentNullException.ThrowIfNull(node);

            if (node.List != null)
            {
                throw new InvalidOperationException("Node is already part of a different DoublyLinkedList");
            }
        }

        private void ValidateNode(DoublyLinkedListNode<T>? node)
        {
            ArgumentNullException.ThrowIfNull(node);

            if (node.List != this)
            {
                throw new InvalidOperationException("The node does not belong to the current DoublyLinkedList");
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public sealed class DoublyLinkedListNode<T>
    {
        public DoublyLinkedList<T>? List { get; internal set; }
        public DoublyLinkedListNode<T>? Next { get; internal set; }
        public DoublyLinkedListNode<T>? Previous { get; internal set; }
        public T? Value { get; }

        internal DoublyLinkedListNode(DoublyLinkedList<T> list, T? value)
        {
            List = list;
            Value = value;
        }

        public DoublyLinkedListNode(T? value)
        {
            Value = value;
        }

        public void Invalidate()
        {
            List = null;
            Next = null;
            Previous = null;
        }
    }
}