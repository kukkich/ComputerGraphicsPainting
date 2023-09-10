using System;
using System.Collections.Generic;
using System.Drawing;

namespace Lab1.Models;

public class PointContext
{
    public IReadOnlyList<IReadOnlyList<PointF>> PointGroups => _pointGroups;
    public IReadOnlyList<PointF> CurrentGroup => _pointGroups[_currentGroupIndex];
    public PointF? CursorPosition { get; set; }

    private int _currentGroupIndex;
    private readonly List<List<PointF>> _pointGroups;

    public PointContext()
    {
        _pointGroups = new List<List<PointF>> {new()};
    }

    public void AddGroup()
    {
        _pointGroups.Add(new List<PointF>());
        _currentGroupIndex++;
    }

    public void AddPoint(PointF point)
    {
        _pointGroups[_currentGroupIndex].Add(point);
    }

    public List<PointF> RemoveLastGroup()
    {
        var removed = _pointGroups[_currentGroupIndex];

        _pointGroups.RemoveAt(_currentGroupIndex);
        _currentGroupIndex--;
        if (_currentGroupIndex < 0)
        {
            _currentGroupIndex = 0;
            _pointGroups.Add(new List<PointF>());
        }

        return removed;
    }

    public void RemoveLastPoint()
    {
        if (CurrentGroup.Count == 0)
        {
            if (_currentGroupIndex == 0)
            {
                throw new InvalidOperationException("Was no points");
            }
            RemoveLastGroup();
        }
        _pointGroups[_currentGroupIndex].RemoveAt(CurrentGroup.Count - 1);
    }
}