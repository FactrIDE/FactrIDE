﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Idealde.Converters
{
    public class EmptyStringToUnsetConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            string.IsNullOrWhiteSpace(value as string) ? DependencyProperty.UnsetValue : value;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            throw new NotSupportedException();
    }
}