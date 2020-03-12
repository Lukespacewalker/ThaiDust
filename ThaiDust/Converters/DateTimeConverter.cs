using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using ReactiveUI;

namespace ThaiDust.Converters
{
    public class DateTimeConverter : IValueConverter
    {
        private static readonly CultureInfo ThaiCultureInfo = CultureInfo.GetCultureInfo("th-Th");
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (!(value is DateTime)) throw new ArgumentException("only DateTime is suppirted");
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

    public class EnumValueToRadioButtonIsCheckConverter : IBindingTypeConverter
    {
        public int GetAffinityForObjects(Type fromType, Type toType)
        {
            if ((toType == typeof(bool) || toType == typeof(bool?)) && fromType.IsEnum)
            {
                return 1;
            }
            if ((fromType == typeof(bool) || fromType == typeof(bool?)) && toType.IsEnum)
            {
                return 1;
            }
            return 0;
        }

        public bool TryConvert(object @from, Type toType, object conversionHint, out object result)
        {
            // From IsChecked to Enumeration
            if (toType.IsEnum)
            {
                string radioButtonTag = conversionHint as string;
                if (string.IsNullOrWhiteSpace(radioButtonTag)) throw new ArgumentException("Expect target enumeration value name", nameof(conversionHint));
                bool? isChecked = (@from as bool?);
                if (isChecked.HasValue)
                {
                    // Get Enumeration from toType
                    if (isChecked.Value)
                    {
                        object enumValue = Enum.Parse(toType, conversionHint.ToString());
                        result = enumValue;
                        return true;
                    }
                    else
                    {
                        result = null;
                        return false;
                    }
                }
                else
                {
                    result = null;
                    return false;
                }
            }
            // From Enumeration to IsChecked
            if (toType == typeof(bool) || toType == typeof(bool?))
            {
                string radioButtonTag = conversionHint as string;
                if (string.IsNullOrWhiteSpace(radioButtonTag)) throw new ArgumentException("Expect target enumeration value name", nameof(conversionHint));

                string enumName = Enum.GetName(@from.GetType(), @from);
                result = enumName.Equals(radioButtonTag, StringComparison.InvariantCultureIgnoreCase);
                return true;
            }
            result = null;
            return false;
        }
    }
}
