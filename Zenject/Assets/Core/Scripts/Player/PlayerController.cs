using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;

    public float _movementSpeed = 5f;
    public float _rotationSpeed = 5f;
    public float _maxSpeed = 15f;
    public ForceMode movementForceMode;

    private Vector2 posJoystickInput;
    private Vector2 rotJoystickInput;

    private IInput inputService;

    [Inject]
    private void Construct(IInput inputService)
    {
        this.inputService = inputService;
        inputService.TargetMovement += TargetMovementReceived;
        inputService.TargetRotation += TargetRotationReceived;

        rb = GetComponent<Rigidbody>();
    }

    private void OnDestroy()
    {
        inputService.TargetMovement -= TargetMovementReceived;
        inputService.TargetRotation -= TargetRotationReceived;
    }

    private void TargetMovementReceived(Vector2 targetMovement) => posJoystickInput = targetMovement;
    private void TargetRotationReceived(Vector2 targetRotation)
    {
        rotJoystickInput = targetRotation;
    }

    private void FixedUpdate()
    {
        ManageMovement();
        ManageRotation();
        ManageMaxForce();
    }

    void ManageMovement()
    {
        if (posJoystickInput == Vector2.zero) return;
        Vector3 translation = new Vector3(posJoystickInput.x, 0, posJoystickInput.y).normalized * _movementSpeed * Time.deltaTime;
        rb.AddForce(translation, movementForceMode);
    }

    void ManageRotation()
    {
        Vector2 value;

        if (posJoystickInput != Vector2.zero || rotJoystickInput != Vector2.zero)
        {
            if (rotJoystickInput != Vector2.zero) value = rotJoystickInput;
            else value = posJoystickInput;

            float yAxis = Mathf.Atan2(value.x, value.y) * Mathf.Rad2Deg;
            Quaternion targetRot = Quaternion.Euler(transform.rotation.x, yAxis, transform.rotation.z);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, _rotationSpeed * Time.deltaTime);

            //TryToShoot(value, targetRot);
        }
    }

    void ManageMaxForce()
    {
        if(posJoystickInput != Vector2.zero)
        {
            if (rb.velocity.magnitude > _maxSpeed)
                rb.velocity = rb.velocity.normalized * _maxSpeed;
        }
        else
        {
            rb.velocity = Vector3.zero;
        }
        rb.angularVelocity = Vector3.zero;
    }
}
