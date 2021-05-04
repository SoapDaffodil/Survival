using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Drone : MonoBehaviour
{
    private Light flash;
    private Camera droneCam;
    public bool isDroneMoving = false;

    // Start is called before the first frame update
    void Start()
    {       
        flash = gameObject.GetComponent<Light>();
        droneCam = GameObject.Find("DroneCam").GetComponent<Camera>();

        flash.type = LightType.Point;
        flash.range = 0f;
        droneCam.enabled = false;
    }
   
    public void Moving()
    {
        gameObject.transform.parent.GetComponent<Camera>().enabled = false;
        gameObject.transform.parent.GetComponent<PlayerController>().enabled = false;
        droneCam.enabled = true;       
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
            droneCam.enabled = false;
            isDroneMoving = false;
            gameObject.transform.parent.GetComponent<Camera>().enabled = true;
            gameObject.transform.parent.GetComponent<PlayerController>().enabled = true;            

            ClientSend.DroneStop(this.gameObject);
        }
    }


    public void OnFlash()
    {
        flash.range = 8f;
    }
}
