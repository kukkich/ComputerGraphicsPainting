using Lab1.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;

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
        }
    }
    private int _index;

    public float[] Color
    {
        get => _color;
        set
        {
            _color = value;
            OnPropertyChanged();
        }
    }
    private float[] _color;

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