using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

public class GameController : MonoBehaviour
{
    // References to other stuff in the Scene that the GameController
    //      will need to ... control
    [SerializeField] private PlayerController _player;
    [SerializeField] private GameObject _gameOverScreen;  // TODO: use events
    [SerializeField] private TMP_Text _scoreText;

    // Prefabs for the environment pieces that the GameController will spawn
    // and keep track of
    // & references for WHERE to spawn them (anchors)
    [SerializeField] private MoveLeft _pipePrefab;
    [SerializeField] private Transform _pipeAnchor;

    // Tuning for how fast the pipes appear
    [SerializeField] private float _pipeSpawnRate = 2.0f;

    // Events for UI to hook into to respond to game happenings.
    public delegate void IntDelegate(int x);
    public delegate void EmptyDelegate();

    public event IntDelegate PointsChanged;
    public event EmptyDelegate GameEnded;

    // a list of all of the moving pieces in the game
    // keeping track of them so that we can despawn them when they go off screen
    private List<MoveLeft> _environment;

    // The countdown to spawning a new pipe set.
    private float _pipeCountdown;

    // Half the size of the screen, used for placing objects correctly upon spawn.
    private float _halfWorldWidth;

    // Keeping track of player score.
    private int _points;

    // The maximum world bounds.
    public Vector3 GetWorldMax () =>
                Camera.main.ScreenToWorldPoint(
                    new Vector3(
                        Screen.width,
                        Screen.height,
                        Camera.main.transform.position.z
                    )
                );

    // The minimum world bounds.
    private Vector3 GetWorldMin () =>
                Camera.main.ScreenToWorldPoint(
                    new Vector3(
                        0,
                        0,
                        -Camera.main.transform.position.z
                    )
                );

    private void Start()
    {
        SetupGame();
    }

    private void Update ()
    {
        Vector3 worldMin = GetWorldMin();

        // Find and kill any environment pieces
        //      that have moved past the far left of the screen.
        // Because you can't change the contents of a List while iterating through it,
        //      we must first create a separate list of items to remove (despawnList).
        List<MoveLeft> despawnList = new List<MoveLeft>();
        foreach(MoveLeft item in _environment)
        {
            if(item.transform.position.x + item.transform.lossyScale.x < worldMin.x - _halfWorldWidth)
            {
                despawnList.Add(item);
            }
        }

        // Now that we're done looping through _environment,
        //      we loop through despawnList and remove any items 
        //      from _environment that are on this kill list.
        foreach(MoveLeft despawnItem in despawnList)
        {
            _environment.Remove(despawnItem);
            Despawn(despawnItem);
        }

        // Spawn a new pipe every [_pipeSpawnRate] seconds.
        _pipeCountdown -= Time.deltaTime;
        if(_pipeCountdown <= 0.0f)
        {
            SpawnPipe();
            _pipeCountdown = _pipeSpawnRate;
        }
    }

    private void DespawnAllMoveLeft ()
    {
        if(_environment == null || _environment.Count == 0) return;

        foreach(MoveLeft despawnItem in _environment)
        {
            Despawn(despawnItem);
        }
        _environment.Clear();
    }


    public void PlayerHitPipe()
    {
        GameOver();
    }

    public void PlayerEarnedPoint ()
    {
        _points++;
        PointsChanged?.Invoke(_points);
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
        // cache the world size
        // MUST do this before spawning environment!!
        _halfWorldWidth = (GetWorldMax().x - GetWorldMin().x) / 2.0f;
        Debug.Log("half world width: "+ _halfWorldWidth);

        // set up player
        _player.Setup();
        _player.enabled = true;

        // reset points
        _points = 0;
        PointsChanged?.Invoke(_points);

        // set up UI
        _gameOverScreen.SetActive(false);

        // spawn initial pipe for environment
        // keep track of all of these pieces because they're moving
        DespawnAllMoveLeft();
        _environment = new List<MoveLeft>();
        SpawnPipe();

        // reset timers
        _pipeCountdown = _pipeSpawnRate;
    }

    private void SpawnPipe ()
    {
        // Instantiate a Pipe object
        // this makes a copy of the Pipe Prefab and adds it to the scene
        // also, put the Pipe under the _pipeAnchor object in the Hierarchy

        // Because the return type of Instantiate is Object, 
        // but our _pipePrefab IS a MoveLeft (which is a MonoBehaviour, which is an Object),
        // we can cast the result of Instantiate as MoveLeft
        MoveLeft pipe = Instantiate(_pipePrefab, _pipeAnchor) as MoveLeft;

        // check that the instantiation completed successfully
        Assert.IsNotNull(pipe);

        // add the MoveLeft component of the Pipe object to our list 
        _environment.Add(pipe);

        // tell the MoveLeft component to get ready
        pipe.SetInitialPosition(_halfWorldWidth);
    }

    private void Despawn (MoveLeft moveLeft)
    {
        // Remove this GameObject from the Scene.
        Destroy(moveLeft.gameObject);
    }

    private void GameOver()
    {
        GameEnded?.Invoke();
        _gameOverScreen.SetActive(true); // TODO: use events

        _player.enabled = false;

        foreach(MoveLeft m in _environment)
        {
            Destroy(m.gameObject);
        }
        _environment.Clear();
    }
}
