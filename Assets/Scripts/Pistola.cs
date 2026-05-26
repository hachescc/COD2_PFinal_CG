using UnityEngine;

public class Pistola : MonoBehaviour
{
    private Arma pistola = new Arma(25, 1f, 6);

    public void Disparar()
    {
        pistola.Disparar();
    }

    public void Recargar()
    {
        pistola.Recargar(6);
    }
}