using UnityEngine;

public class Rifle : MonoBehaviour
{
    private Arma rifle = new Arma(25, 0.5f, 10);

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            rifle.Disparar();
        }

        if (Input.GetMouseButtonDown(1))
        {
            rifle.Recargar(10);
        }
    }
}
