using UnityEngine;
using UnityEngine.AI;

public class EnemigoIA : MonoBehaviour
{
    [Header("Referencias")]
    public Transform jugador;

    [Header("Movimiento")]
    public float velocidad        = 3.5f;
    public float distanciaAtaque  = 2f;
    public float distanciaVision  = 15f;

    [Header("Combate")]
    public float danioAtaque      = 10f;
    public float tiempoEntreAtaques = 1.5f;

    NavMeshAgent agente;
    float        tiempoUltimoAtaque = 0f;
    bool         jugadorEnRango     = false;

    void Start()
    {
        agente = GetComponent<NavMeshAgent>();

        if (agente != null)
        {
            agente.speed = velocidad;
        }

        if (jugador == null)
        {
            GameObject go = GameObject.FindWithTag("Player");
            if (go != null)
            {
                jugador = go.transform;
            }
        }
    }

    void Update()
    {
        if (jugador == null) return;
        if (agente  == null) return;

        float distancia = Vector3.Distance(transform.position, jugador.position);

        if (distancia <= distanciaVision)
        {
            agente.SetDestination(jugador.position);

            if (distancia <= distanciaAtaque)
            {
                agente.ResetPath();
                jugadorEnRango = true;
                Atacar();
            }
            else
            {
                jugadorEnRango = false;
            }
        }
        else
        {
            agente.ResetPath();
            jugadorEnRango = false;
        }
    }

    void Atacar()
    {
        if (Time.time < tiempoUltimoAtaque + tiempoEntreAtaques) return;

        tiempoUltimoAtaque = Time.time;
        Debug.Log(gameObject.name + " ataca al jugador!");

        SaludJugador saludJugador = jugador.GetComponent<SaludJugador>();
        if (saludJugador != null)
        {
            saludJugador.getDamage(danioAtaque);
        }

        if (GestorAudio.Instance != null)
        {
            GestorAudio.Instance.ReproducirEfecto("ataque_enemigo");
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, distanciaVision);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, distanciaAtaque);
    }
}