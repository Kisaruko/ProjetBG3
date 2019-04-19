using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BinaryLight : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private LineRenderer viseur;
    private float baseRotationSpeed;
    [Header("Light Attributes", order = 0)]
    [Space(10, order = 1)]
    public bool gotLight;
    public GameObject LightObject;
    public Transform lightAnchor;
    public bool isAimingLight;

    [Header("Physics Attributes", order = 0)]
    [Space(10, order = 1)]
    public float ejectionForce;
    private Rigidbody lightRb;
    public float ejectionDistance;
    public float ejectionHeight;
    public bool isThrown;
    bool reachedMaxRange = false;

    [Header("Physics Attributes", order = 0)]
    [Space(10, order = 1)]
    public float speedWhileAiming;
    public GameObject reticule;
    public float aimingSpeed;
    private Vector3 lastPosReticule;
    public float maxRange;

    [Header("Vfx Attributes", order = 0)]
    [Space(10, order = 1)]
    public GameObject VfxAppear;
    public ParticleSystem VfxDisappear;
    public ParticleSystem.EmissionModule emi;
    private float baseSpeed;


    /// <summary>
    /// //////////////
    /// </summary>
    /// 
    private void Start()
    {
        viseur = GetComponent<LineRenderer>(); // get le line renderer
        viseur.positionCount = 0; // la ligne a 0 vertex, elle n'apparait donc pas
        lightRb = LightObject.GetComponent<Rigidbody>();
        playerMovement = FindObjectOfType<PlayerMovement>();
        emi = VfxDisappear.emission;
        baseSpeed = aimingSpeed;
        baseRotationSpeed = playerMovement.rotationSpeed;
    }
    private void Update()
    {
        if (gotLight)
        {
            if (Input.GetButtonDown("Attack"))
            {
                emi.rateOverTime = 30;
                Aiming();
            }
            if (isAimingLight)
            {
                ManageReticule();
            }
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            DropLight();
        }
        if (Input.GetKeyDown(KeyCode.G) || (Input.GetButtonDown("Fire3")))
        {
            GetLight();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        GetLight();
        isThrown = false;
    }

    /// <summary>
    /// /////////
    /// </summary>
    public void DropLight()
    {
        gotLight = false;
        LightObject.transform.parent = null;
        lightRb.isKinematic = false;
        lightRb.useGravity = true;
        Vector3 ejectionDirection = new Vector3(Random.Range(ejectionDistance, -ejectionDistance), ejectionHeight, Random.Range(ejectionDistance, -ejectionDistance));
        lightRb.AddForce(ejectionDirection);
    }
    public void GetLight()
    {
        gotLight = true;
        lightRb.isKinematic = true;
        lightRb.useGravity = false;
        LightObject.transform.position = lightAnchor.position;
        LightObject.transform.parent = transform;
    }
    void Aiming()
    {
        reticule.SetActive(true); // activer le fx de load
        isAimingLight = true;
        playerMovement.rotationSpeed = playerMovement.rotationSpeed / 10;
        //playerMovement.moveSpeed =  speedWhileAiming;

    }
    void ManageReticule()
    {
        if (reachedMaxRange == false)
        {
            reticule.transform.Translate((Vector3.up * -1 * aimingSpeed) * Time.deltaTime, Space.Self);
        }
        viseur.positionCount = 3; // la ligne fait possede 2 vertex
        viseur.SetPosition(0, new Vector3(transform.position.x, transform.position.y + 1, transform.position.z)); // le premier point est sur le player
        Vector3 newDirection = (transform.position + reticule.transform.position);
        viseur.SetPosition(1, newDirection - transform.position);
        viseur.SetPosition(2, new Vector3(reticule.transform.position.x, reticule.transform.position.y + 1, reticule.transform.position.z)); // le deuxieme point est devant le player + une range de 20

        if (Vector3.Distance(transform.position, reticule.transform.position) > maxRange)
        {
            reachedMaxRange = true;
        }
        if (Input.GetButtonUp("Attack"))
        {
            playerMovement.rotationSpeed = baseRotationSpeed;
            ThrowLight();
            //playerMovement.moveSpeed = 11;

            reachedMaxRange = false;
        }
    }
    void ThrowLight()
    {
        viseur.positionCount = 0; // la ligne a 0 vertex, elle n'apparait donc pas

        emi.rateOverTime = 0;
        aimingSpeed = baseSpeed;
        lastPosReticule = reticule.transform.position;
        //Components modifications
        LightObject.transform.parent = null;
        lightRb.isKinematic = false;
        gotLight = false;
        lightRb.useGravity = true;

        //Physics alterations
        isThrown = true;
        //reset modifications
        isAimingLight = false;
        reticule.transform.position = transform.position;
        reticule.SetActive(false);
        Instantiate(VfxAppear, lastPosReticule, Quaternion.identity);
        LightObject.transform.position = new Vector3(lastPosReticule.x, lastPosReticule.y + 1, lastPosReticule.z);

    }

}
