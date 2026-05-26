using UnityEngine;
using UnityEngine.UI;

public class Movimiento : MonoBehaviour
{
    #region movimiento
    [Header("Movimiento")]
    public CharacterController controlador;
    public float veloMovi = 2f;
    public float veloCorrer = 6f;
    public float gravedad = -9.81f;
    public float salto = 4f;
    #endregion

    #region arma
    [Header("Arma")]
    public GameObject carryPoint;
    public GameObject SecondaryCarryPoint;
    public GameObject ThirdCarryPoint;
    GameObject armaCerca;
    GameObject armaActualObjeto;
    public GameObject armaPistolaPrefab;
    public GameObject armaRiflePrefab;
    public GameObject armaEscopetaPrefab;
    #endregion

    #region ui
    [Header("UI")]
    public Text textoRecoger;
    public Camera camara;
    public GameObject panelRecoger;
    public GameObject panelMira;
    public GameObject miraPequena;
    #endregion

    [Header("Combate")]
    public float danioPistola = 20f;
    public float danioRifle = 35f;
    public float danioEscopeta = 50f;

    public Transform checkPiso;
    public float distanciaPiso = 0.4f;
    public LayerMask piso;
    bool enPiso;
    Vector3 velocidad;

    public GameObject inventarioPanelPistola;
    public GameObject inventarioPanelRifle;
    public GameObject inventarioPanelEscopeta;

    void Start()
    {
        if (textoRecoger == null && panelRecoger != null)
        {
            textoRecoger = panelRecoger.GetComponentInChildren<Text>(true);
        }

        if (camara == null)
        {
            camara = Camera.main;
        }

        if (carryPoint != null && carryPoint.transform.childCount > 0)
        {
            armaActualObjeto = carryPoint.transform.GetChild(0).gameObject;
            armaActualObjeto.SetActive(true);
        }

        MostrarMensajeRecoger(false);
    }

    void Update()
    {
        enPiso = Physics.CheckSphere(checkPiso.position, distanciaPiso, piso);

        if (enPiso && velocidad.y < 0)
        {
            velocidad.y = -2f;
        }

        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        float velActual = Input.GetKey(KeyCode.LeftShift) ? veloCorrer : veloMovi;

        Vector3 movimiento = transform.right * x + transform.forward * z;
        controlador.Move(movimiento * velActual * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && enPiso)
        {
            velocidad.y = Mathf.Sqrt(salto * -2f * gravedad);
        }

        velocidad.y += gravedad * Time.deltaTime;
        controlador.Move(velocidad * Time.deltaTime);

        if (Input.GetMouseButtonDown(0))
        {
            Disparar();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Recargar();
        }

        RecogerArma();
        Aim();
        SelectArma();

        if (armaActualObjeto != null)
        {
            Pistola pistola = armaActualObjeto.GetComponent<Pistola>();
            Rifle rifle = armaActualObjeto.GetComponent<Rifle>();
            Escopeta escopeta = armaActualObjeto.GetComponent<Escopeta>();

            if (pistola != null)
            {
                HUDController.Instance.ActualizarMunicion(pistola.Cartucho, pistola.Balas);
            }
            else if (rifle != null)
            {
                HUDController.Instance.ActualizarMunicion(rifle.Cartucho, rifle.Balas);
            }
            else if (escopeta != null)
            {
                HUDController.Instance.ActualizarMunicion(escopeta.Cartucho, escopeta.Balas);
            }
        }
    }

