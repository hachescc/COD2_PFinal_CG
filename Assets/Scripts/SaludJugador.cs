using UnityEngine;

public class SaludJugador : MonoBehaviour
{
    [Header("Salud")]
    public float saludMaxima = 100f;
    [SerializeField] private float saludActual;

    void Start()
    {
        saludActual = saludMaxima;

        if (HUDController.Instance != null)
        {
            HUDController.Instance.ActualizarSalud(saludActual, saludMaxima);
        }
    }

    public void getDamage(float danio)
    {
        saludActual -= danio;
        saludActual  = Mathf.Clamp(saludActual, 0f, saludMaxima);

        Debug.Log("Jugador recibio danio: " + danio + " | Salud: " + saludActual);

        if (HUDController.Instance != null)
        {
            HUDController.Instance.ActualizarSalud(saludActual, saludMaxima);
        }

        if (saludActual <= 0f)
        {
            Morir();
        }
    }

    public void Curar(float cantidad)
    {
        saludActual += cantidad;
        saludActual  = Mathf.Clamp(saludActual, 0f, saludMaxima);

        Debug.Log("Jugador curado: " + cantidad + " | Salud: " + saludActual);

        if (HUDController.Instance != null)
        {
            HUDController.Instance.ActualizarSalud(saludActual, saludMaxima);
        }
    }

    void Morir()
    {
        Debug.Log("El jugador murio!");

        if (GameManager.Instance != null)
        {
            GameManager.Instance.GameOver();
        }
    }
}