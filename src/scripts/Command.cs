public interface ICommand
{
    public void Execute();
    public void Undo();
}
public class CommandManager
{
    readonly System.Collections.Generic.Stack<ICommand> UndoStack = new(64);

    public void Undo()
    {
        if (UndoStack.Count > 0)
            UndoStack.Pop().Undo();
    }
    public void Add(ICommand command)
    {
        command.Execute();
        UndoStack.Push(command);
    }

}
