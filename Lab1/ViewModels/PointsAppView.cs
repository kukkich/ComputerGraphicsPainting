using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using Lab1.Models;
using Xceed.Wpf.Toolkit;

namespace Lab1.ViewModels;

public class PointsAppView : INotifyPropertyChanged
{
    public ColorPicker ColorPicker { get; }

    public ObservableCollection<PointsGroupView> PointsGroup
    {
        get => _pointsGroup; 
        set
        {
            _pointsGroup = value;
            OnPropertyChanged();
        }
    }
    private ObservableCollection<PointsGroupView> _pointsGroup;

    public int CurrentGroupIndex
    {
        get => _currentGroupIndex;
        set
        {
            _currentGroupIndex = value;
            if (value < 0)
            {
                _groupsTable.UnselectAll();
                ColorPicker.SelectedColor = null;
                ColorPicker.ShowDropDownButton = false;
            }
            else
            {
                SelectedInTableGroup = _pointsGroup[value];
                _groupsTable.SelectedIndex = value;

                var color = SelectedInTableGroup.Color;
                ColorPicker.SelectedColor = color;
                ColorPicker.ShowDropDownButton = true;
            }
            OnPropertyChanged();
        }
    }
    private int _currentGroupIndex = -1;

    public PointsGroupView SelectedInTableGroup
    {
        get => _selectedInTableGroup;
        set
        {
            _selectedInTableGroup = value;
            OnPropertyChanged();
        }
    }
    private PointsGroupView _selectedInTableGroup;

    public string State
    {
        get => _state;
        set
        {
            _state = value;
            
            OnPropertyChanged();
        }
    }
    private string _state = "";

    private readonly ListView _groupsTable;

    public PointsAppView(ListView groupsTable, ColorPicker colorPicker)
    {
        ColorPicker = colorPicker;
        _groupsTable = groupsTable;

        _pointsGroup = new ObservableCollection<PointsGroupView>();
        _pointsGroup.CollectionChanged += ( _, _) 
            => OnPropertyChanged(nameof(PointsGroup));
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}