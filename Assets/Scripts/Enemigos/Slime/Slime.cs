using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour
{
    public enum EnemyState { Null, Pacifist, Pursuit, AttackClose, Caution }
    public EnemyState currentState;

    private IEnemyState currentStateBehavior;
    private Dictionary<EnemyState, IEnemyState> stateBehaviors;

    public Transform player;
    public Animator animator;

    public float moveSpeed = 3.5f;
    public float detectionRange = 30f;
    public float attackRangeClose = 3f;
    public float attackRangeLong = 6f;
    public float runSpeedMultiplier = 1.5f;
    public float cautionTime = 3f; // Duración del estado de precaución
    public int damage = 1;
    public float attackCooldown = 2f;

    public void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        stateBehaviors = new Dictionary<EnemyState, IEnemyState>
        {
            { EnemyState.Pacifist, new PacifistState(this) },
            { EnemyState.Pursuit, new PursuitState(this) },
            { EnemyState.AttackClose, new AttackCloseState(this) },
            { EnemyState.Caution, new CautionState(this) }
        };

        SetState(EnemyState.Null);

        StartCoroutine(StartEnemyState(0.01f));
    }

    public void Update()
    {
        currentStateBehavior?.UpdateState();
    }

    public void SetState(EnemyState newState)
    {
        if (currentState != newState)
        {
            currentState = newState;
            currentStateBehavior = stateBehaviors[newState];
            currentStateBehavior.EnterState();
        }
    }

    private IEnumerator StartEnemyState(float delay)
    {
        yield return new WaitForSeconds(delay);

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if(distanceToPlayer > detectionRange)
        {
            SetState(EnemyState.Pacifist);
        }
        else if(distanceToPlayer <= detectionRange)
        {
            SetState(EnemyState.Pursuit);
        }
    }
}