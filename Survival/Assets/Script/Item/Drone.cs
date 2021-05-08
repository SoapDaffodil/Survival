using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Drone : MonoBehaviour
{
   // private Light flash;
    private Camera droneCam;
    public bool isDroneMoving = false;

    // Start is called before the first frame update
    void Start()
    {       
       // flash = gameObject.GetComponent<Light>();
        droneCam = GameObject.Find("DroneCam").GetComponent<Camera>();

       // flash.type = LightType.Point;
       // flash.range = 0f;
        droneCam.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && isDroneMoving)
        {
            isDroneMoving = false;
            droneCam.gameObject.SetActive(false);
            this.transform.parent.GetChild(0).GetComponent<Camera>().gameObject.SetActive(true);
            this.transform.parent.GetComponent<PlayerController>().enabled = true;

            transform.SetParent(null, true);

            Debug.Log("드론 멈춤");
            ClientSend.DroneStop(this.gameObject);
        }

        // 드론 빛 이동
        if (Input.GetKeyDown(KeyCode.C) && isDroneMoving)
        {
            if (this.transform.parent != null)
            {
                isDroneMoving = false;
                PlayerController _player = this.transform.parent.GetComponent<PlayerController>();
                transform.SetParent(null, true);
                droneCam.gameObject.SetActive(false);
                _player.transform.position = this.transform.position;
                _player.transform.rotation = this.transform.rotation;
                _player.enabled = true;
                _player.camTransform.gameObject.SetActive(true);
                Destroy(this, 1f);
            }
        }
    }
    public void Moving()
    {
        if (this.transform.parent != null)
        {
            this.transform.parent.GetComponent<PlayerController>().enabled = false;
            this.transform.parent.GetComponent<PlayerController>().camTransform.gameObject.SetActive(false);
            this.transform.position += new Vector3(0f, 5f, 0f);
            droneCam.gameObject.SetActive(true);
            isDroneMoving = true;
        }
        else
        {
            Debug.Log($"Error - parent null");
        }
        /*player.transform.GetChild(0).GetComponent<Camera>().gameObject.SetActive(false);
        player.GetComponent<PlayerController>().enabled = false;
        gameObject.transform.position += new Vector3 (0f, 5f, 0f);
        droneCam.gameObject.SetActive(true);       
        isDroneMoving = true;*/
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

    public void OnFlash()
    {
       // flash.range = 8f;
    }
}
