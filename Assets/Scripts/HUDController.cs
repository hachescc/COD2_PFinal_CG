using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    public static HUDController Instance;

    [Header("Salud")]
    public Slider barraVida;
    public Text textoVida;
    public GameObject panelBajaVida;
    public GameObject panelMuyBajaVida;

    [Header("Puntuacion")]
    public Text textoPuntuacion;

    [Header("Municion")]
    public Text textoMunicion;



    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void ActualizarSalud(float actual, float maxima)
    {
        float porcentaje = actual / maxima;

        if (barraVida != null)
        {
            barraVida.value = porcentaje;
        }

        if (textoVida != null)
        {
            textoVida.text = Mathf.CeilToInt(actual) + " / " + Mathf.CeilToInt(maxima);
        }

        if (panelBajaVida != null)
        {
            panelBajaVida.SetActive(porcentaje <= 0.5f && porcentaje > 0.25f);
        }

        if (panelMuyBajaVida != null)
        {
            panelMuyBajaVida.SetActive(porcentaje <= 0.25f);
        }
    }

    public void ActualizarPuntuacion(int puntos)
    {
        if (textoPuntuacion != null)
        {
            textoPuntuacion.text = "Puntos: " + puntos;
        }
    }

    public void ActualizarMunicion(int cartucho, int reserva)
    {
        if (textoMunicion != null)
        {
            textoMunicion.text = cartucho + " / " + reserva;
        }
    }
}