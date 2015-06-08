using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(ChargedObject))]
public class MovingChargedObject : MonoBehaviour
{
    public float mass = 1;
    public Vector3 startVelocity;
    //private Rigidbody rigidbody;
    private ChargedObject chargedObject;

    private GameObject vectorGameObject;
    private static GameObject vectorPrefab;

    void Start()
    {
        //throws a LogError if no region manager
        RegionManager.GetMyRegionManager(gameObject);

        GetRigidbody().velocity = startVelocity;
        GetRigidbody().mass = mass;
    }

    public void SetFrozenPosition(bool isFrozen)
    {
        GetRigidbody().constraints = isFrozen ? RigidbodyConstraints.FreezeAll : RigidbodyConstraints.None;
    }

    public void UpdateValues(ChargedObjectSettings chargedObjectSettings)
    {
        mass = chargedObjectSettings.mass;
        startVelocity = chargedObjectSettings.startVelocity;
        UpdateAppearance();
    }

    public void ApplyStartVelocity()
    {
        //if this is done while game is paused, it may not work
        if (startVelocity.sqrMagnitude > 0)
            gameObject.GetComponent<Rigidbody>().velocity = startVelocity;

        if (vectorGameObject != null)
            Destroy(vectorGameObject);
    }

    public void UpdateAppearance()
    {
        //size
        float radius = Mathf.Pow(mass, 0.3333f);
        transform.localScale = new Vector3(1, 1, 1) * radius;

        //vector arrow
        if (ShouldShowVelocityVector())
        {
            vectorGameObject = Instantiate(GetVectorPrefab(), transform.position, transform.rotation) as GameObject;
            vectorGameObject.transform.SetParent(transform);
            vectorGameObject.transform.rotation = Quaternion.LookRotation(startVelocity.normalized);
            float sidewaysScale = radius/2;
            float lengthScale = Mathf.Pow(startVelocity.magnitude,0.3333f);
            vectorGameObject.transform.GetChild(0).localScale = new Vector3(sidewaysScale, sidewaysScale, lengthScale);
        }
    }

    public bool ShouldShowVelocityVector()
    {
        return GameManager.GetGameManager() != null && !GameManager.GetGameManager().HasSimulationBegun() && startVelocity.sqrMagnitude > 0;
    }

    public ChargedObject GetChargedObject()
    {
        if (chargedObject == null)
            chargedObject = GetComponent<ChargedObject>();
        return chargedObject;
    }

    public float GetCharge()
    {
        return GetChargedObject().charge;
    }

    public void AddForce(Vector3 force)
    {
        if (GetComponent<Rigidbody>() != null)
            GetComponent<Rigidbody>().AddForce(force);
    }

    public Rigidbody GetRigidbody()
    {
        if (GetComponent<Rigidbody>() == null)
        {
            if (GetComponent<Rigidbody>() == null)
                gameObject.AddComponent<Rigidbody>();
            if (mass <= 0)
                Debug.LogError("mass is below zero. " + mass);
            GetComponent<Rigidbody>().mass = mass;
            GetComponent<Rigidbody>().useGravity = false;
        }
        return GetComponent<Rigidbody>();
    }

    private GameObject GetVectorPrefab()
    {
        if (vectorPrefab == null)
            vectorPrefab = Resources.Load("prefabs/vector_arrow") as GameObject;
        return vectorPrefab;
    }
}
