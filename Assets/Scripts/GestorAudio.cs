using UnityEngine;

public class GestorAudio : MonoBehaviour
{
    public static GestorAudio Instance;

    [Header("Fuentes de audio")]
    public AudioSource fuenteEfectos;
    public AudioSource fuenteMusica;

    [Header("Efectos de sonido")]
    public AudioClip sonidoDisparo;
    public AudioClip sonidoImpacto;
    public AudioClip sonidoRecoger;
    public AudioClip sonidoAtaqueEnemigo;
    public AudioClip sonidoMuerteEnemigo;
    public AudioClip sonidoDanioJugador;

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

    public void ReproducirEfecto(string nombre)
    {
        if (fuenteEfectos == null) return;


        AudioClip clip = ObtenerClip(nombre);

        if (clip == null)
        {
            Debug.Log("GestorAudio: no se encontro el clip '" + nombre + "'");
            return;
        }

        fuenteEfectos.PlayOneShot(clip);
    }

    public void ReproducirMusica(AudioClip clip, bool loop = true)
    {
        if (fuenteMusica == null) return;
        if (clip == null) return;

        fuenteMusica.clip = clip;
        fuenteMusica.loop = loop;
        fuenteMusica.Play();
    }

    public void DetenerMusica()
    {
        if (fuenteMusica == null) return;
        fuenteMusica.Stop();
    }

    public void CambiarVolumenMusica(float volumen)
    {
        if (fuenteMusica == null) return;
        fuenteMusica.volume = Mathf.Clamp01(volumen);
    }

    public void CambiarVolumenEfectos(float volumen)
    {
        if (fuenteEfectos == null) return;
        fuenteEfectos.volume = Mathf.Clamp01(volumen);
    }

    AudioClip ObtenerClip(string nombre)
    {
        switch (nombre)
        {
            case "disparo": return sonidoDisparo;
            case "impacto": return sonidoImpacto;
            case "recoger": return sonidoRecoger;
            case "ataque_enemigo": return sonidoAtaqueEnemigo;
            case "muerte_enemigo": return sonidoMuerteEnemigo;
            case "danio_jugador": return sonidoDanioJugador;
            default: return null;
        }
    }
}