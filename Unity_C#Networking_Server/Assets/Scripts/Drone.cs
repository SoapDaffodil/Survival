using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone : MonoBehaviour
{
    public CharacterController controller;  //드론의 컨트롤러
    private bool[] inputs = new bool[5];    //드론 움직임 조작 inputs
    public float moveSpeed = 5f;            //드론 움직임속도
    public bool isDroneMoving = false;     //드론 조종 가능

    /// <summary>Processes player input and moves the player.</summary>
    public void FixedUpdate()
    {
        if(isDroneMoving)
        {
            Vector2 _inputDirection = Vector2.zero;
            if (inputs[0])
            {
                _inputDirection.y += 1;
            }
            if (inputs[1])
            {
                _inputDirection.y -= 1;
            }
            if (inputs[2])
            {
                _inputDirection.x -= 1;
            }
            if (inputs[3])
            {
                _inputDirection.x += 1;
            }
            if (controller.enabled)
            {
                Move(_inputDirection);
            }
        }
        
    }


    /// <summary>드론의 input값 받기.</summary>
    /// <param name="_inputs">The new key inputs.</param>
    /// <param name="_rotation">The new rotation.</param>
    public void SetInput(bool[] _inputs, Quaternion _rotation)
    {
        inputs = _inputs;
        transform.rotation = _rotation;
    }

    /// <summary>받은 데이터를 통해 드론의 움직임을 계산</summary>
    /// <param name="_inputDirection"></param>
    private void Move(Vector2 _inputDirection)
    {
        Vector3 _moveDirection = transform.right * _inputDirection.x + transform.forward * _inputDirection.y;
        _moveDirection *= moveSpeed;

        //_moveDirection.y = moveSpeed;
        controller.Move(_moveDirection);

        ServerSend.DronePosition(this);
        ServerSend.DroneRotation(this);
    }
}
