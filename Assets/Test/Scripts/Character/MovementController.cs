using UnityEngine;
using System;
using System.Collections;

public class MovementController : MonoBehaviour {

    public LayerMask myLayerMask;

    public float MinAcceleration = 0.1f
                , RunAcceleration = 0.6f
                , MaxAcceleration = 1.0f
                , acceleration = 0.2f;

    public Animator animator;
    public float forwardSpeed = 10,
                 backSpeed = 4,
                 sidesSpeed = 8,                 
                 jumpPower = 50;

    private float currentAcceleration;

    private DispatcherTable<MovementInput> movementDispatcher;

    private void Awake() {
        InitMovementDispatcher();        
        PlayerInputManager.Instance.OnCharacterMovementInput += OnMovementInput;
    }

    private void Start() {
        CurrentAcceleration = MinAcceleration;
    }

    private void Update() {
        CurrentAcceleration -= (acceleration*0.05f) * Time.deltaTime;
    }

    private void OnDestroy() {
        if (PlayerInputManager.Instance)
            PlayerInputManager.Instance.OnCharacterMovementInput -= OnMovementInput;
    }

    private bool IsGrounded {
        get {                        
            RaycastHit hitInfo = new RaycastHit();
            if (Physics.Raycast(transform.position, -transform.up, out hitInfo, myLayerMask))
                return hitInfo.distance < 0.1f;
            else return true;
        }
    }

    private void OnMovementInput(MovementInput input, Vector3 movementInput) {
        movementDispatcher.Dispatch<Vector3>(input, movementInput);
    }

    //@NOTE: rigidbody does not work properly due to the very low scale of the stage
    private void InitMovementDispatcher() {
        movementDispatcher = new DispatcherTable<MovementInput>();
        movementDispatcher.AddAction(MovementInput.Forward, new Action<Vector3>(movementInput => {
            CurrentAcceleration += acceleration * Time.deltaTime;
            //rigidbody.AddForce(Vector3.forward * forwardSpeed * acceleration * Time.deltaTime);
            transform.Translate(Vector3.forward * forwardSpeed * acceleration * Time.deltaTime);
        }));
        movementDispatcher.AddAction(MovementInput.Back, new Action<Vector3>(movementInput => {
            CurrentAcceleration = MinAcceleration + 0.01f;
            //rigidbody.AddForce(Vector3.back * backSpeed * Time.deltaTime);
            transform.Translate(Vector3.back * backSpeed * Time.deltaTime);
        }));
        movementDispatcher.AddAction(MovementInput.Left, new Action<Vector3>(movementInput => {
            CurrentAcceleration += acceleration * Time.deltaTime;
            //rigidbody.AddForce(Vector3.left * sidesSpeed * acceleration * Time.deltaTime);
            transform.Translate(Vector3.left * sidesSpeed * acceleration * Time.deltaTime);
        }));
        movementDispatcher.AddAction(MovementInput.Right, new Action<Vector3>(movementInput => {
            CurrentAcceleration += acceleration * Time.deltaTime;
            //rigidbody.AddForce(Vector3.right * sidesSpeed * acceleration * Time.deltaTime);
            transform.Translate(Vector3.right * sidesSpeed * acceleration * Time.deltaTime);
        }));
        movementDispatcher.AddAction(MovementInput.Up, new Action<Vector3>(movementInput => {
            if (IsGrounded) {                
                //rigidbody.AddForce(Vector3.up * jumpPower * Time.deltaTime);
                transform.Translate(Vector3.up * jumpPower * Time.deltaTime);
            }
        }));

        movementDispatcher.AddAction(MovementInput.Axis, new Action<Vector3>(movementInput => {
            if (!movementInput.Equals(Vector3.zero))
                CurrentAcceleration += acceleration * Time.deltaTime;
            movementInput.x *= acceleration * sidesSpeed;
            movementInput.z *= (movementInput.z > 0) ? acceleration * forwardSpeed : backSpeed;
            //rigidbody.AddForce(movementInput * Time.deltaTime);
            transform.Translate(movementInput * Time.deltaTime);
        }));

        movementDispatcher.AddAction(MovementInput.None, new Action<Vector3>(movementInput => {
            CurrentAcceleration = MinAcceleration;
        }));
    }

    public void RefreshAnimator() {
        animator.SetBool("IsIdle", CurrentAcceleration == MinAcceleration);
        animator.SetBool("IsWalking", CurrentAcceleration < RunAcceleration && CurrentAcceleration != MinAcceleration);
        animator.SetBool("IsRunning", CurrentAcceleration >= RunAcceleration);
    }

    public float CurrentAcceleration {
        get { return currentAcceleration; }
        set {
            if (value > MaxAcceleration) currentAcceleration = MaxAcceleration;
            else if (value < MinAcceleration) currentAcceleration = MinAcceleration;
            else currentAcceleration = value;
            RefreshAnimator();
        }
    }
}