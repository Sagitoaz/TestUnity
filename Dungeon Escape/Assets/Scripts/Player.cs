using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D _rigid;
    [SerializeField] private float _jumpForce = 5.0f;
    [SerializeField] private LayerMask _groundLayer;
    private bool _resetJump = false;
    private bool _grounded = false;
    [SerializeField] private float _speed = 5.0f;
    private PlayerAnimation _playerAnim;
    private SpriteRenderer _playerSprite;
    void Start() {
        _rigid = GetComponent<Rigidbody2D>();   
        _playerAnim = GetComponent<PlayerAnimation>(); 
        _playerSprite = GetComponentInChildren<SpriteRenderer>();
    }
    void Update()  {
        Movement();
        if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.J)) && IsGrounded()) {
            _playerAnim.Attack();
        }
    }
    void Movement() {
        float move = Input.GetAxisRaw("Horizontal");
        
        _grounded = IsGrounded();

        Flip(move);

        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded()) {
            _rigid.linearVelocity = new Vector2(_rigid.linearVelocityX, _jumpForce);
            StartCoroutine(ResetJumpRoutine());
            _playerAnim.Jump(true);
        }

        _rigid.linearVelocity = new Vector2(move * _speed, _rigid.linearVelocityY);
        _playerAnim.Move(move);
    }
    bool IsGrounded() {
        RaycastHit2D hitInfor = Physics2D.Raycast(transform.position, Vector2.down, 0.6f, _groundLayer);
        if (hitInfor.collider != null) {
            if (!_resetJump) {
                _playerAnim.Jump(false);
                return true;
            }
        }
        return false;
    }
    void Flip(float move) {
        if (move > 0) {
            _playerSprite.flipX = false;
        } else if (move < 0) {
            _playerSprite.flipX = true;
        }
    }
    IEnumerator ResetJumpRoutine() {
        _resetJump = true;
        yield return new WaitForSeconds(0.1f);
        _resetJump = false;
    }
}
