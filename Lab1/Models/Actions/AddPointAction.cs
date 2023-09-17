using System.Drawing;

namespace Lab1.Models.Actions;

public class AddPointAction : IAction
{
    private readonly PointContext _pointContext;
    private readonly PointF _point;

    public AddPointAction(PointContext pointContext, PointF point)
    {
        _pointContext = pointContext;
        _point = point;
    }

    public void Do()
    {
        _pointContext.AddPoint(_point);
    }

    public void Undo()
    {
        _pointContext.RemoveLastPoint();
    }
}