using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

/// <summary>
/// 서버에서 클라이언트로 패킷전송 (한번만 전송하는것은 TCP, 지속적인 전송은 UDP)
/// </summary>
public class ServerSend
{
    /// <summary>_toClient에 해당하는 클라이언트에 패킷전송 _TCP</summary>
    /// <param name="_toClient">수신할 클라이언트</param>
    /// <param name="_packet">발신할 패킷</param>
    private static void SendTCPData(int _toClient, Packet _packet)
    {
        _packet.WriteLength();
        Server.clients[_toClient].tcp.SendData(_packet);
    }

    /// <summary>_toClient에 해당하는 클라이언트에 패킷전송 _UDP</summary>
    /// <param name="_toClient">수신할 클라이언트</param>
    /// <param name="_packet">발신할 패킷</param>
    private static void SendUDPData(int _toClient, Packet _packet)
    {
        _packet.WriteLength();
        Server.clients[_toClient].udp.SendData(_packet);
    }

    /// <summary>연결된 모든 클라이언트에 패킷전송 _TCP</summary>
    /// <param name="_packet">발신할 패킷</param>
    private static void SendTCPDataToAll(Packet _packet)
    {
        _packet.WriteLength();
        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            Server.clients[i].tcp.SendData(_packet);
        }
    }

    /// <summary>_exceptClient를 제외한 모든 클라이언트에 패킷 전송 _TCP</summary>
    /// <param name="_exceptClient">예외 클라이언트번호</param>
    /// <param name="_packet">발신할 패킷</param>
    private static void SendTCPDataToAll(int _exceptClient, Packet _packet)
    {
        _packet.WriteLength();
        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            if (i != _exceptClient)
            {
                Server.clients[i].tcp.SendData(_packet);
            }
        }
    }

    /// <summary>연결된 모든 클라이언트에 패킷전송 _UDP</summary>
    /// <param name="_packet">발신할 패킷</param>
    private static void SendUDPDataToAll(Packet _packet)
    {
        _packet.WriteLength();
        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            Server.clients[i].udp.SendData(_packet);
        }
    }

    /// <summary>_exceptClient를 제외한 모든 클라이언트에 패킷 전송 _UDP</summary>
    /// <param name="_exceptClient">예외 클라이언트번호</param>
    /// <param name="_packet">발신할 패킷</param>
    private static void SendUDPDataToAll(int _exceptClient, Packet _packet)
    {
        _packet.WriteLength();
        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            if (i != _exceptClient)
            {
                Server.clients[i].udp.SendData(_packet);
            }
        }
    }

    #region Packets
    /// <summary>server 에서 _toClient에 해당하는 클라이언트로 메세지(welcome) 전송</summary>
    /// <param name="_toClient">수신할 클라이언트</param>
    /// <param name="_msg">패킷화 할 string data</param>
    public static void Welcome(int _toClient, string _msg)
    {
        using (Packet _packet = new Packet((int)ServerPackets.welcome))
        {
            _packet.Write(_msg);
            _packet.Write(_toClient);

            SendTCPData(_toClient, _packet);
        }
    }

    /// <summary>에러발생</summary>
    /// <param name="_message">에러 메세지 전송</param>
    public static void Error(int _toClient, string _message)
    {
        using (Packet _packet = new Packet((int)ServerPackets.error))
        {
            _packet.Write(_message);

            SendTCPData(_toClient, _packet);
        }
    }

    /// <summary>클라이언트들에게 스폰된 플레이어를 전달(한번만 전달하기 때문에 TCP)</summary>
    /// <param name="_toClient">플레이어를 스폰해야하는 클라이언트</param>
    /// <param name="_player">스폰되는 플레이어</param>
    public static void SpawnPlayer(int _toClient, Player _player)
    {
        using (Packet _packet = new Packet((int)ServerPackets.spawnPlayer))
        {
            _packet.Write(_player.id);
            _packet.Write((int)_player.playerType);
            _packet.Write(_player.transform.position);
            _packet.Write(_player.transform.rotation);

            //한번만 전달하기 때문에 데이터가 충돌할일이 거의 없음
            SendTCPData(_toClient, _packet);
        }
    }

    /// <summary>플레이어의 위치를 모든 클라이언트에 UDP전송</summary>
    /// <param name="_player">The player whose position to update.</param>
    public static void PlayerPosition(Player _player, bool _walk, bool _run)
    {
        using (Packet _packet = new Packet((int)ServerPackets.playerPosition))
        {
            _packet.Write(_player.id);
            _packet.Write(_player.transform.position);
            _packet.Write(_walk);
            _packet.Write(_run);

            SendUDPDataToAll(_packet);
        }
    }

    /// <summary>플레이어의 rotation을 자기자신을 제외한(localplayer에 중복으로 써지는것을 막기위함) 모든 클라이언트에 UDP전송</summary>
    /// <param name="_player">The player whose rotation to update.</param>
    public static void PlayerRotation(Player _player)
    {
        using (Packet _packet = new Packet((int)ServerPackets.playerRotation))
        {
            _packet.Write(_player.id);
            _packet.Write(_player.transform.rotation);

            SendUDPDataToAll(_player.id, _packet);
        }
    }
    
    /// <summary>특정 플레이어가 연결이 끊길때 모든 클라이언트에게 해당사항 TCP전송</summary>
    /// <param name="_playerId">연결이 끊긴 플레이어ID</param>
    public static void PlayerDisconnected(int _playerId)
    {
        using (Packet _packet = new Packet((int)ServerPackets.playerDisconnected))
        {
            _packet.Write(_playerId);

            SendTCPDataToAll(_packet);
        }
    }

    /// <summary>특정 플레이어의 HP정보를 모든 클라이언트에게 TCP전송</summary>
    /// <param name="_player">전송하고싶은 Player</param>
    public static void PlayerHP(Player _player)
    {
        using (Packet _packet = new Packet((int)ServerPackets.playerHP))
        {
            _packet.Write(_player.id);
            _packet.Write(_player.hp);

            SendTCPDataToAll(_packet);
        }
    }

    /// <summary>리스폰된 특정 플레이어를 모든 클라이언트에게 TCP전송</summary>
    /// <param name="_player">전송하고싶은 Player</param>
    public static void PlayerRespawned(Player _player)
    {
        using (Packet _packet = new Packet((int)ServerPackets.playerRespawned))
        {
            _packet.Write(_player.id);

            SendTCPDataToAll(_packet);
        }
    }

    /// <summary>스폰될 아이템 TCP전송</summary>
    /// <param name="_clientId">전송할 client</param>
    /// <param name="_item">스폰될 아이템</param>
    public static void ItemSpawned(int _clientId, ItemSpawner _item)
    {
        using (Packet _packet = new Packet((int)ServerPackets.itemSpawned))
        {
            _packet.Write(_item.spawnerId);
            _packet.Write(_item.transform.position);
            _packet.Write(_item.tag);
            _packet.Write(_item.hasItem);
            _packet.Write(_item.itemModel.enabled);

            SendTCPData(_clientId, _packet);
        }
    }

    /// <summary>Trap 아이템 TCP전송</summary>
    /// <param name="_clientId">전송할 client</param>
    /// <param name="_item">Trap 아이템</param>
    public static void ItemSetTrap(int _clientId, int _spawnerId, int _floor)
    {
        using (Packet _packet = new Packet((int)ServerPackets.itemSetTrap))
        {
            _packet.Write(_spawnerId);
            _packet.Write(_floor);

            SendTCPData(_clientId, _packet);
        }
    }

    /// <summary>스폰된 아이템 TCP전송</summary>
    /// <param name="_spawnerId">스폰된 아이템</param>
    /// <param name="_spawnerPosition">아이템 position</param>
    /// <param name="_tag">아이템 종류</param>
    public static void ItemSpawned(int _spawnerId, Vector3 _spawnerPosition, string _tag)
    {
        using (Packet _packet = new Packet((int)ServerPackets.itemSpawned))
        {
            _packet.Write(_spawnerId);
            _packet.Write(_spawnerPosition);
            _packet.Write(_tag);

            SendTCPDataToAll(_packet);
        }
    }

    /// <summary>획득한 아이템ID, 획득한 플레이어 TCP전송</summary>
    /// <param name="_spawnerId">획득한 아이템ID</param>
    /// <param name="_byPlayer">아이템을 획득한 플레이어</param>
    public static void ItemPickedUp(int _spawnerId, int _byPlayer)
    {
        using (Packet _packet = new Packet((int)ServerPackets.itemPickedUp))
        {
            _packet.Write(_spawnerId);
            _packet.Write(_byPlayer);

            SendTCPDataToAll(_packet);
        }
    }

    /// <summary>버린 아이템ID, 버린 플레이어 TCP전송</summary>
    /// <param name="_spawnerId">버린 아이템ID</param>
    /// <param name="_byPlayer">아이템을 버린 플레이어</param>
    /// <param name="_position">아이템을 버린 위치</param>
    public static void ItemThrow(int _spawnerId, int _byPlayer, Vector3 _position)
    {
        using (Packet _packet = new Packet((int)ServerPackets.itemThrow))
        {
            _packet.Write(_spawnerId);
            _packet.Write(_byPlayer);
            _packet.Write(_position);

            SendTCPDataToAll(_packet);
        }
    }

    /// <summary>버린 아이템ID, 버린 플레이어 TCP전송</summary>
    /// <param name="_spawnerId">버린 아이템ID</param>
    /// <param name="_byPlayer">아이템을 버린 플레이어</param>
    /// <param name="_position">아이템을 버린 위치</param>
    public static void ItemGrab(int _spawnerId, int _byPlayer, Vector3 _position)
    {
        using (Packet _packet = new Packet((int)ServerPackets.itemGrab))
        {
            _packet.Write(_spawnerId);
            _packet.Write(_byPlayer);
            _packet.Write(_position);

            SendTCPDataToAll(_packet);
        }
    }

    /// <summary>폭탄 생성 TCP전송</summary>
    /// <param name="_projectile">폭탄</param>
    /// <param name="_thrownByPlayer">폭탄을 던진 player</param>
    public static void SpawnProjectile(Projectile _projectile, int _thrownByPlayer)
    {
        using (Packet _packet = new Packet((int)ServerPackets.spawnProjectile))
        {
            _packet.Write(_projectile.id);
            _packet.Write(_projectile.transform.position);
            _packet.Write(_thrownByPlayer);

            SendTCPDataToAll(_packet);
        }
    }

    /// <summary>폭탄 위치 UDP전송(여러번 반복전송)</summary>
    /// <param name="_projectile">폭탄</param>
    public static void ProjectilePosition(Projectile _projectile)
    {
        using (Packet _packet = new Packet((int)ServerPackets.projectilePosition))
        {
            _packet.Write(_projectile.id);
            _packet.Write(_projectile.transform.position);

            SendUDPDataToAll(_packet);
        }
    }

    /// <summary>폭탄 폭발 TCP전송</summary>
    /// <param name="_projectile">폭탄</param>
    public static void ProjectileExploded(Projectile _projectile)
    {
        using (Packet _packet = new Packet((int)ServerPackets.projectileExploded))
        {
            _packet.Write(_projectile.id);
            _packet.Write(_projectile.transform.position);

            SendTCPDataToAll(_packet);
        }
    }

    /// <summary>Key Change 실행</summary>
    /// <param name="_playerID">플레이어 ID</param>
    public static void KeyChange(int _playerID)
    {
        using (Packet _packet = new Packet((int)ServerPackets.keyChange))
        {
            _packet.Write(_playerID);

            SendTCPData(_playerID, _packet);
        }
    }
    
    /// <summary>EMP 설치완료</summary>
    /// <param name="_spawnerId">아이템 ID</param>
    /// <param name="_byPlayer">플레이어 ID</param>
    /// <param name="_position">아이템 포지션</param>
    public static void InstallEMP(int _spawnerId, Vector3 _position, int _byPlayer)
    {
        using (Packet _packet = new Packet((int)ServerPackets.installEMP))
        {
            _packet.Write(_spawnerId);
            _packet.Write(_position);
            _packet.Write(_byPlayer);

            SendTCPDataToAll(_packet);
        }
    }


    /// <summary>Trap 설치완료</summary>
    /// <param name="_spawnerId">아이템 ID</param>
    /// <param name="_position">아이템 포지션</param>
    /// <param name="_floor">아이템이 위치한 층</param>
    public static void InstallTrap(int _spawnerId, Vector3 _position, int _floor, int _byPlayer)
    {
        using (Packet _packet = new Packet((int)ServerPackets.installTrap))
        {
            _packet.Write(_spawnerId);
            _packet.Write(_position);
            _packet.Write(_floor);
            _packet.Write(_byPlayer);

            SendTCPDataToAll(_packet);
        }
    }

    /// <summary>드론 활성화 정보를 클라이언트에 TCP전송</summary>
    public static void DroneEnabled(int _playerID)
    {
        using (Packet _packet = new Packet((int)ServerPackets.droneEnabled))
        {
            _packet.Write(_playerID);

            SendTCPData(_playerID, _packet);
        }
    }

    /// <summary>드론 비활성화 정보를 클라이언트에 TCP전송</summary>
    public static void DroneEnabledFalse(int _playerID)
    {
        using (Packet _packet = new Packet((int)ServerPackets.droneEnabledFalse))
        {
            _packet.Write(_playerID);

            SendTCPData(_playerID, _packet);
        }
    }

    /// <summary>드론의 위치를 모든 클라이언트에 UDP전송</summary>
    /// <param name="_drone">드론</param>
    public static void DronePosition(Drone _drone)
    {
        using (Packet _packet = new Packet((int)ServerPackets.dronePosition))
        {
            _packet.Write(_drone.GetComponent<ItemSpawner>().spawnerId);
            _packet.Write(_drone.transform.position);

            SendUDPDataToAll(_packet);
        }
    }

    /// <summary>드론의 rotation을 자기자신을 제외한(localplayer에 중복으로 써지는것을 막기위함) 모든 클라이언트에 UDP전송</summary>
    /// <param name="_player">The player whose rotation to update.</param>
    public static void DroneRotation(Drone _drone)
    {
        using (Packet _packet = new Packet((int)ServerPackets.droneRotation))
        {
            _packet.Write(_drone.GetComponent<ItemSpawner>().spawnerId);
            _packet.Write(_drone.transform.rotation);

            SendUDPDataToAll(_packet);
        }
    }

    /// <summary>총알 생성 TCP전송</summary>
    /// <param name="_bullet">총알</param>
    /// <param name="_thrownByPlayer">폭탄을 던진 player</param>
    public static void SpawnBullet(Bullet _bullet, int _thrownByPlayer)
    {
        using (Packet _packet = new Packet((int)ServerPackets.spawnBullet))
        {
            _packet.Write(_bullet.id);
            _packet.Write(_bullet.transform.position);
            _packet.Write(_thrownByPlayer);

            SendTCPDataToAll(_packet);
        }
    }

    /// <summary>총알 위치 UDP전송(여러번 반복전송)</summary>
    /// <param name="_bullet">총알</param>
    public static void BulletPosition(Bullet _bullet)
    {
        using (Packet _packet = new Packet((int)ServerPackets.bulletPosition))
        {
            _packet.Write(_bullet.id);
            _packet.Write(_bullet.transform.position);

            SendUDPDataToAll(_packet);
        }
    }

    /// <summary>총알 충돌 TCP전송</summary>
    /// <param name="_bullet">총알</param>
    public static void BulletCrush(Bullet _bullet)
    {
        using (Packet _packet = new Packet((int)ServerPackets.bulletCrush))
        {
            _packet.Write(_bullet.id);

            SendTCPDataToAll(_packet);
        }
    }

    /// <summary>괴물 공격 성공 TCP전송</summary>
    /// /// <param name="_playerID">괴물 플레이어 id</param>
    /// <param name="_isCreatureAttack">괴물 공격 성공 여부</param>
    public static void CreatureAttackTrue(int _playerID, bool _isCreatureAttack)
    {
        using (Packet _packet = new Packet((int)ServerPackets.creatureAttackTrue))
        {
            _packet.Write(_playerID);
            _packet.Write(_isCreatureAttack);

            SendTCPData(_playerID, _packet);
        }
    }

    /// <summary>플레이어의 앉기정보를 모든 클라이언트에 TCP전송</summary>
    /// <param name="_playerId"></param>
    /// <param name="_sit"></param>
    public static void MotionSit(int _playerId, bool _sit)
    {
        using (Packet _packet = new Packet((int)ServerPackets.motionSit))
        {
            _packet.Write(_playerId);
            _packet.Write(_sit);

            SendTCPDataToAll(_packet);
        }
    }

    /// <summary>플레이어의 공격정보를 모든 클라이언트에 TCP전송</summary>
    /// <param name="_playerId"></param>
    /// <param name="_sit"></param>
    public static void MotionAttack(int _playerId, bool _attack)
    {
        using (Packet _packet = new Packet((int)ServerPackets.motionAttack))
        {
            _packet.Write(_playerId);
            _packet.Write(_attack);

            SendTCPDataToAll(_packet);
        }
    }

    /// <summary>플레이어의 설치정보를 모든 클라이언트에 TCP전송</summary>
    /// <param name="_playerId"></param>
    /// <param name="_install"></param>
    public static void MotionInstall(int _playerId, bool _install)
    {
        using (Packet _packet = new Packet((int)ServerPackets.motionInstall))
        {
            _packet.Write(_playerId);
            _packet.Write(_install);

            SendTCPDataToAll(_packet);
        }
    }

    /// <summary>플레이어의 맞은정보를 모든 클라이언트에 TCP전송</summary>
    /// <param name="_playerId"></param>
    /// <param name="_hit"></param>
    public static void MotionHit(int _playerId, bool _hit)
    {
        using (Packet _packet = new Packet((int)ServerPackets.motionHit))
        {
            _packet.Write(_playerId);
            _packet.Write(_hit);

            SendTCPDataToAll(_packet);
        }
    }

    /// <summary>플레이어의 공간이동정보를 모든 클라이언트에 TCP전송</summary>
    /// <param name="_playerId"></param>
    /// <param name="_teleport"></param>
    public static void MotionTeleportation(int _playerId, bool _teleport)
    {
        using (Packet _packet = new Packet((int)ServerPackets.motionTeleportation))
        {
            _packet.Write(_playerId);
            _packet.Write(_teleport);

            SendTCPDataToAll(_packet);
        }
    }

    /// <summary>치료중인지 판단</summary>
    /// <param name="_playerId">플레이어 ID</param>
    /// <param name="_cure">회복중</param>
    public static void MotionCure(int _playerId, bool _cure)
    {
        using (Packet _packet = new Packet((int)ServerPackets.motionCure))
        {
            _packet.Write(_playerId);
            _packet.Write(_cure);

            SendTCPDataToAll(_packet);
        }
    }

    /// <summary>치료중인지 판단</summary>
    /// <param name="_playerId">플레이어 ID</param>
    /// <param name="_die">회복중</param>
    public static void MotionDie(int _playerId, bool _die)
    {
        using (Packet _packet = new Packet((int)ServerPackets.motionDie))
        {
            _packet.Write(_playerId);
            _packet.Write(_die);

            SendTCPDataToAll(_packet);
        }
    }

    //임시 시간초
    public static void StartTime(float _time)
    {
        using (Packet _packet = new Packet((int)ServerPackets.startTime))
        {
            _packet.Write(_time);
            SendTCPDataToAll(_packet);
        }
    }

    /*
    public static void UDPTest(int _toClient)
    {
        using (Packet _packet = new Packet((int)ServerPackets.udpTest))
        {
            _packet.Write("A test packet for UDP.");

            SendUDPData(_toClient, _packet);
        }
    }*/
    #endregion
}