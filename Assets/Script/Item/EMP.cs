using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public  class EMP : MonoBehaviour
{
    protected float minGauge = 15f;
    protected float maxGauge = 45f;
    protected float chargingTime = 2f;
    protected Slider powerSlider;

    protected float currenGauge;
    protected float chargingSpeed;
    public  bool finished;
    protected int empAmount = 0;

    public int count = 0;

    
    private void Start()
    {        
        powerSlider = GameObject.Find("Power Slider").GetComponent<Slider>();       
        chargingSpeed = (maxGauge - minGauge) / chargingTime;

        SetUp();

        if(powerSlider != null)
        {
            Debug.Log("slider : " + powerSlider);
        }
        else
        {
            Debug.Log("slider : null " );
        }
       
    }


    public void SetUp()
    {
        if (powerSlider != null)
        {
            finished = false;
            currenGauge = minGauge;
            powerSlider.value = minGauge;
            powerSlider.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("slider : " + powerSlider);
            Debug.Log("슬라이더가 없음");
            
        }      
    }


  //  public abstract void Install();

    public void CheckEMP()
    {
        for (int i = 0; i < Item.arrayIndex; i++)
        {
            if (Item.myItem[i].name == "EMP")
            {
                empAmount += 1;
                Debug.Log("emp : " + empAmount);
            }            
        }
    }
}

