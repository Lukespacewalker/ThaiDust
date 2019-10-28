using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace ThaiDust.Converters
{
    public class DateTimeConverter : IValueConverter
    {
        private static readonly CultureInfo ThaiCultureInfo = CultureInfo.GetCultureInfo("th-Th");
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if(!(value is DateTime)) throw new ArgumentException("only DateTime is suppirted");
            switch (parameter)
            {
                case "Date":
                    return ((DateTime)value).ToString("dddd dd MMMM yyyy", ThaiCultureInfo);
                case "DateTime":
                    return ((DateTime)value).ToString("dd MMMM yyyy hh:mm", ThaiCultureInfo);
                case "DateTimeName":
                    return ((DateTime)value).ToString("dddd dd MMMM yyyy hh:mm", ThaiCultureInfo);
                case "Time":
                    return ((DateTime)value).ToString("hh:mm", ThaiCultureInfo);
                default:
                    return ((DateTime)value).ToString("dd MMMM yyyy", ThaiCultureInfo);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
