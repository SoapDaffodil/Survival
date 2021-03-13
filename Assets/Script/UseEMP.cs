using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseEMP : MonoBehaviour
{
    public EMP emp;


    private void Start()
    {
        emp.SetUp();
    }

    private void OnTriggerStay(Collider other)
    {
        if(Input.GetKeyDown(KeyCode.E) && other.CompareTag("EMPZone"))
        {
            emp.CheckEMP();
            emp.Install();
        }
        else
        {
            return;
        }
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.E))
        {
            Debug.Log("설치 시작");
            emp.CheckEMP();
            emp.Install();
            //emp.OnDetect();
        }
    }
}
