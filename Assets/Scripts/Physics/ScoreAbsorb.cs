using UnityEngine;
using System.Collections;

public class ScoreAbsorb : MonoBehaviour
{

    public float velocityMultiplier = 0;
    public float massMultiplier = 0;
    public float chargeMultiplier = 0;
    public float positiveChargeMultiplier = 0;
    public float negativeChargeMultiplier = 0;
    public float baseValue = 1;
    public bool startVisible = false;

    private float score = 0;
    private RegionManager regionManager;
    private static GameObject particlesPrefab;
    private static AudioClip[] lightningSounds;
    private static GameObject particlesContainer;

    void Start()
    {
        SetAllChildren();
        SetRenderVisibility(startVisible);
    }

    private void SetRenderVisibility(bool isVisibile)
    {
        foreach (GameObject child in ParentChildFunctions.GetAllChildren(gameObject, true))
        {
            if (child.GetComponent<Renderer>() != null)
                child.GetComponent<Renderer>().enabled = isVisibile;
        }
    }

    private void SetAllChildren()
    {
        foreach (GameObject child in ParentChildFunctions.GetAllChildren(gameObject, false))
        {
            if (child.GetComponent<Collider>() != null)
            {
                ScoreAbsorb scoreAbsorb = child.AddComponent<ScoreAbsorb>();
                scoreAbsorb.velocityMultiplier = velocityMultiplier;
                scoreAbsorb.massMultiplier = massMultiplier;
                scoreAbsorb.chargeMultiplier = chargeMultiplier;
                scoreAbsorb.positiveChargeMultiplier = positiveChargeMultiplier;
                scoreAbsorb.negativeChargeMultiplier = negativeChargeMultiplier;
                scoreAbsorb.baseValue = baseValue;
                scoreAbsorb.startVisible = startVisible;
            }
        }
    }

    private GameObject GetParticlesContainer()
    {
        if (particlesContainer == null)
            particlesContainer = new GameObject("particles container");
        return particlesContainer;
    }

    public float GetScore()
    {
        return score;
    }

    private static AudioClip GetRandomLightningSound()
    {
        if (lightningSounds == null)
        {
            lightningSounds = new AudioClip[3];
            for (int i = 0; i < 3; i++)
                lightningSounds[i] = Resources.Load("lightning" + (i + 1)) as AudioClip;
        }
        return lightningSounds[Random.Range(0, 3)];
    }

    private static GameObject GetParticlesPrefab()
    {
        if (particlesPrefab == null)
            particlesPrefab = Resources.Load("prefabs/lightning") as GameObject;
        return particlesPrefab;
    }

    public RegionManager GetRegionManager()
    {
        if (regionManager == null)
            regionManager = RegionManager.GetMyRegionManager(gameObject);
        return regionManager;
    }

    private void AddScore(Collision collision)
    {
        float points = baseValue;
        points += collision.relativeVelocity.magnitude * velocityMultiplier;

        Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
        if (rb != null)
            points += rb.mass * massMultiplier;

        ChargedObject co = collision.gameObject.GetComponent<ChargedObject>();
        if (co != null)
        {
            points += co.charge * chargeMultiplier;
            if (co.charge > 0)
                points += co.charge * positiveChargeMultiplier;
            if (co.charge < 0)
                points += co.charge * negativeChargeMultiplier;
        }

        score += points;
    }

    private void MakeLightning(Vector3 position)
    {
        GameObject particles = Instantiate(GetParticlesPrefab(), position, transform.rotation) as GameObject;
        particles.transform.parent = GetParticlesContainer().transform;
        float lifetime = particles.GetComponent<ParticleSystem>().duration + particles.GetComponent<ParticleSystem>().startLifetime;
        Destroy(particles, lifetime);

        float volume = 100 / Mathf.Pow(Vector3.Distance(Camera.main.gameObject.transform.position, position), 2);
        volume = Mathf.Clamp(volume, 0, 1);
        AudioSource.PlayClipAtPoint(GetRandomLightningSound(), position, volume);
    }

    void OnCollisionEnter(Collision collision)
    {
        ChargedObject chargedObject = collision.gameObject.GetComponent<ChargedObject>();
        if (chargedObject != null)
        {
            AddScore(collision);
            SetRenderVisibility(true);
            MakeLightning(collision.transform.position);
            GetRegionManager().DestroyChargedObject(chargedObject);
        }
    }
}
