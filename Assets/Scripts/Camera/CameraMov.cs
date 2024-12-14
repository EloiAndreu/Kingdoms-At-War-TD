using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMov : MonoBehaviour
{
    public float panSpeed = 0.5f;
    public float zoomSpeed = 0.1f;
    public float minZoom = 45f;
    public float maxZoom = 70f;

    private Vector3 touchStart;
    float maxCameraHeight, maxCameraWidth;

    public bool potArrossegar = true;

    GameObject towerSelection;

    void Start()
    {
        towerSelection = GameObject.FindGameObjectWithTag("UISelectTower");
        maxCameraHeight = Camera.main.orthographicSize * 2;
        maxCameraWidth = maxCameraHeight * Camera.main.aspect;
    }

    void Update(){
        if(potArrossegar){
            //CONTROLS RATOLI
            if (Input.GetMouseButton(0)){
                HandleMouseMovement();
            }

            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll != 0f){
                if(towerSelection != null) towerSelection.GetComponent<UIManager>().AmagarObjectes();
                Zoom(scroll * zoomSpeed * 100f);
            }

            //CONTROLS DITS
            if (Input.touchCount == 1){
                HandleTouchMovement();
            }

            if (Input.touchCount == 2){
                HandleTouchZoom();
            }
        }
    }

    void HandleMouseMovement()
    {
        if (Input.GetMouseButtonDown(0)){
            touchStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(0)){
            Vector3 direction = touchStart - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 newPosition = Camera.main.transform.localPosition + direction * panSpeed;
            ApplyMovementLimits(newPosition);
        }
    }

    void HandleTouchMovement(){

        Touch touch = Input.GetTouch(0);

        if (touch.phase == TouchPhase.Began){
            touchStart = Camera.main.ScreenToWorldPoint(touch.position);
        }
        else if (touch.phase == TouchPhase.Moved)
        {
            Vector3 direction = touchStart - Camera.main.ScreenToWorldPoint(touch.position);
            Vector3 newPosition = Camera.main.transform.localPosition + direction * panSpeed;
            ApplyMovementLimits(newPosition);
        }
    }

    void Zoom(float increment){
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - increment, minZoom, maxZoom);
        if(increment<0) ApplyMovementLimits(transform.localPosition);
    }

    void HandleTouchZoom(){
        if(towerSelection != null) towerSelection.GetComponent<UIManager>().AmagarObjectes();

        Touch touchZero = Input.GetTouch(0);
        Touch touchOne = Input.GetTouch(1);

        Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
        Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

        float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
        float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

        float difference = prevMagnitude - currentMagnitude;

        Zoom(-(difference * zoomSpeed));
    }

    void ApplyMovementLimits(Vector3 newPosition){

        float cameraHeight = Camera.main.orthographicSize * 2f;
        float cameraWidth = cameraHeight * Camera.main.aspect;

        //Debug.Log("MAX Height: " + maxCameraHeight + ", MAX Width: " + maxCameraWidth);
        //Debug.Log("Height: " + cameraHeight + ", Width: " + cameraWidth);

        float maxX = (maxCameraWidth / 2f) - (cameraWidth / 2f);
        float minX = -maxX;
        float maxY = (maxCameraHeight / 2f) - (cameraHeight / 2f);
        float minY = -maxY;

        //Debug.Log("MINX: " + minX + ", MAXX: " + maxX);
        //Debug.Log("MINY: " + minY + ", MAXY: " + maxY);

        newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
        newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);
        newPosition.z = Mathf.Clamp(newPosition.z, 0f, 0f);

        Camera.main.transform.localPosition = newPosition;
    }
}
