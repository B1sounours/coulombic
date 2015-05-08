using UnityEngine;
using System.Collections;

public class ShipController : MonoBehaviour
{

    private class MovementData
    {
        public float acceleration = 3f;
        public float speed = 0;
        public float maxSpeed = 30;
        public float decayRate = 0.95f;
    }
    MovementData forward;
    MovementData sideways;

    void Update()
    {
        updateMovementData(getForward(), KeyCode.W, KeyCode.UpArrow, KeyCode.S, KeyCode.DownArrow, KeyCode.Space);
        updateMovementData(getSideways(), KeyCode.A, KeyCode.LeftArrow, KeyCode.D, KeyCode.RightArrow, KeyCode.Space);

        Vector3 sidewaysVector = Vector3.Cross(Camera.main.transform.forward, Vector3.up).normalized;
        Vector3 velocity = Camera.main.transform.forward * getForward().speed + sidewaysVector * getSideways().speed;
        velocity *= Time.deltaTime;
        transform.position = transform.position + velocity;
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

    private void updateMovementData(MovementData movementData, KeyCode increase1, KeyCode increase2, KeyCode decrease1, KeyCode decrease2, KeyCode halt)
    {
        if (Input.GetKey(increase1) || Input.GetKey(increase2))
            movementData.speed += movementData.acceleration * Time.deltaTime;
        else if (Input.GetKey(decrease1) || Input.GetKey(decrease2))
            movementData.speed -= movementData.acceleration * Time.deltaTime;
        else if (Input.GetKey(halt))
            movementData.speed = 0;
        else
        {
            movementData.speed *= movementData.decayRate;
            if (Mathf.Abs(movementData.speed) < 0.01)
                movementData.speed = 0;
        }

        movementData.speed = Mathf.Clamp(movementData.speed, -movementData.maxSpeed, movementData.maxSpeed);
    }
}
