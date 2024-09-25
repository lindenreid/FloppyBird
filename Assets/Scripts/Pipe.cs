using UnityEngine;

public class Pipe : MonoBehaviour
{
    // References to the pipe objects
    [SerializeField] private Transform _topPipe;
    [SerializeField] private Transform _bottomPipe;
    // Tuning for the max distance we might shift the pipes up (maxShift) or down (minShift)
    [SerializeField] private float _minShift = -2.0f;
    [SerializeField] private float _maxShift = 4.0f;

    private void Start ()
    {
        // Pick a random distance to shift the pipes up or down
        float shift = UnityEngine.Random.Range(_minShift, _maxShift);

        Debug.Log("shift: " + shift);

        // Apply the shift ONLY to the y-position of both pipes
        _topPipe.position = new Vector3(
            _topPipe.position.x,
            _topPipe.position.y + shift,
            _topPipe.position.z
        );

        _bottomPipe.position = new Vector3(
            _bottomPipe.position.x,
            _bottomPipe.position.y + shift,
            _bottomPipe.position.z
        );
    }
}