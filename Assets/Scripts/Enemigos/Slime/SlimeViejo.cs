// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class SlimeViejo : MonoBehaviour
// {
//     public enum EnemyState { Pacifist, Pursuit, AttackClose, AttackLong, Caution }; // Estados del enemigo
//     public EnemyState currentState = EnemyState.Pacifist; // Estado inicial
//     public Animator animator;

//     public float moveSpeed = 3.4f;
//     public float detectionRange = 10f;
//     public float attackRangeClose = 3f;
//     public float attackRangeLong = 6f;
//     public float runSpeedMultiplier = 1.5f; // Multiplicador de velocidad al perseguir al jugador
//     public float cautionTime = 3f; // Duración del estado de precaución
//     public int damage = 1;
    
    // public float attackCooldown = 1.5f;
    // private float attackTimer = 3f;
    // public float heightRange = 0.2f;

//     private Transform player;
//     private Vector3 initialPosition;
//     private float cautionTimer = 0f;

//     private void Start()
//     {
//         player = GameObject.FindGameObjectWithTag("Player").transform;
//         initialPosition = transform.position; // Guardar la posición inicial
//     }

//     private void Update()
//     {
//         PlayerStats playerStats = player.GetComponent<PlayerStats>();

//         if (playerStats != null && playerStats.isDead)
//         {
//             currentState = EnemyState.Pacifist;
//             animator.SetBool("IdleNormal", true);
//             animator.SetBool("IdleBattle", false);
//             return;
//         }


//         float distanceToPlayer = Vector3.Distance(transform.position, player.position);

//         if (attackTimer > 0)
//         {
//             attackTimer -= Time.deltaTime;
//         }

//         switch (currentState)
//         {
//             case EnemyState.Pacifist:
//                 animator.SetBool("IdleNormal", true);
//                 animator.SetBool("IdleBattle", false);
//                 // Si el jugador está dentro del rango de detección, cambiar al modo de persecución                
//                 if (distanceToPlayer <= detectionRange)
//                 {
//                     currentState = EnemyState.Pursuit;
//                 }
//                 break;

//             case EnemyState.Pursuit:
//                 animator.SetBool("IdleNormal", false);
//                 animator.SetBool("IdleBattle", false);
                
//                 Vector3 directionToPlayer = (player.position - transform.position).normalized;
//                 directionToPlayer.y = 0;

//                 // Determinar la velocidad de movimiento (base o aumentada)
//                 float currentMoveSpeed = moveSpeed;
//                 if (distanceToPlayer > detectionRange / 2f) // Si el jugador está lejos
//                 {
//                     currentMoveSpeed *= runSpeedMultiplier; // Aumentar la velocidad
//                     animator.SetBool("RunFWD", true);
//                     animator.SetBool("WalkFWD", false);
//                 }
//                 else
//                 {
//                     animator.SetBool("WalkFWD", true);
//                     animator.SetBool("RunFWD", false);
//                 }

//                 // Mover al enemigo en dirección al jugador
//                 transform.position += directionToPlayer * currentMoveSpeed * Time.deltaTime;

//                 // Hacer que el enemigo siempre mire hacia el jugador, excepto en el modo pacífico
//                 if (currentState != EnemyState.Pacifist)
//                 {
//                     transform.LookAt(player.position);
//                 }

//                 // Si el jugador está fuera del rango de persecución, volver al modo pacífico
//                 if (distanceToPlayer > detectionRange)
//                 {
//                     currentState = EnemyState.Caution;
//                     cautionTimer = cautionTime;
//                 }
//                 // Si el jugador está dentro del rango de ataque cercano, cambiar al modo de ataque cercano
//                 else if (distanceToPlayer <= attackRangeClose)
//                 {
//                     currentState = EnemyState.AttackClose;
//                 }
//                 break;

//             case EnemyState.AttackClose:
//                 animator.SetBool("Attack02", false);
//                 animator.SetBool("Attack01", true);
//                 animator.SetBool("WalkFWD", false);

//                 Attack();

//                 // Si el jugador está fuera del rango de ataque cercano pero dentro del rango de ataque lejano, cambiar al modo de ataque lejano
//                 if (distanceToPlayer > attackRangeClose && distanceToPlayer <= attackRangeLong)
//                 {
//                     animator.SetBool("Attack01", false);
//                     currentState = EnemyState.AttackLong;
//                 }
//                 break;

//             case EnemyState.AttackLong:
//                 animator.SetBool("Attack01", false);
//                 animator.SetBool("Attack02", true);

//                 Attack();

//                 // Si el jugador está dentro del rango de ataque cercano, volver al modo de ataque cercano
//                 if (distanceToPlayer <= attackRangeClose)
//                 {
//                     animator.SetBool("Attack02", false);
//                     currentState = EnemyState.AttackClose;
//                 }
//                 // Si el jugador está fuera del rango de ataque lejano, volver al modo de persecución
//                 else if (distanceToPlayer > attackRangeLong)
//                 {
//                     animator.SetBool("Attack02", false);
//                     currentState = EnemyState.Pursuit;
//                 }
//                 break;

//             case EnemyState.Caution:
//                 animator.SetBool("IdleBattle", true);
//                 animator.SetBool("RunFWD", false);
//                 cautionTimer -= Time.deltaTime;
//                 if (cautionTimer <= 0)
//                 {
//                     currentState = EnemyState.Pacifist;
//                 }
//                 break;    
//         }
//     }

//     private void Attack()
//     {
//         // Solo atacar si el jugador está frente al enemigo y si el temp llegó a 0
//         if (attackTimer <= 0 && IsPlayerInFront())
//         {
//             PlayerStats playerStats = player.GetComponent<PlayerStats>();

//             if (playerStats != null)
//             {
//                 playerStats.TakeDamage(damage);
//             }

//             attackTimer = attackCooldown;
//         }
//     }

//     private bool IsPlayerInFront()
//     {
//         Vector3 directionToPlayer = (player.position - transform.position).normalized;

//         // Producto escalar entre la dirección hacia el jugador y el frente del enemigo
//         float dotProduct = Vector3.Dot(transform.forward, directionToPlayer);
//         // Diferencia de altura entre el enemigo y el jugador
//         float heightDifference = Mathf.Abs(player.position.y - transform.position.y);

//         return dotProduct > 0.6f && heightDifference >= -heightRange && heightDifference <= heightRange;
//     }
// }