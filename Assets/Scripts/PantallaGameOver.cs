using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PantallaGameOver : MonoBehaviour
{
    [Header("UI")]
    public Text textoPuntuacionFinal;
    public Text textoMensaje;

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 1f;

        if (textoPuntuacionFinal != null && GameManager.Instance != null)
        {
            textoPuntuacionFinal.text = "Puntuacion: " + GameManager.Instance.puntuacion;
        }

        if (textoMensaje != null)
        {
            if (GameManager.Instance != null && GameManager.Instance.gano)
            {
                textoMensaje.text = "¡GANASTE!";
            }
            else
            {
                textoMensaje.text = "GAME OVER";
            }
        }

        Debug.Log("Pantalla final cargada");
    }

    public void Reiniciar()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ReiniciarJuego();
        }
        else
        {
            SceneManager.LoadScene("Mapa");
        }
    }

    public void IrAlMenu()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.IrAlMenu();
        }
        else
        {
            SceneManager.LoadScene("Menú");
        }
    }
}