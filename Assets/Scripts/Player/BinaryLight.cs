using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BinaryLight : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private BondCylinder bondCylinder;
    private Animator anim;
    private LineRenderer viseur;
    private float baseRotationSpeed;
    public bool teleport = false;
    public bool isInvicible = false;
    public float invincibleDuration;

    [Header("Light Attributes", order = 0)]
    [Space(10, order = 1)]
    public bool gotLight;
    public GameObject LightObject;
    public Transform lightAnchor;
    public bool isAimingLight;
    public bool isRegrabable;
    private MeshRenderer mesh;
    private SkinnedMeshRenderer charRenderer;
    private Material charMaterial;

    [Header("Physics Attributes", order = 0)]
    [Space(10, order = 1)]
    public float ejectionForce;
    private Rigidbody lightRb;
    public float ejectionDistance;
    public float ejectionHeight;
    public bool isThrown;
    bool reachedMaxRange = false;

    [Header("Reticule Attributes", order = 0)]
    [Space(10, order = 1)]
    public float speedWhileAiming;
    public GameObject reticule;
    public float aimingSpeed;
    private Vector3 lastPosReticule;
    public float maxRange;
    [Range(0, 500)]
    public float throwHorizontalSpeed;
    [Range(0, 500)]
    public float throwVerticalSpeed;

    [Header("Vfx Attributes", order = 0)]
    [Space(10, order = 1)]
    public GameObject VfxAppear;
    public ParticleSystem VfxDisappear;
    public ParticleSystem.EmissionModule emi;
    private float baseSpeed;
    public GameObject vfxGrabLight;
    private Vector3 vfxPos;



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
        baseSpeed = playerMovement.moveSpeed;
        baseRotationSpeed = playerMovement.rotationSpeed;
        anim = GetComponentInChildren<Animator>();
        mesh = LightObject.GetComponentInChildren<MeshRenderer>();
        mesh.enabled = false;

        bondCylinder = GetComponent<BondCylinder>();


        charRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        charMaterial = charRenderer.material;
    }
    private void Update()
    {
        if (gotLight)
        {
            charMaterial.SetColor("_EmissionColor", Color.white); //Active l'emissive du bras du joueur

            if (Input.GetButtonDown("Throw") || Input.GetMouseButtonDown(0))
            {
                emi.rateOverTime = 30;
                anim.SetBool("isAiming", true);
                anim.SetBool("launch", false);

                Aiming();
            }
            if (isAimingLight)
            {
                ManageReticule();
            }
        }

        if(!gotLight)
        {
            charMaterial.SetColor("_EmissionColor", Color.black); //Désactive l'émissive du bras du joueur
        }

        //Debug
        if (Input.GetKeyDown(KeyCode.D))
        {
            DropLight();
        }
        if (Input.GetKeyDown(KeyCode.G) || (Input.GetButtonDown("Throw")))
        {
            GetLight();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == LightObject)
        {
            anim.SetBool("getLight", true);

            vfxPos = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
            Instantiate(vfxGrabLight, vfxPos, Quaternion.identity);
            GetLight();
            isThrown = false;
            mesh.enabled = false;
        }
    }
    private void AnimatorSetter()
    {
        anim.SetBool("getLight", false);
    }
    /// <summary>
    /// /////////
    /// </summary>
    public void DropLight()
    {
        mesh.enabled = true;
        LightObject.GetComponent<SphereCollider>().isTrigger = false;
        lightRb.drag = 0;
        isRegrabable = false;
        Invoke("LightCanBeRegrabed", 2f);
        gotLight = false;
        LightObject.transform.parent = null;
        lightRb.isKinematic = false;
        lightRb.useGravity = true;
        Vector3 ejectionDirection = new Vector3(Random.Range(ejectionDistance, -ejectionDistance), ejectionHeight, Random.Range(ejectionDistance, -ejectionDistance));
        lightRb.AddForce(ejectionDirection);
        bondCylinder.EnableEffects();

    }
    public void GetLight()
    {
        if (isRegrabable)
        {
            LightObject.GetComponent<SphereCollider>().isTrigger = true;
            gotLight = true;
            lightRb.isKinematic = true;
            lightRb.useGravity = false;
            LightObject.transform.position = lightAnchor.position;
            LightObject.transform.parent = lightAnchor;
            bondCylinder.DisableEffects();
            Invoke("AnimatorSetter", 0.1f);
        }
    }
    public void LightCanBeRegrabed()
    {
        isRegrabable = true;
        lightRb.drag = 10;
    }
    void Aiming()
    {
        reticule.SetActive(true); // activer le fx de load
        isAimingLight = true;
        playerMovement.rotationSpeed = playerMovement.rotationSpeed / 10;

    }
    void ManageReticule()
    {
        playerMovement.moveSpeed = speedWhileAiming;

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
        if (Input.GetButtonUp("Throw") || Input.GetMouseButtonUp(0))
        {
            mesh.enabled = true;
            anim.SetBool("isAiming", false);
            anim.SetBool("launch", true);
            playerMovement.rotationSpeed = baseRotationSpeed;
            ThrowLight();

            playerMovement.moveSpeed = baseSpeed;

            reachedMaxRange = false;
        }
    }
    void ThrowLight()
    {
        LightObject.GetComponent<SphereCollider>().isTrigger = false;

        isRegrabable = true;
        lightRb.drag = 0;
        viseur.positionCount = 0; // la ligne a 0 vertex, elle n'apparait donc pas
        Invoke("LightCanBeRegrabed", 2f);
        //particles
        //emi.rateOverTime = 0;
        //resetspeed
        aimingSpeed = baseSpeed;
        //saveTheLastPosReticule
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
        //Instantiate(VfxAppear, lastPosReticule, Quaternion.identity);
        bondCylinder.EnableEffects();

        if (teleport)
        {
            LightObject.transform.position = new Vector3(lastPosReticule.x, lastPosReticule.y + 1, lastPosReticule.z);
        }
        else
        {
            LightObject.transform.position = transform.position + transform.forward;
            lightRb.AddForce(transform.forward * throwHorizontalSpeed + Vector3.up * throwVerticalSpeed);
        }

    }
    #region Player Taking Damage

    public void TakeHit()
    {
        if (isInvicible == false)
        {
            isInvicible = true;
            StartCoroutine("InvicibleTime");
        }
    }

    void Death()
    {
        Destroy(this.gameObject);
    }

    IEnumerator InvicibleTime()
    {
        yield return new WaitForSeconds(invincibleDuration);
        isInvicible = false;
        StopCoroutine("InvincibleTime");
    }
    #endregion
}
