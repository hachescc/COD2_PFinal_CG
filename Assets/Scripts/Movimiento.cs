using UnityEngine;

public class Movimiento : MonoBehaviour
{
    public CharacterController controlador;
    public float veloMovi = 2f;
    public float gravedad = -9.81f;
    public float salto = 4f;

    public Transform checkPiso;
    public float distanciaPiso = 0.4f;
    public LayerMask piso;

    bool enPiso;
    Vector3 velocidad;
    Pistola Pistola = new Pistola(10);

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

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            veloMovi *= 2f;
        }

        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            veloMovi /= 2f;
        }
    }

    void Disparar()
    {
        if (Pistola.Balas <= 0)
        {
            Pistola.Disparar();
            return;
        }

        Pistola.Disparar();

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit))
        {
            if (hit.collider.CompareTag("Enemigo"))
            {
                Debug.Log("hit");
            }

            else if (Pistola.Balas <= 0)
            {
                Debug.Log("Sin balas para disparar");
            }
        }
    }

    void Recargar()
    {
        Pistola.Recargar(10);
    }
}
