using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseEMP : MonoBehaviour
{
    public EMP emp;
    public EmpForTrap trap;
    public EMP installation;
    private EmpForTrap empTrapInstance;
    private EMP installingInstance;

    private void OnTriggerStay(Collider other)
    {
        Debug.Log("collider other : " + other.gameObject);

        if(emp != null)
        {
            if (other.CompareTag("EMPZone"))
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    emp.CheckEMP();
                }
                else if (Input.GetKey(KeyCode.E))
                {                    
                    emp.Install();
                }
                else if (Input.GetKeyUp(KeyCode.E))
                {
                    emp.Install();
                }
                if (emp.finished == true && installingInstance == null)
                {
                    installingInstance = Instantiate(installation, transform);
                    installingInstance.transform.SetParent(null);
                    emp.count++;
                    Debug.Log("EMP가 설치 완료 되었습니다!");
                    Debug.Log("설치한 EMP 갯수 : " + emp.count);

                    emp.finished = false;
                }
            }

            else if(other.tag != "EMPZone" || other == null)
            {
                if (Input.GetKey(KeyCode.E))
                {
                    if (empTrapInstance == null)
                    {
                        emp.CheckEMP();
                    }
                    emp.Install();
                }
                else if (Input.GetKeyUp(KeyCode.E))
                {
                    emp.Install();
                }

                if (emp.finished == true)
                {
                    Debug.Log("trap 생성!");
                    empTrapInstance = Instantiate(trap, transform);
                    empTrapInstance.transform.SetParent(null);

                    empTrapInstance.isDetectedMode = true;
                    emp.finished = false;
                }
            }

           
        }       
    }



}
