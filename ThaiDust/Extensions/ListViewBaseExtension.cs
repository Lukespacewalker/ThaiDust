using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using DynamicData.Binding;
using ThaiDust.Core.Model.Persistent;
using System.Collections.Specialized;

namespace ThaiDust.Extensions
{
    public static class ListViewBaseExtension
    {
        public static IObservable<T[]> GetSelectedChangedObservable<T>(this ListView listview)
        {
            return Observable.FromEvent<SelectionChangedEventHandler, (object, SelectionChangedEventArgs)>(
                    rxHandler => (sender, selectionChangedEventArgs) => rxHandler((sender, selectionChangedEventArgs)),
                    handler => listview.SelectionChanged += handler,
                    handler => listview.SelectionChanged -= handler)
                .Select(_ => listview.SelectedItems.Cast<T>().ToArray());
        }

        public static IObservable<T[]> GetSelectedChangedObservable<T>(this GridView gridView)
        {
            return Observable.FromEvent<SelectionChangedEventHandler, (object, SelectionChangedEventArgs)>(
                    rxHandler => (sender, selectionChangedEventArgs) => rxHandler((sender, selectionChangedEventArgs)),
                    handler => gridView.SelectionChanged += handler,
                    handler => gridView.SelectionChanged -= handler)
                .Select(_ => gridView.SelectedItems.Cast<T>().ToArray());
        }
    }
}
