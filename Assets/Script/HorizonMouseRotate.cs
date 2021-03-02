using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//상하회전
public class HorizonMouseRotate : MonoBehaviour
{
    public float rotateSpeed = 5f; //마우스 속도
    public bool moving = false;
    public float rotationY = 0;
    

    // Start is called before the first frame update
    void Start()
    {
        rotateSpeed = 5f;
        moving = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!moving)
        {
            moving = true;
            Rotate();
        }
    }

    private void Rotate()
    {
        Vector3 rot = transform.localRotation.eulerAngles; // 현재 카메라의 각도를 Vector3로 반환

        rotationY += Input.GetAxis("Mouse X") * rotateSpeed; // 마우스 X 위치 * 회전 스피드
        //rotationY = Mathf.Clamp(rotationY, -90, 90);
        rot.y = rotationY;
        Quaternion q = Quaternion.Euler(rot); // Quaternion으로 변환
        q.z = 0;
        transform.localRotation = Quaternion.Slerp(transform.localRotation, q, 2f); // 자연스럽게 회전

        moving = false;
    }
}
