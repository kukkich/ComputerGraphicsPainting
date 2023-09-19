namespace Lab1.Models.Actions;

public class SelectGroupAction : PreviousStateMemorizedAction
{
    private readonly int _newGroupIndex;
    private int _oldGroup;

    public SelectGroupAction(PointsApp app, int newGroupIndex)
        : base(app)
    {
        _newGroupIndex = newGroupIndex;
    }

    public override void Do()
    {
        base.Do();
        App.State = AppState.PointPlacement;

        _oldGroup = App.PointContext.CurrentGroupIndex;
        App.PointContext.SelectGroup(_newGroupIndex);
    }

    public override void Undo()
    {
        base.Undo();

        App.PointContext.SelectGroup(_oldGroup);
    }
}