    void SelectArma()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) SeleccionarArma(typeof(Pistola));
        if (Input.GetKeyDown(KeyCode.Alpha2)) SeleccionarArma(typeof(Rifle));
        if (Input.GetKeyDown(KeyCode.Alpha3)) SeleccionarArma(typeof(Escopeta));
    }

    void Aim()
    {
        if (armaActualObjeto == null) return;
        if (camara == null) return;

        if (Input.GetMouseButton(1))
        {
            Rifle rifle = armaActualObjeto.GetComponent<Rifle>();
            if (rifle)
            {
                if (panelMira != null) panelMira.SetActive(true);
                if (miraPequena != null) miraPequena.SetActive(false);
                camara.fieldOfView = 30f;
            }
            else
            {
                camara.fieldOfView = 40f;
            }
        }
        else if (Input.GetMouseButtonUp(1))
        {
            Rifle rifle = armaActualObjeto.GetComponent<Rifle>();
            if (rifle)
            {
                if (panelMira != null) panelMira.SetActive(false);
                if (miraPequena != null) miraPequena.SetActive(true);
            }
            camara.fieldOfView = 60f;
        }
    }

    void Disparar()
    {
        if (armaActualObjeto == null)
        {
            Debug.Log("No tienes arma equipada!");
            return;
        }

        Pistola pistola = armaActualObjeto.GetComponent<Pistola>();
        Rifle rifle = armaActualObjeto.GetComponent<Rifle>();
        Escopeta escopeta = armaActualObjeto.GetComponent<Escopeta>();

        float danio = 0f;

        if (pistola != null)
        {
            pistola.Disparar();
            danio = danioPistola;
        }
        else if (rifle != null)
        {
            rifle.Disparar();
            danio = danioRifle;
        }
        else if (escopeta != null)
        {
            escopeta.Disparar();
            danio = danioEscopeta;
        }
        else
        {
            return;
        }

        if (GestorAudio.Instance != null)
        {
            GestorAudio.Instance.ReproducirEfecto("disparo");
        }

        if (camara == null)
        {
            Debug.Log("No hay camara asignada!");
            return;
        }

        RaycastHit hit;
        if (Physics.Raycast(camara.transform.position, camara.transform.forward, out hit))
        {
            Debug.Log("Disparo impacto: " + hit.collider.name);

            if (hit.collider.CompareTag("Enemigo"))
            {
                SaludEnemigo saludEnemigo = hit.collider.GetComponent<SaludEnemigo>();
                if (saludEnemigo != null)
                {
                    saludEnemigo.getDamage(danio);
                }

                if (GestorAudio.Instance != null)
                {
                    GestorAudio.Instance.ReproducirEfecto("impacto");
                }
            }
        }
    }

    void Recargar()
    {
        if (armaActualObjeto == null)
        {
            Debug.Log("No tienes arma equipada!");
            return;
        }

        Pistola pistola = armaActualObjeto.GetComponent<Pistola>();
        Rifle rifle = armaActualObjeto.GetComponent<Rifle>();
        Escopeta escopeta = armaActualObjeto.GetComponent<Escopeta>();

        if (pistola != null) pistola.Recargar();
        else if (rifle != null) rifle.Recargar();
        else if (escopeta != null) escopeta.Recargar();
    }

    public bool AgregarMunicionAleatoria(int cantidad, out string tipoArma)
    {
        tipoArma = "";

        GameObject[] armas = new GameObject[3];
        int totalArmas = 0;

        GameObject pistolaObjeto = BuscarArma(typeof(Pistola));
        if (pistolaObjeto != null) armas[totalArmas++] = pistolaObjeto;

        GameObject rifleObjeto = BuscarArma(typeof(Rifle));
        if (rifleObjeto != null) armas[totalArmas++] = rifleObjeto;

        GameObject escopetaObjeto = BuscarArma(typeof(Escopeta));
        if (escopetaObjeto != null) armas[totalArmas++] = escopetaObjeto;

        if (totalArmas == 0) return false;

        GameObject armaElegida = armas[Random.Range(0, totalArmas)];

        Pistola pistola = armaElegida.GetComponent<Pistola>();
        Rifle rifle = armaElegida.GetComponent<Rifle>();
        Escopeta escopeta = armaElegida.GetComponent<Escopeta>();

        if (pistola != null)
        {
            pistola.AgregarBalas(cantidad);
            tipoArma = "pistola";
            if (armaElegida == armaActualObjeto && HUDController.Instance != null)
            {
                HUDController.Instance.ActualizarMunicion(pistola.Cartucho, pistola.Balas);
            }
            return true;
        }

        if (rifle != null)
        {
            rifle.AgregarBalas(cantidad);
            tipoArma = "rifle";
            if (armaElegida == armaActualObjeto && HUDController.Instance != null)
            {
                HUDController.Instance.ActualizarMunicion(rifle.Cartucho, rifle.Balas);
            }
            return true;
        }

        if (escopeta != null)
        {
            escopeta.AgregarBalas(cantidad);
            tipoArma = "escopeta";
            if (armaElegida == armaActualObjeto && HUDController.Instance != null)
            {
                HUDController.Instance.ActualizarMunicion(escopeta.Cartucho, escopeta.Balas);
            }
            return true;
        }

        return false;
    }

    void RecogerArma()
    {
        if (Input.GetKeyDown(KeyCode.E) && armaCerca != null)
        {
            if (carryPoint == null)
            {
                Debug.Log("No hay carry point asignado!");
                return;
            }

            if (armaActualObjeto != null && !GuardarArmaActual())
            {
                Debug.Log("Inventario lleno!");
                return;
            }

            armaActualObjeto = armaCerca;
            armaCerca = null;

            PonerArmaEnPunto(armaActualObjeto, carryPoint.transform, true);

            Collider[] colliders = armaActualObjeto.GetComponentsInChildren<Collider>();
            foreach (Collider col in colliders)
            {
                col.enabled = false;
            }

            Rigidbody rb = armaActualObjeto.GetComponent<Rigidbody>();
            if (rb != null) rb.isKinematic = true;

            if (armaActualObjeto.GetComponent<Pistola>() != null)
            {
                if (inventarioPanelPistola != null) inventarioPanelPistola.SetActive(true);
            }
            else if (armaActualObjeto.GetComponent<Rifle>() != null)
            {
                if (inventarioPanelRifle != null) inventarioPanelRifle.SetActive(true);
            }
            else if (armaActualObjeto.GetComponent<Escopeta>() != null)
            {
                if (inventarioPanelEscopeta != null) inventarioPanelEscopeta.SetActive(true);
            }

            if (GestorAudio.Instance != null)
            {
                GestorAudio.Instance.ReproducirEfecto("recoger");
            }

            MostrarMensajeRecoger(false);
        }
    }

    bool GuardarArmaActual()
    {
        if (SecondaryCarryPoint != null && SecondaryCarryPoint.transform.childCount == 0)
        {
            PonerArmaEnPunto(armaActualObjeto, SecondaryCarryPoint.transform, false);
            return true;
        }
        else if (ThirdCarryPoint != null && ThirdCarryPoint.transform.childCount == 0)
        {
            PonerArmaEnPunto(armaActualObjeto, ThirdCarryPoint.transform, false);
            return true;
        }
        return false;
    }

    void SeleccionarArma(System.Type tipoArma)
    {
        if (carryPoint == null) return;

        GameObject arma = BuscarArma(tipoArma);

        if (arma == null || arma == armaActualObjeto) return;

        Transform puntoAnterior = arma.transform.parent;

        if (armaActualObjeto != null)
        {
            PonerArmaEnPunto(armaActualObjeto, puntoAnterior, false);
        }

        armaActualObjeto = arma;
        PonerArmaEnPunto(armaActualObjeto, carryPoint.transform, true);
    }

    GameObject BuscarArma(System.Type tipoArma)
    {
        GameObject arma = BuscarArmaEnPunto(carryPoint, tipoArma);
        if (arma == null) arma = BuscarArmaEnPunto(SecondaryCarryPoint, tipoArma);
        if (arma == null) arma = BuscarArmaEnPunto(ThirdCarryPoint, tipoArma);
        return arma;
    }

    GameObject BuscarArmaEnPunto(GameObject punto, System.Type tipoArma)
    {
        if (punto == null) return null;
        Component arma = punto.GetComponentInChildren(tipoArma, true);
        if (arma != null) return arma.gameObject;
        return null;
    }

    void PonerArmaEnPunto(GameObject arma, Transform punto, bool equipada)
    {
        if (arma == null || punto == null) return;
        arma.transform.SetParent(punto);
        arma.transform.localPosition = Vector3.zero;
        arma.transform.localRotation = Quaternion.identity;
        arma.SetActive(equipada);
    }

    void OnTriggerEnter(Collider other)
    {
        GameObject arma = ObtenerArma(other.gameObject);
        if (arma != null)
        {
            armaCerca = arma;
            MostrarMensajeRecoger(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        GameObject arma = ObtenerArma(other.gameObject);
        if (arma != null && arma == armaCerca)
        {
            armaCerca = null;
            MostrarMensajeRecoger(false);
        }
    }

    GameObject ObtenerArma(GameObject objeto)
    {
        Pistola pistola = objeto.GetComponent<Pistola>();
        Rifle rifle = objeto.GetComponent<Rifle>();
        Escopeta escopeta = objeto.GetComponent<Escopeta>();

        if (pistola != null) return pistola.gameObject;
        if (rifle != null) return rifle.gameObject;
        if (escopeta != null) return escopeta.gameObject;
        return null;
    }

    void MostrarMensajeRecoger(bool mostrar)
    {
        if (panelRecoger != null) panelRecoger.SetActive(mostrar);
        if (textoRecoger != null)
        {
            textoRecoger.text = "Presiona E para recoger";
            textoRecoger.gameObject.SetActive(mostrar);
        }
    }
}
