using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Estado del juego")]
    public int  puntuacion     = 0;
    public bool juegoTerminado = false;
    public bool gano           = false;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void AgregarPuntos(int puntos)
    {
        if (juegoTerminado) return;

        puntuacion += puntos;
        Debug.Log("Puntuacion: " + puntuacion);

        if (HUDController.Instance != null)
        {
            HUDController.Instance.ActualizarPuntuacion(puntuacion);
        }

        if (puntuacion >= 800)
        {
            Ganar();
        }
    }

    public void GameOver()
    {
        if (juegoTerminado) return;

        juegoTerminado = true;
        gano = false;
        Debug.Log("GAME OVER");
        SceneManager.LoadScene("Game Over");
    }

    public void Ganar()
    {
        if (juegoTerminado) return;

        juegoTerminado = true;
        gano = true;
        Debug.Log("GANASTE!");
        SceneManager.LoadScene("Game Over");
    }

    public void ReiniciarJuego()
    {
        juegoTerminado = false;
        puntuacion     = 0;
        gano           = false;
        SceneManager.LoadScene("Mapa");
    }

    public void IrAlMenu()
    {
        juegoTerminado = false;
        puntuacion     = 0;
        gano           = false;
        SceneManager.LoadScene("Menú");
    }
}