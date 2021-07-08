using UnityEngine;

public class PlayerEntity : EntityBase
{
    #region VARIABLES

    [Header("Behaviours")]
    public GridMovement3D MovementBehaviour;


    private Vector2 _inputAxis = Vector2.zero;

    #endregion

    #region UNITY METHODS

    public void Update()
    {
        ProcessInput();
        MovementBehaviour.Movement(_inputAxis);
    }

    public void LateUpdate()
    {
        MovementBehaviour.DrawRayToTargetPosition();
    }

    #endregion

    #region METHODS

    private void ProcessInput()
    {
        _inputAxis.x = Input.GetAxisRaw("Horizontal");
        _inputAxis.y = Input.GetAxisRaw("Vertical");
    }

    #endregion
}