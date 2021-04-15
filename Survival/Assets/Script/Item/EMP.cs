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
    public bool isInstalling = false;

    
    private void Start()
    {        
        //UIManager.instance.powerSlider = GameObject.Find("Power Slider").GetComponent<Slider>();       
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


    public void Install()
    {
        if(finished)
        {
            currenGauge = minGauge;
            finished = false;
            UIManager.instance.powerSlider.value = currenGauge;
            UIManager.instance.powerSlider.gameObject.SetActive(false);

            Debug.Log("이미 설치가 완료 되었습니다");
        }

        else if(currenGauge >= maxGauge && !finished)
        {
            finished = true;
            currenGauge = maxGauge;
            isInstalling = false;

            ClientSend.InstallEMP(transform.position, GameManager.players[Client.instance.myId].GetComponent<PlayerManager>().playerItem.item_number2[0]);
        }

        else if(currenGauge < maxGauge && !finished)
        {
            currenGauge = currenGauge * chargingSpeed * Time.deltaTime;
            UIManager.instance.powerSlider.value = currenGauge;
            isInstalling = true;
        }
    }

    public void InstallEMPTrap()
    {
        if (finished)
        {
            currenGauge = minGauge;
            finished = false;
            UIManager.instance.powerSlider.value = currenGauge;
            UIManager.instance.powerSlider.gameObject.SetActive(false);

            Debug.Log("이미 설치가 완료 되었습니다");
        }

        else if (currenGauge >= maxGauge && !finished)
        {
            finished = true;
            currenGauge = maxGauge;
            isInstalling = false;

            ClientSend.InstallEMPTrap(transform.position, GameManager.players[Client.instance.myId].GetComponent<PlayerManager>().playerItem.item_number2[0]);
        }

        else if (currenGauge < maxGauge && !finished)
        {
            currenGauge = currenGauge * chargingSpeed * Time.deltaTime;
            UIManager.instance.powerSlider.value = currenGauge;
            isInstalling = true;
        }
    }

    public void InstallCancle()
    {
        currenGauge = minGauge;
        UIManager.instance.powerSlider.value = minGauge;
    }


    /*
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
    */

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

