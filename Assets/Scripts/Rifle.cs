using UnityEngine;

public class Rifle : MonoBehaviour
{
    private Arma rifle = new Arma(25, 0.5f, 10);

    public void Disparar()
    {
        rifle.Disparar();
    }

    public void Recargar()
    {
        rifle.Recargar(10);
    }
}