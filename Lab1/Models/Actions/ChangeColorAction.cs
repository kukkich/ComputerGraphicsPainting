using System.Windows.Media;

namespace Lab1.Models.Actions;

public class ChangeColorAction : PreviousStateMemorizedAction
{
    private readonly Color _newColor;
    private Color _oldColor;

    public ChangeColorAction(PointsApp app, Color newColor) : base(app)
    {
        _newColor = newColor;
    }

    public override void Do()
    {
        base.Do();
        _oldColor = App.PointContext.CurrentGroup.Color;
        App.SelectNewColor(_newColor);
    }

    public override void Undo()
    {
        base.Undo();
        App.SelectNewColor(_oldColor);
    }
}