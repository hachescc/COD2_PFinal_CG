using UnityEngine;

[System.Serializable]
public class Arma
{
    private int balas;
    private int cadencia;
    private int cartucho;

    public Arma(int balas, int cadencia, int cartucho)
    {
        this.balas = balas;
        this.cadencia = cadencia;
        this.cartucho = cartucho;
    }


    public int Balas { get => balas; set => balas = value; }
    public int Cadencia { get => cadencia; set => cadencia = value; }
    public int Cartucho { get => cartucho; set => cartucho = value; }


    public void Disparar()
    {
        if (Time.time >= cadencia && cartucho > 0)
        {
            Debug.Log("Disparando...");
            cartucho--;
        }
        else
        {
            Debug.Log("No hay balas en el arma!");

        }
    }

    public void Recargar(int cantidad)
    {
        if (cartucho < cantidad && balas > 0)
        {
            balas -= cantidad - cartucho;
            cartucho += cantidad - cartucho;
            Debug.Log("Recargando... Balas actuales: " + balas);
            Debug.Log("Balas al máximo: " + balas);

        }
    }
}
