using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Crash : MonoBehaviour
{
    private float speed = 2f;
    private float lastTime;
    private float coolTime = 5f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (Time.time >= lastTime + coolTime)
            {
                transform.position += transform.forward * speed;
                lastTime = Time.time;
                Debug.Log("돌진!");
            }
            else
            {
                Debug.Log("쿨타임");
            }
            
        }
    }
}
