using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BondCylinder : MonoBehaviour
{
    [SerializeField]
    private Transform cylinderPrefab;
    public bool isEmitting;
    public Transform player;
    public Transform lantern;
    private GameObject cylinder;
    private MeshRenderer mesh;

    private void Start()
    {
        InstantiateCylinder(cylinderPrefab, player.transform.position, lantern.transform.position);
    }

    private void Update()
    {
        if (isEmitting)
        {
            UpdateCylinderPosition(cylinder, player.transform.position, lantern.transform.position);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            ShrinkMesh(0.1f);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            ExtendMesh(0.1f);
        }

    }

    private void InstantiateCylinder(Transform cylinderPrefab, Vector3 beginPoint, Vector3 endPoint)
    {
        cylinder = Instantiate<GameObject>(cylinderPrefab.gameObject, Vector3.zero, Quaternion.identity);
        mesh = cylinder.GetComponent<MeshRenderer>();

        UpdateCylinderPosition(cylinder, beginPoint, endPoint);
    }

    private void UpdateCylinderPosition(GameObject cylinder, Vector3 beginPoint, Vector3 endPoint)
    {
        Vector3 offset = endPoint - beginPoint;
        Vector3 position = beginPoint + (offset / 2.0f);

        cylinder.transform.position = position;
        cylinder.transform.LookAt(beginPoint);
        Vector3 localScale = cylinder.transform.localScale;
        localScale.z = (endPoint - beginPoint).magnitude;
        cylinder.transform.localScale = localScale;
    }

    public void ShrinkMesh(float shrinkFactor)
    {
        cylinder.transform.localScale = new Vector3(cylinder.transform.localScale.x - shrinkFactor, cylinder.transform.localScale.y - shrinkFactor, cylinder.transform.localScale.z);
    }
    public void ExtendMesh(float extendFactor)
    {
        cylinder.transform.localScale = new Vector3(cylinder.transform.localScale.x + extendFactor, cylinder.transform.localScale.y + extendFactor, cylinder.transform.localScale.z);
    }
    public void DisableEffects()
    {
        mesh.enabled = false;
        isEmitting = false;
        //desactivate vfx
        //desactivate interactions
    }
    public void EnableEffects()
    {
        mesh.enabled = true;
        isEmitting = true;
        //activate vfx
        //activate interactions
    }
}
