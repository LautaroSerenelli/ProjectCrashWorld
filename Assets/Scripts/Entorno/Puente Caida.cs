using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuenteCaida : MonoBehaviour
{
    public float TiempoParaCaer;
    public float IntensidadVibracion;
    public float DuracionVibracion;
    public float VelocidadCaida;
    public float VelocidadSubida;
    public float AlturaCaida;
    public float TiempoReaparicion;
    public bool Reaparecer = true;

    private bool estaPisando = false;
    private bool estaVibrando = false;
    private float TiempoTranscurrido = 0f;

    private Vector3 PosicionOriginal;

    void Start()
    {
        PosicionOriginal = transform.position;
    }

    void OnTriggerEnter (Collider other)
    {
        if (other.CompareTag ("Player") && !estaPisando)
        {
            estaPisando = true;
            estaVibrando = true;
            TiempoTranscurrido = 0f;
        }
    }

    void Update()
    {
        if (estaPisando)
        {
            TiempoTranscurrido += Time.deltaTime;

            if (estaVibrando && TiempoTranscurrido <= DuracionVibracion)
            {
                VibrarTablon();
            }
            else if (estaVibrando && TiempoTranscurrido > DuracionVibracion)
            {
                estaVibrando = false;
                transform.position = PosicionOriginal;
            }

            if (TiempoTranscurrido >= TiempoParaCaer)
            {
                CaerTablon();
            }
        }
    }

    void VibrarTablon()
    {
        Vector3 vibracion = new Vector3(
            Random.Range(-IntensidadVibracion, IntensidadVibracion), 0,
            Random.Range(-IntensidadVibracion, IntensidadVibracion)
        );

        transform.position = PosicionOriginal + vibracion;
    }

    void CaerTablon()
    {
        transform.Translate(Vector3.down * VelocidadCaida * Time.deltaTime);

        if (transform.position.y < PosicionOriginal.y - AlturaCaida)
        {
            if (Reaparecer)
            {
                Invoke("SubirTablon", TiempoReaparicion);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    void SubirTablon()
    {
        if (gameObject.CompareTag("SueloDesmoronableReaparecer"))
        {
            transform.position = Vector3.MoveTowards(transform.position,PosicionOriginal, VelocidadSubida * Time.deltaTime);

            if (transform.position == PosicionOriginal)
            {
                ResetearTablon();
            }
        }
    }

    void ResetearTablon()
    {
        estaPisando = false;
        estaVibrando = false;
        TiempoTranscurrido = 0f;
        transform.position = PosicionOriginal;
    }
}