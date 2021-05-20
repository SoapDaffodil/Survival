using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject verticalRotationObj;
    public GameObject horizontalRotationObj;
    public float sensitivity = 100f;
    public float clampAngle = 85f;

    private float verticalRotation;
    private float horizontalRotation;

    private void Start()
    {
        verticalRotation = transform.localEulerAngles.x;
        horizontalRotation = horizontalRotationObj.transform.eulerAngles.y;
    }

    private void Update()
    {
        //esc를 누르면 마우스커서 on
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleCursorMode();
        }

        if (Cursor.lockState == CursorLockMode.Locked)
        {
            Look();
        }
        Look();
        //Debug.DrawRay(transform.position, transform.forward * 2, Color.red);
    }

    private void Look()
    {
        float _mouseVertical = -Input.GetAxis("Mouse Y")*10;
        float _mouseHorizontal = Input.GetAxis("Mouse X")*10;

        verticalRotation += _mouseVertical * sensitivity * Time.deltaTime;
        horizontalRotation += _mouseHorizontal * sensitivity * Time.deltaTime;

        verticalRotation = Mathf.Clamp(verticalRotation, -clampAngle, clampAngle);

        if(verticalRotationObj == horizontalRotationObj)
        {
            verticalRotationObj.transform.localRotation = Quaternion.Euler(verticalRotation, horizontalRotation, 0f);
        }
        else{
            verticalRotationObj.transform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
            horizontalRotationObj.transform.localRotation = Quaternion.Euler(0f, horizontalRotation, 0f);
        }
    }

    /// <summary>esc를 누르면 마우스커서 on</summary>
    private void ToggleCursorMode()
    {
        Cursor.visible = !Cursor.visible;

        if (Cursor.lockState == CursorLockMode.None)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
