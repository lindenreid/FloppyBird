using UnityEngine;

public class MoveLeft : MonoBehaviour
{
    public float _speed = 1.0f;
    public Transform _target;
    public Vector3 _originalPosition;

    void Update()
    {
        _target.position += Vector3.left * _speed;
    }

    public void Reset()
    {
        _target.position = _originalPosition;
    }
}
