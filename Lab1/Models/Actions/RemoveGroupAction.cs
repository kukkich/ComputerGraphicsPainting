namespace Lab1.Models.Actions;

public class RemoveGroupAction : PreviousStateMemorizedAction
{
    private readonly int _removedIndex;
    private PointsGroup _removedGroup;

    public RemoveGroupAction(PointsApp app, int removedIndex)
        : base(app)
    {
        _removedIndex = removedIndex;
    }

    public override void Do()
    {
        base.Do();

        App.State = AppState.Initial;
        _removedGroup = App.PointContext.RemoveGroupAt(_removedIndex);
    }

    public override void Undo()
    {
        base.Undo();

        App.PointContext.InsertGroupAt(_removedIndex, _removedGroup);
        App.PointContext.SelectGroup(_removedIndex);
    }
}