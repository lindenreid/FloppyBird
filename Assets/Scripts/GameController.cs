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
    [SerializeField] private MoveLeft _pipePrefab;
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

        // Find and kill any environment pieces (pipes or floor tiles)
        //      that have moved past the far left of the screen.
        // Because you can't change the contents of a List while iterating through it,
        //      we must first create a separate list of items to remove (despawnList).
        List<MoveLeft> despawnList = new List<MoveLeft>();
        foreach(MoveLeft item in _environment)
        {
            if(item.transform.position.x + item.transform.lossyScale.x < worldMin.x)
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
            DespawnAndReplace(despawnItem);
        }
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
            SpawnPipe(i);
        }

        for(int i = 0; i < _numStartFloorTiles; i++)
        {
            SpawnFloorTile(i);
        }
    }

    private void SpawnPipe (int placeInLine)
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
        pipe.SetInitialPosition(placeInLine);
    }

    private void SpawnFloorTile (int placeInLine)
    {
        // Instantiate a floor tile object
        // Casting here works the same as it does for the Pipe above!
        MoveLeft floorTile = Instantiate(_floorPrefab, _floorAnchor) as MoveLeft;

        // check that the instantiation completed successfully
        // & add the tile to our environment list
        Assert.IsNotNull(floorTile);
        _environment.Add(floorTile);

        // tell the MoveLeft component to get ready
        floorTile.SetInitialPosition(placeInLine);
    }

    private void DespawnAndReplace (MoveLeft moveLeft)
    {
        // Check the Tags set in the Inspector on this GameObject
        //      to decide whether we need to spawn a new Pipe
        //      or floor tile to replace it.
        if(moveLeft.CompareTag("Pipe"))
        {
            SpawnPipe(_numStartPipes - 1);
        }
        else if(moveLeft.CompareTag("Platform"))
        {
            SpawnFloorTile(_numStartFloorTiles - 1);
        } else 
        {
            Assert.IsTrue(false, "No valid tag set on MoveLeft object.");
        }

        // Remove this GameObject from the Scene.
        Destroy(moveLeft.gameObject);
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
