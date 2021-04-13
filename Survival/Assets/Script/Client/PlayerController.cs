using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform camTransform;
    public bool getKeyDownF = false;
    public bool getKeyDownE = false;
    public KeyCode[] input = { KeyCode.W , KeyCode.S , KeyCode.A , KeyCode.D , KeyCode.Space };

    private void Start()
    {
        input[0] = KeyCode.W;
        input[1] = KeyCode.S;
        input[2] = KeyCode.A;
        input[3] = KeyCode.D;
        input[4] = KeyCode.Space;
    }

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
        if (Input.GetKeyDown(KeyCode.M))
        {
            UIManager.instance.mapActive = !UIManager.instance.mapActive;
            UIManager.instance.map.SetActive(UIManager.instance.mapActive);
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            KeyChange();
        }
        if (Input.GetKey(KeyCode.E))
        {
            getKeyDownE = true;

            // 아이템 가져오기
            EMP emp = GameManager.players[Client.instance.myId].GetComponent<PlayerManager>().playerItem.item_number2[0].GetComponent<EMP>();

            if(!emp && !emp.finished)
            {
                emp.Install();
            }
            else if (Input.GetKeyUp(KeyCode.E) && !emp.finished)
            {
                getKeyDownE = false;
                emp.Install();
            }
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
            Input.GetKey(input[0]),
            Input.GetKey(input[1]),
            Input.GetKey(input[2]),
            Input.GetKey(input[3]),
            Input.GetKey(input[4])
            };

            ClientSend.PlayerMovement(_inputs);
        }

        public void KeyChange()
        {
            KeyCode[] changeInput = new KeyCode[input.Length];
            int[] index = new int[input.Length];
            int[] random = new int[input.Length];
            for (int i = 0; i < input.Length; i++)
            {
                index[i] = i;
            }
            for (int i = input.Length; i > 0; i--)
            {
                int value = Random.Range(0, i - 1);
                random[i - 1] = index[value];
                index[value] = index[i - 1];
            }
            for (int i = 0; i < input.Length; i++)
            {
                changeInput[i] = input[random[i]];
            }
            input = changeInput;


            UIManager.instance.monsterKey[0].text = "전진 :" + input[0].ToString();
            UIManager.instance.monsterKey[1].text = "왼쪽 :" + input[1].ToString();
            UIManager.instance.monsterKey[2].text = "뒤로 :" + input[2].ToString();
            UIManager.instance.monsterKey[3].text = "오른쪽 :" + input[3].ToString();
            UIManager.instance.monsterKey[4].text = "점프 :" + input[4].ToString();
        }


        public void OnTriggerStay(Collider other)
        {
            if (getKeyDownF)
            {
                getKeyDownF = false;
                //아이템획득
                if (other.CompareTag("Item"))
                {
                    /*if (Item.arrayIndex == Item.inventoryBox.Length)
                    {
                        Debug.Log("아이템 창이 가득 찼습니다");
                    }
                    else
                    {
                        ClientSend.PlayerGetItem(other.gameObject);
                    }*/
                    ClientSend.PlayerGetItem(other.gameObject);

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

                else if (getKeyDownE)
                {
                /*
                    getKeyDownE = false;

                    EMP emp = GameManager.players[Client.instance.myId].GetComponent<PlayerManager>().playerItem.item_number2[0].GetComponent<EMP>();

                    if (other.CompareTag("EMPZONE") && !emp)
                    {
                        emp.Install();
                        emp.count++;
                    }
                */
                }
            }
        }
    
}