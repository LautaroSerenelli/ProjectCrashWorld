using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PursuitState : IEnemyState
{
    private Slime slime;

    public PursuitState(Slime slime)
    {
        this.slime = slime;
    }

    public void EnterState()
    {
        slime.animator.SetBool("IdleNormal", false);
        slime.animator.SetBool("IdleBattle", false);
    }

    public void UpdateState()
    {
        float distanceToPlayer = Vector3.Distance(slime.transform.position, slime.player.position);
        Vector3 directionToPlayer = (slime.player.position - slime.transform.position).normalized;

        if (distanceToPlayer > slime.detectionRange / 2f) // Lejos
        {
            slime.animator.SetBool("RunFWD", true);
            slime.animator.SetBool("WalkFWD", false);
            slime.transform.position += new Vector3(directionToPlayer.x, 0f, directionToPlayer.z) * slime.moveSpeed * slime.runSpeedMultiplier * Time.deltaTime;
        }
        else // Cerca
        {
            slime.animator.SetBool("RunFWD", false);
            slime.animator.SetBool("WalkFWD", true);
            slime.transform.position += new Vector3(directionToPlayer.x, 0f, directionToPlayer.z) * slime.moveSpeed * Time.deltaTime;
        }

        slime.transform.LookAt(slime.player.position);

        if (distanceToPlayer > slime.detectionRange)
        {
            slime.SetState(Slime.EnemyState.Caution);
        }
        else if (distanceToPlayer <= slime.attackRangeClose)
        {
            slime.SetState(Slime.EnemyState.AttackClose);
        }
    }
}