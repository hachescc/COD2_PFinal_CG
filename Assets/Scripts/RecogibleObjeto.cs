using UnityEngine;

public class RecogibleObjeto : MonoBehaviour
{
    public enum TipoObjeto
    {
        Municion,
        Salud
    }

    [Header("Configuracion")]
    public TipoObjeto tipo      = TipoObjeto.Municion;
    public float      cantidad  = 10f;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (tipo == TipoObjeto.Salud)
        {
            SaludJugador saludJugador = other.GetComponent<SaludJugador>();
            if (saludJugador != null)
            {
                saludJugador.Curar(cantidad);
                Debug.Log("Jugador curo " + cantidad + " puntos de salud");
            }
        }
        else if (tipo == TipoObjeto.Municion)
        {
            Debug.Log("Jugador recogió " + cantidad + " balas");

            if (GestorAudio.Instance != null)
            {
                GestorAudio.Instance.ReproducirEfecto("recoger");
            }
        }

        Destroy(gameObject);
    }
}