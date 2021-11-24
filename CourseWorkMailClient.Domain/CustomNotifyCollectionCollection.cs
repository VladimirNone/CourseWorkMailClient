﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWorkMailClient.Domain
{
    public class CustomNotifyCollectionCollection<T> : INotifyCollectionChanged, IEnumerable
    {
        private Collection<T> collection = new Collection<T>();
        public event NotifyCollectionChangedEventHandler CollectionChanged;

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
            OnCollectionChange(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, newItem, oldItem, index));
        }

        protected virtual void OnCollectionChange(NotifyCollectionChangedEventArgs e)
        {
            if (CollectionChanged != null)
                CollectionChanged(this, e);
        }

        public IEnumerator GetEnumerator()
        {
            return collection.GetEnumerator();
        }
    }
}