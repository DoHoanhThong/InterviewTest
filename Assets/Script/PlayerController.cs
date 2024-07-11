using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static Action<bool> collideWithBall;
    [SerializeField] float _moveSpeed = 5f;
    [SerializeField] float _rotationSpeed = 700f;
    [SerializeField] FixedJoystick _joystick;
    PlayerState _state;
    [SerializeField] AnimationController _anim;
    Vector3 _movement;
    Rigidbody _rigi;
    // Start is called before the first frame update
    void Start()
    {
        _rigi = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
        UpdateState();
    }
    void Move()
    {
        if (GameController.instant.isBallMoving)
            return;
        float moveHorizontal = 0, moveVertical = 0;
        if (Application.isEditor)
        {
            moveHorizontal = Input.GetAxisRaw("Horizontal");
            moveVertical = Input.GetAxisRaw("Vertical");
        }
        else
        {
            moveHorizontal = _joystick.Horizontal;
            moveVertical = _joystick.Vertical;
        }


        _movement = new Vector3(moveHorizontal, 0, moveVertical).normalized;
        if (_movement != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(_movement, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, _rotationSpeed * Time.deltaTime);
        }
        _rigi.MovePosition(transform.position + _movement * _moveSpeed * Time.fixedDeltaTime);
    }
    void UpdateState()
    {
        if (_movement != Vector3.zero)
        {
            _state = PlayerState.RUN;
        }
        else
        {
            _state = PlayerState.IDLE;
        }
        _anim.UpdateAnimationPlayer(_state);
    }
    public enum PlayerState
    {
        IDLE,
        RUN
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other == null) return;
        if (other.gameObject.tag == "ball")
        {
            collideWithBall?.Invoke(true);
            GameController.instant.SetBall(other.gameObject);
        }
    }
    private void OnCollisionExit(Collision other)
    {
        if (other == null) return;
        if (other.gameObject.tag == "ball")
        {
            collideWithBall?.Invoke(false);

        }
    }
}
