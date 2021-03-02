using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//좌우회전
public class VerticalMouseRotate : MonoBehaviour
{
    public float rotateSpeed = 5f; //마우스 속도
    public bool moving = false;
    public float rotationX = 0;
    

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

        rotationX += Input.GetAxis("Mouse Y") * -rotateSpeed; // 마우스 Y 위치 * 회전 스피드
        rotationX = Mathf.Clamp(rotationX, -85, 85);
        rot.x = rotationX;
        Quaternion q = Quaternion.Euler(rot); // Quaternion으로 변환
        q.z = 0;
        transform.localRotation = Quaternion.Slerp(transform.localRotation, q, 2f); // 자연스럽게 회전

        moving = false;
    }
}
