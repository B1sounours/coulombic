using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(ChargedObject))]
public class MovingChargedObject : MonoBehaviour
{
    public float mass = 1;
    public Vector3 startVelocity;
    private Rigidbody rigidbody;
    private ChargedObject chargedObject;

    void Start()
    {
        if (transform.parent.gameObject.GetComponent<RegionManager>() == null)
            Debug.LogError("MovingChargedObject not the child of any RegionManager.");
        GetRigidbody().velocity = startVelocity;
        GetRigidbody().mass = mass;
        UpdateSize();
    }

    public void UpdateSize()
    {
        float radius = Mathf.Pow(mass, 0.3333f);
        transform.localScale = new Vector3(1, 1, 1) * radius;
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
        if (rigidbody != null)
            rigidbody.AddForce(force);
    }

    public Rigidbody GetRigidbody()
    {
        if (rigidbody == null)
        {
            if (GetComponent<Rigidbody>() == null)
                gameObject.AddComponent<Rigidbody>();
            rigidbody = GetComponent<Rigidbody>();
            if (mass <= 0)
                Debug.LogError("mass is below zero. " + mass);
            rigidbody.mass = mass;
            rigidbody.useGravity = false;
        }
        return rigidbody;
    }
}
