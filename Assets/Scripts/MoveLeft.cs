using UnityEngine;

public class MoveLeft : MonoBehaviour
{
    [SerializeField] private float _speed = 1.0f;
    [SerializeField] private Transform _target;
    [SerializeField] private Vector3 _originalPosition;

    void Update()
    {
        _target.position += Vector3.left * _speed * Time.deltaTime;
    }

    public void Reset()
    {
        _target.position = _originalPosition;
    }
}
