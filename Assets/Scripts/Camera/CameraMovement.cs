using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float speed = 10f;
    public Vector2 xLimits = new Vector2(-10f, 10f);
    public Vector2 zLimits = new Vector2(-10f, 10f);
    Vector3 initalPos = new Vector3(0f, 55f, -75f);

    void Start(){
        transform.position = initalPos;
    }

    void Update()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 newPosition = transform.position + new Vector3(moveX, 0, moveZ) * speed * Time.deltaTime;

        newPosition.x = Mathf.Clamp(newPosition.x, xLimits.x, xLimits.y);
        newPosition.z = Mathf.Clamp(newPosition.z, zLimits.x, zLimits.y);

        transform.position = newPosition;
    }
}
