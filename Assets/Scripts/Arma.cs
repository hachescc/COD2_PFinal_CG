using UnityEngine;

[System.Serializable]
public class Arma
{
    private int balas;
    private float cadencia;
    private int cartucho;
    private float tiempoUltimoDisparo = 0f;

    public Arma(int balas, float cadencia, int cartucho)
    {
        this.balas    = balas;
        this.cadencia = cadencia;
        this.cartucho = cartucho;
    }

    public int   Balas    { get => balas;    set => balas    = value; }
    public float Cadencia { get => cadencia; set => cadencia = value; }
    public int   Cartucho { get => cartucho; set => cartucho = value; }

    public bool Disparar()
    {
        if (cartucho <= 0)
        {
            Debug.Log("Sin balas en el cargador, recarga!");
            return false;
        }

        if (Time.time < tiempoUltimoDisparo + cadencia)
        {
            return false;
        }

        tiempoUltimoDisparo = Time.time;
        cartucho--;
        Debug.Log("Disparando... Cargador: " + cartucho + " | Reserva: " + balas);
        return true;
    }

    public void Recargar(int tamanioCargador)
    {
        if (cartucho >= tamanioCargador)
        {
            Debug.Log("El cartucho ya esta lleno!");
            return;
        }

        if (balas <= 0)
        {
            Debug.Log("No tienes balas para recargar!");
            return;
        }

        int necesarias = tamanioCargador - cartucho;

        if (balas >= necesarias)
        {
            balas    -= necesarias;
            cartucho  = tamanioCargador;
        }
        else
        {
            cartucho += balas;
            balas     = 0;
        }

        Debug.Log("Recargando... Balas actuales: " + balas);
        Debug.Log("Balas al maximo: " + cartucho);
    }
}