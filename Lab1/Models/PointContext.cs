using System;
using System.Collections.Generic;
using System.Drawing;
using Lab1.ViewModels;

namespace Lab1.Models;

public class PointContext
{
    public List<PointsGroup> Groups => _groups;
    public PointsGroup? CurrentGroup => CurrentGroupIndex < 0 
        ? null 
        : _groups[CurrentGroupIndex];
    public int CurrentGroupIndex { get; private set; }

    public Cursor Cursor { get; set; }
    public int? ClosestPointIndex { get; private set; }
    public PointF? ClosestPoint 
    {
        get
        {
            if (ClosestPointIndex is null)
            {
                return null;
            }

            return CurrentGroup!.Points[ClosestPointIndex.Value];
        }
    }

    public int? EditingPointIndex { get; private set; }

    private readonly List<PointsGroup> _groups;
    private readonly PointsAppView _view;

    public PointContext(PointsAppView view)
    {
        CurrentGroupIndex = -1;
        _groups = new List<PointsGroup>();

        _view = view;

        Cursor = new Cursor();
    }

    public void AddGroup()
    {
        _groups.Add(new PointsGroup());
        CurrentGroupIndex = _groups.Count - 1;

        _view.PointsGroup.Add(
            new PointsGroupView(
                CurrentGroup!,
                CurrentGroupIndex
            )
        );
        _view.CurrentGroupIndex = CurrentGroupIndex;
    }

    public void AddPoint(PointF point)
    {
        _groups[CurrentGroupIndex].Points.Add(point);
        _view.PointsGroup[CurrentGroupIndex].PointsCount++;
    }

    public PointsGroup RemoveLastGroup()
    {
        var removed = _groups[CurrentGroupIndex];

        _groups.RemoveAt(CurrentGroupIndex);
        _view.PointsGroup.RemoveAt(CurrentGroupIndex);

        CurrentGroupIndex--;
        _view.CurrentGroupIndex--;

        if (CurrentGroupIndex < 0)
        {
            CurrentGroupIndex = 0;
            _groups.Add(new());
        }

        return removed;
    }

    public void RemoveLastPoint()
    {
        var totalPoints = CurrentGroup.Points.Count;
        if (totalPoints == 0)
        {
            if (CurrentGroupIndex == 0)
            {
                throw new InvalidOperationException("Was no points");
            }
            RemoveLastGroup();
        }

        _groups[CurrentGroupIndex].Points.RemoveAt(totalPoints - 1);
        _view.PointsGroup[CurrentGroupIndex].PointsCount--;
    }

    public void SelectClosestPointInCurrentGroup()
    {
        var group = CurrentGroup;
        if (group is null)
        {
            ClosestPointIndex = null;
            return;
        }

        var cursor = Cursor.Position;
        var result = -1;
        var minDistance = 1f;

        for (var i = 0; i < group.Points.Count; i++)
        {
            var p = group.Points[i];
            var delta = new PointF(
                MathF.Abs(cursor.X - p.X),
                MathF.Abs(cursor.Y - p.Y)
            );

            const float eps = 5e-2f;
            var distance = MathF.Sqrt(MathF.Pow(delta.X, 2) + MathF.Pow(delta.Y, 2));
            if (distance < eps && distance < minDistance)
            {
                minDistance = distance;
                result = i;
            }
        }

        ClosestPointIndex = result == -1
            ? null
            : result;
    }

    public void StartPointEditing(int pointIndex)
    {
        EditingPointIndex = pointIndex;
    }

    public void StopPointEditing()
    {
        EditingPointIndex = null;
    }

    public void CommitEditing(PointF newPosition)
    {
        try
        {
            CurrentGroup!.Points[EditingPointIndex!.Value] = newPosition;

            StopPointEditing();
        }
        catch (NullReferenceException)
        {
            throw new InvalidOperationException("""
                Не выбрана текущая группу или точка для редактирования.
                Невозмонжо зафиксировать изменение.
                """
            );
        }
    }

    public void SelectGroup(int index)
    {
        if (index < 0 || index >= Groups.Count)
        {
            throw new ArgumentOutOfRangeException();
        }

        CurrentGroupIndex = index;
        _view.CurrentGroupIndex = index;
    }
}