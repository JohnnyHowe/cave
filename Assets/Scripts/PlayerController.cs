using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Horizontal Movement")]
    public float accelerationForce = 1f;
    public float maxRunSpeed = 1f;
    public float minSlideTime = 1f;
    public float slideImpulse = 100f;
    public float passiveSlideForce = 1f;
    [Header("Vertical Movement")]
    public float jumpHeight = 4;
    [Header("Other")]
    public LayerMask groundLayer;
    public Transform groundCheck;
    public PlayerStateChange runStateChange;
    public PlayerStateChange jumpStateChange;
    public PlayerStateChange slideStateChange;
    public PlayerStateChange dropStateChange;

    float currentSlideTime = 0;
    public PlayerState state
    {
        get { return _state; }
        set
        {
            if (_state != value)
            {
                PlayerStateChange last = GetPlayerStateChange(_state);
                _state = value;
                PlayerStateChange next = GetPlayerStateChange(_state);
                last.onEnd.Invoke();
                last.body.SetActive(false);
                next.onStart.Invoke();
                next.body.SetActive(true);
            } else if (GetPlayerStateChange(value).canRepeat) {
                GetPlayerStateChange(value).onStart.Invoke();
            }
        }
    }
    PlayerState _state = PlayerState.running;
    Rigidbody2D rb2d;

    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        // Passive state changes
        switch (state)
        {
            case PlayerState.running:
                Run();
                CapSpeed();
                break;
            case PlayerState.jumping:
                Run();
                CapSpeed();
                if (rb2d.velocity.y < 0 && OnGround())
                {
                    state = PlayerState.running;
                }
                break;
            case PlayerState.dropping:
                Run();
                CapSpeed();
                if (OnGround())
                {
                    Slide();
                }
                break;
            case PlayerState.sliding:
                if (!OnGround()) { state = PlayerState.running; }
                rb2d.AddForce(Vector2.right * passiveSlideForce);
                currentSlideTime -= Time.fixedDeltaTime;
                if (currentSlideTime < 0)
                {
                    if (CanStand())
                    {
                        state = PlayerState.running;
                    }
                }
                break;
        }
    }

    void Update()
    {
        // Active state changes
        SwipeDirection swipeDirection = InputController.Instance.GetSwipeDirection();
        switch (swipeDirection)
        {
            case SwipeDirection.up:
                TryJump();
                break;
            case SwipeDirection.down:
                TrySlide();
                break;
        }
    }

    void CapSpeed()
    {
        rb2d.velocity = new Vector2(Mathf.Min(maxRunSpeed, rb2d.velocity.x), rb2d.velocity.y);
    }

    void Run()
    {
        rb2d.AddForce(Vector2.right * accelerationForce);
    }

    bool OnGround()
    {
        return !!Physics2D.OverlapBox(groundCheck.position, groundCheck.lossyScale, 0, groundLayer);
    }

    bool CanStand()
    {
        float margin = 0.1f;
        return !Physics2D.OverlapBox(
            runStateChange.body.transform.position + margin * Vector3.up,
            runStateChange.body.transform.lossyScale - margin * Vector3.up,
            0, groundLayer
            );
    }

    void TrySlide()
    {
        if (state != PlayerState.sliding)
        {
            if (OnGround())
            {
                Slide();
            }
            else
            {
                Drop();
            }
        }
    }

    void TryJump()
    {
        if (OnGround())
        {
            Jump();
        }
    }

    void Slide()
    {
        state = PlayerState.sliding;
        rb2d.velocity = new Vector2(0, rb2d.velocity.y);
        rb2d.AddForce(Vector2.right * slideImpulse, ForceMode2D.Impulse);
        currentSlideTime = minSlideTime;
    }

    void Drop()
    {
        state = PlayerState.dropping;
    }

    void Jump()
    {
        float jumpSpeed = Mathf.Sqrt(2 * jumpHeight * 9.8f * rb2d.gravityScale);
        rb2d.velocity = new Vector2(rb2d.velocity.x, jumpSpeed);
        state = PlayerState.jumping;
    }

    PlayerStateChange GetPlayerStateChange()
    {
        return GetPlayerStateChange(state);
    }

    PlayerStateChange GetPlayerStateChange(PlayerState state)
    {
        switch (state)
        {
            case PlayerState.jumping:
                return jumpStateChange;
            case PlayerState.sliding:
                return slideStateChange;
            default:
                return runStateChange;
        }
    }
}


[System.Serializable]
public enum PlayerState
{
    running,
    jumping,
    sliding,
    dropping,
}

[System.Serializable]
public struct PlayerStateChange
{
    public GameObject body;
    public UnityEvent onStart;
    public UnityEvent onEnd;
    public bool canRepeat;
}