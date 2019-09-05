using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{
    public void LoadStartMenu()
    {
        SceneManager.LoadScene(0);
        GameSession gameSession = FindObjectOfType<GameSession>();
        gameSession.ResetGame();
    }

    public void LoadMainGame()
    {
        SceneManager.LoadScene("GameCore");
        GameSession gameSession = FindObjectOfType<GameSession>();
        gameSession.ResetGame();
    }

    public void LoadGameOver()
    {
        StartCoroutine(GameOverCoroutine());
    }

    private IEnumerator GameOverCoroutine()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("GameOver");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
