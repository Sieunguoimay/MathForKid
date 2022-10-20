using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;

public class Gameplay
{
    private readonly Graph _graph;
    private readonly BoardVisual _boardVisual;
    private readonly IScore _score;
    private readonly ITimer _timer;
    private readonly GameController _controller;
    private Node _prevNode;

    public Gameplay(Graph graph, BoardVisual boardVisual, IScore score, ITimer timer, GameController controller)
    {
        _timer = timer;
        _score = score;
        _graph = graph;
        _boardVisual = boardVisual;
        _controller = controller;
    }

    public void SetFirstNode()
    {
        var firstNode = _graph.Columns[0].Nodes[_graph.RowNum / 2];
        var bt = _boardVisual.Buttons[GraphHelper.GetIndexInArray(_graph, firstNode)];
        _controller.girl.position = bt.transform.position;

        bt?.SetSelectable(true);

        _boardVisual.StartCoroutine(Delay(.5f, () =>
        {
            bt.Click();
            _boardVisual.EnableNextNodes(firstNode);
        }));


        foreach (var button in _boardVisual.Buttons)
        {
            button.Node.Selector = new NodeSelector(this);
        }
    }

    public void CorrectMove(Node node)
    {
        var nodes = _boardVisual.Buttons.Where(b => b.Node.Condition.IsCorrect());
        var finished = nodes.Count(b => b.Node.Selector.Selected) == _controller.correctAnswerNum;
        if (finished)
        {
            Finish();
        }

        if (_prevNode != null)
        {
            var line = _boardVisual.GetLine(_boardVisual.Buttons[GraphHelper.GetIndexInArray(_graph, _prevNode)], _boardVisual.Buttons[GraphHelper.GetIndexInArray(_graph, node)]);
            line?.SetCorrect(true);
        }

        _prevNode = node;
    }

    public void WrongMove()
    {
        _score.Score -= 10;
        if (_score.Score <= 70)
        {
            ShowYouLoseTooManyWrongSelection();
        }
    }

    public void Finish()
    {
        Debug.Log("Win");
        _timer.StopTimer();
        _controller.PlayFX();
        RayPointer.Instance.Interactable = false;
        _boardVisual.StartCoroutine(Delay(1f, () =>
        {
            _controller.Stop();
            _controller.ShowResultYouWin(_score.Score, _timer.Time);
            RayPointer.Instance.Interactable = true;
        }));
    }

    public void ShowYouLoseTimeOut()
    {
        Debug.Log("ShowYouLoseTimeOut");
        _controller.Stop();
        _controller.ShowResultYouLose(true);
    }

    public void ShowYouLoseTooManyWrongSelection()
    {
        Debug.Log("ShowYouLoseTooManyWrongSelection");
        _timer.StopTimer();
        _controller.Stop();
        _controller.ShowResultYouLose(false);
    }

    public class NodeSelector : INodeSelector
    {
        private readonly Gameplay _gameplay;

        public NodeSelector(Gameplay gameplay)
        {
            _gameplay = gameplay;
        }

        public void Select(Node node)
        {
            Selected = true;

            var ok = true;
            if (_gameplay._prevNode != null)
            {
                var line = _gameplay._boardVisual.GetLine(_gameplay._boardVisual.Buttons[GraphHelper.GetIndexInArray(_gameplay._graph, _gameplay._prevNode)], _gameplay._boardVisual.Buttons[GraphHelper.GetIndexInArray(_gameplay._graph, node)]);
                ok = line != null;
            }

            if (node.Condition.IsCorrect() && ok)
            {
                _gameplay._boardVisual.EnableNextNodes(node);
                _gameplay.CorrectMove(node);
                _gameplay._controller.girl.position = _gameplay._boardVisual
                    .Buttons[GraphHelper.GetIndexInArray(_gameplay._graph, node)].transform.position;
            }
            else
            {
                _gameplay.WrongMove();
            }
        }

        public bool Selected { get; private set; }
    }

    private static IEnumerator Delay(float duration, Action onDone)
    {
        yield return new WaitForSeconds(duration);
        onDone?.Invoke();
    }
}