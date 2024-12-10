using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruits : MonoBehaviour
{
    private Rigidbody fruitRb;
    private Collider fruitCollider;
    public float decelerationDuration = 1f;

    public Collider playerCollider;
    public LayerMask groundLayer;

    private ItemPool itemPool;

    public void Awake()
    {
        fruitRb = GetComponent<Rigidbody>();
        fruitCollider = GetComponent<Collider>();

        itemPool = FindObjectOfType<ItemPool>();

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        playerCollider = playerObject.GetComponent<Collider>();

        Physics.IgnoreCollision(playerCollider, fruitCollider, true);
    }

    public void Launch()
    {
        fruitCollider.isTrigger = true;
        Vector3 jumpForce = new Vector3(
                        Random.Range(-1f, 1f),
                        Random.Range(3f, 8f),
                        Random.Range(-1f, 1f)
                    );
        
        fruitRb.velocity = jumpForce;

        StartCoroutine(ReactivarTrigger());

        StartCoroutine(ReturnToPoolAfterTime(20f));
    }

    IEnumerator ReactivarTrigger()
    {
        yield return new WaitForSeconds(0.1f);
        fruitRb.useGravity = true;
        yield return new WaitForSeconds(0.5f);
        fruitCollider.isTrigger = false;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            fruitRb.velocity = Vector3.zero;
            fruitRb.useGravity = false;
            fruitCollider.isTrigger = true;
            fruitRb.constraints = RigidbodyConstraints.FreezeRotation;
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Fruits"))
        {
            Collider otherCollider = collision.gameObject.GetComponent<Collider>();

            Physics.IgnoreCollision(otherCollider, fruitCollider, true);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Crates"))
        {
            Collider otherCollider = collision.gameObject.GetComponent<Collider>();

            Physics.IgnoreCollision(otherCollider, fruitCollider, true);
        }
    }

    IEnumerator ReturnToPoolAfterTime(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        itemPool.ReturnItemToPool(gameObject);
    }
}