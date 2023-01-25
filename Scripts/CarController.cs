using UnityEngine;
using UnityEngine.InputSystem;


public enum WheelDrive { FrontWheel, RearWheel, FourWheel };
public class CarController : MonoBehaviour

{
    /**
   **NASTAVENI VOZIDLA"
   */
    [Header("NASTAVENI  VOZIDLA")]
    [Tooltip("Náhon kol")]
    [SerializeField] private WheelDrive wheelDrive;
    [Tooltip("Nastav sílu motoru")]
    [SerializeField] private float torque = 950f;

    [Tooltip("Udej maximální náklon kol")]
    [SerializeField] private float maxSteeringAngle = 30f;

    [Tooltip("Nasatv brzdu motoru")]
    [SerializeField] private float maxBraking = 8000f;
    [Space(20)]
    /**
    **NASTAVENÍ KOL VOZIDLA"
    */
    [Header("KOLA VOZIDLA")]
    [Tooltip("Priřaď collidery kol")]
    [SerializeField] private WheelCollider[] wheelColliders;
    /**
    **EFEKTY VOZIDLA"
    */
    [Header("EFEKTY VOZIDLA")]
    [Tooltip("Kouř pneumatik")]
    [SerializeField] private ParticleSystem wheelSmokePrefab;

    [Tooltip("Přiřad Audio pro zvuk brzdy")]
    [SerializeField] private AudioClip skidSoundEffect;

    [Tooltip("Čaš interv. pro brzdící zvuk a kouř")]
    [SerializeField] private float skidTreshold = 0.4f;

    /**
    **PRIVÁTNÍ HODNOTY"
    */
    private ParticleSystem[] wheelSmokes = new ParticleSystem[4];
    private PlayerInput playerInput;
    private float braking;
    private AudioSource audioSource;


    private void Awake()
    {

        playerInput = GetComponent<PlayerInput>();

        if (playerInput == null)
        {
            Debug.LogError("Vozidlo nema player input");
        }

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("Vozidlo nema zvuk brzdeni");
        }

        // vytvořeni kouře pneumatik
        for (int i = 0; i < wheelColliders.Length; i++)
        {
            wheelSmokes[i] = Instantiate(wheelSmokePrefab);
            wheelSmokes[i].Stop();
        }

    }

    void FixedUpdate()
    {
        //Přiřazení input akce ktera je vytvořena v GUI INPUTU
        Vector2 moveInput = playerInput.actions["Move"].ReadValue<Vector2>();
        float acc = moveInput.y;
        float steering = moveInput.x;

        Move(acc, steering, braking);
        skidCheck();
    }

    //Otacení kol a pohyb vozidla
    private void Move(float acceleration, float steering, float braking)
    {
        Quaternion quat;
        Vector3 position;

        // Mathf.Clamp
        // Upevní danou hodnotu mezi zadané minimální a maximální hodnoty float.
        // Vrátí zadanou hodnotu, pokud se nachází v minimálním a maximálním rozsahu.
        // https://docs.unity3d.com/2022.2/Documentation/ScriptReference/Mathf.Clamp.html
        acceleration = Mathf.Clamp(acceleration, -1f, 1f);

        steering = Mathf.Clamp(steering, -1f, 1f) * maxSteeringAngle;

        braking = Mathf.Clamp(braking, -1, 1) * maxBraking;

        // calculate the thrust torque
        float thrustTorque = acceleration * torque;

        // aplikuje silu na kola
        for (int i = 0; i < wheelColliders.Length; i++)
        {

            //Podminky na náhok kol
            if (wheelDrive != WheelDrive.FrontWheel && wheelDrive != WheelDrive.FourWheel)
            {
                wheelColliders[2].motorTorque = thrustTorque;
                wheelColliders[3].motorTorque = thrustTorque;
            }
            else if (wheelDrive != WheelDrive.RearWheel && wheelDrive != WheelDrive.FourWheel)
            {
                wheelColliders[0].motorTorque = thrustTorque;
                wheelColliders[1].motorTorque = thrustTorque;
            }
            else
            {
                wheelColliders[i].motorTorque = thrustTorque;
            }
            //zatacení kol
            if (i < 2)
            {
                wheelColliders[i].steerAngle = steering;
            }
            else
            {
                wheelColliders[i].brakeTorque = braking;
            }

            wheelColliders[i].GetWorldPose(out position, out quat);

            // Aplikovaní zmeny polohy na colider kola a tím auta
            wheelColliders[i].transform.GetChild(0).transform.position = position;
            // pusobení rotace na mesh kola
            wheelColliders[i].transform.GetChild(0).transform.rotation = quat;
        }
    }

    public void Braking(InputAction.CallbackContext context)
    {
        Debug.Log(context);
        if (context.started) braking = 1.0f;
        else if (context.canceled) braking = 0.0f;
    }

    //Metoda pro kouř a zvuk pneumatik
    private void skidCheck()
    {
        int skidCount = 0;

        for (int i = 0; i < wheelColliders.Length; i++)
        {
            WheelHit wheelHit;

            wheelColliders[i].GetGroundHit(out wheelHit);

            if (Mathf.Abs(wheelHit.forwardSlip) >= skidTreshold ||
               Mathf.Abs(wheelHit.sidewaysSlip) >= skidTreshold)
            {
                skidCount++;
                if (!audioSource.isPlaying)
                {
                    audioSource.PlayOneShot(skidSoundEffect);
                }

                wheelSmokes[i].transform.position = wheelColliders[i].transform.position - wheelColliders[i].transform.up * wheelColliders[i].radius;
                wheelSmokes[i].Emit(1);
            }
        }

        if (skidCount == 0 && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}
