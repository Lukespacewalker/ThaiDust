using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Reactive.Linq;
using System.Text;

namespace ThaiDust.Core.Extensions
{
    public static class ObservableCollectionExtension
    {
        public static IObservable<System.Reactive.EventPattern<NotifyCollectionChangedEventArgs>> GetCollectionChangedObservable<T>(this ObservableCollection<T> collection)
        {
            return Observable.FromEventPattern<NotifyCollectionChangedEventHandler, NotifyCollectionChangedEventArgs>(handler => collection.CollectionChanged += handler, handler => collection.CollectionChanged -= handler);
        }
    }
}
