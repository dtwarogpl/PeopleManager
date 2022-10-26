﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace PeopleManager.View.Converters;

public class BoolToVisibilityConverterInverted : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool b)
        {
            return b ? Visibility.Collapsed : Visibility.Visible;
        }

        return Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}