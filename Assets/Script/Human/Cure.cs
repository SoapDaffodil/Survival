using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cure : MonoBehaviour
{
    private int hp = 1;
    public bool isDead = false;
    private Slider hpSlider;
    private float minGauge = 15f;
    private float maxGauge = 45f;
    private float currenGauge;
    private float chargingSpeed;
    private float chargingTime = 3f;
    private bool finished = false;

    private void Start()
    {
        //hpSlider = GameObject.Find("hpSlider").GetComponent<Slider>();
        chargingSpeed = (maxGauge - minGauge) / chargingTime;
        currenGauge = minGauge;
        //hpSlider.value = minGauge;
        //hpSlider.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("CureZone") && isDead == false)
        {
            if (hp >= 3)
            {
                Debug.Log("체력이 모두 가득 찼습니다.");
            }
            else if (hp == 0)
            {
                Debug.Log("죽었습니다.");
            }
            else
            {
                hpSlider.gameObject.SetActive(true);
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("CureZone") && isDead == false)
        {
            if (hp >= 3)
            {
                Debug.Log("체력이 모두 가득 찼습니다.");
            }
            else if (hp == 0)
            {
                Debug.Log("죽었습니다.");
            }
            else if(currenGauge >= maxGauge)
            {
                finished = true;
                currenGauge = minGauge;
                hpSlider.gameObject.SetActive(false);

                Debug.Log("현재 hp : " + hp);
                hp = hp + 1;
                Debug.Log("회복 후 hp : " + hp);

            }
            else if(hp > 0 && finished == false)
            {
                currenGauge += chargingSpeed * Time.deltaTime;
                hpSlider.value = currenGauge;                
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("CureZone"))
        {
            if(finished == false)
            {
                currenGauge = minGauge;
                hpSlider.value = currenGauge;
            }
            else
            {
                finished = false;
                Debug.Log("finished : " + finished);
            }
        }
    }
}
