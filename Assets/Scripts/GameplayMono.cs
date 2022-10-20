using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameplayMono : MonoBehaviour
{
    [SerializeField] private BoardVisual boardVisual;
    [SerializeField] private ScoreDisplay score;
    [SerializeField] private TimerDisplay timer;
    [SerializeField] private Transform market;
    [SerializeField, Min(1)] private int col = 3;
    [SerializeField, Min(1)] private int row = 3;

    [SerializeField] private ItemArray[] table;

    [Serializable]
    public class ItemArray
    {
        public Item[] items;
    }

    [Serializable]
    public class Item
    {
        public string value;
        public bool isCorrect;
    }

    public void Setup(GameController gameController, bool randomize)
    {
        // var graph = GraphGenerator.GenerateGraph(row, col, new Modulo69Problem());
        var graph = randomize ? GraphGenerator.GenerateGraph(row, col, new Modulo69Problem()) : GraphGenerator.GenerateGraphWithFixedTable(table);
        gameController.correctAnswerNum = randomize ? GraphHelper.CountCorrectMove(graph) : 8;
        boardVisual.Setup(graph);
        gameController.cameraSizeAuto.Resize(col * boardVisual.Spacing / 1.2f);

        var pos = score.transform.localPosition;
        pos.z = row * boardVisual.Spacing / 2f + .5f;
        score.transform.localPosition = pos;

        var pos2 = timer.transform.localPosition;
        pos2.z = -row * boardVisual.Spacing / 2f - .5f;
        timer.transform.localPosition = pos2;

        if (randomize)
        {
            var pos3 = market.localPosition;
            pos3.x = col * boardVisual.Spacing / 2f + 1f;
            market.localPosition = pos3;
        }
        else
        {
            market.position = boardVisual.Buttons[^1].transform.position;
        }

        score.Score = 100;
        var gameplay = new Gameplay(graph, boardVisual, score, timer, gameController);
        gameplay.SetFirstNode();
        timer.StartTimer(gameplay.ShowYouLoseTimeOut);
    }

    public class Modulo69Problem : IProblemSpecific
    {
        public int GetInvalidNumber()
        {
            var valid = Random.Range(1, 999);

            while (valid % 6 == 0 && valid % 9 == 0)
            {
                valid = Random.Range(1, 999);
            }

            return valid;
        }

        public int GetSolutionNumber()
        {
            return Random.Range(1, 20) * 6 * 9;
        }

        public ICondition GetCondition(Node node)
        {
            return new ModuloCondition(node, 6, 9);
        }
    }

    public class ModuloCondition : ICondition
    {
        private readonly int _operand;
        private readonly int _operand2;
        private readonly Node _node;

        public ModuloCondition(Node node, int operand, int operand2)
        {
            _node = node;
            _operand = operand;
            _operand2 = operand2;
        }

        public bool IsCorrect()
        {
            return (int) _node.Number % _operand == 0 && (int) _node.Number % _operand2 == 0;
        }
    }
}