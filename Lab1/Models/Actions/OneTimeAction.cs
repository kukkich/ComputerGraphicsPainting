using System;

namespace Lab1.Models.Actions;

public abstract class OneTimeAction : IAction
{
    private bool _did;
    public virtual void Do()
    {
        if (_did)
            throw new InvalidOperationException();
        _did = true;
    }

    public virtual void Undo()
    {
        if (!_did)
            throw new InvalidOperationException();
        _did = false;
    }
}