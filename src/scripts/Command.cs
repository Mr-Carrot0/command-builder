using Godot;
using System;
using System.Collections.Generic;
public class CommandManager
{
    public CommandManager()
    {
        UndoStack = new(64);
    }
    public Stack<ICommand> UndoStack;

    public void Undo()
    {
        UndoStack.Pop().Undo();
    }
    public void Add(ICommand command)
    {
        command.Execute();
        UndoStack.Push(command);
    }

}

public interface ICommand
{
    public void Execute();
    public void Undo();
}


