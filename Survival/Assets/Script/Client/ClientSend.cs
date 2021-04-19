using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientSend : MonoBehaviour
{
    /// <summary>클라이언트에서 서버로 TCP형태로 패킷전송</summary>
    /// <param name="_packet"></param>
    private static void SendTCPData(Packet _packet)
    {
        _packet.WriteLength();
        Client.instance.tcp.SendData(_packet);
    }

    /// <summary>클라이언트에서 서버로 UDP형태로 패킷전송</summary>
    private static void SendUDPData(Packet _packet)
    {
        _packet.WriteLength();
        Client.instance.udp.SendData(_packet);
    }
    
    #region Packets
    /// <summary>welcome메세지를 받고난 후 동작하는 함수 (잘 받았다고 서버로 전송)</summary>
    public static void WelcomeReceived()
    {
        using (Packet _packet = new Packet((int)ClientPackets.welcomeReceived))
        {
            _packet.Write(Client.instance.myId);
            _packet.Write(UIManager.instance.usernameField.text);

            SendTCPData(_packet);
        }
        switch (UIManager.instance.usernameField.text)
        {
            case "0": case "monster": case "Monster":
                GameManager.character_human = false;
                UIManager.instance.SetActiveTrueMonsterKey();
                break;
            case "1": case "human": case "Human":
                GameManager.character_human = true;
                UIManager.instance.SetActiveFalseMonsterKey();
                break;
        }
    }
    /// <summary>player 움직임에 대한 packet UDP전송(주기적으로 전송하기때문에 패킷의 끝을 확인해야함)</summary>
    public static void PlayerMovement(bool[] _inputs)
    {
        using (Packet _packet = new Packet((int)ClientPackets.playerMovement))
        {
            _packet.Write(_inputs.Length);
            foreach(bool _input in _inputs)
            {
                _packet.Write(_input);
            }
            _packet.Write(GameManager.players[Client.instance.myId].transform.rotation);

            //주기적으로 보내기때문에 데이터충돌발생확률이 높음 > UDP
            SendUDPData(_packet);
        }
    }
    /// <summary>player 공격에 대한 packet TCP전송(공격할 때 한번만 전송하므로 누락이 될지언정 오류가 발생하지는 않음)</summary>
    public static void PlayerShootBullet(Vector3 _facing)
    {
        using (Packet _packet = new Packet((int)ClientPackets.playerShootBullet))
        {
            _packet.Write(_facing);
            _packet.Write(GameManager.EMPInstallFinished);

            SendTCPData(_packet);
        }
    }

    /// <summary>아이템 버리기에 대한 packet TCP전송(한번만 전송하므로 누락이 될지언정 오류가 발생하지는 않음)</summary>
    /// <param name="_facing">버릴 위치</param>
    public static void PlayerShootBomb(Vector3 _facing)
    {
        using (Packet _packet = new Packet((int)ClientPackets.playerShootBomb))
        {
            _packet.Write(_facing);

            SendTCPData(_packet);
        }
    }

    /// <summary>아이템 획득에 대한 packet TCP전송(한번만 전송하므로 누락이 될지언정 오류가 발생하지는 않음)</summary>
    /// <param name="_item">획득한 아이템종류</param>
    public static void PlayerGetItem(GameObject _item)
    {
        using (Packet _packet = new Packet((int)ClientPackets.playerGetItem))
        {
            _packet.Write(_item.name);
            _packet.Write(_item.GetComponent<ItemSpawner>().spawnerId);

            SendTCPData(_packet);
        }
    }

    /// <summary>아이템 버리기에 대한 packet TCP전송(한번만 전송하므로 누락이 될지언정 오류가 발생하지는 않음)</summary>
    /// <param name="_item">버릴 아이템종류</param>
    /// <param name="_position">버릴위치</param>
    public static void PlayerThrowItem(GameObject _item, Vector3 _position)
    {
        using (Packet _packet = new Packet((int)ClientPackets.playerThrowItem))
        {
            _packet.Write(_item.name);
            _packet.Write(_item.GetComponent<ItemSpawner>().spawnerId);

            _packet.Write(_position.x);
            _packet.Write(0.5f);
            _packet.Write(_position.z);

            SendTCPData(_packet);
        }
    }

    /// <summary>설치한 EMP packet TCP전송(한번만 전송하므로 누락이 될지언정 오류가 발생하지는 않음)</summary>
    /// <param name="_position">플레이어 위치</param>
    /// <param name="_emp">설치하는 emp</param>
    public static void InstallEMP(Vector3 _position, GameObject _emp)
    {
        using (Packet _packet = new Packet((int)ClientPackets.installEMP))
        {
            _packet.Write(_position.x);
            _packet.Write(0.5f);
            _packet.Write(_position.z);

            _packet.Write(_emp.GetComponent<ItemSpawner>().spawnerId);
            

            SendTCPData(_packet);
        }
    }

    /// <summary>플레이어 체력 회복 packet TCP전송(한번만 전송하므로 누락이 될지언정 오류가 발생하지는 않음)</summary>
    /// <param name="_hp">플레이어 체력</param>
    public static void Cure(float _hp)
    {
        using (Packet _packet = new Packet((int)ClientPackets.cure))
        {
            _packet.Write(_hp);

            SendTCPData(_packet);
        }
    }

    /// <summary>플레이어 은폐 packet TCP전송(한번만 전송하므로 누락이 될지언정 오류가 발생하지는 않음)</summary>
    /// <param name="_position">은폐하는 위치</param>
    public static void Hide(Vector3 _position)
    {
        using (Packet _packet = new Packet((int)ClientPackets.hide))
        {
            _packet.Write(_position.x);
            _packet.Write(_position.y);
            _packet.Write(_position.z);

            SendTCPData(_packet);
        }
    }
}

    /*
    public static void UDPTestReceived()
    {
        using (Packet _packet = new Packet((int)ClientPackets.udpTestReceived))
        {
            _packet.Write("Received a UDP packet.");
            SendUDPData(_packet);
        }
    }*/
    #endregion

