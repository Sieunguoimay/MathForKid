using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Graph
{
    public Column[] Columns;
    public int RowNum;
    public int ColumnNum;
}

public interface ICondition
{
    bool IsCorrect();
}

public class FalseCondition : ICondition
{
    public bool IsCorrect()
    {
        return false;
    }
}

public class TrueCondition : ICondition
{
    public bool IsCorrect()
    {
        return true;
    }
}

public interface IProblemSpecific
{
    int GetInvalidNumber();
    int GetSolutionNumber();
    ICondition GetCondition(Node node);
}

public static class GraphGenerator
{
    public static Graph GenerateGraph(int rowNum, int columnNum, IProblemSpecific problemSpecific)
    {
        var graph = new Graph
        {
            Columns = new Column[columnNum],
            RowNum = rowNum,
            ColumnNum = columnNum
        };

        for (var i = 0; i < columnNum; i++)
        {
            graph.Columns[i] = new Column {Nodes = new Node[rowNum]};
            for (var j = 0; j < rowNum; j++)
            {
                graph.Columns[i].Nodes[j] = new Node
                {
                    Condition = new FalseCondition(),
                    Number = problemSpecific.GetInvalidNumber(),
                    ColumnIndex = i,
                    RowIndex = j
                };
            }
        }

        var startPoint = new Vector2Int(0, rowNum / 2);
        var currentPoint = startPoint;

        var startNode = graph.Columns[currentPoint.x].Nodes[currentPoint.y];
        startNode.Condition = problemSpecific.GetCondition(startNode);
        startNode.Number = problemSpecific.GetSolutionNumber();

        while (true)
        {
            var validMoves = new List<Move>();
            if (currentPoint.x == columnNum - 1)
            {
                validMoves.Add(Move.Terminate);
            }

            if (IsValidIndexInColumn(currentPoint.y - 1, currentPoint.x, rowNum, graph))
            {
                validMoves.Add(Move.Up);
            }

            if (IsValidIndexInColumn(currentPoint.y + 1, currentPoint.x, rowNum, graph))
            {
                validMoves.Add(Move.Down);
            }

            if (currentPoint.x + 1 < graph.ColumnNum)
            {
                validMoves.Add(Move.Right);
            }

            if (validMoves.Count == 0)
            {
                Debug.LogError("Bleh");
                break;
            }

            var selectedMove = validMoves[Random.Range(0, validMoves.Count)];
            if (selectedMove == Move.Terminate)
            {
                break;
            }

            switch (selectedMove)
            {
                case Move.Up:
                    currentPoint.y--;
                    break;
                case Move.Down:
                    currentPoint.y++;
                    break;
                case Move.Right:
                    currentPoint.x++;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            var node = graph.Columns[currentPoint.x].Nodes[currentPoint.y];
            node.Condition = problemSpecific.GetCondition(node);
            node.Number = problemSpecific.GetSolutionNumber();
        }

        return graph;
    }

    public static Graph GenerateGraphWithFixedTable(GameplayMono.ItemArray[] table)
    {
        var graph = new Graph
        {
            Columns = new Column[table.Length],
            RowNum = table[0].items.Length,
            ColumnNum = table.Length
        };

        for (var col = 0; col < table.Length; col++)
        {
            graph.Columns[col] = new Column() {Nodes = new Node[graph.RowNum]};
            for (var row = 0; row < table[col].items.Length; row++)
            {
                var isTrue = table[col].items[row].isCorrect;
                graph.Columns[col].Nodes[row] = new Node
                {
                    Number = table[col].items[row].value,
                    Condition = isTrue ? new TrueCondition() : new FalseCondition(),
                    RowIndex = row, ColumnIndex = col
                };
            }
        }

        return graph;
    }

    public static void LogGraph(Graph graph)
    {
        var str = "";
        for (var i = 0; i < graph.ColumnNum; i++)
        {
            for (var j = 0; j < graph.RowNum; j++)
            {
                str += graph.Columns[i].Nodes[j].Condition.IsCorrect() + " ";
            }

            str += "\n";
        }

        Debug.Log(str);
    }

    public static bool IsValidIndexInColumn(int rowIndex, int columnIndex, int rowNum, Graph graph)
    {
        if (rowIndex < 0 || rowIndex >= rowNum || columnIndex >= graph.Columns.Length) return false;
        if (columnIndex - 1 < 0) return true;
        if (graph.Columns[columnIndex].Nodes[rowIndex].Condition.IsCorrect())
        {
            return false;
        }

        return !graph.Columns[columnIndex - 1].Nodes[rowIndex].Condition.IsCorrect();
    }

    public enum Move
    {
        Up,
        Down,
        Right,
        Terminate,
    }
}