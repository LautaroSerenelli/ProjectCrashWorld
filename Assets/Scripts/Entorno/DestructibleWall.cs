using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleWall : MonoBehaviour
{
    public GameObject EfectoDestruccion;

    public void RomperPared()
    {
        if (EfectoDestruccion != null)
        {
            Instantiate(EfectoDestruccion, transform.position, transform.rotation);
        }

        gameObject.SetActive (false);
    }
}