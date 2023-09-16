using System;
using System.Collections.Generic;
using System.Drawing;
using Lab1.ViewModels;

namespace Lab1.Models;

public class PointContext
{
    public List<PointsGroup> Groups => _groups;
    public PointsGroup? CurrentGroup => _currentGroupIndex < 0 
        ? null 
        : _groups[_currentGroupIndex];

    public Cursor Cursor { get; set; }

    private int _currentGroupIndex;
    private readonly List<PointsGroup> _groups;
    private readonly PointsAppView _view;

    public PointContext(PointsAppView view)
    {
        _currentGroupIndex = -1;
        _groups = new List<PointsGroup>();

        _view = view;

        Cursor = new Cursor();
    }

    public void AddGroup()
    {
        _groups.Add(new PointsGroup());
        _currentGroupIndex++;

        _view.PointsGroup.Add(
            new PointsGroupView(
                CurrentGroup,
                _currentGroupIndex
            )
        );
        _view.CurrentGroupIndex++;
    }

    public void AddPoint(PointF point)
    {
        _groups[_currentGroupIndex].Points.Add(point);
        _view.PointsGroup[_currentGroupIndex].PointsCount++;
    }

    public PointsGroup RemoveLastGroup()
    {
        var removed = _groups[_currentGroupIndex];

        _groups.RemoveAt(_currentGroupIndex);
        _view.PointsGroup.RemoveAt(_currentGroupIndex);

        _currentGroupIndex--;
        _view.CurrentGroupIndex--;

        if (_currentGroupIndex < 0)
        {
            _currentGroupIndex = 0;
            _groups.Add(new());
        }

        return removed;
    }

    public void RemoveLastPoint()
    {
        var totalPoints = CurrentGroup.Points.Count;
        if (totalPoints == 0)
        {
            if (_currentGroupIndex == 0)
            {
                throw new InvalidOperationException("Was no points");
            }
            RemoveLastGroup();
        }

        _groups[_currentGroupIndex].Points.RemoveAt(totalPoints - 1);
        _view.PointsGroup[_currentGroupIndex].PointsCount--;
    }
}