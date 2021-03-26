using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmpForTrap : EMP
{
    public bool isDetectedMode = false;


    private void OnTriggerStay(Collider other)
    {
        if (isDetectedMode == true)
        {
            Debug.Log("감지용 설치 완료!");
            gameObject.GetComponent<SphereCollider>().radius = 5f;
            gameObject.GetComponent<SphereCollider>().isTrigger = true;

            if (other.CompareTag("Player"))
            {
                Debug.Log("Player가 접근하고 있습니다!!");
            }
        }
    }
}
