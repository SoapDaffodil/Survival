using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public  class EMP : MonoBehaviour
{
<<<<<<< HEAD
    protected float minGauge = 15f;
    protected float maxGauge = 45f;
    protected float chargingTime = 2f;
    protected Slider powerSlider;
=======
    private float minGauge = 15f;
    private float maxGauge = 45f;
    private float chargingTime = 2f;
>>>>>>> ed4ea78d44db50ea7759824ea6ade76dbf0b81cc

    protected float currenGauge;
    protected float chargingSpeed;
    public  bool finished;
    protected int empAmount = 0;

    public int count = 0;

    
    private void Start()
    {        
        UIManager.instance.powerSlider = GameObject.Find("Power Slider").GetComponent<Slider>();       
        chargingSpeed = (maxGauge - minGauge) / chargingTime;

        SetUp();

        if(UIManager.instance.powerSlider != null)
        {
            Debug.Log("slider : " + UIManager.instance.powerSlider);
        }
        else
        {
            Debug.Log("slider : null " );
        }
       
    }


    public void SetUp()
    {
        if (UIManager.instance.powerSlider != null)
        {
            finished = false;
            currenGauge = minGauge;
            UIManager.instance.powerSlider.value = minGauge;
            UIManager.instance.powerSlider.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("slider : " + UIManager.instance.powerSlider);
            Debug.Log("슬라이더가 없음");
            
        }      
    }


<<<<<<< HEAD
  //  public abstract void Install();
=======
    public void Install()
    {
        if (UIManager.instance.powerSlider != null)
        {
            UIManager.instance.powerSlider.gameObject.SetActive(true);
        }
        else
        {
            Debug.Log("슬라이더가 없음");
        }
        

        if (finished == true || empAmount == 0)
        {
            currenGauge = minGauge;
            UIManager.instance.powerSlider.value = currenGauge;
            UIManager.instance.powerSlider.gameObject.SetActive(false);

            return;
        }        

        if(currenGauge >= maxGauge && !finished)
        {
            currenGauge = maxGauge;
            finished = true;
            empAmount = empAmount - 1;

            Item.EliminateItem();
        }

        else if(Input.GetKey(KeyCode.E) && !finished)
        {
            currenGauge = currenGauge + chargingSpeed * Time.deltaTime;
            UIManager.instance.powerSlider.value = currenGauge;
        }

        else if(Input.GetKeyUp(KeyCode.E))
        {
            currenGauge = minGauge;
            UIManager.instance.powerSlider.value = minGauge;
        }     
    }
>>>>>>> ed4ea78d44db50ea7759824ea6ade76dbf0b81cc

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
<<<<<<< HEAD
=======

    public void CheckSlider()
    {
        Debug.Log("나의 slider : " + UIManager.instance.powerSlider);
    }
>>>>>>> ed4ea78d44db50ea7759824ea6ade76dbf0b81cc
}

