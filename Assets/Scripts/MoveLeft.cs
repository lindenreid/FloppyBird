using UnityEngine;

public class MoveLeft : MonoBehaviour
{
    [SerializeField] private float _speed = 1.0f;
    [SerializeField] private Transform _target;
    [SerializeField] private BoxCollider2D _collider;

    void Update()
    {
        _target.position += Vector3.left * _speed * Time.deltaTime;
    }

    public void SetInitialPosition (float offset)
    {
        _target.localPosition = new Vector3(
            _collider.size.x * 2.0f + offset,
            0,
            0
        );
    }
}
