using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class ClientHandle : MonoBehaviour
{
    /// <summary>서버로 부터 welcome data를 받음</summary>
    public static void Welcome(Packet _packet)
    {
        string _msg = _packet.ReadString();
        int _myId = _packet.ReadInt();

        //Debug.Log($"Message from server: {_msg}");
        Client.instance.myId = _myId;
        //ClientSend.WelcomeReceived();

        ///Client.instance.tcp.socket.Client : 클라이언트의 소켓
        ///클라이언트의 소켓의 endpoint의 포트를 통해 udp 인스턴스에 연결
        Client.instance.udp.Connect(((IPEndPoint)Client.instance.tcp.socket.Client.LocalEndPoint).Port);

        ClientSend.WelcomeReceived();
        ClientSend.GameStart();
    }

    /// <summary>서버로 부터 welcome data를 받음</summary>
    public static void Error(Packet _packet)
    {
        string _msg = _packet.ReadString();

        //Debug.Log($"Error _ Message from server: {_msg}");
    }

    public static void SpawnPlayer(Packet _packet)
    {
        int _id = _packet.ReadInt();
        int _userType = _packet.ReadInt();
        Vector3 _position = _packet.ReadVector3();
        Quaternion _rotation = _packet.ReadQuaternion();

        GameManager.instance.SpawnPlayer(_id, _userType, _position, _rotation);
    }
    /// <summary>player position을 담은 패킷을 받음</summary>
    /// <param name="_packet"></param>
    public static void PlayerPosition(Packet _packet)
    {
        int _id = _packet.ReadInt();
        Vector3 _position = _packet.ReadVector3();
        bool _walk = _packet.ReadBool();
        bool _run = _packet.ReadBool();

        if (GameManager.players.TryGetValue(_id, out PlayerManager _player))
        {
            _player.transform.position = _position;
            _player.animator.SetBool("Walk", _walk);
            _player.animator.SetBool("Run", _run);
            //Debug.Log($"walk : {_player.animator.GetBool("Walk")}, run : {_player.animator.GetBool("Run")}, stand : {!_player.animator.GetBool("Walk") && !_player.animator.GetBool("Run")}");

            if (_id == Client.instance.myId)
            {
                //UIManager.instance.fisrtFloorPlayer.transform.position = _position + UIManager.instance.position_UI_LightTrap[0];
                UIManager.instance.fisrtFloorPlayer.GetComponent<RectTransform>().localPosition = new Vector3(_position.x * UIManager.instance.objectUIRatio.x + 50,
                    _position.y * UIManager.instance.objectUIRatio.y, _position.z * UIManager.instance.objectUIRatio.z) + UIManager.instance.position_UI_LightTrap[0];

                if (_player.GetComponent<AudioSource>().clip != null && !_player.GetComponent<PlayerManager>().isInstalling && _player.playerType == PlayerType.HUMAN)
                {
                    if(_player.GetComponent<AudioSource>().clip != _player.footStepSound)
                    {
                        _player.GetComponent<AudioSource>().clip = _player.footStepSound;
                    }
                    if ((_player.GetComponent<AudioSource>().clip == _player.footStepSound && _player.GetComponent<AudioSource>().isPlaying) && !_walk && !_run
                            || _player.GetComponent<AudioSource>().clip != _player.footStepSound)
                    {
                        _player.GetComponent<AudioSource>().Stop();
                    }
                    else if (_run && (_player.GetComponent<AudioSource>().clip == _player.footStepSound && !_player.GetComponent<AudioSource>().isPlaying)
                                || _player.GetComponent<AudioSource>().clip != _player.footStepSound)
                    {
                        _player.GetComponent<AudioSource>().clip = _player.footStepSound;
                        _player.GetComponent<AudioSource>().pitch = 1f;
                        _player.GetComponent<AudioSource>().Play();
                    }
                    else if (_walk && (_player.GetComponent<AudioSource>().clip == _player.footStepSound && !_player.GetComponent<AudioSource>().isPlaying)
                              || _player.GetComponent<AudioSource>().clip != _player.footStepSound)
                    {
                        _player.GetComponent<AudioSource>().clip = _player.footStepSound;
                        _player.GetComponent<AudioSource>().pitch = 0.5f;
                        _player.GetComponent<AudioSource>().Play();
                    }
                }
                else if(_player.GetComponent<AudioSource>().clip != null && !_player.GetComponent<PlayerManager>().isInstalling && _player.playerType == PlayerType.CREATURE)
                {
                    //괴물 걷는 소리
                }
            }

        }
    }
    /// <summary>player Rotation을 담은 패킷을 받음</summary>
    /// <param name="_packet"></param>
    public static void PlayerRotation(Packet _packet)
    {
        int _id = _packet.ReadInt();
        Quaternion _rotation = _packet.ReadQuaternion();

        if (GameManager.players.TryGetValue(_id, out PlayerManager _player))
        {
            _player.transform.rotation = _rotation;
        }
    }
    
    /// <summary>연결 해제 > dictionary 에 남아있는 player 정보 삭제</summary>
    /// <param name="_packet"></param>
    public static void PlayerDisconnected(Packet _packet)
    {
        int _id = _packet.ReadInt();

        Destroy(GameManager.players[_id].gameObject);
        GameManager.players.Remove(_id);
    }

    /// <summary>패킷에 담긴 HP정보 update</summary>
    /// <param name="_packet"></param>
    public static void PlayerHP(Packet _packet)
    {
        int _id = _packet.ReadInt();
        float _health = _packet.ReadFloat();

        GameManager.players[_id].SetHealth(_health);
    }

    /// <summary>패킷에 담긴 리스폰정보 update</summary>
    /// <param name="_packet"></param>
    public static void PlayerRespawned(Packet _packet)
    {
        int _id = _packet.ReadInt();

        GameManager.players[_id].Respawn();
    }


    /// <summary>패킷에 담긴 아이템 스폰정보(id) update</summary>
    /// <param name="_packet"></param>
    public static void ItemSpawned(Packet _packet)
    {
        int _spawnerId = _packet.ReadInt();
        Vector3 _spawnerPosition = _packet.ReadVector3();
        string _tag = _packet.ReadString();
        ItemType _type = (ItemType)ItemType.Parse(typeof(ItemType), _tag);

        bool _hasItem = _packet.ReadBool();
        bool _modelEnabled = _packet.ReadBool();

        GameManager.instance.CreateItemSpawner(_spawnerId, _spawnerPosition, _type, _hasItem, _modelEnabled);
    }

    /// <summary>패킷에 담긴 trap 아이템정보 세팅</summary>
    /// <param name="_packet"></param>
    public static void ItemSetTrap(Packet _packet)
    {
        int _spawnerId = _packet.ReadInt();
        int _floor = _packet.ReadInt();

        GameManager.instance.SetTrap(_spawnerId, _floor);
    }

    /// <summary>아이템 획득정보 update</summary>
    /// <param name="_packet"></param>
    public static void ItemPickedUp(Packet _packet)
    {
        int _spawnerId = _packet.ReadInt();
        int _playerId = _packet.ReadInt();

        GameManager.instance.SaveItemToPlayer(GameManager.itemSpawners[_spawnerId], GameManager.players[_playerId]);
    }

    /// <summary>아이템 버리기정보 update</summary>
    /// <param name="_packet"></param>
    public static void ItemThrow(Packet _packet)
    {
        int _spawnerId = _packet.ReadInt();
        int _byPlayer = _packet.ReadInt();
        Vector3 _position = _packet.ReadVector3();

        GameManager.itemSpawners[_spawnerId].ItemThrow(_position);
    }

    /// <summary>아이템 들기정보 update</summary>
    /// <param name="_packet"></param>
    public static void ItemGrab(Packet _packet)
    {
        int _spawnerId = _packet.ReadInt();
        int _byPlayer = _packet.ReadInt();
        Vector3 _position = _packet.ReadVector3();

        GameManager.itemSpawners[_spawnerId].ItemGrab(GameManager.players[_byPlayer], _position);
    }

    public static void KeyChange(Packet _packet)
    {
        // 패킷에서 받아오는 정보
        int _playerId = _packet.ReadInt();
        
        GameManager.players[_playerId].GetComponent<PlayerController>().KeyChange();
    }
    public static void InstallEMP(Packet _packet)
    {
        int _spawnerId = _packet.ReadInt();
        Vector3 _position = _packet.ReadVector3();
        int _playerId = _packet.ReadInt();

        GameManager.instance.empCount++;
        GameManager.itemSpawners[_spawnerId].InstallEMP(_position);
        GameManager.players[_playerId].isInstalling = false;
        GameManager.players[_playerId].PlayerInstallingSound(GameManager.players[_playerId].isInstalling);
    }

    /// <summary>LightTrap 설치</summary>
    /// <param name="_packet"></param>
    public static void InstallTrap(Packet _packet)
    {
        int _spawnerId = _packet.ReadInt();
        Vector3 _position = _packet.ReadVector3();
        int _floor = _packet.ReadInt();
        int _playerId = _packet.ReadInt();

        GameManager.itemSpawners[_spawnerId].InstallTrap(_position, _floor);
        GameManager.players[_playerId].isInstalling = false;
        GameManager.players[_playerId].PlayerInstallingSound(GameManager.players[_playerId].isInstalling);
    }

    /// <summary>패킷에 담긴 폭탄 생성정보(ID,position,던진player)를 통해 폭탄생성</summary>
    /// <param name="_packet"></param>
    public static void SpawnProjectile(Packet _packet)
    {
        int _projectileId = _packet.ReadInt();
        Vector3 _position = _packet.ReadVector3();
        int _thrownByPlayer = _packet.ReadInt();

        GameManager.instance.SpawnProjectile(_projectileId, _position);
        GameManager.players[_thrownByPlayer].playerItem.item_number1.GetComponent<Gun>().currentBattery -= 5;
        UIManager.instance.currentBulletText.text = string.Format(" {0:} ", GameManager.players[_thrownByPlayer].playerItem.item_number1.GetComponent<Gun>().currentBattery);
    }

    /// <summary>패킷에 담긴 정보를 통해(position) 폭탄 위치 update</summary>
    /// <param name="_packet"></param>
    public static void ProjectilePosition(Packet _packet)
    {
        int _projectileId = _packet.ReadInt();
        Vector3 _position = _packet.ReadVector3();

        //dictionary에 id가 같은 폭탄이 있는지 확인 후 position update
        if (GameManager.projectiles.TryGetValue(_projectileId, out ProjectileManager _projectile))
        {
            _projectile.transform.position = _position;
        }
    }

    /// <summary>패킷에 담긴 정보(id, position)를 통해 폭탄 폭발</summary>
    /// <param name="_packet"></param>
    public static void ProjectileExploded(Packet _packet)
    {
        int _projectileId = _packet.ReadInt();
        Vector3 _position = _packet.ReadVector3();

        if (GameManager.projectiles.ContainsKey(_projectileId))
        {
            GameManager.projectiles[_projectileId].Explode(_position);
        }       
    }

    /*
    /// <summary>플레이어 은폐</summary>
    /// <param name="_packet"></param>
    public static void Hide(Packet _packet)
    {
        int _id = _packet.ReadInt();
        Vector3 _position = _packet.ReadVector3();

        GameManager.players[_id].GetComponent<PlayerManager>().Hide(_position);
    }
    */
    public static void DroneEnabled(Packet _packet)
    {
        int _playerId = _packet.ReadInt();

        GameManager.players[_playerId].playerItem.GrabItem.GetComponent<Drone>().Moving();
        GameManager.players[_playerId].playerItem.GrabItem.GetComponent<Drone>().GetComponent<AudioSource>().clip
            = GameManager.players[_playerId].playerItem.GrabItem.GetComponent<Drone>().droneMovingSound;
        GameManager.players[_playerId].playerItem.GrabItem.GetComponent<Drone>().GetComponent<AudioSource>().Play();
        GameManager.players[_playerId].playerItem.GrabItem.GetComponent<Drone>().GetComponent<AudioSource>().loop = true;
    }

    public static void DroneEnabledFalse(Packet _packet)
    {
        int _playerId = _packet.ReadInt();

        GameManager.players[_playerId].playerItem.GrabItem.GetComponent<Drone>().Stop();
        GameManager.players[_playerId].playerItem.GrabItem.GetComponent<Drone>().GetComponent<AudioSource>().Stop();
    }

    /// <summary>드론 position을 담은 패킷을 받음</summary>
    /// <param name="_packet"></param>
    public static void DronePosition(Packet _packet)
    {
        int _id = _packet.ReadInt();
        Vector3 _position = _packet.ReadVector3();

        GameManager.itemSpawners[_id].transform.position = _position;
    }

    /// <summary>드론 Rotation을 담은 패킷을 받음</summary>
    /// <param name="_packet"></param>
    public static void DroneRotation(Packet _packet)
    {
        int _id = _packet.ReadInt();
        Quaternion _rotation = _packet.ReadQuaternion();

        GameManager.itemSpawners[_id].transform.rotation = _rotation;
    }

    
    /// <summary>패킷에 담긴 총알 생성정보(ID,position,던진player)를 통해 총알 생성</summary>
    /// <param name="_packet"></param>
    public static void SpawnBullet(Packet _packet)
    {
        int _bulletId = _packet.ReadInt();
        Vector3 _position = _packet.ReadVector3();
        int _thrownByPlayer = _packet.ReadInt();

        GameManager.instance.SpawnBullet(_bulletId, _position);
        GameManager.players[_thrownByPlayer].playerItem.item_number1.GetComponent<Gun>().currentBattery--;
        UIManager.instance.currentBulletText.text = string.Format(" {0:} ", GameManager.players[_thrownByPlayer].playerItem.item_number1.GetComponent<Gun>().currentBattery);
    }

    /// <summary>패킷에 담긴 정보를 통해(position) 총알 위치 update</summary>
    /// <param name="_packet"></param>
    public static void BulletPosition(Packet _packet)
    {
        int _bulletId = _packet.ReadInt();
        Vector3 _position = _packet.ReadVector3();

        //dictionary에 id가 같은 총알이 있는지 확인 후 position update
        if (GameManager.bullets.TryGetValue(_bulletId, out BulletManager _bullet))
        {
            _bullet.transform.position = _position;
        }
    }

    /// <summary>패킷에 담긴 정보(id)를 통해 총알 충돌</summary>
    /// <param name="_packet"></param>
    public static void BulletCrush(Packet _packet)
    {
        int _bulletId = _packet.ReadInt();

        if(GameManager.bullets.ContainsKey(_bulletId))
        {
            GameManager.bullets[_bulletId].Crush();
        }
        
    }

    /// <summary> 괴물 공격 성공시 처리</summary>
    /// <param name="_packet"></param>
    public static void CreatureAttackTrue(Packet _packet)
    {
        int _playerID = _packet.ReadInt();
        bool _isCreatureAttack = _packet.ReadBool();

        GameManager.players[_playerID].isCreatureAttack = _isCreatureAttack;

    }

    /// <summary>player 앉기 정보를 담은 패킷을 받음</summary>
    /// <param name="_packet"></param>
    public static void MotionSit(Packet _packet)
    {
        int _id = _packet.ReadInt();
        bool _sit = _packet.ReadBool();

        if (GameManager.players.TryGetValue(_id, out PlayerManager _player))
        {
            _player.animator.SetBool("Sit", _sit);
        }
    }

    /// <summary>player 공격 정보를 담은 패킷을 받음</summary>
    /// <param name="_packet"></param>
    public static void MotionAttack(Packet _packet)
    {
        int _id = _packet.ReadInt();
        bool _attack = _packet.ReadBool();

        if (GameManager.players.TryGetValue(_id, out PlayerManager _player))
        {
            _player.animator.SetBool("Attack", _attack);
        }
    }
    /// <summary>player 설치 정보를 담은 패킷을 받음</summary>
    /// <param name="_packet"></param>
    public static void MotionInstall(Packet _packet)
    {
        int _id = _packet.ReadInt();
        bool _install = _packet.ReadBool();

        if (GameManager.players.TryGetValue(_id, out PlayerManager _player))
        {
            _player.animator.SetBool("Install", _install);
        }
    }

    /// <summary>player 맞은정보를 담은 패킷을 받음</summary>
    /// <param name="_packet"></param>
    public static void MotionHit(Packet _packet)
    {
        int _id = _packet.ReadInt();
        bool _hit = _packet.ReadBool();

        if (GameManager.players.TryGetValue(_id, out PlayerManager _player))
        {
            _player.animator.SetBool("HitReaction", _hit);
        }
    }

    /// <summary>player 공간이동정보를 담은 패킷을 받음</summary>
    /// <param name="_packet"></param>
    public static void MotionTeleportation(Packet _packet)
    {
        int _id = _packet.ReadInt();
        bool _teleport = _packet.ReadBool();

        if (GameManager.players.TryGetValue(_id, out PlayerManager _player))
        {
            _player.animator.SetBool("SkillTeleportation", _teleport);
        }
    }

    /// <summary>player 치료 정보를 담은 패킷을 받음</summary>
    /// <param name="_packet"></param>
    public static void MotionCure(Packet _packet)
    {
        int _id = _packet.ReadInt();
        bool _cure = _packet.ReadBool();

        if (GameManager.players.TryGetValue(_id, out PlayerManager _player))
        {
            _player.animator.SetBool("Cure", _cure);
        }
    }

    /// <summary>player 죽음 정보를 담은 패킷을 받음</summary>
    /// <param name="_packet"></param>
    public static void MotionDie(Packet _packet)
    {
        int _id = _packet.ReadInt();
        bool _die = _packet.ReadBool();

        if (GameManager.players.TryGetValue(_id, out PlayerManager _player))
        {
            _player.animator.SetBool("Die", _die);
            UIManager.instance.endImageUI.gameObject.SetActive(true);
            _player.GetComponent<AudioSource>().clip = _player.endSound;
            _player.GetComponent<AudioSource>().Play();
            for (int i = 0; i < UIManager.instance.HPBarUI.Length; i++)
            {
                UIManager.instance.HPBarUI[i].gameObject.SetActive(false);
            }
            for (int i = 0; i < UIManager.instance.skillImageUI.Length; i++)
            {
                UIManager.instance.skillImageUI[i].gameObject.SetActive(false);
                UIManager.instance.itemImageUI[i].gameObject.SetActive(false);
                UIManager.instance.itemCountText[i].gameObject.SetActive(false);
            }
            for (int i = 0; i < UIManager.instance.creatureKey.Length; i++)
            {
                UIManager.instance.creatureKey[i].gameObject.SetActive(false);
            }
            GameObject.Find("Aim").gameObject.SetActive(false);

            

            if (_player.playerType == Client.playerType)
            {
                //패배
                UIManager.instance.endImageUI.sprite = UIManager.instance.endImage[(int)UIManager.EndType.DEFEAT];
            }
            else
            {
                //승리
                UIManager.instance.endImageUI.sprite = UIManager.instance.endImage[(int)UIManager.EndType.VICTORY];
            }
        }
    }

    /*
    /// <summary>서버로 부터 UDPTest data를 받음</summary>
    public static void UDPTest(Packet _packet)
    {
        string _msg = _packet.ReadString();

        Debug.Log($"Received packet via UDP. Contains message: {_msg}");
        ClientSend.UDPTestReceived();
    }*/
}
