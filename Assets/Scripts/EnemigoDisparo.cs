using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EnemigoDisparo : MonoBehaviour
{
    [Header("Prefab y punto de disparo")]
    public GameObject    prefabBala;
    public Transform     puntoDisparo;   // hijo vacío en la punta del arma

    [Header("Estadísticas")]
    public float danioBalas        = 10f;
    public float velocidadBala     = 20f;
    public float tiempoEntreDisparos = 1.5f;

    [Header("Munición y recarga")]
    public int   municionCargador  = 10;
    public int   municionActual;
    public float tiempoRecarga     = 3f;

    [Header("Animaciones - Nombres de parámetros")]
    public string paramDisparar    = "Disparar";   // Trigger
    public string paramRecargar    = "Recargar";   // Trigger  ← agregar cuando tengas la anim

    EnemigoIA movimiento;
    Animator          anim;

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
        if (!movimiento.JugadorEnRango) return;
        if (estaCargando) return;

        if (municionActual <= 0)
        {
            StartCoroutine(Recargar());
            return;
        }

        if (Time.time >= tiempoUltimoDisparo + tiempoEntreDisparos)
        {
            Disparar();
        }
    }

    void Disparar()
    {
        if (puntoDisparo == null || prefabBala == null) return;

        tiempoUltimoDisparo = Time.time;
        municionActual--;

        // Instanciar bala
        GameObject balaGO = Instantiate(
            prefabBala,
            puntoDisparo.position,
            puntoDisparo.rotation
        );

        // Pasar datos a la bala
        BalaEnemigo bala = balaGO.GetComponent<BalaEnemigo>();
        if (bala != null)
        {
            bala.danio     = danioBalas;
            bala.velocidad = velocidadBala;
        }

        // Animación (cuando la tengas lista)
        anim.SetTrigger(paramDisparar);

        // Audio
        if (GestorAudio.Instance != null)
            GestorAudio.Instance.ReproducirEfecto("disparo_enemigo");

        Debug.Log($"{gameObject.name} dispara | Munición: {municionActual}/{municionCargador}");
    }

    System.Collections.IEnumerator Recargar()
    {
        estaCargando = true;
        Debug.Log($"{gameObject.name} recargando...");

        // Animación de recarga (cuando la tengas)
        // anim.SetTrigger(paramRecargar);

        if (GestorAudio.Instance != null)
            GestorAudio.Instance.ReproducirEfecto("recarga_enemigo");

        yield return new WaitForSeconds(tiempoRecarga);

        municionActual = municionCargador;
        estaCargando   = false;
        Debug.Log($"{gameObject.name} recarga completa.");
    }

    // Para debug en Inspector
    void OnGUI()
    {
#if UNITY_EDITOR
        // Puedes comentar esto en producción
#endif
    }
}