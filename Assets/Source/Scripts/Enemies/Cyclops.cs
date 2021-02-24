using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cyclops : BasicEnemy
{
    protected override void OnPlayerEnteredAttackZone(PlayerController player)
    {
        StartAttacking(player);
    }

    protected override void OnPlayerExitedAttackZone(PlayerController player)
    {
        StopAttacking();
    }
}
