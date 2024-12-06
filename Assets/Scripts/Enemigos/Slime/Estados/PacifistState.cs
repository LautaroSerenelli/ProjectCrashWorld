using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacifistState : IEnemyState
{
    private Slime slime;

    public PacifistState(Slime slime)
    {
        this.slime = slime;
    }

    public void EnterState()
    {
        slime.animator.SetBool("IdleNormal", true);
        slime.animator.SetBool("IdleBattle", false);
    }

    public void UpdateState()
    {
        float distanceToPlayer = Vector3.Distance(slime.transform.position, slime.player.position);
        if (distanceToPlayer <= slime.detectionRange)
        {
            slime.SetState(Slime.EnemyState.Pursuit);
        }
    }
}