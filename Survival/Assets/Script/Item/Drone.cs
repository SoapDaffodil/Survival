using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Drone : MonoBehaviour
{
   // private Light flash;
    private Camera droneCam;
    public bool isDroneMoving = false;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {       
       // flash = gameObject.GetComponent<Light>();
        droneCam = GameObject.Find("DroneCam").GetComponent<Camera>();

       // flash.type = LightType.Point;
       // flash.range = 0f;
        droneCam.gameObject.SetActive(false);
        player = GameObject.FindGameObjectWithTag("Player").gameObject;
    }
   
    public void Moving()
    {
        player.transform.GetChild(0).GetComponent<Camera>().gameObject.SetActive(false);
        player.GetComponent<PlayerController>().enabled = false;
        gameObject.transform.position += new Vector3 (0f, 5f, 0f);
        droneCam.gameObject.SetActive(true);       
        isDroneMoving = true;
    }
  
    private void FixedUpdate()
    {
        if (isDroneMoving)
        {
            bool[] _inputs = new bool[]
            {
            Input.GetKey(KeyCode.W),
            Input.GetKey(KeyCode.A),
            Input.GetKey(KeyCode.S),
            Input.GetKey(KeyCode.D),
            Input.GetKey(KeyCode.Space),
            };

            ClientSend.DroneMovement(_inputs, this.gameObject);
        }        
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.R) && isDroneMoving)
        {
            droneCam.gameObject.SetActive(false);
            isDroneMoving = false;
            player.transform.GetChild(0).GetComponent<Camera>().gameObject.SetActive(true);
            player.GetComponent<PlayerController>().enabled = true;

            gameObject.transform.SetParent(null);

            Debug.Log("드론 멈춤");
            ClientSend.DroneStop(this.gameObject);
        }

        if(Input.GetKeyDown(KeyCode.C) && isDroneMoving)
        {
            // 드론 빛 이동
        }
    }


    public void OnFlash()
    {
       // flash.range = 8f;
    }
}
