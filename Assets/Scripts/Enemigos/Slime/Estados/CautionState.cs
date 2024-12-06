using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CautionState : IEnemyState
{
    private Slime slime;
    private float cautionTimer;

    public CautionState(Slime slime)
    {
        this.slime = slime;
    }

    public void EnterState()
    {
        cautionTimer = slime.cautionTime;

        slime.animator.SetBool("IdleBattle", false);
    }

    public void UpdateState()
    {
        float distanceToPlayer = Vector3.Distance(slime.transform.position, slime.player.position);

        cautionTimer -= Time.deltaTime;

        if (distanceToPlayer <= slime.detectionRange)
        {
            slime.SetState(Slime.EnemyState.Pursuit);
            return;
        }

        if (cautionTimer <= 0)
        {
            slime.SetState(Slime.EnemyState.Pacifist);
            return;
        }

        Vector3 directionToPlayer = (slime.player.position - slime.transform.position).normalized;

        slime.transform.position += directionToPlayer * slime.moveSpeed * Time.deltaTime;

        slime.transform.LookAt(slime.player.position);
    }
}