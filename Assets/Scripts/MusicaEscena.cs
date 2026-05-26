using UnityEngine;

public class MusicaEscena : MonoBehaviour
{
    [Header("Musica")]
    public AudioClip musicaEscena;

    [Range(0f, 1f)]
    public float volumen = 0.5f;

    void Start()
    {
        if (GestorAudio.Instance == null) return;
        if (musicaEscena == null) return;

        GestorAudio.Instance.CambiarVolumenMusica(volumen);
        GestorAudio.Instance.ReproducirMusica(musicaEscena);
    }
}