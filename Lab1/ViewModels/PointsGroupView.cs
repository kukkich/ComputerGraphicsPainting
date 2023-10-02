using Lab1.Models;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using System.Windows.Media;

namespace Lab1.ViewModels;

public class PointsGroupView : INotifyPropertyChanged
{
    public int Index
    {
        get => _index;
        set
        {
            _index = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(Number));
        }
    }
    private int _index;

    public int Number => Index + 1;

    public Color Color
    {
        get => _color;
        set
        {
            _color = value;
            OnPropertyChanged();
        }
    }
    private Color _color;

    public int PointsCount
    {
        get => _pointsCount;
        set
        {
            _pointsCount = value;
            OnPropertyChanged();
        }
    }
    private int _pointsCount;

    public PointsGroupView(PointsGroup group, int index)
    {
        _color = group.Color;
        _pointsCount = group.Points.Count;
        _index = index;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

public class ColorToBrushConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is Color color)
        {
            return new SolidColorBrush(Color.FromRgb(color.R, color.G, color.B));
        }

        return null; // Если значение не является Color, возвращаем null или другое значение по умолчанию
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException(); // Этот метод не используется в данном примере
    }
}