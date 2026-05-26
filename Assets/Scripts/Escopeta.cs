using UnityEngine;

public class Escopeta : MonoBehaviour
{
    private Arma escopeta = new Arma(25, 1.5f, 4);

    public int Balas => escopeta.Balas;
    public int Cartucho => escopeta.Cartucho;

    public void Disparar()
    {
        escopeta.Disparar();
    }

    public void Recargar()
    {
        escopeta.Recargar(4);
    }

    public void AgregarBalas(int cantidad)
    {
        escopeta.Balas += cantidad;
    }
}
