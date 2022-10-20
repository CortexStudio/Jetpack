using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class Jetpack : MonoBehaviour
{
    public float magnitudePower; // Força para destruição
    public float mainThrustPower; // Força do impulso principal
    public float sideThrustPower; // Força do impulso
    public Transform thrustRight, thrustLeft, thrustButton; // objeto de tranformação do impulso
    public Transform spawnExplosion; // referência do solo
    public Transform _parentParticle; // referência do objetos da particulas de poeira
    public GameObject plThrustLeft, plThrustRight; // referência das chamas do jetpack
    public GameObject explosionPrefab; // referência do prefab da explosão
    public GameObject playerDead; // referência do prefab do jodador articulado
    public LayerMask layerGround; // Layers de de aproximação
    public ParticleSystem particle; // referência da particulas de poeira
    public AudioMixer AudioMixer;

    private float fuel; // Combustivel
    private float currentFuel; // Combustivel
    private bool isAlive; // Verifaca se esta vivo
    private bool isLanding; // Verifaca se pousou
    private bool toLeft, toRight; // controles

    // Referência dos componentes
    private Rigidbody2D rb;
    private Animator anim;
    private Image fuelProgress;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        PlayThrusterSfx(-80);
        fuel = 10f;
        currentFuel = fuel;
        isAlive = true;
        isLanding = false;

        plThrustLeft.SetActive(false);
        plThrustRight.SetActive(false);

        // Configura alvo da camera
        if (MyCameraManger.Instance != null)
            MyCameraManger.Instance.FollowPlayer(this.gameObject);

        // Progresso combustivel
        fuelProgress = GameObject.FindGameObjectWithTag("GUIFuel").GetComponent<Image>();

        if (fuelProgress != null)
            fuelProgress.fillAmount = 1f;
    }

    void Update()
    {
        if (!isAlive)
            return;

        IsGround(); // Verifica se esta próximo do solo
        Inputs(); // Entradas
        IsLanding(); // Verifica pouso    
    }

    private void FixedUpdate()
    {
        if (!isAlive)
            return;

        if (toLeft && toRight) // (Input.GetKey(KeyCode.UpArrow))
        {
            ApplyForce(thrustButton, mainThrustPower);
        }
        else if (toLeft)
        {
            ApplyForce(thrustLeft, sideThrustPower);
        }
        else if (toRight)
        {
            ApplyForce(thrustRight, sideThrustPower);
        }
    }

    /// <summary>
    /// Método para verificar se esta próximo do solo
    /// </summary>
    void IsGround()
    {
        //bool hit = Physics2D.Raycast(transform.position, -transform.up, 1f, layerGround);
        //anim.SetBool("flying", hit);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, -transform.up, 1.0f, layerGround);

        if (hit.collider != null)
        {
            anim.SetBool("flying", true);
            _parentParticle.position = hit.point;
            _parentParticle.up = hit.normal;

            if (!particle.isPlaying && (toLeft || toRight))
                particle.Play();
        }
        else
        {
            anim.SetBool("flying", false);
            particle.Stop();
        }
    }

    /// <summary>
    /// Método para verificar se pousou
    /// </summary>
    void IsLanding()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -transform.up, 0.1f, layerGround);

        if (hit && !isLanding)
        {
            anim.SetTrigger("landing");
            isLanding = true;
        }
        else if (!hit)
        {
            isLanding = false;
        }
    }

    /// <summary>
    /// Método para aplica forçã de propulsão
    /// </summary>
    /// <param name="thrusterTransform"></param>
    /// <param name="thrustPower"></param>
    private void ApplyForce(Transform thrusterTransform, float thrustPower)
    {
        //var direction = transform.position - thrusterTransform.position;
        var direction = thrusterTransform.position - transform.position;
        rb.AddForceAtPosition(direction.normalized * thrustPower * Time.deltaTime, thrusterTransform.position);

        currentFuel -= 0.01f;

        if (fuelProgress != null)
            fuelProgress.fillAmount = currentFuel / fuel;
    }

    /// <summary>
    /// Método para detectar magmitude da colisão
    /// </summary>
    /// <param name="hitInfo"></param>
    void OnCollisionEnter2D(Collision2D hitInfo)
    {
        if (hitInfo.relativeVelocity.magnitude > magnitudePower && isAlive)
        {
            HandleLanderDestroy();
        }
    }

    /// <summary>
    /// Verifica se pegou combustivel
    /// </summary>
    /// <param name="collider"></param>
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Fuel")
        {
            currentFuel += 10f;

            if (currentFuel > fuel)
                currentFuel = fuel;

            if (fuelProgress != null)
                fuelProgress.fillAmount = currentFuel / fuel;

            Destroy(collider.gameObject);
        }
    }

    /// <summary>
    /// Método para destruir
    /// </summary>
    public void HandleLanderDestroy()
    {
        isAlive = false;
        PlayThrusterSfx(-80);

        MyCameraManger.Instance.CameraShake(5f, 0.3f);

        if (explosionPrefab != null)
            Instantiate(explosionPrefab, spawnExplosion.transform.position, Quaternion.identity);

        if (playerDead != null)
            Instantiate(playerDead, transform.position, transform.rotation);

        Destroy(this.gameObject);
    }

    /// <summary>
    /// Método para controle de efeito de audio
    /// </summary>
    private void PlayThrusterSfx(float volume)
    {
        AudioMixer.SetFloat("RocketVolume", volume);
    }

    void Inputs()
    {
        if (currentFuel > 0f)
        {

#if UNITY_EDITOR

            toRight = Input.GetKey(KeyCode.RightArrow);
            toLeft = Input.GetKey(KeyCode.LeftArrow);
#endif

            foreach (Touch touch in Input.touches)
            {
                if (touch.phase != TouchPhase.Ended && touch.phase != TouchPhase.Canceled)
                {
                    if (touch.position.x < Screen.width / 2)
                        toLeft = true;

                    if (touch.position.x > Screen.width / 2)
                        toRight = true;
                }
                else
                {
                    if (toLeft)
                        toLeft = false;

                    if (toRight)
                        toRight = false;
                }
            }

            plThrustLeft.SetActive(toLeft);
            plThrustRight.SetActive(toRight);

            //  if (toLeft || toRight)
            //    AudioManager.main.Shot(clip);

            // Pausa o som
            if (!toLeft && !toRight)
                PlayThrusterSfx(-80);
            else
                PlayThrusterSfx(0);

        }
        else
        {
            toRight = false;
            toLeft = false;
            plThrustLeft.SetActive(toLeft);
            plThrustRight.SetActive(toRight);
            PlayThrusterSfx(-80);

            // Após o combustivel acabar verifica se esta parado e chama o método para destruir
            if (rb.velocity.sqrMagnitude < 0.01f && rb.angularVelocity < .01f && isAlive)
            {
                isAlive = false;
                Invoke("HandleLanderDestroy", 1f);
            }
        }
    }
}
