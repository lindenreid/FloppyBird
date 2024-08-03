using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float _jumpForce = 1.0f;
    public Transform _target;
    public Rigidbody2D _physics;
    public GameController _gameController;

    public void Setup()
    {
        _target.position = Vector3.zero;
    }

    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            //Debug.Log("pressed space");
            _physics.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        //Debug.Log("hit " + collider.name);
        if(collider.gameObject.CompareTag("Pipe"))
        {
            _gameController.PlayerHitPipe();
        }
    }
}
