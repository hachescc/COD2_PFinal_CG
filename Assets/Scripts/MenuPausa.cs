using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuPausa : MonoBehaviour
{
    [Header("Panel de pausa")]
    public GameObject panelPausa;

    bool pausado = false;

    public bool Pausado { get => pausado; set => pausado = value; }

    void Start()
    {
        if (panelPausa != null)
        {
            panelPausa.SetActive(false);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pausado)
            {
                Reanudar();
            }
            else
            {
                Pausar();
            }
        }
    }

    public void Pausar()
    {
        pausado = true;
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (panelPausa != null)
        {
            panelPausa.SetActive(true);

        }

        if (GestorAudio.Instance != null)
        {
            GestorAudio.Instance.CambiarVolumenMusica(0.2f);
        }

        Debug.Log("Juego pausado");
    }

    public void Reanudar()
    {
        Time.timeScale = 1f;
        pausado = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (panelPausa != null)
        {
            panelPausa.SetActive(false);
        }

        if (GestorAudio.Instance != null)
        {
            GestorAudio.Instance.CambiarVolumenMusica(0.5f);
        }

        Debug.Log("Juego reanudado");
    }

    public void IrAlMenu()
    {
        pausado = false;
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.IrAlMenu();
        }
        else
        {
            SceneManager.LoadScene("Menú");
        }
    }

    public void SalirJuego()
    {
        Debug.Log("Saliendo del juego");
        Application.Quit();
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // solo en editor
    #endif
    }
}