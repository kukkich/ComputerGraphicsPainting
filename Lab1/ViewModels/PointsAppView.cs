﻿using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using Lab1.Models;

namespace Lab1.ViewModels;

public class PointsAppView : INotifyPropertyChanged
{

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
            }
            else
            {
                _groupsTable.SelectedIndex = value;
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
            if (value == nameof(AppState.Initial))
            {
                CurrentGroupIndex = -1;
            }
            OnPropertyChanged();
        }
    }
    private string _state = "";

    private readonly ListView _groupsTable;

    public PointsAppView(ListView groupsTable)
    {
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