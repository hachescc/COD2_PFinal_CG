using UnityEngine;
using UnityEngine.SceneManagement;

public class CinematicaInicial : MonoBehaviour
{
    [Header("Configuracion")]
    public float duracion = 5f;
    public bool  saltable = true;

    float tiempoTranscurrido = 0f;

    void Update()
    {
        tiempoTranscurrido += Time.deltaTime;

        if (tiempoTranscurrido >= duracion)
        {
            CargarMapa();
            return;
        }

        if (saltable && Input.GetKeyDown(KeyCode.Space))
        {
            CargarMapa();
        }
    }

    void CargarMapa()
    {
        Debug.Log("Cargando Mapa...");
        SceneManager.LoadScene("Mapa");
    }
}