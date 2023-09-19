using System.Drawing;

namespace Lab1.Models.Actions;

public class CommitPointEditingAction : PreviousStateMemorizedAction
{
    private readonly PointF _newPosition;
    private int _editedPointIndex;
    private PointF _oldPosition;

    public CommitPointEditingAction(PointsApp app, PointF newPosition)
        : base(app)
    {
        _newPosition = newPosition;
    }

    public override void Do()
    {
        base.Do();

        App.State = AppState.SelectingPointToEdit;

        _editedPointIndex = App.PointContext.EditingPointIndex!.Value;

        _oldPosition = App.PointContext.CurrentGroup!.Points[_editedPointIndex];

        App.PointContext.CommitEditing(_newPosition);
    }

    public override void Undo()
    {
        base.Undo();

        App.PointContext.CurrentGroup!.Points[_editedPointIndex] = _oldPosition;

        App.PointContext.StartPointEditing(_editedPointIndex);
    }
}