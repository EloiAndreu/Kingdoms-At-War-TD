using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float panSpeed = 0.5f; // Velocitat de moviment de la càmera
    public float zoomSpeed = 0.1f; // Velocitat del zoom
    public float minZoom = 45f; // Mínim zoom
    public float maxZoom = 70f; // Màxim zoom

    private Vector3 touchStart; // Punt inicial de contacte (també per al ratolí)
    float maxCameraHeight, maxCameraWidth;

    void Start()
    {
        maxCameraHeight = Camera.main.orthographicSize * 2;
        maxCameraWidth = maxCameraHeight * Camera.main.aspect;
    }

    void Update()
    {
        // Moviment amb dit (tacte)
        if (Input.touchCount == 1)
        {
            HandleTouchMovement();
        }

        // Zoom amb dos dits (tacte)
        if (Input.touchCount == 2)
        {
            HandleTouchZoom();
        }

        // Moviment amb el ratolí (clic esquerre)
        if (Input.GetMouseButton(0)) // Botó esquerre del ratolí
        {
            HandleMouseMovement();
        }

        // Zoom amb la rodeta del ratolí
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0f)
        {
            Zoom(scroll * zoomSpeed * 100f); // Multiplicació per ajustar la sensibilitat del zoom
        }
    }

    // Moviment amb dit
    void HandleTouchMovement()
    {
        if (Camera.main.orthographicSize == maxZoom)
        {
            return; // No permetem moure la càmera si el zoom és màxim
        }

        Touch touch = Input.GetTouch(0);

        if (touch.phase == TouchPhase.Began)
        {
            touchStart = Camera.main.ScreenToWorldPoint(touch.position);
        }
        else if (touch.phase == TouchPhase.Moved)
        {
            Vector3 direction = touchStart - Camera.main.ScreenToWorldPoint(touch.position);
            Vector3 newPosition = Camera.main.transform.position + direction * panSpeed;
            ApplyMovementLimits(newPosition);
        }
    }

    // Moviment amb el ratolí
    void HandleMouseMovement()
    {
        if (Camera.main.orthographicSize == maxZoom)
        {
            return; // No permetem moure la càmera si el zoom és màxim
        }

        if (Input.GetMouseButtonDown(0)) // Quan es fa clic
        {
            touchStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(0)) // Quan es manté el clic
        {
            Vector3 direction = touchStart - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 newPosition = Camera.main.transform.position + direction * panSpeed;
            ApplyMovementLimits(newPosition);
        }
    }

    // Zoom amb dos dits
    void HandleTouchZoom()
    {
        Touch touchZero = Input.GetTouch(0);
        Touch touchOne = Input.GetTouch(1);

        Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
        Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

        float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
        float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

        float difference = prevMagnitude - currentMagnitude;

        Zoom(difference * zoomSpeed);
    }

    // Zoom (tant per tacte com per ratolí)
    void Zoom(float increment)
    {
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize + increment, minZoom, maxZoom);
        ApplyMovementLimits(transform.position);
    }

    // Aplica els límits de moviment de la càmera
    void ApplyMovementLimits(Vector3 newPosition)
    {
        float cameraHeight = Camera.main.orthographicSize * 2;
        float cameraWidth = cameraHeight * Camera.main.aspect;

        Debug.Log("MAX Height: " + maxCameraHeight + ", MAX Width: " + maxCameraWidth);
        Debug.Log("Height: " + cameraHeight + ", Width: " + cameraWidth);

        float maxX = (maxCameraWidth / 2) - (cameraWidth / 2);
        float minX = -maxX;
        float maxY = (maxCameraHeight / 2) - (cameraHeight / 2);
        float minY = -maxY;

        Debug.Log("MINX: " + minX + ", MAXX: " + maxX);
        Debug.Log("MINY: " + minY + ", MAXY: " + maxY);

        newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
        newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);
        // Si no necessitem moviment en l'eix Z, fixem-lo a la posició actual
        //newPosition.z = Camera.main.transform.localPosition.z;

        // Aplicar la nova posició a la càmera
        Camera.main.transform.localPosition = newPosition;
    }
}
