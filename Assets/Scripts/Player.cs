using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    public float acceleration = 5f;
    public float maxSpeed = 10f;
    public float rotationSpeed = 100f;
    public float deceleration = 5f;
    public float reverseDecelerationMultiplier = 2f;

    public TextMeshProUGUI lapsText;
    public TextMeshProUGUI congratulationText; // Referencia al TextMeshProUGUI para mostrar felicitaciones
    public Camera mainCamera; // Referencia a la cámara principal

    private int lapsCompleted = 0;
    private int totalLaps = 3;

    private Rigidbody rb;
    private float currentSpeed = 0f;

    private bool raceCompleted = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        congratulationText.gameObject.SetActive(false); // Desactivar el texto de felicitaciones al inicio
    }

    void FixedUpdate()
    {
        if (!raceCompleted)
        {
            // Movimiento del carro
            float moveInput = Input.GetAxis("Vertical");

            if (moveInput != 0)
            {
                // Si se presiona una tecla de movimiento, acelerar gradualmente hacia adelante o hacia atrás
                float desiredSpeed = maxSpeed * moveInput;

                if (Mathf.Sign(desiredSpeed) != Mathf.Sign(currentSpeed) && moveInput != 0)
                {
                    // Si se presiona la tecla opuesta a la dirección actual del movimiento, desacelerar más rápido
                    currentSpeed = Mathf.MoveTowards(currentSpeed, desiredSpeed, deceleration * reverseDecelerationMultiplier * Time.fixedDeltaTime);
                }
                else
                {
                    currentSpeed = Mathf.MoveTowards(currentSpeed, desiredSpeed, acceleration * Time.fixedDeltaTime);
                }
            }
            else
            {
                // Si no se presiona ninguna tecla de movimiento, desacelerar gradualmente
                currentSpeed = Mathf.MoveTowards(currentSpeed, 0f, deceleration * Time.fixedDeltaTime);
            }

            // Aplicar la velocidad al Rigidbody para mover el carro
            rb.velocity = transform.forward * currentSpeed;

            // Rotación del carro
            float rotateInput = Input.GetAxis("Horizontal");
            transform.Rotate(Vector3.up, rotateInput * rotationSpeed * Time.fixedDeltaTime);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Si el carro colisiona con algo, detener su velocidad
        currentSpeed = 0f;
        rb.velocity = Vector3.zero;
    }

    void OnTriggerEnter(Collider other)
    {
        // Verificar si el vehículo ha pasado por la meta (el collider del objeto Meta)
        if (other.CompareTag("Meta"))
        {
            lapsCompleted++; // Aumentar el contador de vueltas completadas

            if (lapsText != null)
            {
                lapsText.text = "Vuelta " + lapsCompleted.ToString() + "/" + totalLaps.ToString(); // Actualizar el texto en el TextMeshProUGUI
            }

            if (lapsCompleted >= totalLaps)
            {
                raceCompleted = true;
                Debug.Log("¡Carrera completada! Felicitaciones.");

                congratulationText.gameObject.SetActive(true); // Activar el texto de felicitaciones
                congratulationText.text = "Felicitaciones!";

                // Centrar el texto de felicitaciones en la mitad de la pantalla
                Vector3 screenCenter = mainCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 10f)); // 10f es la distancia desde la cámara
                congratulationText.transform.position = screenCenter;

                // Desactivar el movimiento del jugador
                this.enabled = false; // Deshabilitar este script para detener el movimiento del jugador
            }
        }
    }
}