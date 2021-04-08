using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Drone : MonoBehaviour
{
    private Slider charge;
    public Move droneMoving;
    private Light flash;
    private float minGauge = 15f;
    private float maxGauge = 45f;
    private float chargingTime = 2f;

    private float chargingSpeed;
    private float currentGauge;
    public bool finished;
    public bool isGaugeFull = false;

    // Start is called before the first frame update
    void Start()
    {
        charge = GameObject.Find("DroneSlider").GetComponent<Slider>();
        droneMoving = gameObject.GetComponent<Move>();
        flash = gameObject.GetComponent<Light>();

        droneMoving.enabled = false;
        SetUp();
    }


    void SetUp()
    {
        if (charge != null)
        {
            currentGauge = minGauge;
            charge.value = minGauge;
            chargingSpeed = (maxGauge - minGauge) / chargingTime;
            finished = false;
            charge.gameObject.SetActive(false);
        }

        else if(flash != null)
        {
            flash.type = LightType.Point;
            flash.range = 0f;
        }
    }

    public void ChargingGauge()
    {
        charge.gameObject.SetActive(true);

        if(finished == true)
        {
            Debug.Log("드론 충전이 이미 완료 되었습니다!");
        }

        else if(currentGauge >= maxGauge && !finished)
        {
            Debug.Log("드론 충전 완료!");
            finished = true;
            currentGauge = minGauge;
            charge.value = minGauge;
            isGaugeFull = true;

            charge.gameObject.SetActive(false);
        }
        
        else if(Input.GetMouseButton(0) && !finished)
        {
            currentGauge += chargingSpeed * Time.deltaTime;
            charge.value = currentGauge;
        }
        
        else if(Input.GetMouseButtonUp(0) && !finished)
        {
            currentGauge = minGauge;
            charge.value = currentGauge;
        }
    }

    public void OnFlash()
    {
        flash.range = 8f;
    }

    public void OffFlash()
    {
        flash.range = 0f;
    }
}
