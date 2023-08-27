using System.Collections.Generic;

namespace LimitedSizeStack;

internal interface ICommand
{
    void Execute();
    void Undo();
}

internal class RemoveItem<TItem> : ICommand
{
    private readonly int _index;
    private readonly TItem _item;
    private readonly List<TItem> _items;

    public RemoveItem(int index, TItem item, List<TItem> items)
    {
        _index = index;
        _item = item;
        _items = items;
    }

    public void Execute()
    {
        _items.RemoveAt(_index);
    }

    public void Undo()
    {
        _items.Insert(_index, _item);
    }
}

internal class AddItem<TItem> : ICommand
{
    private readonly int _index;
    private readonly TItem _item;
    private readonly List<TItem> _items;

    public AddItem(int index, TItem item, List<TItem> items)
    {
        _index = index;
        _item = item;
        _items = items;
    }

    public void Execute()
    {
        _items.Add(_item);
    }

    public void Undo()
    {
        _items.RemoveAt(_index);
    }
}

public class ListModel<TItem>
{
    public List<TItem> Items { get; }
    private readonly LimitedSizeStack<ICommand> _actionsToUndo;

    public ListModel(int undoLimit) : this(new List<TItem>(), undoLimit)
    {
    }

    public ListModel(List<TItem> items, int undoLimit)
    {
        Items = items;
        _actionsToUndo = new LimitedSizeStack<ICommand>(undoLimit);
    }

    public void AddItem(TItem item)
    {
        var command = new AddItem<TItem>(Items.Count, item, Items);
        command.Execute();
        _actionsToUndo.Push(command);
    }

    public void RemoveItem(int index)
    {
        var command = new RemoveItem<TItem>(index, Items[index], Items);
        command.Execute();
        _actionsToUndo.Push(command);
    }

    public bool CanUndo()
    {
        return _actionsToUndo.Count > 0;
    }

    public void Undo()
    {
        var lastAction = _actionsToUndo.Pop();
        lastAction.Undo();
    }
}