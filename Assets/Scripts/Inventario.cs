using UnityEngine;

public class Inventario : MonoBehaviour
{
    public static Inventario Instance;

    [Header("Municion por arma")]
    [SerializeField] private int municionPistola  = 25;
    [SerializeField] private int municionRifle    = 50;
    [SerializeField] private int municionEscopeta = 20;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public int ObtenerMunicion(string tipoArma)
    {
        switch (tipoArma)
        {
            case "pistola":   return municionPistola;
            case "rifle":     return municionRifle;
            case "escopeta":  return municionEscopeta;
            default:          return 0;
        }
    }

    public void AgregarMunicion(string tipoArma, int cantidad)
    {
        switch (tipoArma)
        {
            case "pistola":
                municionPistola += cantidad;
                Debug.Log("Municion pistola: " + municionPistola);
                break;
            case "rifle":
                municionRifle += cantidad;
                Debug.Log("Municion rifle: " + municionRifle);
                break;
            case "escopeta":
                municionEscopeta += cantidad;
                Debug.Log("Municion escopeta: " + municionEscopeta);
                break;
        }

        if (HUDController.Instance != null)
        {
            ActualizarHUDMunicion(tipoArma);
        }
    }

    void ActualizarHUDMunicion(string tipoArma)
    {
        int reserva = ObtenerMunicion(tipoArma);

        if (HUDController.Instance != null)
        {
            HUDController.Instance.ActualizarMunicion(0, reserva);
        }
    }
}