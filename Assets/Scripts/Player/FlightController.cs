using UnityEngine;
using System.Collections;

public class FlightController : MonoBehaviour
{

    private class MovementData
    {
        public float acceleration = 5f;
        public float speed = 0;
        public float maxSpeed = 30;
        public float decayRate = 0.95f;
    }
    MovementData forward, sideways, up;

    private float sprintMultiplier = 3f;

    void Update()
    {
        updateMovementData(getForward(), KeyCode.W, KeyCode.UpArrow, KeyCode.S, KeyCode.DownArrow, KeyCode.Space);
        updateMovementData(getSideways(), KeyCode.A, KeyCode.LeftArrow, KeyCode.D, KeyCode.RightArrow, KeyCode.Space);
        updateMovementData(getUp(), KeyCode.Q, KeyCode.Plus, KeyCode.E, KeyCode.Minus, KeyCode.Space);

        Vector3 sidewaysVector = Vector3.Cross(Camera.main.transform.forward, Vector3.up).normalized;
        Vector3 velocity = Camera.main.transform.forward * getForward().speed + sidewaysVector * getSideways().speed + Camera.main.transform.up * getUp().speed;
        velocity *= Time.deltaTime;
        transform.position = transform.position + velocity;
    }

    private bool IsSprinting()
    {
        return Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
    }

    private MovementData getForward()
    {
        if (forward == null)
            forward = new MovementData();
        return forward;
    }

    private MovementData getSideways()
    {
        if (sideways == null)
        {
            sideways = new MovementData();
            sideways.acceleration = forward.acceleration * 2;
            sideways.maxSpeed = forward.maxSpeed / 2;
        }
        return sideways;
    }

    private MovementData getUp()
    {
        if (up == null)
        {
            up = new MovementData();
            up.acceleration = forward.acceleration * 2;
            up.maxSpeed = forward.maxSpeed / 2;
        }
        return up;
    }

    private void updateMovementData(MovementData movementData, KeyCode increase1, KeyCode increase2, KeyCode decrease1, KeyCode decrease2, KeyCode halt)
    {
        float deltaV = movementData.acceleration * Time.deltaTime;
        if (IsSprinting())
            deltaV *= sprintMultiplier;

        if (Input.GetKey(increase1) || Input.GetKey(increase2))
            movementData.speed += deltaV;
        else if (Input.GetKey(decrease1) || Input.GetKey(decrease2))
            movementData.speed -= deltaV;
        else if (Input.GetKey(halt))
            movementData.speed = 0;
        else
        {
            movementData.speed *= movementData.decayRate;
            if (Mathf.Abs(movementData.speed) < 0.01)
                movementData.speed = 0;
        }

        float maxSpeed = movementData.maxSpeed;
        if (IsSprinting())
            maxSpeed *= sprintMultiplier;
        movementData.speed = Mathf.Clamp(movementData.speed, -maxSpeed, maxSpeed);
    }
}
