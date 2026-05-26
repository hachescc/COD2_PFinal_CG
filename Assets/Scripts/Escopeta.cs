using UnityEngine;

public class Escopeta : MonoBehaviour
{
    private Arma escopeta = new Arma(25, 1.5f, 4);


    public void Disparar()
    {
        escopeta.Disparar();
    }

    public void Recargar()
    {
        escopeta.Recargar(4);
    }
}
