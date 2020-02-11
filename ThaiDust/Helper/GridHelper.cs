using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ThaiDust.Helper
{
    public class Grid
    {
        #region Rows Property

        /// <summary>
        /// Adds the specified number of Rows to RowDefinitions. 
        /// Default Height is Auto
        /// </summary>
        public static readonly DependencyProperty RowsProperty =
            DependencyProperty.RegisterAttached(
                "Rows", typeof(string), typeof(Grid),
                new PropertyMetadata(string.Empty, RowCountChanged));
        // Get
        public static string GetRows(DependencyObject obj)
        {
            return (string)obj.GetValue(RowsProperty);
        }

        // Set
        public static void SetRows(DependencyObject obj, string value)
        {
            obj.SetValue(RowsProperty, value);
        }

        // Change Event - Adds the Rows
        public static void RowCountChanged(
            DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is Windows.UI.Xaml.Controls.Grid grid && !string.IsNullOrWhiteSpace(e.NewValue as string))
            {
                string[] definitions = (e.NewValue as string).Split(',');
                grid.RowDefinitions.Clear();
                foreach (string definition in definitions)
                {
                    if (definition.Contains('*'))
                    {
                        int.TryParse(definition.Substring(0, definition.Length - 1), out int length);
                        if (length == 0) length = 1;
                        grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(length, GridUnitType.Star)});
                    }
                    else if (int.TryParse(definition, out int length))
                    {
                        grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(length, GridUnitType.Pixel) });
                    }
                    else
                    {
                        grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                    }
                }
            }
        }

        #endregion

        #region Column Property

        /// <summary>
        /// Adds the specified number of Columns to ColumnDefinitions. 
        /// Default Width is Auto
        /// </summary>
        public static readonly DependencyProperty ColumnsProperty =
            DependencyProperty.RegisterAttached(
                "Columns", typeof(string), typeof(Grid),
                new PropertyMetadata(string.Empty, ColumnCountChanged));
        // Get
        public static string GetColumns(DependencyObject obj)
        {
            return (string)obj.GetValue(ColumnsProperty);
        }

        // Set
        public static void SetColumns(DependencyObject obj, string value)
        {
            obj.SetValue(ColumnsProperty, value);
        }

        // Change Event - Adds the Rows
        public static void ColumnCountChanged(
            DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is Windows.UI.Xaml.Controls.Grid grid && !string.IsNullOrWhiteSpace(e.NewValue as string))
            {
                string[] definitions = (e.NewValue as string).Split(',');
                grid.ColumnDefinitions.Clear();
                foreach (string definition in definitions)
                {
                    if (definition.Contains('*'))
                    {
                        int.TryParse(definition.Substring(0, definition.Length - 1), out int length);
                        if (length == 0) length = 1;
                        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(length, GridUnitType.Star) });
                    }
                    else if (int.TryParse(definition, out int length))
                    {
                        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(length, GridUnitType.Pixel) });
                    }
                    else
                    {
                        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                    }
                }
            }
        }

    }

    #endregion

}
