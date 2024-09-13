using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

public class GameController : MonoBehaviour
{
    // References to other stuff in the Scene that the GameController
    //      will need to ... control
    [SerializeField] private PlayerController _player;
    [SerializeField] private GameObject _gameOverScreen;
    [SerializeField] private TMP_Text _scoreText;

    // Prefabs for the environment pieces that the GameController will spawn
    // and keep track of
    // & references for WHERE to spawn them (anchors)
    [SerializeField] private Pipe _pipePrefab;
    [SerializeField] private MoveLeft _floorPrefab;
    [SerializeField] private Transform _pipeAnchor;
    [SerializeField] private Transform _floorAnchor;

    // the number of pipes and tiles that will spawn immediately upon game load
    // I exposed these to the Inspector to make it super easy to tune them
    [SerializeField] private int _numStartPipes = 5;
    [SerializeField] private int _numStartFloorTiles = 2;

    // a list of all of the moving pieces in the game
    // keeping track of them so that we can despawn them when they go off screen
    private List<MoveLeft> _environment;

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

    // Create the environment, 
    // reset the player and points,
    // and setup the UI for playing the game
    private void SetupGame()
    {
        // set up player
        _player.Setup();
        _player.enabled = true;

        // set up UI
        _gameOverScreen.SetActive(false);

        // spawn pipes and floor tiles for environment
        // keep track of all of these pieces because they're moving
        _environment = new List<MoveLeft>();
        for(int i = 0; i < _numStartPipes; i++)
        {
            SpawnPipe();
        }

        for(int i = 0; i < _numStartFloorTiles; i++)
        {
            SpawnFloorTile();
        }
    }

    private void SpawnPipe ()
    {
        // Instantiate a Pipe object
        // this makes a copy of the Pipe Prefab and adds it to the scene
        // also, put the Pipe under the _pipeAnchor object in the Hierarchy

        // Because the return type of Instantiate is Object, 
        // but our _pipePrefab IS a Pipe (which is a MonoBehaviour, which is an Object),
        // we can cast the result of Instantiate as Pipe
        Pipe pipe = Instantiate(_pipePrefab, _pipeAnchor) as Pipe;

        // check that the instantiation completed successfully
        Assert.IsNotNull(pipe);

        // tell the Pipe component to get ready
        pipe.Setup();

        MoveLeft ml = pipe.GetComponent<MoveLeft>();
        // check that the Pipe object has a MoveLeft component
        Assert.IsNotNull(ml);

        // add the MoveLeft component of the Pipe object to our list 
        _environment.Add(ml);
    }

    private void SpawnFloorTile ()
    {
        // Instantiate a floor tile object
        // Casting here works the same as it does for the Pipe above!
        MoveLeft floorTile = Instantiate(_floorPrefab, _floorAnchor) as MoveLeft;

        // check that the instantiation completed successfully
        // & add the tile to our environment list
        Assert.IsNotNull(floorTile);
        _environment.Add(floorTile);
    }

    private void GameOver()
    {
        _gameOverScreen.SetActive(true);
        _player.enabled = false;

        foreach(MoveLeft m in _environment)
        {
            Destroy(m.gameObject);
        }
        _environment.Clear();
    }
}
