using UnityEngine;
using System;

public enum MovementInput {
    None,
    Forward,
    Back,
    Left,
    Right,    
    Up,
    Axis
}

public class PlayerInputManager : SingletonMono<PlayerInputManager> {

    private const int LEFT_CLICK = 0, RIGHT_CLICK = 1;

    public bool AxisBasedMovement = true;

    public delegate void CharacterMovementEvent(MovementInput inputType, Vector3 axisInput);
    public event CharacterMovementEvent OnCharacterMovementInput;

    private Vector3 axisMovement;
    private PlayerInputManager() {
        axisMovement = Vector3.zero;
    }

    private void Update() {
        axisMovement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        if (AxisBasedMovement)
            OnCharacterMovementInput(MovementInput.Axis, axisMovement);
        else {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) OnCharacterMovementInput(MovementInput.Forward, Vector3.zero);
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) OnCharacterMovementInput(MovementInput.Left, Vector3.zero);
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) OnCharacterMovementInput(MovementInput.Right, Vector3.zero);
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) OnCharacterMovementInput(MovementInput.Back, Vector3.zero);
        }
        if (Input.GetKeyUp(KeyCode.Space)) OnCharacterMovementInput(MovementInput.Up, Vector3.zero);

        if (axisMovement.Equals(Vector3.zero))
            OnCharacterMovementInput(MovementInput.None, Vector3.zero);        
    }
}