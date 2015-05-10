using UnityEngine;
using System.Collections;

public class ScoreWall : MonoBehaviour
{

    public float velocityMultiplier = 1;
    public float massMultiplier = 0;
    public float chargeMultiplier = 0;

    private float score = 0;
    private RegionManager regionManager;
    private static GameObject particlesPrefab;
    private static AudioClip[] lightningSounds;

    void Start()
    {
        SetAllChildren();
    }

    private void SetAllChildren()
    {
        foreach (GameObject child in ParentChildFunctions.GetAllChildren(gameObject, false))
        {
            if (child.GetComponent<Collider>()!=null){
                ScoreWall scoreWall = child.AddComponent<ScoreWall>();
                scoreWall.velocityMultiplier = velocityMultiplier;
                scoreWall.massMultiplier = massMultiplier;
                scoreWall.chargeMultiplier = chargeMultiplier;
            }
        }

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

    private RegionManager GetRegionManager()
    {
        Transform transformParent = transform.parent;
        while (regionManager == null && transformParent != null)
        {
            if (transformParent.gameObject.GetComponent<RegionManager>() != null)
            {
                regionManager = transformParent.GetComponent<RegionManager>();
                break;
            }
            transformParent = transformParent.parent;
        }

        if (regionManager == null)
            Debug.LogError("ScoreWall must be the child of a RegionManager.");
        return regionManager;
    }

    private void AddScore(Collision collision)
    {
        float points = 0;
        points += collision.relativeVelocity.magnitude * velocityMultiplier;

        Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
        if (rb != null)
            points += rb.mass * massMultiplier;

        ChargedObject co = collision.gameObject.GetComponent<ChargedObject>();
        if (co != null)
            points += co.charge * chargeMultiplier;

        score += points;
    }

    private void MakeLightning(Vector3 position)
    {
        GameObject particles = Instantiate(GetParticlesPrefab(), position, transform.rotation) as GameObject;
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
            MakeLightning(collision.transform.position);
            GetRegionManager().DestroyChargedObject(chargedObject);
        }
    }
}
