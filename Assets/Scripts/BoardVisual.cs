using System;
using System.Linq;
using UnityEngine;

public class BoardVisual : MonoBehaviour
{
    [SerializeField] private NodeVisual buttonPrefab;
    [SerializeField] private Line linePrefab;
    [SerializeField] private float spacing;

    private Graph _graph;

    private int _row;
    private int _column;

    public float Spacing => spacing;

    [field: NonSerialized] public NodeVisual[] Buttons { get; private set; }

    [field: System.NonSerialized] public Line[] Lines { get; private set; }

    public void Setup(Graph graph)
    {
        _graph = graph;
        _row = graph.RowNum;
        _column = graph.ColumnNum;
        SpawnButtons();
        PositionButtons(spacing);
        SpawnLines();
        //Setup buttons
        for (var i = 0; i < _column; i++)
        {
            for (var j = 0; j < _row; j++)
            {
                Buttons[_row * i + j].Setup(_graph.Columns[i].Nodes[j]);
            }
        }
    }

    public void SpawnButtons()
    {
        Buttons = new NodeVisual[_column * _row];
        for (var i = 0; i < _column; i++)
        {
            for (var j = 0; j < _row; j++)
            {
                Buttons[_row * i + j] = Instantiate(buttonPrefab, transform);
            }
        }
    }

    public void SpawnLines()
    {
        var firstHalf = (_column) * (_row - 1);
        var secondHalf = (_column - 1) * (_row);
        Lines = new Line[firstHalf + secondHalf];
        for (var i = 0; i < _column; i++)
        {
            for (var j = 0; j < _row - 1; j++)
            {
                var point1 = Buttons[i * _row + j].transform.position;
                var point2 = Buttons[i * _row + j + 1].transform.position;
                Lines[i * (_row - 1) + j] = Instantiate(linePrefab, transform);
                Lines[i * (_row - 1) + j].SetEndPoints(point1, point2);
                Lines[i * (_row - 1) + j].Node1 = Buttons[i * _row + j];
                Lines[i * (_row - 1) + j].Node2 = Buttons[i * _row + j + 1];
            }
        }

        for (var i = 0; i < _row; i++)
        {
            for (var j = 0; j < _column - 1; j++)
            {
                var point1 = Buttons[j * _row + i].transform.position;
                var point2 = Buttons[(j + 1) * _row + i].transform.position;
                Lines[firstHalf + i * (_column - 1) + j] = Instantiate(linePrefab, transform);
                Lines[firstHalf + i * (_column - 1) + j].SetEndPoints(point1, point2);
                Lines[firstHalf + i * (_column - 1) + j].Node1 = Buttons[j * _row + i];
                Lines[firstHalf + i * (_column - 1) + j].Node2 = Buttons[(j + 1) * _row + i];
            }
        }
    }

    public void PositionButtons(float spacing)
    {
        var x0 = -_column * spacing / 2f + spacing / 2f;
        var y0 = _row * spacing / 2f - spacing / 2f;
        for (var i = 0; i < _column; i++)
        {
            var x = x0 + i * spacing;
            for (var j = 0; j < _row; j++)
            {
                var y = y0 - j * spacing;
                Buttons[_row * i + j].transform.localPosition = y * Vector3.up + x * Vector3.right;
            }
        }
    }

    public void EnableNextNodes(Node node)
    {
        for (var i = 0; i < _column; i++)
        {
            for (var j = 0; j < _row; j++)
            {
                if (!Buttons[_row * i + j].Node.Selector.Selected)
                {
                    Buttons[_row * i + j].SetSelectable(false);
                }
            }
        }

        // var moves = Buttons.Select(b => b.Node);
        var moves = GraphHelper.GetNextMoveNodes(_graph, node.ColumnIndex, node.RowIndex);
        foreach (var index in from move in moves
            where !move.Selector.Selected
            select GraphHelper.GetIndexInArray(_graph, move))
        {
            Buttons[index].PlayBounceAnim();
            Buttons[index].SetSelectable(true);
        }
    }

    public Line GetLine(NodeVisual a, NodeVisual b)
    {
        return Lines.FirstOrDefault(l => (l.Node1 == a && l.Node2 == b) || (l.Node1 == b && l.Node2 == a));
    }
}