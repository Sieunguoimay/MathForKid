public class Node
{
    public int Number;
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