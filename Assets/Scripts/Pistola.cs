using UnityEngine;

[System.Serializable]
public class Pistola
{
    private int balas;

    public Pistola(int balas)
    {
        this.balas = balas;

    }

    public int Balas { get => balas; set => balas = value; }


    public void Disparar()
    {
        if (balas > 0)
        {
            Debug.Log("Disparando...");
            balas--;
        }
        else
        {
            Debug.Log("No hay balas!");

        }
    }

    public void Recargar(int cantidad)
    {
        if (balas < cantidad)
        {
            balas += cantidad - balas;
            Debug.Log("Recargando... Balas actuales: " + balas);
            Debug.Log("Balas al máximo: " + balas);

        }
    }
}
