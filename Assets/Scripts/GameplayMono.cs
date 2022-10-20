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

    // private void Start()
    // {
    //     Setup();
    // }

    public void Setup(GameController gameController)
    {
        var graph = GraphGenerator.GenerateGraph(row, col, new Modulo6Problem());
        boardVisual.Setup(graph);
        gameController.cameraSizeAuto.Resize(col * boardVisual.Spacing / 1.2f);

        var pos = score.transform.localPosition;
        pos.z = row * boardVisual.Spacing / 2f + .5f;
        score.transform.localPosition = pos;

        var pos2 = timer.transform.localPosition;
        pos2.z = -row * boardVisual.Spacing / 2f - .5f;
        timer.transform.localPosition = pos2;

        var pos3 = market.localPosition;
        pos3.x = col * boardVisual.Spacing / 2f + 1f;
        market.localPosition = pos3;

        score.Score = 100;
        var gameplay = new Gameplay(graph, boardVisual, score, timer, gameController);
        gameplay.SetFirstNode();
        timer.StartTimer(gameplay.ShowYouLoseTimeOut);
    }

    public class Modulo6Problem : IProblemSpecific
    {
        public int GetInvalidNumber()
        {
            var valid = Random.Range(1, 600);
            while (valid % 6 == 0 || valid % 9 == 0)
            {
                valid = Random.Range(1, 600);
            }

            return valid;
        }

        public int GetSolutionNumber()
        {
            return Random.Range(1, 100) * 6;
        }

        public ICondition GetCondition(Node node)
        {
            return new ModuloCondition(node, 6);
        }
    }

    public class ModuloCondition : ICondition
    {
        private readonly int _operand;
        private readonly Node _node;

        public ModuloCondition(Node node, int operand)
        {
            _node = node;
            _operand = operand;
        }

        public bool IsCorrect()
        {
            return _node.Number % _operand == 0;
        }
    }
}