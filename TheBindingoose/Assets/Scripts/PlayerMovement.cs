using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerMovement : Character
{
    private Vector2 _movementInput;
    [SerializeField] private float _speed;
    private PlayerInput _playerInput;
    private Transform objetoVacio;
    public SpriteRenderer playerRenderer;
    private bool isHit;
    private GameObject dave;
    private Animator animatordave;
    private int animacion;

    private int armaActual = 0;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _playerInput = GetComponent<PlayerInput>();
        objetoVacio = transform.Find("Move it Dave"); //
        dave = GameObject.Find("Dave");
        animatordave = dave.GetComponent<Animator>();
    }

    private void OnEnable()
    {
        _playerInput.actions["Move"].performed += ctx => Move(ctx);
        _playerInput.actions["Move"].canceled += ctx => Move(ctx);
        _playerInput.actions["Look"].performed += ctx => Look(ctx);
        _playerInput.actions["Look"].canceled += ctx => Look(ctx);
        _playerInput.actions["SwitchWun"].performed += ctx => CambiarArma(ctx);
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        originalColor = playerRenderer.color;
        vida = 100;
        animacion = 0;

    }


    public void Move(InputAction.CallbackContext ctx)
    {
        float hor = ctx.ReadValue<Vector2>().x;
        float ver = ctx.ReadValue<Vector2>().y;

        if (hor != 0 || ver != 0)
        {
            animator.SetFloat("Horizontal", hor);
            animator.SetFloat("Vertical", ver);
            animator.SetFloat("Velocidad", 1);
        }
        else
        {
            animator.SetFloat("Velocidad", 0);
        }

        // Actualiza la direcci�n de movimiento para el movimiento del jugador
        _movementInput = new Vector2(hor, ver);
    }

    public void Look(InputAction.CallbackContext ctx)
    {
        if (ctx.control.device is Keyboard || ctx.control.device is Gamepad)
        {
            Vector2 input = ctx.ReadValue<Vector2>();

            if (input != Vector2.zero)
            {
                // Calcula el �ngulo de rotaci�n directamente
                float targetAngle = Mathf.Atan2(input.x, input.y) * Mathf.Rad2Deg;

                // Ajusta el �ngulo a 0, 90, 180 o 270 grados
                targetAngle = Mathf.Round(targetAngle / 90) * -90;

                // Aplica la rotaci�n al objeto vac�o
                objetoVacio.rotation = Quaternion.Euler(0, 0, targetAngle);
            }
        }
    }

    public void CambiarArma(InputAction.CallbackContext ctx)
    {
        Debug.Log("1");
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            Debug.Log("Presionaste la tecla E");
            animacion++;
            if (animacion>4)
            {
                animacion=1;
            }
            animatordave.SetInteger("Animacion", animacion);


        }
        // Verifica si se presiona la tecla "Q"
        else if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            Debug.Log("Presionaste la tecla Q");
            animacion--;
            if(animacion < 1)
            {
                animacion = 4;
            }
            animatordave.SetInteger("Animacion", animacion);

        }
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        vida = vida - 25;
        if (other.CompareTag("Enemy") && !isHit)
        {
            if (vida <= 0)
            {
                animator.SetBool("Death", true);
            }
            else
            {
                Debug.Log("da�o resibio");
                // Cambia el color a rojo
                playerRenderer.color = Color.red;
                isHit = true;
                StartCoroutine(ResetColorAfterDelay(0.5f));
            }
        }
    }

    IEnumerator ResetColorAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Restaura el color original
        playerRenderer.color = originalColor;

        isHit = false;
    }

    private void FixedUpdate()
    {
        _rb.velocity = _movementInput * _speed;
    }
}



