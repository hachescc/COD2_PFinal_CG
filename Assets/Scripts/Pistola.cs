using UnityEngine;

public class Pistola : MonoBehaviour
{
    private Arma pistola = new Arma(25, 1f, 6);

    public int Balas => pistola.Balas;
    public int Cartucho => pistola.Cartucho;

    public void Disparar()
    {
        pistola.Disparar();
    }

    public void Recargar()
    {
        pistola.Recargar(6);
    }

    public void AgregarBalas(int cantidad)
    {
        pistola.Balas += cantidad;
    }
}
