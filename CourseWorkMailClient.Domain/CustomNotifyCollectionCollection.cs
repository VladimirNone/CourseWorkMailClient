using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWorkMailClient.Domain
{
    public class CustomNotifyCollectionCollection<T> : INotifyCollectionChanged, IEnumerable<T>
    {
        private List<T> collection = new List<T>();
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public CustomNotifyCollectionCollection()
        {
            
        }

        public CustomNotifyCollectionCollection(List<T> list)
        {
            foreach (var item in list)
            {
                collection.Add(item);
            }
        }

        public void Add(T item)
        {
            collection.Insert(collection.Count, item);
            OnCollectionChange(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));
        }

        public void Replace(T oldItem, T newItem)
        {
            var index = collection.IndexOf(oldItem);
            collection[index] = newItem;
            OnCollectionChange(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, newItem, oldItem, index));
        }

        public void Delete(T item)
        {
            var index = collection.IndexOf(item);
            collection.Remove(item);
            OnCollectionChange(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, index));
        }

        public void Reset(List<T> items)
        {
            collection.Clear();
            foreach (var item in items)
            {
                collection.Add(item);
            }

            OnCollectionChange(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        protected virtual void OnCollectionChange(NotifyCollectionChangedEventArgs e)
        {
            if (CollectionChanged != null)
                CollectionChanged(this, e);
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return collection.GetEnumerator();
        }

        public IEnumerator GetEnumerator()
        {
            return collection.GetEnumerator();
        }
    }
}
