using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
    public PlayerController _player;
    public MoveLeft _environment;
    public GameObject _gameOverScreen;
    public TMP_Text _scoreText;

    void Start()
    {
        SetupGame();
    }

    public void PlayerHitPipe()
    {
        GameOver();
    }

    public void RestartButton ()
    {
        SetupGame();
    }

    private void SetupGame()
    {
        _player.Setup();
        _gameOverScreen.SetActive(false);
        _player.enabled = true;
        _environment.Reset();
    }

    private void GameOver()
    {
        _gameOverScreen.SetActive(true);
        _player.enabled = false;
    }
}
