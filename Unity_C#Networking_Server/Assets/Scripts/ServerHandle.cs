using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class ServerHandle
{
    /// <summary>welcome 잘받았다는 메세지 도착 시 클라이언트ID가 맞는지 확인 후 서버에 출력</summary>
    /// <param name="_fromClient"></param>
    /// <param name="_packet"></param>
    public static void WelcomeReceived(int _fromClient, Packet _packet)
    {
        int _clientIdCheck = _packet.ReadInt();
        string _username = _packet.ReadString();
        
        Debug.Log($"{Server.clients[_fromClient].tcp.socket.Client.RemoteEndPoint} connected successfully and is now player {_fromClient}.");
        if (_fromClient != _clientIdCheck)
        {
            Debug.Log($"Player \"{_username}\" (ID: {_fromClient}) has assumed the wrong client ID({_clientIdCheck})!");
        }
        // TODO: send
    }

    /// <summary>게임시작</summary>
    /// <param name="_fromClient"></param>
    /// <param name="_packet"></param>
    public static void GameStart(int _fromClient, Packet _packet)
    {
        int _clientIdCheck = _packet.ReadInt();
        string _username = _packet.ReadString();

        Debug.Log($"{Server.clients[_fromClient].tcp.socket.Client.RemoteEndPoint} connected successfully and is now player {_fromClient}.");
        if (_fromClient != _clientIdCheck)
        {
            Debug.Log($"Player \"{_username}\" (ID: {_fromClient}) has assumed the wrong client ID({_clientIdCheck})!");
        }
        Server.clients[_fromClient].SendIntoGame(_username);
        // TODO: send
    }

    /// <summary>플레이어의 움직임packet을 받아 움직임 처리</summary>
    /// <param name="_fromClient"></param>
    /// <param name="_packet"></param>
    public static void PlayerMovement(int _fromClient, Packet _packet)
    {
        bool[] _inputs = new bool[_packet.ReadInt()];
        for (int i = 0; i < _inputs.Length; i++)
        {
            _inputs[i] = _packet.ReadBool();
        }
        Quaternion _rotation = _packet.ReadQuaternion();

        Server.clients[_fromClient].player.SetInput(_inputs, _rotation);
    }

    /// <summary>player 공격에 대한 패킷을 통해 shoot 처리</summary>
    /// <param name="_fromClient"></param>
    /// <param name="_packet"></param>
    public static void PlayerShootBullet(int _fromClient, Packet _packet)
    {
        Vector3 _shootDirection = _packet.ReadVector3();
        bool _EMPInstallFinished = _packet.ReadBool();

        Server.clients[_fromClient].player.Shoot(_shootDirection, _EMPInstallFinished);
    }

    /// <summary>괴물 공격에 대한 패킷을 통해 shoot 처리</summary>
    /// <param name="_fromClient"></param>
    /// <param name="_packet"></param>
    public static void CreatureAttack(int _fromClient, Packet _packet)
    {
        //Vector3 _shootDirection = _packet.ReadVector3();
        //Server.clients[_fromClient].player.CreatureAttack(_shootDirection);

        bool _isCreatureAttack = _packet.ReadBool();
        Server.clients[_fromClient].player.creatureAttack = _isCreatureAttack;
    }

    /// <summary>투척에 대한 패킷을 통해 아이템버리는 것 처리</summary>
    /// <param name="_fromClient"></param>
    /// <param name="_packet"></param>
    public static void PlayerShootBomb(int _fromClient, Packet _packet)
    {
        Vector3 _throwDirection = _packet.ReadVector3();

        Server.clients[_fromClient].player.ThrowItem(_throwDirection);
    }

    /// <summary>아이템획득에 관한 패킷을 통해 아이템획득 처리</summary>
    /// <param name="_fromClient"></param>
    /// <param name="_packet"></param>
    public static void PlayerGetItem(int _fromClient, Packet _packet)
    {
        int _spawnerId = _packet.ReadInt();

        ItemSpawner.spawners[_spawnerId].ItemPickedUp(Server.clients[_fromClient].player);
    }

    /// <summary>아이템버리기에 관한 패킷을 통해 아이템버리기 처리</summary>
    /// <param name="_fromClient"></param>
    /// <param name="_packet"></param>
    public static void PlayerThrowItem(int _fromClient, Packet _packet)
    {
        int _spawnerId = _packet.ReadInt();
        Vector3 _position = _packet.ReadVector3();

        ItemSpawner.spawners[_spawnerId].ItemThrow(Server.clients[_fromClient].player, _position);
    }

    /// <summary>아이템들기에 관한 패킷을 통해 아이템들기 처리</summary>
    /// <param name="_fromClient"></param>
    /// <param name="_packet"></param>
    public static void PlayerGrabItem(int _fromClient, Packet _packet)
    {
        int _grabSpawnerId = _packet.ReadInt();
        int _spawnerId = _packet.ReadInt();
        int _key = _packet.ReadInt();

        ItemSpawner.spawners[_spawnerId].ItemGrab(_grabSpawnerId, Server.clients[_fromClient].player, _key);
    }

    /// <summary>아이템사용에 관한 패킷을 통해 아이템들기 처리</summary>
    /// <param name="_fromClient"></param>
    /// <param name="_packet"></param>
    public static void PlayerUseItem(int _fromClient, Packet _packet)
    {
        int _spawnerId = _packet.ReadInt();
        int _itemNumber = _packet.ReadInt();

        ItemSpawner.spawners[_spawnerId].ItemReleaseGrab();
        ServerSend.ItemUse(_spawnerId, _fromClient, _itemNumber);
    }

    /// <summary>설치 관련정보 패킷을 통해 처리</summary>
    /// <param name="_fromClient"></param>
    /// <param name="_packet"></param>
    public static void Install(int _fromClient, Packet _packet)
    {
        Vector3 _position = _packet.ReadVector3();
        int _spawnerId = _packet.ReadInt();
        int _floor = _packet.ReadInt();

        ItemSpawner.spawners[_spawnerId].Install(Server.clients[_fromClient].player.id, _position, _floor);
    }

    /// <summary>EMPZONE에 EMP 설치완료에 관해 패킷을 통해 처리</summary>
    /// <summary>EMP 설치완료에 관해 패킷을 통해 처리</summary>
    /// <param name="_fromClient"></param>
    /// <param name="_packet"></param>
    public static void InstallEMP(int _fromClient, Packet _packet)
    {
       Vector3 _position = _packet.ReadVector3();
       int _spawnerId = _packet.ReadInt();

        ItemSpawner.spawners[_spawnerId].InstallEMP(_position);
    }

    /// <summary>플레이어 체력 회복에 관해 패킷을 통해 처리</summary>
    /// <param name="_fromClient"></param>
    /// <param name="_packet"></param>
    public static void Cure(int _fromClient, Packet _packet)
    {
        float _hp = _packet.ReadFloat();

        Server.clients[_fromClient].player.Cure(_hp);
    }

    /// <summary>플레이어 은폐 관해 패킷을 통해 처리</summary>
    /// <param name="_fromClient"></param>
    /// <param name="_packet"></param>
    public static void Hide(int _fromClient, Packet _packet)
    {
        Vector3 _hidePosition = _packet.ReadVector3();

        Server.clients[_fromClient].player.Hide(_hidePosition);
    }

    /// <summary>치료 스킬</summary>
    /// <param name="_fromClient"></param>
    /// <param name="_packet"></param>
    public static void SkillCure(int _fromClient, Packet _packet)
    {
        bool _cure = _packet.ReadBool();
        ServerSend.MotionCure(_fromClient, _cure);
    }

    /// <summary>순간이동 스킬</summary>
    /// <param name="_fromClient"></param>
    /// <param name="_packet"></param>
    public static void SkillTeleportation(int _fromClient, Packet _packet)
    {
        Vector3 _targetPosition = _packet.ReadVector3();
        Server.clients[_fromClient].player.Teleportation(_targetPosition);
    }

    /// <summary>드론 활성화 스킬</summary>
    /// <param name="_fromClient"></param>
    /// <param name="_packet"></param>
    public static void SkillDrone(int _fromClient, Packet _packet)
    {
        int _spawnerId = _packet.ReadInt();

        Server.clients[_fromClient].player.controller.enabled = false;
        ItemSpawner.spawners[_spawnerId].GetComponent<Drone>().controller.enabled = true;

        ServerSend.DroneEnabled(_fromClient);
    }

    /// <summary>드론의 움직임packet을 받아 움직임 처리</summary>
    /// <param name="_fromClient"></param>
    /// <param name="_packet"></param>
    public static void DroneMovement(int _fromClient, Packet _packet)
    {
        bool[] _inputs = new bool[_packet.ReadInt()];
        for (int i = 0; i < _inputs.Length; i++)
        {
            _inputs[i] = _packet.ReadBool();
        }
        Quaternion _rotation = _packet.ReadQuaternion();

        int _spawnId = _packet.ReadInt();
        
        ItemSpawner.spawners[_spawnId].GetComponent<Drone>().SetInput(_inputs, _rotation);
    }


    /// <summary>드론의 움직임 멈춤 packet을 받아 움직임 처리</summary>
    /// <param name="_fromClient"></param>
    /// <param name="_packet"></param>
    public static void DroneStop(int _fromClient, Packet _packet)
    {        
        int _spawnId = _packet.ReadInt();

        ItemSpawner.spawners[_spawnId].GetComponent<Drone>().controller.enabled = false;
        Server.clients[_fromClient].player.controller.enabled = true;

        ServerSend.DroneEnabledFalse(_fromClient);
    }

    /// <summary>괴물 이속 증가 packet을 받아 움직임 처리</summary>
    /// <param name="_fromClient"></param>
    /// <param name="_packet"></param>
    public static void SkillSpeedUp(int _fromClient, Packet _packet)
    {
        int _playerId = _packet.ReadInt();
        Server.clients[_fromClient].player.SpeedUp();
    }

    /*
    /// <summary>welcome 잘받았다는 메세지 도착 시 클라이언트ID가 맞는지 확인 후 서버에 출력</summary>
    /// <param name="_fromClient"></param>
    /// <param name="_packet"></param>
    public static void UDPTestReceived(int _fromClient, Packet _packet)
    {
        string _msg = _packet.ReadString();

        Debug.Log($"Received packet via UDP. Contains message: {_msg}");
    }*/
}