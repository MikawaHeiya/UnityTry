using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllableBehaveManage : MonoBehaviour
{
    public float rotateSpeed = 200f;
    public float gravity = 9.8f;

    private bool isGrounded = true;
    private CharacterController characterController;
    private Camera controllableComera;
    private Animator controllableAnimator;
    private float cameraRotateX;
    private int moveStatus = 0;

    public float moveSpeed
    {
        set
        {
            _moveSpeed = value;
        }

        get
        {
            switch (moveStatus)
            {
                case 0: return _moveSpeed;
                case 1: return _moveSpeed;
                case 2: return _moveSpeed * 2;
                default: return 0;
            }
        }
    }
    private float _moveSpeed;

    public float jumpSpeed
    {
        set
        {
            _jumpSpeed = value;
        }

        get
        {
            switch (moveStatus)
            {
                case 0: return _jumpSpeed;
                case 1: return _jumpSpeed;
                case 2: return _jumpSpeed * 2;
                default: return 0;
            }
        }
    }
    private float _jumpSpeed;


    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        controllableComera = GetComponentInChildren<Camera>();
        controllableAnimator = GetComponent<Animator>();

        moveSpeed = 2f;
        jumpSpeed = 3f;

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Start()
    {
        cameraRotateX = controllableComera.transform.localRotation.x;
    }

    private void Update()
    {
        //GroundCheck();
        HandleControllableMove();
        HandleAnimate();
        ResetComponent();
    }

    private void GroundCheck()
    {
        if (Physics.Raycast(transform.position, Vector3.down, LayerMask.GetMask("Floor")))
        {
            isGrounded = false;
        }
        else
        {
            isGrounded = true;
        }
        Debug.Log(isGrounded);
    }

    private int MoveStatusCheck(Vector3 move)
    {
        moveStatus = 0;

        if (move != Vector3.zero)
        {
            moveStatus = 1;
            if (Input.GetKeyDown(KeyCode.R))
            {
                moveStatus = 2;
            }
        }

        return moveStatus;
    }

    private void ResetComponent()
    {
        if (moveStatus == 2)
        {
            controllableComera.transform.localPosition = new Vector3(0f, 1.4f, 0.3f);
            characterController.radius = 0.4f;
        }
        else
        {
            controllableComera.transform.localPosition = new Vector3(0f, 1.4f, 0f);
            characterController.radius = 0.3f;
        }
    }

    private void HandleAnimate()
    {
        controllableAnimator.SetInteger("MoveStatus", moveStatus);
    }

    private void HandleKeyboardMove(Vector3 forward)
    {
        Vector3 move = transform.TransformPoint(forward) - transform.position;
        move.x *= moveSpeed;
        move.z *= moveSpeed;
        move.y = (move.y == 0f) ? 0f : (move.y * jumpSpeed - gravity * Time.deltaTime);

        characterController.Move(move * moveSpeed * Time.deltaTime);
    }

    private void HandleHorizontalRotate(float angle)
    {
        transform.Rotate(new Vector3(0f, angle * rotateSpeed * Time.deltaTime, 0f), Space.Self);
    }

    private void HandleVerticalRotate(float angle)
    {
        cameraRotateX = Mathf.Clamp(cameraRotateX + angle * rotateSpeed * Time.deltaTime, -90f, 40f);
        controllableComera.transform.localRotation = Quaternion.Euler(cameraRotateX, 0f, 0f);
    }

    private void HandleControllableMove()
    {
        Vector3 horizontalMove = InputManager.GetKeyboardMove();
        float horizontalRotateAngle = InputManager.GetHorizontalMouseMove();
        float verticalRotateAngle = InputManager.GetVerticalMouseMove();

        MoveStatusCheck(horizontalMove);

        HandleHorizontalRotate(horizontalRotateAngle);
        HandleVerticalRotate(verticalRotateAngle);
        //if (isGrounded)
            HandleKeyboardMove(horizontalMove);
    }
}
