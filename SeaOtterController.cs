using DG.Tweening;
using ManagerControl;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SeaOtterController : MonoBehaviour
{
    private Health _health;
    private SoundManager _soundManager;
    private Transform _childObj;
    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rigid;
    private Vector2 _moveVec, _dashVec;
    private Vector2 _smoothVelocity;
    private Quaternion _seaOtterRotation;

    private float _h, _v;
    private bool _canDash;
    private bool _isDashCool;
    private bool _isStun;
    private bool _isDecreaseSpeed;

    public float moveSpeed;

    [SerializeField] private GameObject TimerController;
    [SerializeField] private Image dashCoolImage;
    [SerializeField] private Animator animator;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float stunTime;
    [SerializeField] private float dashDelay;
    [SerializeField] private float decreaseSpeedTime;

    private void Awake()
    {
        _health = GetComponent<Health>();
        _soundManager = SoundManager.Instance;
        _childObj = transform.GetChild(0);
        _spriteRenderer = _childObj.GetComponent<SpriteRenderer>();
        _rigid = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        MoveInput();
        Dash();
    }

    private void FixedUpdate()
    {
        if (_isStun) return;

        if (_canDash)
        {
            _rigid.MovePosition(_rigid.position + _dashVec * (dashSpeed * Time.deltaTime));
        }
        else
        {
            _rigid.MovePosition(_rigid.position + _moveVec * (moveSpeed * Time.deltaTime));
            PlayerRotate();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Obstacle"))
        {
            if (_isDecreaseSpeed) return;
            _soundManager.PlaySound(SoundManager.OnCollision);
            _isDecreaseSpeed = true;
            var curMoveSpeed = moveSpeed;
            moveSpeed *= 0.5f;
            animator.speed = 0.7f;
            _health.ObstacleDecreaseHealth(15);
            DOVirtual.DelayedCall(decreaseSpeedTime, () =>
            {
                moveSpeed = curMoveSpeed;
                animator.speed = 1;
            });
        }

        else if (collision.CompareTag("Box"))
        {
            GameManager.Instance.BoxFadeIn();
        }
        else if (collision.CompareTag("Box2"))
        {
            GameManager.Instance.Box2FadeIn();
        }
        else if (collision.CompareTag("Box3"))
        {
            // 1차 게임 종료
            //GameManager.Instance.GameClear();
            // 2차 보스 시작
            TimerController.GetComponent<TimerController>().enabled = true;
            
        }

        else if (collision.CompareTag("Oil"))
        {
            GameManager.Instance.GameOver();
            Time.timeScale = 0f;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Glacier"))
        {
            if (_isStun) return;
            _isStun = true;
            DOVirtual.DelayedCall(stunTime, () => _isStun = false);
        }
    }

    private void MoveInput()
    {
        if (_canDash) return;
        _h = Input.GetAxis("Horizontal");
        _v = Input.GetAxis("Vertical");
        _moveVec = new Vector2(_h, _v).normalized;
    }

    private void Dash()
    {
        if (_isDashCool) return;
        if (!Input.GetKeyDown(KeyCode.Space)) return;
        _soundManager.PlaySound(SoundManager.Dash);
        dashCoolImage.fillAmount = 0;
        _canDash = true;
        _isDashCool = true;
        _dashVec = _moveVec;
        dashCoolImage.DOFillAmount(1, dashDelay);
        DOVirtual.DelayedCall(0.3f, () => _canDash = false);
        DOVirtual.DelayedCall(dashDelay, () => _isDashCool = false);
    }

    private void PlayerRotate()
    {
        if (_moveVec == Vector2.zero) return;

        var targetRotation = Quaternion.LookRotation(Vector3.forward, new Vector3(_moveVec.x, _moveVec.y, 0)) *
                             Quaternion.Euler(0, 0, 90);
        _childObj.rotation = Quaternion.Slerp(_childObj.rotation, targetRotation, rotateSpeed * Time.deltaTime);
        _spriteRenderer.flipY = _moveVec.x < 0;
    }
}