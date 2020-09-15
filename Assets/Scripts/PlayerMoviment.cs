using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMoviment : MonoBehaviour
{

    [SerializeField] private float speed = 10f;
    [SerializeField] private float jumpForce = 200f;

    [SerializeField] private LayerMask groundLayer = -1;

    private Rigidbody2D _rigidbody2D;
    private bool _isJumping = false;
    private bool _isOnGround = false;
    private RaycastHit2D _hitGround;

    void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Update() {
        _hitGround = Physics2D.Raycast(transform.position, Vector2.down, 0.65f, groundLayer);
        _isOnGround = _hitGround;
        VerticalMoviment();
    }

    void FixedUpdate() 
    {
        HorizontalMoviment();
    }

    private void HorizontalMoviment()
    {
        float horizontalInput = Input.GetAxis(Axis.AXIS_HORIZONTAL);
        transform.Translate(Vector2.right * speed * horizontalInput);
    }

    private void VerticalMoviment()
    {
        bool jump = Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow);
        bool crouch = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);

        if (_isOnGround && !_isJumping && jump) 
        {
            _rigidbody2D.velocity = Vector2.zero;
            _rigidbody2D.AddForce(Vector2.up * jumpForce);
            _isJumping = true;
        }
        else if (!_isOnGround && _isJumping && crouch) 
        {
            Vector2 velocity = Vector2.down * jumpForce;
            _rigidbody2D.AddForce(velocity / 10f);
            _rigidbody2D.velocity = Vector2.Lerp(Vector2.zero, velocity, _rigidbody2D.velocity.y / velocity.y);
        }
        else if (_isOnGround && _isJumping)
        {
            _isJumping = false;
            _rigidbody2D.velocity = Vector2.zero;
        }
    }

}
