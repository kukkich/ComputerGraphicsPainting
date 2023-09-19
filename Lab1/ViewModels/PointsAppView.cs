using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using Xceed.Wpf.Toolkit;

namespace Lab1.ViewModels;

public class PointsAppView : INotifyPropertyChanged
{
    public ColorPicker ColorPicker => _window.ColorPicker;

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
                _window.HideGroupActions();
            }
            else
            {
                SelectedInTableGroup = _pointsGroup[value];
                _groupsTable.SelectedIndex = value;

                _window.ShowGroupActions();
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
    private readonly MainWindow _window;

    public PointsAppView(ListView groupsTable, MainWindow window)
    {
        _groupsTable = groupsTable;
        _window = window;

        _pointsGroup = new ObservableCollection<PointsGroupView>();
        _pointsGroup.CollectionChanged += (_, _)
            => OnPropertyChanged(nameof(PointsGroup));
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}