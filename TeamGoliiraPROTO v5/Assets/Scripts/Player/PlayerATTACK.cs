using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerATTACK : MonoBehaviour
{
    PlayerFSMManager _manager;
    private void Awake()
    {
        _manager = GetComponent<PlayerFSMManager>();
    }
    public void AttackCheck()
    {
        var hitTarget = GameLib.SimpleDamageProcess(transform, _manager.Stat.AttackRange,
            "Monster", _manager.Stat);
        if (hitTarget != null) _manager._lastAttack = hitTarget;
    }
}
