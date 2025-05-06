using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace SaveUpAppFrontend.Converters
{
    public class PercentageToWidthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double percentage && parameter is VisualElement element)
            {
                // Berechne die Breite basierend auf der Gesamtbreite des Elements
                return (percentage / 100) * element.Width;
            }
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}