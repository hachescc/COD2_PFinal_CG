using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{
    public void JugarJuego()
    {
        SceneManager.LoadScene("Cinematica");
    }

    public void SalirJuego()
    {
        Debug.Log("Saliendo del juego");
        Application.Quit();
    }
}