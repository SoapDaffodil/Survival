using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    float moveSpeed = 15;

    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = 15 * transform.localScale.x;
    }

    void Update()
    {
        //키보드 입력 
        float inputX = Input.GetAxis("Horizontal") / 10;
        float inputZ = Input.GetAxis("Vertical") / 10;

        //키보드 입력 받았을 때만 물체 위치 변경
        if (inputX != 0 || inputZ != 0)
        {
            transform.Translate(new Vector3(inputX, 0f, inputZ) * moveSpeed * Time.deltaTime, Space.Self);
        }


    }
}
