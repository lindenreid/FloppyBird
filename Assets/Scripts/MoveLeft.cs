using UnityEngine;

public class MoveLeft : MonoBehaviour
{
    [SerializeField] private float _speed = 1.0f;
    [SerializeField] private Transform _target;
    [SerializeField] private float _startDistanceFromPlayer = 2.0f;
    [SerializeField] private float _spacingBetweenSets = 1.0f;

    void Update()
    {
        _target.position += Vector3.left * _speed * Time.deltaTime;
    }

    public void SetInitialPosition (int placeInLine)
    {
        // Place entire pipe set at x-position that's spaced:
        //  [_startDistanceFromPlayer] away from the player PLUS
        //  the pipe's placement in the pipe-line * the distance between each pipe set
        _target.localPosition = new Vector3(
            _startDistanceFromPlayer + placeInLine * _spacingBetweenSets,
            0,
            0
        );
    }
}
