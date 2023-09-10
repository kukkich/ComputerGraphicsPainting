using System.Drawing;

namespace Lab1.Models.Actions;

public class AddPointAction : OneTimeAction
{
    private readonly PointContext _pointContext;
    private readonly PointF _point;

    public AddPointAction(PointContext pointContext, PointF point)
    {
        _pointContext = pointContext;
        _point = point;
    }

    public override void Do()
    {
        base.Do();
        _pointContext.AddPoint(_point);
    }

    public override void Undo()
    {
        base.Undo();
        _pointContext.RemoveLastPoint();
    }
}