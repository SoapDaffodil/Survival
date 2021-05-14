using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseLight : MonoBehaviour
{
    /* 삭제
    public  M_Light light;

    // Update is called once per frame
    void Update()
    {
        
        if(light != null)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                ItemOnHand();
            }
            else if (Input.GetKey(KeyCode.E))
            {
                //light.CheckLight();
                //light.Install();
            }
            else if (Input.GetKeyUp(KeyCode.E))
            {
                //light.Install();
            }
            
            if(light.finished == true)
            {
                //light.finished = false;               
            }
        }
    }

    void ItemOnHand()
    {
        if (light == null)
        {
            Debug.Log("소지하고있는 아이템이 없습니다");
        }
        else
        {
            light.transform.position = gameObject.transform.position + new Vector3(-0.7f, 0f, 0f);
            light.gameObject.SetActive(true);
            light.transform.SetParent(transform);
        }
    }
        */
}
