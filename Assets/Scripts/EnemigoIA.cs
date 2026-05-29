using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class EnemigoIA : MonoBehaviour
{
    public enum Estado { Patrullando, Persiguiendo, Atacando }

    [Header("Referencias")]
    public Transform jugador;

    [Header("Patrullaje")]
    public Transform[] waypoints;
    public float       tiempoEsperaWaypoint = 2f;

    [Header("Detección y velocidades")]
    public float distanciaVision    = 15f;
    public float distanciaAtaque    = 8f;
    public float velocidadPatrulla  = 2f;
    public float velocidadPerseguir = 4f;

    [Header("Animación")]
    public string animVelocidad = "velMovimiento";

    NavMeshAgent agente;
    Animator     anim;

    Estado estadoActual   = Estado.Patrullando;
    Estado estadoAnterior = Estado.Patrullando;

    int   waypointActual      = 0;
    float timerEsperaWaypoint = 0f;
    bool  esperandoEnWaypoint = false;

    public bool JugadorEnRango => estadoActual == Estado.Atacando;

    void Start()
    {
        agente = GetComponent<NavMeshAgent>();
        anim   = GetComponent<Animator>();

        if (jugador == null)
        {
            GameObject go = GameObject.FindWithTag("Player");
            if (go != null) jugador = go.transform;
        }

        if (waypoints != null && waypoints.Length > 0)
            IrASiguienteWaypoint();
    }

    void Update()
    {
        if (jugador == null) return;

        float distancia = Vector3.Distance(transform.position, jugador.position);

        if      (distancia <= distanciaAtaque)  estadoActual = Estado.Atacando;
        else if (distancia <= distanciaVision)  estadoActual = Estado.Persiguiendo;
        else                                    estadoActual = Estado.Patrullando;

        if (estadoActual != estadoAnterior)
        {
            AlCambiarEstado(estadoActual);
            estadoAnterior = estadoActual;
        }

        switch (estadoActual)
        {
            case Estado.Patrullando:  Patrullar();  break;
            case Estado.Persiguiendo: Perseguir();  break;
            case Estado.Atacando:     EnAtaque();   break;
        }

        ActualizarAnimacion();
    }

    void AlCambiarEstado(Estado nuevoEstado)
    {
        switch (nuevoEstado)
        {
            case Estado.Patrullando:
                agente.ResetPath();
                agente.isStopped    = false;
                esperandoEnWaypoint = false;
                IrASiguienteWaypoint();
                break;

            case Estado.Persiguiendo:
                agente.isStopped = false;
                break;

            case Estado.Atacando:
                agente.isStopped = true;
                agente.ResetPath();
                agente.velocity = Vector3.zero;
                break;
        }
    }

    void Patrullar()
    {
        if (waypoints == null || waypoints.Length == 0) return;

        agente.speed = velocidadPatrulla;

        if (esperandoEnWaypoint)
        {
            timerEsperaWaypoint -= Time.deltaTime;
            if (timerEsperaWaypoint <= 0f)
            {
                esperandoEnWaypoint = false;
                IrASiguienteWaypoint();
            }
            return;
        }

        if (!agente.pathPending && agente.remainingDistance <= agente.stoppingDistance)
        {
            esperandoEnWaypoint = true;
            timerEsperaWaypoint = tiempoEsperaWaypoint;
        }
    }

    void IrASiguienteWaypoint()
    {
        if (waypoints == null || waypoints.Length == 0) return;
        agente.SetDestination(waypoints[waypointActual].position);
        waypointActual = (waypointActual + 1) % waypoints.Length;
    }

    void Perseguir()
    {
        agente.speed = velocidadPerseguir;
        agente.SetDestination(jugador.position);
    }

    void EnAtaque()
    {
        agente.isStopped = true;
        agente.ResetPath();
        MirarAlJugador();
    }

    void MirarAlJugador()
    {
        Vector3 dir = (jugador.position - transform.position).normalized;
        dir.y = 0f;
        if (dir != Vector3.zero)
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(dir),
                Time.deltaTime * 8f
            );
    }

    void ActualizarAnimacion()
    {
        float vel = agente.isStopped
            ? 0f
            : agente.velocity.magnitude / velocidadPerseguir;

        anim.SetFloat(animVelocidad, vel, 0.1f, Time.deltaTime);
    }
}