using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public float jumpHeight = 4;
    public float accelerationForce = 1f;
    public float maxSpeed = 1f;
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float minDuckTime = 1f;

    public PlayerStateChange runStateChange;
    public PlayerStateChange jumpStateChange;
    public PlayerStateChange duckStateChange;

    float currentDuckTime = 0;

    PlayerState state
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
            }
        }
    }
    PlayerState _state = PlayerState.running;

    const float gravity = 9.8f;
    Rigidbody2D rb2d;

    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        // Ducking finish check
        if (state == PlayerState.ducking)
        {
            currentDuckTime -= Time.deltaTime;
            if (currentDuckTime <= 0)
            {
                state = PlayerState.running;
            }
        }

        // Jumping finish check
        if (state == PlayerState.jumping && OnGround() && rb2d.velocity.y < 0)
        {
            state = PlayerState.running;
        }

        // Run speed
        rb2d.AddForce(Vector2.right * accelerationForce);
        rb2d.velocity = new Vector2(
            Mathf.Min(maxSpeed, rb2d.velocity.x),
            rb2d.velocity.y
            );
    }

    void Update()
    {
        SwipeDirection swipeDirection = InputController.Instance.GetSwipeDirection();
        switch (swipeDirection)
        {
            case SwipeDirection.up:
                if (OnGround())
                {
                    Jump();
                }
                break;
            case SwipeDirection.down:
                Duck();
                break;
        }
    }

    bool OnGround()
    {
        return !!Physics2D.OverlapBox(groundCheck.position, groundCheck.lossyScale, 0, groundLayer);
    }

    void Jump()
    {
        float jumpSpeed = Mathf.Sqrt(2 * jumpHeight * gravity * rb2d.gravityScale);
        // float jumpForce = rb2d.mass * jumpSpeed;
        rb2d.velocity = new Vector2(rb2d.velocity.x, jumpSpeed);
        state = PlayerState.jumping;
    }

    void Duck()
    {
        state = PlayerState.ducking;
        currentDuckTime = minDuckTime;
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
            case PlayerState.ducking:
                return duckStateChange;
            default:
                return runStateChange;
        }
    }
}


public enum PlayerState
{
    running,
    jumping,
    ducking,
}

[System.Serializable]
public struct PlayerStateChange
{
    public GameObject body;
    public UnityEvent onStart;
    public UnityEvent onEnd;
}