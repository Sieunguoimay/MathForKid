public class Node
{
    public object Number;
    public ICondition Condition;
    public int RowIndex;
    public int ColumnIndex;
    public INodeSelector Selector;
}

public interface INodeSelector
{
    void Select(Node node);
    bool Selected { get; }
}