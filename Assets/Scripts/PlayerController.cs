using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _jumpForce = 1.0f;
    [SerializeField] private Transform _target;
    [SerializeField] private Rigidbody2D _physics;
    [SerializeField] private GameController _gameController;

    public void Setup()
    {
        _target.position = Vector3.zero;
    }

    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            //Debug.Log("pressed space");
            _physics.velocity = Vector2.zero;
            _physics.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        //Debug.Log("hit " + collider.name);
        if(collider.gameObject.CompareTag("Environment"))
        {
            _gameController.PlayerHitPipe();
        }
    }
}
