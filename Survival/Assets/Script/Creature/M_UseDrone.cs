using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class M_UseDrone : MonoBehaviour
{

    /* 삭제
    public Drone drone;
    private Drone presentItem;
    private Camera droneCam;
    private Camera creatureCam;
    private bool isFlying = false;

    private void Start()
    {
        droneCam = GameObject.Find("DroneCam").GetComponent<Camera>();
        creatureCam = GameObject.Find("Creature Camera").GetComponent<Camera>();
        creatureCam.enabled = true;
        droneCam.enabled = false;
    }


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
                //drone.ChargingGauge();
            }
            if(presentItem == drone && Input.GetMouseButtonUp(0))
            {
               // drone.ChargingGauge();
            }

            
            if(drone.isGaugeFull == true)
            {
                ControllDrone();
            }
            
            if(isFlying == true && Input.GetKeyDown(KeyCode.E))
            {
                TurnOnLight();
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

    void ControllDrone()
    {
        if (Input.GetMouseButtonDown(0) && !isFlying)
        {
            creatureCam.enabled = false;
            droneCam.enabled = true;
            drone.transform.SetParent(null);
            drone.transform.position += new Vector3(0f, 5f, 0f);
            
            //drone.droneMoving.enabled = true;
            gameObject.GetComponent<Move>().enabled = false;

            isFlying = true;
        }

        else if(isFlying && Input.GetMouseButtonDown(0))
        {
            creatureCam.enabled = true;
            droneCam.enabled = false;
            drone.transform.SetParent(transform);
            drone.transform.position -= new Vector3(0f, 5f, 0f);

            //drone.droneMoving.enabled = false;
            gameObject.GetComponent<Move>().enabled = true;

            TurnOffLight();
            isFlying = false;
        }
    }

    void TurnOnLight()
    {
        drone.OnFlash();
    }

    void TurnOffLight()
    {
        //drone.OffFlash();
    }

  */
}
