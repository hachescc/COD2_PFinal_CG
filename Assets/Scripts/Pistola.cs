using UnityEngine;

public class Pistola : MonoBehaviour
{
    private Arma pistola = new Arma(25, 1f, 6);

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            pistola.Disparar();
        }

        if (Input.GetMouseButtonDown(1))
        {
            pistola.Recargar(6);
        }
    }
}
