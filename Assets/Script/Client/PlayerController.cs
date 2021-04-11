using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform camTransform;
    public bool getKeyDownF = false;

    private void Update()
    {
        //총 발사
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            ClientSend.PlayerShootBullet(camTransform.forward);
        }
        //탄 발사
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            ClientSend.PlayerShootBomb(camTransform.forward);
        }
        //상호작용(아이템획득, 문열기, 은폐 등
        if (Input.GetKeyDown(KeyCode.F))
        {
            getKeyDownF = true;
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            ClientSend.PlayerThrowItem(GameManager.players[Client.instance.myId].grabItem.gameObject, GameManager.players[Client.instance.myId].transform.position);
        }
    }

    private void FixedUpdate()
    {
        SendInputToServer();
    }

    /// <summary>Sends player input to the server.</summary>
    private void SendInputToServer()
    {
        bool[] _inputs = new bool[]
        {
            Input.GetKey(KeyCode.W),
            Input.GetKey(KeyCode.S),
            Input.GetKey(KeyCode.A),
            Input.GetKey(KeyCode.D),
            Input.GetKey(KeyCode.Space)
        };

        ClientSend.PlayerMovement(_inputs);
    }

    public void OnTriggerStay(Collider other)
    {
        if (getKeyDownF)
        {
            getKeyDownF = false;
            //아이템획득
            if (other.CompareTag("Item"))
            {
                if (Item.arrayIndex == Item.inventoryBox.Length)
                {
                    Debug.Log("아이템 창이 가득 찼습니다");
                }
                else
                {
                    ClientSend.PlayerGetItem(other.gameObject);







                    /*
                    Item.myItem[Item.arrayIndex] = other.gameObject;

                    Debug.Log("줍는 아이템 : " + Item.myItem[Item.arrayIndex]);

                    other.gameObject.SetActive(false);
                    //other.gameObject.GetComponent<MeshRenderer>().enabled = false;

                    if (other.GetComponent<Image>() != null)
                    {
                        Item.inventoryBox[Item.arrayIndex].GetComponent<Image>().sprite = other.GetComponent<Image>().sprite;
                    }
                    else
                    {
                        Item.inventoryBox[Item.arrayIndex].GetComponent<Image>().sprite = Resources.Load<Sprite>("./Resources/inventory Background.png");
                    }

                    Item.arrayIndex++;
                    Debug.Log("array index : " + Item.arrayIndex);*/
                }

            }
            //문열기
            else if (other.CompareTag("Door"))
            {

                if (other.gameObject.transform.rotation.y == 0f)
                {
                    other.gameObject.transform.RotateAround(other.gameObject.transform.GetChild(0).position, Vector3.up, -90f);
                }

                else
                {
                    other.gameObject.transform.RotateAround(other.gameObject.transform.GetChild(0).position, Vector3.up, 90f);
                }

            }
        }
    }
}