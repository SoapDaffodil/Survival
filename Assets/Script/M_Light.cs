using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class M_Light : MonoBehaviour
{
    private float minGauge = 15f;
    private float maxGauge = 45f;
    private float chargingTime = 2f;
    public Slider powerSlider;

    private float currenGauge;
    private float chargingSpeed;
    private bool finished;
    private int LightAmount = 0;


    private void Start()
    {

        chargingSpeed = (maxGauge - minGauge) / chargingTime;
    }

    public void SetUp()
    {
        finished = false;
        currenGauge = minGauge;
        powerSlider.value = minGauge;
        powerSlider.gameObject.SetActive(false);
    }


    public void Install()
    {
        powerSlider.gameObject.SetActive(true);

        if (finished == true || LightAmount == 0)
        {
            currenGauge = minGauge;
            powerSlider.value = currenGauge;
            powerSlider.gameObject.SetActive(false);

            return;
        }

        if (currenGauge >= maxGauge && !finished)
        {
            currenGauge = maxGauge;
            finished = true;

            Item.myItem[Item.arrayIndex - 1].SetActive(true);
            Item.myItem[Item.arrayIndex - 1].transform.position = GameObject.Find("CharacterObject").transform.position;
            EliminateItem();
        }

        else if (Input.GetKey(KeyCode.E) && !finished)
        {
            currenGauge = currenGauge + chargingSpeed * Time.deltaTime;
            powerSlider.value = currenGauge;
        }

        else if (Input.GetKeyUp(KeyCode.E))
        {
            currenGauge = minGauge;
            powerSlider.value = minGauge;

            Debug.Log("current : " + currenGauge);
            Debug.Log("slider value : " + powerSlider.value);
        }

        LightAmount = LightAmount - 1;
    }

    public void CheckLight()
    {
        for (int i = 0; i < Item.arrayIndex; i++)
        {
            if (Item.myItem[i].name == "EMP")
            {
                LightAmount += 1;
                Debug.Log("emp : " + LightAmount);
            }

            Debug.Log("emp가 없습니다");
        }
    }


    public void EliminateItem()
    {
        Item.arrayIndex -= 1;
        Item.inventoryBox[Item.arrayIndex].GetComponent<Image>().sprite = Resources.Load<Sprite>("./Resources/inventory Background.png");

        finished = false;
    }



}



