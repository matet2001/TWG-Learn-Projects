using System;
using System.Collections;
using UnityEngine;

public class PoolGameLoopController : MonoBehaviour
{
    [SerializeField] string[] gameEndTexts = new string[2];
    [SerializeField] int gameEndDelay = 3;

    public event Action<string, int> OnGameEndStart;
    public event EventHandler OnGameEnd;
    public event EventHandler OnGameRestart;

    public void StartGameEnd(bool isWin)
    {
        string stringToDisplay = (isWin) ? gameEndTexts[0] : gameEndTexts[1];
        OnGameEndStart?.Invoke(stringToDisplay, gameEndDelay);

        StartCoroutine(GameEndCountDown());
    }
    private IEnumerator GameEndCountDown()
    {
        yield return new WaitForSeconds(gameEndDelay);
        OnGameEnd?.Invoke(this, EventArgs.Empty);
    }
    public void TriggerOnGameRestart()
    {
        OnGameRestart?.Invoke(this, EventArgs.Empty);
    }
}
