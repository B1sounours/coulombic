using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RegionManager : MonoBehaviour
{
    private List<ChargedObject> chargedObjects;
    private List<MovingChargedObject> movingChargedObjects;
    private bool isInitialized = false;

    void Update()
    {
        if (!isInitialized && IsCloningDone())
        {
            FindChargedObjects();
            StartAllCoroutines();
            isInitialized = true;
        }
    }

    private bool IsCloningDone()
    {
        //returns true if some objects in this region are not cloned yet
        foreach (GameObject go in ParentChildFunctions.GetAllChildren(gameObject, false))
            if (go.GetComponent<CloneGenerator>() != null)
                return false;
        return true;
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
        Debug.Log("found " + chargedObjects.Count + " charged");
        Debug.Log("found " + movingChargedObjects.Count + " moving charged");
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
                Debug.Log("smooth: " + Time.smoothDeltaTime + " old: " + GameSettings.magnetInterval + " new:" + newInterval+" ratio:"+ratio);
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

            ApplyMagneticForce(mco);
            yield return new WaitForSeconds(GameSettings.magnetInterval);
        }
    }

    private void ApplyMagneticForce(MovingChargedObject mco)
    {
        Vector3 newForce = new Vector3(0, 0, 0);
        foreach (ChargedObject chargedObject in chargedObjects)
        {
            if (mco.GetChargedObject() == chargedObject)
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
