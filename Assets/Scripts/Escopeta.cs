using UnityEngine;

public class Escopeta : MonoBehaviour
{
    private Arma escopeta = new Arma(25, 1.5f, 4);


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            escopeta.Disparar();
        }

        if (Input.GetMouseButtonDown(1))
        {
            escopeta.Recargar(4);
        }
    }
}
