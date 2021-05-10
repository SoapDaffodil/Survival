using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseEMP : MonoBehaviour
{  /* 삭제
    public List<EMP> empList;

    public EMP emp;
    public EMP installation;
    private EMP installingInstance;

    private void OnTriggerStay(Collider other)
    {
        //Debug.Log("collider other : " + other.gameObject);

        if(emp != null)
        {
            
            empList.Add(this.GetComponent<EMP>());
            empList.Count > 0
            EMP a=    empList[0];
            empList.Remove(a);
            */

    /*
    if (other.CompareTag("EMPZone"))
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            emp.CheckEMP();
        }
        else if (Input.GetKey(KeyCode.E))
        {                    
            //((EmpForInstalling)emp).Install();
        }
        else if (Input.GetKeyUp(KeyCode.E))
        {
            //((EmpForInstalling)emp).Install();
        }
        if emp.finished == true && installingInstance == null)
        {
            installingInstance = Instantiate(installation, transform);
            installingInstance.transform.SetParent(null);
            emp.count++;
            Debug.Log("EMP가 설치 완료 되었습니다!");
            Debug.Log("설치한 EMP 갯수 : " + emp.count);

            emp.finished = false;
        }
    }

    else if(other == null)
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (empTrapInstance == null)
            {
                emp.CheckEMP();
            }

        }
        else if(Input.GetKeyDown(KeyCode.E))
        {
            //((EmpForTrap)emp).Install();
        }
        else if (Input.GetKeyUp(KeyCode.E))
        {
           // ((EmpForTrap)emp).Install();
        }

        if (((EmpForTrap)emp).finished == true && empTrapInstance == null)
        {
            Debug.Log("trap 생성!");
           // empTrapInstance = Instantiate(trap, transform);
           // empTrapInstance.transform.SetParent(null);

            emp.finished = false;
        }
    }


}       
}

*/

}
