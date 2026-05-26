using UnityEngine;
using UnityEngine.UI;

public class Movimiento : MonoBehaviour
{
    public CharacterController controlador;
    public float veloMovi = 2f;
    public float gravedad = -9.81f;
    public float salto = 4f;
    public GameObject carryPoint;
    public GameObject panelRecoger;
    public Text textoRecoger;
    public Transform checkPiso;
    public float distanciaPiso = 0.4f;
    public LayerMask piso;

    bool enPiso;
    Vector3 velocidad;
    GameObject armaCerca;
    GameObject armaActualObjeto;

    void Start()
    {
        if (textoRecoger == null && panelRecoger != null)
        {
            textoRecoger = panelRecoger.GetComponentInChildren<Text>(true);
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

        Vector3 movimiento = transform.right * x + transform.forward * z;
        controlador.Move(movimiento * veloMovi * Time.deltaTime);

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

        if (Input.GetMouseButtonDown(1))
        {
            Recargar();
        }

        RecogerArma();

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            veloMovi *= 2f;
        }

        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            veloMovi /= 2f;
        }


        //if (Input )


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

        if (pistola != null)
        {
            pistola.Disparar();
        }
        else if (rifle != null)
        {
            rifle.Disparar();
        }
        else if (escopeta != null)
        {
            escopeta.Disparar();
        }

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit))
        {
            if (hit.collider.CompareTag("Enemigo"))
            {
                Debug.Log("hit");

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

        if (pistola != null)
        {
            pistola.Recargar();
        }
        else if (rifle != null)
        {
            rifle.Recargar();
        }
        else if (escopeta != null)
        {
            escopeta.Recargar();
        }
    }

    void RecogerArma()
    {
        if (Input.GetKeyDown(KeyCode.E) && armaCerca != null)
        {
            if (armaActualObjeto != null)
            {
                armaActualObjeto.SetActive(false);
            }

            armaActualObjeto = armaCerca;
            armaCerca = null;

            armaActualObjeto.transform.SetParent(carryPoint.transform);
            armaActualObjeto.transform.localPosition = Vector3.zero;
            armaActualObjeto.transform.localRotation = Quaternion.identity;

            Collider[] colliders = armaActualObjeto.GetComponentsInChildren<Collider>();
            foreach (Collider collider in colliders)
            {
                collider.enabled = false;
            }

            Rigidbody rigidbody = armaActualObjeto.GetComponent<Rigidbody>();
            if (rigidbody != null)
            {
                rigidbody.isKinematic = true;
            }

            MostrarMensajeRecoger(false);
        }
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
        Pistola pistola = objeto.GetComponentInChildren<Pistola>();
        Rifle rifle = objeto.GetComponentInChildren<Rifle>();
        Escopeta escopeta = objeto.GetComponentInChildren<Escopeta>();

        if (pistola != null)
        {
            return pistola.gameObject;
        }
        else if (rifle != null)
        {
            return rifle.gameObject;
        }
        else if (escopeta != null)
        {
            return escopeta.gameObject;
        }

        return null;
    }

    void MostrarMensajeRecoger(bool mostrar)
    {
        if (panelRecoger != null)
        {
            panelRecoger.SetActive(mostrar);
        }

        if (textoRecoger != null)
        {
            textoRecoger.text = "Presiona E para recoger";
            textoRecoger.gameObject.SetActive(mostrar);
        }
    }

}
