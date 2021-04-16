﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmpTrap : MonoBehaviour
{
    public int id;
    public bool isDetectedMode = false;

    public void Initialize(int _id)
    {
        id = _id;
    }

    private void OnTriggerStay(Collider other)
    {
        if (isDetectedMode == true)
        {
            Debug.Log("감지용 설치 완료!");
            gameObject.GetComponent<SphereCollider>().radius = 5f;
            gameObject.GetComponent<SphereCollider>().isTrigger = true;
        }

        if (other.CompareTag("Monster"))
        {
            Debug.Log("몬스터가 감지 되었습니다");
        }
    }

    

    
}