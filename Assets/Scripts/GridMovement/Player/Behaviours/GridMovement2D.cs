using System.Collections;
using UnityEngine;

public class GridMovement2D : MonoBehaviour
{
    #region VARIABLES

    [Header("Debug")]
    public bool DrawRayLines = false;

    [Header("Movement Type")]
    public GridMovementType GridMovementType = GridMovementType.BySpeed;

    [Header("Movement")]
    public float MoveSpeed = 5f;
    public float TimeToMove = 0.2f;

    [Header("Contact Mask")]
    public LayerMask ContactMask;

    [Header("Orientation")]
    public bool IsMoving;

    [Header("Settings")]
    [Tooltip("Offset the Target Position by this Offset Value.")]
    public float Offset = 1f;

    private Camera _camera;
    private Vector2 _originPosition;
    private Vector2 _targetPosition;

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
        if (IsMoving) return;

        LerpTowardsTarget(inputAxis, Vector2.left);
        LerpTowardsTarget(inputAxis, Vector2.right);
        LerpTowardsTarget(inputAxis, Vector2.up);
        LerpTowardsTarget(inputAxis, Vector2.down);
    }

    private void LerpTowardsTarget(Vector2 inputAxis, Vector2 inputDirection)
    {
        if (inputAxis != inputDirection) return;
        StartCoroutine(LerpMovementRoutine(inputDirection * Offset));
    }

    private IEnumerator LerpMovementRoutine(Vector2 direction)
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
        // Replace Vectors with Vector3
        if (IsMoving)
        {
            transform.position =
                Vector3.MoveTowards(transform.position, _targetPosition, MoveSpeed * Time.deltaTime);
        }

        if (!(Vector3.Distance(transform.position, _targetPosition) <= 0.05f)) return;

        transform.position = _targetPosition;
        IsMoving = false;
        UpdateTargetPosition(inputAxis, Vector2.left);
        UpdateTargetPosition(inputAxis, Vector2.right);
        UpdateTargetPosition(inputAxis, Vector2.up);
        UpdateTargetPosition(inputAxis, Vector2.down);
    }

    private void UpdateTargetPosition(Vector2 inputAxis, Vector2 inputDirection)
    {
        if (inputAxis != inputDirection) return;
        if (!Physics.Raycast(transform.position, inputDirection * Offset, 2f, ContactMask))
        {
            _targetPosition += inputDirection * Offset;
        }

        IsMoving = true;
    }

    #endregion

    #region HELPER METHODS

    public void DrawRayToTargetPosition()
    {
        if (!DrawRayLines) return;

        Debug.DrawRay(transform.position, Vector3.left  * Offset, Color.yellow);
        Debug.DrawRay(transform.position, Vector3.right * Offset, Color.yellow);
        Debug.DrawRay(transform.position, Vector3.up    * Offset, Color.yellow);
        Debug.DrawRay(transform.position, Vector3.down  * Offset, Color.yellow);
    }

    #endregion
}