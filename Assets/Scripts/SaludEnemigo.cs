using UnityEngine;

public class SaludEnemigo : MonoBehaviour
{
    [Header("Salud")]
    public float saludMaxima = 100f;
    [SerializeField] private float saludActual;

    [Header("Puntos al morir")]
    public int puntosAlMorir = 100;

    void Start()
    {
        saludActual = saludMaxima;
    }

    public void getDamage(float danio)
    {
        saludActual -= danio;
        saludActual  = Mathf.Clamp(saludActual, 0f, saludMaxima);

        Debug.Log(gameObject.name + " recibio danio: " + danio + " | Salud: " + saludActual);

        if (GestorAudio.Instance != null)
        {
            GestorAudio.Instance.ReproducirEfecto("impacto");
        }

        if (saludActual <= 0f)
        {
            Morir();
        }
    }

    void Morir()
    {
        Debug.Log(gameObject.name + " murio!");

        if (GameManager.Instance != null)
        {
            GameManager.Instance.AgregarPuntos(puntosAlMorir);
        }

        if (GestorAudio.Instance != null)
        {
            GestorAudio.Instance.ReproducirEfecto("muerte_enemigo");
        }

        Destroy(gameObject);
    }
}