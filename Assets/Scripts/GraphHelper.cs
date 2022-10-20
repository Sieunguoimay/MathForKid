using System.Collections.Generic;
using System.Net.Http.Headers;

public static class GraphHelper
{
    public static IEnumerable<Node> GetNextMoveNodes(Graph graph, int colIndex, int rowIndex)
    {
        var nodes = new List<Node>();
        if (IsValidIndexInGraph(graph, rowIndex + 1, colIndex))
        {
            nodes.Add(graph.Columns[colIndex].Nodes[rowIndex + 1]);
        }

        if (IsValidIndexInGraph(graph, rowIndex - 1, colIndex))
        {
            nodes.Add(graph.Columns[colIndex].Nodes[rowIndex - 1]);
        }

        if (IsValidIndexInGraph(graph, rowIndex, colIndex + 1))
        {
            nodes.Add(graph.Columns[colIndex + 1].Nodes[rowIndex]);
        }

        if (IsValidIndexInGraph(graph, rowIndex, colIndex - 1))
        {
            nodes.Add(graph.Columns[colIndex - 1].Nodes[rowIndex]);
        }

        return nodes;
    }

    public static bool IsValidIndexInGraph(Graph graph, int rowIndex, int columnIndex)
    {
        if (rowIndex < 0 || rowIndex >= graph.RowNum) return false;
        if (columnIndex < 0 || columnIndex >= graph.ColumnNum) return false;
        if (graph.Columns[columnIndex].Nodes[rowIndex].Selector.Selected) return false;
        return true;
    }

    public static int GetIndexInArray(Graph graph, Node node)
    {
        return node.ColumnIndex * graph.RowNum + node.RowIndex;
    }

    public static int CountCorrectMove(Graph graph)
    {
        var count = 0;
        for (var i = 0; i < graph.ColumnNum; i++)
        {
            for (var j = 0; j < graph.RowNum; j++)
            {
                if (graph.Columns[i].Nodes[j].Condition.IsCorrect()) count++;
            }
        }

        return count;
    }
}