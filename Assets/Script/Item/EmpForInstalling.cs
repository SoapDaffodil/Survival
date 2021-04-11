using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmpForInstalling : EMP
{
    
    /*
    public override void Install()
    {
        throw new System.NotImplementedException();

        if (powerSlider != null)
        {
            powerSlider.gameObject.SetActive(true);
        }
        else
        {
            Debug.Log("슬라이더가 없음");
        }


        if (finished == true || empAmount == 0)
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

            //emp 개수 줄이기
            //Item.EliminateItem();
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
        }
    }
    */
    
}
