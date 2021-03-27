using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class M_UseDrone : MonoBehaviour
{
    public M_Drone drone;
    private M_Drone presentItem;


    // Update is called once per frame
    void Update()
    {
        if(drone != null)
        {
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                ItemOnHand();
                presentItem = drone;
            }

            if(presentItem == drone && Input.GetMouseButton(0))
            {
                drone.ChargingGauge();
            }
            if(presentItem == drone && Input.GetMouseButtonUp(0))
            {
                drone.ChargingGauge();
            }
        }
    }



    void ItemOnHand()
    {
        if (drone == null)
        {
            Debug.Log("소지하고있는 아이템이 없습니다");
        }
        else
        {
            drone.transform.position = gameObject.transform.position + new Vector3(-0.7f, 0f, 0f);
            drone.gameObject.SetActive(true);
            drone.transform.SetParent(transform);

        }
    }
}
