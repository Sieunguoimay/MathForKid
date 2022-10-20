using System;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameplayMono prefab;
    [SerializeField] private TextMeshPro result;
    [SerializeField] public CameraSizeAuto cameraSizeAuto;
    [SerializeField] private Transform screen;
    [SerializeField] public Transform girl;
    [SerializeField] public ParticleSystem fx;
    [SerializeField] public bool randomize;
    [SerializeField] public GameObject tick;
    [SerializeField] public GameObject close;

    private GameplayMono _gameplayMono;
    private Vector3 _position;
    public int correctAnswerNum = 8;

    private void Start()
    {
        _position = girl.position;
        ResetGame();
        tick.SetActive(randomize);
#if UNITY_WEBGL
        close.SetActive(false);
#endif
    }

    public void Play()
    {
        _gameplayMono = Instantiate(prefab, transform);
        _gameplayMono.Setup(this, randomize);
        screen.gameObject.SetActive(false);
    }

    public void ResetGame()
    {
        result.text = "";
        screen.gameObject.SetActive(true);
        girl.position = _position;
        cameraSizeAuto.cam.orthographicSize = 5;
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    public void Toggle()
    {
        randomize = !randomize;
        tick.SetActive(randomize);
    }

    public void Stop()
    {
        Destroy(_gameplayMono.gameObject);
        ResetGame();
    }

    public void PlayFX()
    {
        fx.Play();
    }

    public void ShowResultYouWin(int score, float timer)
    {
        result.text = $"<size=7>You win!</size>\nScore: {score}\nTime: {timer}";
    }

    public void ShowResultYouLose(bool timeout)
    {
        result.text = $"<size=7>You lose!</size>\n{(timeout ? "Timeout" : "Too many wrong answers..")}";
    }
}