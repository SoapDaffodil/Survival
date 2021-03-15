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
        Debug.Log("other = " + other.gameObject.name);

        if (Input.GetKey(KeyCode.E) && other.CompareTag("EMPZone"))
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
            emp.CheckEMP();
            emp.Install();
        }

        if(emp.isDetectedMode == true)
        {
            emp.OnDetect();
        }
        
    }

    public void CompliteEMPInstall()
    {
        if(emp.count >= 2)
        {
            Debug.Log("EMP 설치 완료! 괴물에게 공격이 가능합니다!");
            //ToDo : 괴물에게 공격하면 괴물이 데미지를 입을 수 있는 상태가 됨.
        }
    }
}
