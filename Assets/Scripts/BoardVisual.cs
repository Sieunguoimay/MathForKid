using System;
using System.Linq;
using UnityEngine;

public class BoardVisual : MonoBehaviour
{
    [SerializeField] private NodeVisual buttonPrefab;
    [SerializeField] private float spacing;

    private Graph _graph;

    private int _row;
    private int _column;

    public float Spacing => spacing;

    [field: NonSerialized] public NodeVisual[] Buttons { get; private set; }

    public void Setup(Graph graph)
    {
        _graph = graph;
        SpawnButtons(graph.ColumnNum, graph.RowNum);
        PositionButtons(spacing);
        //Setup buttons
        for (var i = 0; i < _column; i++)
        {
            for (var j = 0; j < _row; j++)
            {
                Buttons[_row * i + j].Setup(_graph.Columns[i].Nodes[j]);
            }
        }
    }

    public void SpawnButtons(int column, int row)
    {
        _row = row;
        _column = column;
        Buttons = new NodeVisual[_column * _row];
        for (var i = 0; i < _column; i++)
        {
            for (var j = 0; j < _row; j++)
            {
                Buttons[_row * i + j] = Instantiate(buttonPrefab, transform);
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

        var moves = GraphHelper.GetNextMoveNodes(_graph, node.ColumnIndex, node.RowIndex);
        foreach (var index in from move in moves
            where !move.Selector.Selected
            select GraphHelper.GetIndexInArray(_graph, move))
        {
            Buttons[index].PlayBounceAnim();
            Buttons[index].SetSelectable(true);
        }
    }
}