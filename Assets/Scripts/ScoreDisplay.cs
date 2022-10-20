using TMPro;
using UnityEngine;

public class ScoreDisplay : MonoBehaviour, IScore
{
    [SerializeField] private TextMeshPro text;
    private int _score;

    public int Score
    {
        get => _score;
        set
        {
            _score = value;
            text.text = $"Score: {_score}";
        }
    }
}

public interface IScore
{
    int Score { get; set; }
}