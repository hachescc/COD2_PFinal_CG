using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class BalaEnemigo : MonoBehaviour
{
    [Header("Stats (se asignan desde EnemigoDisparo)")]
    public float danio     = 10f;
    public float velocidad = 20f;

    [Header("Vida útil")]
    public float tiempoVida = 5f;

    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.linearVelocity = transform.forward * velocidad;

        // Auto-destruir si no golpea nada
        Destroy(gameObject, tiempoVida);
    }

    // Usa IsTrigger = true en el Collider de la bala
    void OnTriggerEnter(Collider otro)
    {

        if (otro.CompareTag("Player"))
        {
            SaludJugador salud = otro.GetComponent<SaludJugador>();
            if (salud != null)
            {
                salud.getDamage(danio);
                Debug.Log($"Bala impacta al jugador: -{danio} HP");
            }
        }

        // Efecto de impacto (si tienes partículas)
        // Instantiate(prefabImpacto, transform.position, Quaternion.identity);

        // Sonido de impacto
        if (GestorAudio.Instance != null)
            GestorAudio.Instance.ReproducirEfecto("impacto_bala");

        Destroy(gameObject);
    }
}