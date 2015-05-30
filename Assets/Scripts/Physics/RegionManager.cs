using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RegionManager : MonoBehaviour
{
    public bool isSplashMenu = false;
    private List<ChargedObject> chargedObjects;
    private List<MovingChargedObject> movingChargedObjects;
    private bool isInitialized = false;
    private GameManager gameManager;
    private bool hasAppliedStartVelocity = false;

    void Start()
    {
        foreach (MovingChargedObject mco in FindObjectsOfType<MovingChargedObject>())
            mco.UpdateSize();
    }

    void Update()
    {
        if (!isInitialized && AllChargedObjectsAreGenerated())
        {
            FindChargedObjects();
            StartAllCoroutines();
            isInitialized = true;
        }
        if (!hasAppliedStartVelocity && (isSplashMenu || !GetGameManager().GetIsPaused()))
            ApplyStartVelocities();
    }

    private void ApplyStartVelocities()
    {
        hasAppliedStartVelocity = true;
        foreach (MovingChargedObject mco in movingChargedObjects)
        {
            if (mco.startVelocity.sqrMagnitude > 0)
                mco.gameObject.GetComponent<Rigidbody>().velocity = mco.startVelocity;
        }
    }

    public static RegionManager GetMyRegionManager(GameObject childObject)
    {
        RegionManager regionManager = null;
        Transform transformParent = childObject.transform.parent;
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

    private GameManager GetGameManager()
    {
        if (gameManager == null)
            gameManager = FindObjectOfType<GameManager>();
        return gameManager;
    }

    private bool AllChargedObjectsAreGenerated()
    {
        //returns true if some objects in this region are not cloned yet
        foreach (GameObject go in ParentChildFunctions.GetAllChildren(gameObject, true))
        {
            if (go.GetComponent<CloneGenerator>() != null)
                return false;
        }
        return true;
    }

    public void DestroyChargedObject(ChargedObject chargedObject)
    {
        MovingChargedObject movingChargedObject = chargedObject.gameObject.GetComponent<MovingChargedObject>();
        if (movingChargedObject != null)
        {
            if (movingChargedObjects.Contains(movingChargedObject))
                movingChargedObjects.Remove(movingChargedObject);
            else
                Debug.LogError("DestroyChargedObject called but RegionManager does not have this mco.");
        }
        chargedObjects.Remove(chargedObject);

        Destroy(chargedObject.gameObject);
        Destroy(chargedObject);
        Destroy(movingChargedObject);
    }

    private void FindChargedObjects()
    {
        chargedObjects = new List<ChargedObject>();
        movingChargedObjects = new List<MovingChargedObject>();
        foreach (GameObject go in ParentChildFunctions.GetAllChildren(gameObject, false))
        {
            ChargedObject co = go.GetComponent<ChargedObject>();
            if (co != null)
            {
                chargedObjects.Add(co);
                co.UpdateAppearance();
                if (go.GetComponent<MovingChargedObject>() != null)
                    movingChargedObjects.Add(go.GetComponent<MovingChargedObject>());
            }
        }
        //Debug.Log("found " + chargedObjects.Count + " charged");
        //Debug.Log("found " + movingChargedObjects.Count + " moving charged");
    }

    private void StartAllCoroutines()
    {
        foreach (MovingChargedObject mco in movingChargedObjects)
            StartCoroutine(Cycle(mco));
        //StartCoroutine(RecalculateMagnetInterval());
    }


    private IEnumerator RecalculateMagnetInterval()
    {
        while (true)
        {
            if (Time.smoothDeltaTime > 0)
            {
                float ratio = Time.smoothDeltaTime * GameSettings.targetFPS;
                float newInterval = GameSettings.magnetInterval * ratio;
                Debug.Log("smooth: " + Time.smoothDeltaTime + " old: " + GameSettings.magnetInterval + " new:" + newInterval + " ratio:" + ratio);
                newInterval = Mathf.Clamp(newInterval, GameSettings.minimumMagnetInterval, 1);
                GameSettings.magnetInterval = newInterval;
            }
            yield return new WaitForSeconds(1);
        }

    }

    private IEnumerator Cycle(MovingChargedObject mco)
    {
        bool isFirst = true;
        while (true)
        {
            if (isFirst)
            {
                isFirst = false;
                yield return new WaitForSeconds(Random.Range(0, GameSettings.magnetInterval));
            }

            if (mco == null)
            {
                break;
            }
            else
            {
                if (isSplashMenu || !GetGameManager().GetIsPaused())
                    ApplyMagneticForce(mco);
                yield return new WaitForSeconds(GameSettings.magnetInterval);
            }
        }
    }

    private void ApplyMagneticForce(MovingChargedObject mco)
    {
        Vector3 newForce = new Vector3(0, 0, 0);

        foreach (ChargedObject chargedObject in chargedObjects)
        {
            if (mco.GetChargedObject() == chargedObject || chargedObject.ignoreOtherMovingChargedObjects)
                continue;

            float distance = Vector3.Distance(mco.transform.position, chargedObject.gameObject.transform.position);
            float force = 1000 * mco.GetCharge() * chargedObject.charge / Mathf.Pow(distance, 2);
            Vector3 direction;

            direction = mco.transform.position - chargedObject.transform.position;
            direction.Normalize();

            newForce += force * direction * GameSettings.magnetInterval;
        }

        mco.AddForce(newForce);
    }

}
