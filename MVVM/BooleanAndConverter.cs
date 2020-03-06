using System;
using System.Globalization;
using System.Windows.Data;

namespace MVVM
{
    public class BooleanAndConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            foreach (var value in values) {
                if (value is bool flag) {
                    if (!flag) {
                        return false;
                    }
                }
            }

            return true;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
