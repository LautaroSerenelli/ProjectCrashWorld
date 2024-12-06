using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCloseState : IEnemyState
{
    private Slime slime;
    private float attackTimer;
    private bool isPreparingAttack = false;
    private bool isAttacking = false;
    private bool isLongRangeAttack = false;

    public AttackCloseState(Slime slime)
    {
        this.slime = slime;
    }

    public void EnterState()
    {
        attackTimer = slime.attackCooldown;
        isPreparingAttack = false;
        isAttacking = false;
        isLongRangeAttack = false;

        slime.animator.SetBool("IdleBattle", true);
        slime.animator.SetBool("RunFWD", false);
        slime.animator.SetBool("WalkFWD", false);
    }

    public void UpdateState()
    {
        float distanceToPlayer = Vector3.Distance(slime.transform.position, slime.player.position);

        if (distanceToPlayer > slime.attackRangeLong)
        {
            slime.SetState(Slime.EnemyState.Pursuit);
            return;
        }

        if (distanceToPlayer > slime.attackRangeClose && distanceToPlayer <= slime.attackRangeLong)
        {
            isLongRangeAttack = true;
        }
        else if (distanceToPlayer <= slime.attackRangeClose)
        {
            isLongRangeAttack = false;
        }

        if (!isPreparingAttack && !isAttacking)
        {
            // Sigue al jugador con la mirada durante IdleBattle
            FollowPlayerWithLook();

            // Cuenta atrás para iniciar el ataque
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0)
            {
                PrepareAttack();
            }
        }
        else if (isAttacking)
        {
            if (slime.animator.GetCurrentAnimatorStateInfo(0).IsName(isLongRangeAttack ? "Attack02" : "Attack01") &&
                slime.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                ResetAttack();
            }
        }
    }

    private void FollowPlayerWithLook()
    {
        if (!isAttacking)
        {
            Vector3 directionToPlayer = (slime.player.position - slime.transform.position).normalized;
            slime.transform.rotation = Quaternion.Slerp(slime.transform.rotation,
                Quaternion.LookRotation(directionToPlayer),
                Time.deltaTime);
        }
    }

    private void PrepareAttack()
    {
        isPreparingAttack = true;

        slime.StartCoroutine(WaitBeforeAttack(0.5f));
    }

    private IEnumerator WaitBeforeAttack(float delay)
    {
        yield return new WaitForSeconds(delay);
        
        isAttacking = true;
        slime.animator.SetBool("IdleBattle", false);
        Debug.Log(isLongRangeAttack);
        slime.animator.SetTrigger(isLongRangeAttack ? "Attack02" : "Attack01");
    }

    private void ResetAttack()
    {
        isAttacking = false;
        attackTimer = slime.attackCooldown;
        slime.animator.SetBool("IdleBattle", true);
        EnterState();
    }
}