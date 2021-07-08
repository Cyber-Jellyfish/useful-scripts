using System.Collections;
using UnityEngine;

public enum GridMovementType
{
    BySpeed,
    ByTime,
}

public class GridMovement3D : MonoBehaviour
{
    #region VARIABLES

    [Header("Debug")]
    public bool DrawRayLines = false;

    [Header("Movement Type")]
    public GridMovementType GridMovementType = GridMovementType.BySpeed;

    [Header("Movement")]
    public float MoveSpeed = 5f;
    public float RotationSpeed = 5f;
    public float TimeToMove    = 0.2f;

    [Header("Contact Mask")]
    public LayerMask ContactMask;

    [Header("Orientation")]
    public bool IsMoving;

    [Header("Settings")]
    [Tooltip("Offset the Target Position by this Offset Value.")]
    public float Offset = 1f;

    private Camera  _camera;
    private Vector3 _originPosition;
    private Vector3 _targetPosition;
    private float   _targetAngle;

    #endregion

    #region UNITY METHODS

    private void Awake()
    {
        _camera = Camera.main;
    }

    private void Start()
    {
        // Set Origin and Target to Objects current position
        _originPosition = _targetPosition = transform.position;
    }

    #endregion

    #region STEP MOVEMENT METHODS

    public void Movement(Vector2 inputAxis)
    {
        if (GridMovementType == GridMovementType.ByTime)
            GridMovementByTime(inputAxis);
        if (GridMovementType == GridMovementType.BySpeed)
            GridMovementBySpeed(inputAxis);
    }

    private void GridMovementByTime(Vector2 inputAxis)
    {
        RotatePlayer(inputAxis);

        if (IsMoving) return;

        LerpTowardsTarget(inputAxis, Vector2.left, Vector3.left);
        LerpTowardsTarget(inputAxis, Vector2.right, Vector3.right);
        LerpTowardsTarget(inputAxis, Vector2.up, Vector3.forward);
        LerpTowardsTarget(inputAxis, Vector2.down, Vector3.back);
    }

    private void LerpTowardsTarget(Vector2 inputAxis, Vector2 inputDirection, Vector3 movementDirection)
    {
        if (inputAxis != inputDirection) return;
        StartCoroutine(LerpMovementRoutine(movementDirection * Offset));
    }

    private IEnumerator LerpMovementRoutine(Vector3 direction)
    {
        IsMoving = true;

        float elapsedTime = 0f;

        _originPosition = transform.position;
        _targetPosition = _originPosition + direction;

        if (Physics.Raycast(_originPosition, direction, 2f, ContactMask))
        {
            IsMoving = false;
            yield break;
        }

        while (elapsedTime < TimeToMove)
        {
            transform.position = Vector3.Lerp(_originPosition, _targetPosition,
                                              elapsedTime / TimeToMove);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = _targetPosition;

        IsMoving = false;
    }

    #endregion

    #region MOVE TOWARDS METHODS

    private void GridMovementBySpeed(Vector2 inputAxis)
    {
        RotatePlayer(inputAxis);

        // Replace Vectors with Vector3
        if (IsMoving)
        {
            transform.position =
                Vector3.MoveTowards(transform.position, _targetPosition, MoveSpeed * Time.deltaTime);
        }

        if (!(Vector3.Distance(transform.position, _targetPosition) <= 0.05f)) return;

        transform.position = _targetPosition;
        IsMoving = false;
        UpdateTargetPosition(inputAxis, Vector2.left, Vector3.left);
        UpdateTargetPosition(inputAxis, Vector2.right, Vector3.right);
        UpdateTargetPosition(inputAxis, Vector2.up, Vector3.forward);
        UpdateTargetPosition(inputAxis, Vector2.down, Vector3.back);
    }

    private void UpdateTargetPosition(Vector2 inputAxis, Vector2 inputDirection, Vector3 movementDirection)
    {
        if (inputAxis != inputDirection) return;
        if (!Physics.Raycast(transform.position, movementDirection * Offset, 2f, ContactMask))
        {
            _targetPosition += movementDirection * Offset;
        }

        IsMoving = true;
    }

    private void RotatePlayer(Vector2 inputAxis)
    {
        if (inputAxis == Vector2.zero) return;
        _targetAngle = Mathf.Atan2(inputAxis.x, inputAxis.y) * Mathf.Rad2Deg + _camera.transform.eulerAngles.y;
        transform.rotation = Quaternion.Lerp(transform.rotation,
                                             Quaternion.Euler(0f, _targetAngle, 0f),
                                             Time.deltaTime * RotationSpeed);
    }

    #endregion

    #region HELPER METHODS

    public void DrawRayToTargetPosition()
    {
        if (!DrawRayLines) return;

        Debug.DrawRay(transform.position, Vector3.left    * Offset, Color.yellow);
        Debug.DrawRay(transform.position, Vector3.right   * Offset, Color.yellow);
        Debug.DrawRay(transform.position, Vector3.forward * Offset, Color.yellow);
        Debug.DrawRay(transform.position, Vector3.back    * Offset, Color.yellow);
    }

    #endregion
}