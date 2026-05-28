using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class EnemigoDisparo : MonoBehaviour
{
    [Header("Prefab y punto de disparo")]
    public GameObject prefabBala;
    public Transform  puntoDisparo;

    [Header("Estadísticas")]
    public float danioBalas          = 10f;
    public float velocidadBala       = 20f;
    public float tiempoEntreDisparos = 1.5f;

    [Header("Munición y recarga")]
    public int   municionCargador = 10;
    public int   municionActual;
    public float tiempoRecarga    = 3f;

    [Header("Rotación")]
    public float velocidadRotacion = 8f;

    const string ANIM_DISPARANDO = "disparando"; 
    const string ANIM_RECARGAR   = "Recargar";   

    EnemigoIA movimiento;
    Animator  anim;

    float tiempoUltimoDisparo = 0f;
    bool  estaCargando        = false;

    void Start()
    {
        movimiento     = GetComponent<EnemigoIA>();
        anim           = GetComponent<Animator>();
        municionActual = municionCargador;
    }

    void Update()
    {
        if (movimiento == null) return;

        if (movimiento.JugadorEnRango && movimiento.jugador != null)
            MirarAlJugador();

        if (!movimiento.JugadorEnRango || estaCargando)
        {
            anim.SetBool(ANIM_DISPARANDO, false);
            return;
        }

        if (municionActual <= 0)
        {
            anim.SetBool(ANIM_DISPARANDO, false);
            StartCoroutine(Recargar());
            return;
        }

        if (Time.time >= tiempoUltimoDisparo + tiempoEntreDisparos)
            Disparar();
    }

    void MirarAlJugador()
    {
        Vector3 direccion = (movimiento.jugador.position - transform.position).normalized;
        direccion.y = 0f;
        if (direccion == Vector3.zero) return;

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            Quaternion.LookRotation(direccion),
            Time.deltaTime * velocidadRotacion
        );
    }

    void Disparar()
    {
        if (prefabBala == null || puntoDisparo == null) return;

        tiempoUltimoDisparo = Time.time;
        municionActual--;

        GameObject  go   = Instantiate(prefabBala, puntoDisparo.position, puntoDisparo.rotation);
        BalaEnemigo bala = go.GetComponent<BalaEnemigo>();

        if (bala != null)
        {
            bala.danio     = danioBalas;
            bala.velocidad = velocidadBala;
        }

        anim.SetBool(ANIM_DISPARANDO, true);

        Debug.Log($"{gameObject.name} dispara | Munición: {municionActual}/{municionCargador}");
    }

    IEnumerator Recargar()
    {
        estaCargando = true;
        Debug.Log($"{gameObject.name} recargando...");
        yield return new WaitForSeconds(tiempoRecarga);
        municionActual = municionCargador;
        estaCargando   = false;
        Debug.Log($"{gameObject.name} recarga completa.");
    }
}