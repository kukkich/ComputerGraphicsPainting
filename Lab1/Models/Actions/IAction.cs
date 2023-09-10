namespace Lab1.Models.Actions;

public interface IAction
{
    void Do();
    void Undo();
}