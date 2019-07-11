using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BinaryLight : MonoBehaviour
{

    public bool controlsAreEnabled = true;

    private PlayerMovement playerMovement;
    private Animator anim;
    private float baseRotationSpeed;
    public bool teleport = false;
    public bool isInvicible = false;
    public float invincibleDuration;
    private MeshRenderer meshrenderer;

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
    private SphereCollider myCollider;
    public float timeBeforeFallAndRegrabable;

    [Header("Reticule Attributes", order = 0)]
    [Space(10, order = 1)]
    public float speedWhileAiming;
    public float aimingSpeed;
    private Vector3 lastPosReticule;
    private Vector3 cursor = Vector3.zero;
    Vector3 boundsMin;
    Vector3 boundsMax;
    public float maxRange;
    [Range(0, 500)]
    public float throwHorizontalSpeed;
    [Range(0, 500)]
    public float throwVerticalSpeed;
    public GameObject rangeStart;
    public GameObject rangeEnd;
    public GameObject rangeUnderCrosshair;
    private GameObject start;
    private GameObject end;
    private GameObject range;
    public float lightSpeed;
    [Header("Vfx Attributes", order = 0)]
    [Space(10, order = 1)]
    private float baseSpeed;
    public GameObject vfxGrabLight;
    private Vector3 vfxPos;
    public LayerMask cloneDetection;
    public GameObject vfxBond;
    private GameObject vfxBondClone;
    private Transform vfxDestination;
    public float vfxSpeed;
    ParticleSystem ps;
    ParticleSystem.MainModule ma;
    Color basecol;


    [Header("Shader Attributes", order = 0)]
    [Space(10, order = 1)]
    public float maxEmissionIntensity;
    public float minEmissionIntensity;


    [Header("Mouse Variables", order = 0)]
    [Space(10, order = 1)]
    public float speedMultiplierWithMouse;
    public bool playWithMouse;

    private void Start()
    {
        lightRb = LightObject.GetComponent<Rigidbody>();
        playerMovement = FindObjectOfType<PlayerMovement>();
        baseSpeed = playerMovement.moveSpeed;
        baseRotationSpeed = playerMovement.rotationSpeed;
        anim = GetComponentInChildren<Animator>();
        mesh = LightObject.GetComponentInChildren<MeshRenderer>();
        mesh.enabled = false;

        myCollider = LightObject.GetComponent<SphereCollider>();

        charRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        charMaterial = charRenderer.material;
        ps = GetComponentInChildren<ParticleSystem>();
        ma = ps.main;
        basecol = ma.startColor.color;
    }
    private void Update()
    {
        if (controlsAreEnabled)
        {
            if (gotLight)
            {
                //charMaterial.SetColor("_EmissionColor", Color.white); //Active l'emissive du bras du joueur OBSOLETE AVEC LE SHADER SILHOUETTE

                if (Input.GetButtonDown("Throw") || Input.GetMouseButtonDown(1))
                {
                    anim.SetBool("isAiming", true);
                    anim.SetBool("launch", false);

                    Aiming();
                }
                if (isAimingLight)
                {
                    ManageReticule();
                }
            }

            /*if (!gotLight)
            {
                //charMaterial.SetColor("_EmissionColor", Color.black); //Désactive l'émissive du bras du joueur OBSOLETE AVEC LE SHADER SILHOUETTE
            }*/

            //Debug
            /*if (Input.GetKeyDown(KeyCode.D))
             {
                 DropLight(ejectionDistance, ejectionHeight);
             }*/
            if (Input.GetKeyDown(KeyCode.G))
            {
                GetLight();
            }
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject == LightObject && isRegrabable == true)
        {
            vfxPos = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
            Instantiate(vfxGrabLight, vfxPos, Quaternion.identity);
            GetLight();
            isThrown = false;
        }
    }
    private void AnimatorSetter()
    {
        anim.SetBool("getLight", false);
        anim.SetBool("GetOnStatue", false);
        playerMovement.moveSpeed = baseSpeed;
    }
    /// <summary>
    /// /////////
    /// </summary>
    public void DropLight(float ejectionDistance, float ejectionHeight)
    {
        charMaterial.SetFloat("_EmissiveIntensity", minEmissionIntensity);
        mesh.enabled = true;
        myCollider.isTrigger = false;
        lightRb.drag = 0;
        isRegrabable = false;
        Invoke("LightCanBeRegrabed", 2f);
        gotLight = false;
        LightObject.transform.parent = null;
        lightRb.isKinematic = false;
        lightRb.useGravity = true;
        Vector3 ejectionDirection = new Vector3(Random.Range(ejectionDistance, -ejectionDistance), ejectionHeight, Random.Range(ejectionDistance, -ejectionDistance));
        lightRb.AddForce(ejectionDirection);
        TakeHit();
        ArianeBond();
    }
    public void GetLight()
    {
        if (isRegrabable)
        {
            charMaterial.SetFloat("_EmissiveIntensity", maxEmissionIntensity);
            if (LightObject.GetComponent<LightDetection>().IsInAGodRay)
            {
                anim.SetBool("GetOnStatue", true);
            }
            else
            {
                anim.SetBool("getLight", true);
            }
            mesh.enabled = false;
            playerMovement.moveSpeed = 0f;
            myCollider.isTrigger = true;
            gotLight = true;
            lightRb.isKinematic = true;
            lightRb.useGravity = false;
            LightObject.transform.position = lightAnchor.position;
            LightObject.transform.parent = lightAnchor;
            Invoke("AnimatorSetter", 0.2f);
            /*foreach (Collider hitcol in Physics.OverlapSphere(transform.position, 1000f, cloneDetection)) // crée une sphere de detection
            {
                hitcol.GetComponent<SimpleAI>().DestroyClone();
            }*/
            LightObject.GetComponent<LightDetection>().activeMagnetism = false;
            Destroy(vfxBondClone.gameObject);
        }
    }
    public void LightCanBeRegrabed()
    {
        isRegrabable = true;
        LightObject.transform.DOPause();
        LightObject.GetComponent<LightDetection>().activeMagnetism = true;
        // lightRb.drag = 0;
    }
    void Aiming()
    {
        isAimingLight = true;
        playerMovement.rotationSpeed = playerMovement.rotationSpeed / 10;
    }
    void ManageReticule()
    {
        playerMovement.moveSpeed = speedWhileAiming;

        if (playerMovement.isInRotation && gotLight)
        {
            playerMovement.moveSpeed = 0f;
        }

        Vector3 playerPosition = this.transform.position;

        if (start == null)
        {
            start = Instantiate(rangeStart) as GameObject;
            start.transform.position = playerPosition + (Vector3.up / 2) + (transform.forward * 1.5f);
        }

        if (end == null)
        {
            end = Instantiate(rangeEnd) as GameObject;
            //end.transform.parent = this.transform;
            cursor = playerPosition + (transform.forward * 1.5f);
            end.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        }

        if (range == null)
        {
            range = Instantiate(rangeUnderCrosshair) as GameObject;
            range.transform.position = playerPosition + (Vector3.up / 2) + (transform.forward * 1.5f);
        }

        if (end != null)
        {
            if (cursor == Vector3.zero)
            {
                cursor = transform.position;
            }

            if (!playWithMouse)
            {
                Vector3 cursorDirection = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
                cursorDirection *= aimingSpeed * Time.deltaTime;
                Vector3 cursorExpectedPosition = cursor + cursorDirection;

                Vector3 toCursor = cursorExpectedPosition - transform.position;
                toCursor = Vector3.ClampMagnitude(toCursor, maxRange);
                cursor = transform.position + toCursor;

            }
            if (playWithMouse)
            {


                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                Plane plane = new Plane(Vector3.up, transform.position);
                float distance;
                if (plane.Raycast(ray, out distance))
                {
                    Vector3 target = ray.GetPoint(distance);
                    Vector3 direction = target - transform.position;
                    float rotation = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.Euler(0, rotation, 0);

                    direction = Vector3.ClampMagnitude(direction, maxRange);
                    cursor = transform.position + direction;
                }
            }

            end.transform.position = cursor + (Vector3.up / 2);

        }

        if (range != null)
        {
            RaycastHit hit;
            if (Physics.Raycast(cursor, -Vector3.up, out hit, Mathf.Infinity))
            {
                float dist = hit.distance;
                Debug.DrawRay(cursor, -Vector3.up * hit.distance, Color.red);

                range.transform.DOScaleY(dist, 0.2f);
            }
            range.transform.position = cursor;
        }

        if (start != null)
        {
            float dist = Vector3.Distance(playerPosition, cursor);
            start.transform.localScale = new Vector3(start.transform.localScale.x, start.transform.localScale.y, -dist + ((end.transform.localScale.z / 100) / 2));

            Vector3 dir = cursor - transform.position;
            float yAngle = Vector3.SignedAngle(dir, Vector3.forward, Vector3.up);

            start.transform.position = playerPosition + (Vector3.up / 2);
            start.transform.rotation = Quaternion.Euler(0f, -yAngle, 0f);
        }

        if (Vector3.Distance(end.transform.position, transform.position) < maxRange)
        {
            playerMovement.moveSpeed = 0f;
        }
        else
        {
            playerMovement.moveSpeed = speedWhileAiming;
        }



        #region OLD Reticule Avec Zoom
        /*
        //Build start and end range indicator in order to use them
        if (start == null)
        {
            start = Instantiate(rangeStart) as GameObject;
            start.transform.parent = this.transform;
            start.transform.localPosition = Vector3.up;
            start.transform.localScale = new Vector3(1f, 5f, 1f);
        }
        if (end == null)
        {
            end = Instantiate(rangeEnd) as GameObject;
            end.transform.parent = this.transform;
            end.transform.localPosition = Vector3.zero;
        }
        float endWidth = 0f;
        if (end != null)
            endWidth = end.GetComponent<SpriteRenderer>().bounds.size.x;
        //End building start and end range indicator

        float currentYSize;
        float minRange = 2f;
        currentYSize = start.transform.localScale.y;

        float triggers;
        float mouseScroll;
        triggers = Input.GetAxis("Triggers");
        mouseScroll = Input.GetAxis("Mouse ScrollWheel");

        if (triggers > 0 || mouseScroll> 0 )
        {
            if (currentYSize <= maxRange)
            {
                if (!playWithMouse)
                {
                    start.transform.localScale += new Vector3(0f, aimingSpeed * Time.deltaTime, 0f);
                }
                else
                {
                    start.transform.localScale += new Vector3(0f, aimingSpeed * speedMultiplierWithMouse * Time.deltaTime, 0f);
                }
            }
        }

        if (triggers < 0 || mouseScroll <0)
        {
            if (currentYSize >= minRange)
            {
                if (!playWithMouse)
                {
                    start.transform.localScale -= new Vector3(0f, aimingSpeed * Time.deltaTime, 0f);
                }
                else
                {
                    start.transform.localScale -= new Vector3(0f, aimingSpeed * speedMultiplierWithMouse * Time.deltaTime, 0f);
                }
            }
        }

        //Set the scale of the range indicator
        //start.transform.localScale += new Vector3(0f, aimingSpeed * Time.deltaTime, 0f);


        //Set the position and rotation of the range indicator
        start.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);

        if (end != null)
        {
            end.transform.localPosition = new Vector3(0f, 1f, start.transform.localScale.y);
            end.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
        }*/
        #endregion

        #region OLD
        /*if (reachedMaxRange == false)
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
        }*/
        #endregion

        if (Input.GetButtonUp("Throw") || Input.GetMouseButtonUp(1))
        {
            //Physics alterations
            isThrown = true;
            mesh.enabled = true;

            anim.SetBool("isAiming", false);
            anim.SetBool("launch", true);
            playerMovement.rotationSpeed = baseRotationSpeed;
            ThrowLight();

            Destroy(start);
            Destroy(end);
            Destroy(range);

            playerMovement.moveSpeed = baseSpeed;

            reachedMaxRange = false;

        }
    }
    void ThrowLight()
    {
        charMaterial.SetFloat("_EmissiveIntensity", minEmissionIntensity);

        lightRb.isKinematic = false;
        lightRb.useGravity = true;

        myCollider.isTrigger = false;

        isRegrabable = false;
        lightRb.drag = 0;
        Invoke("LightCanBeRegrabed", timeBeforeFallAndRegrabable);
        //resetspeed
        //playerMovement.moveSpeed = baseSpeed;

        //Components modifications
        LightObject.transform.parent = null;
        gotLight = false;


        //reset modifications
        isAimingLight = false;

        //LightObject.transform.DOJump(end.transform.position, 2, 1, 1.5f); //-- CA PERMET DE FAIRE JUMP LA LUMIERE SI BESOIN --

        if (LightObject.transform.parent == null)
        {
            LightObject.transform.DOMove(end.transform.position, lightSpeed * Time.deltaTime);
            SendMessage("Play");
        }
        ArianeBond();
        #region OLD
        /*if (teleport)
        {
            LightObject.transform.position = new Vector3(lastPosReticule.x, lastPosReticule.y + 1, lastPosReticule.z);
        }
        else
        {
            LightObject.transform.position = transform.position + transform.forward;
            lightRb.AddForce(transform.forward * throwHorizontalSpeed + Vector3.up * throwVerticalSpeed);
        }*/
        #endregion
    }


    public void EnableControls()
    {
        controlsAreEnabled = true;
    }
    public void DisableControls()
    {
        controlsAreEnabled = false;
        anim.SetBool("isAiming", false);
        anim.SetBool("launch", false);
        anim.SetBool("GetOnStatue", false);
        anim.SetBool("getLight", false);
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
    #region VFX Management
    private void ArianeBond()
    {
        vfxBondClone = Instantiate(vfxBond, lightAnchor.position, Quaternion.identity);
    }
    #endregion

}
