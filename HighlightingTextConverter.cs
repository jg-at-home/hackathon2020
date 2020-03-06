using System;
using System.Globalization;
using System.IO;
using System.Security;
using System.Windows.Data;
using System.Windows.Markup;
using System.Xml;

namespace Hackathon2020
{
    [ValueConversion(typeof(string), typeof(object))]
    public class HighlightingTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string input) {
                var escapedXml = SecurityElement.Escape(input);
                var withTags = escapedXml.Replace("|~S~|", "<Run Style=\"{DynamicResource highlight}\">");
                withTags = withTags.Replace("|~E~|", "</Run>");

                var wrappedInput =
                    $"<TextBlock xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" TextWrapping=\"Wrap\">{withTags}</TextBlock>";

                using (StringReader stringReader = new StringReader(wrappedInput)) {
                    using (XmlReader xmlReader = XmlReader.Create(stringReader)) {
                        return XamlReader.Load(xmlReader);
                    }
                }
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
