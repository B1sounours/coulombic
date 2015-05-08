using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RegionManager : MonoBehaviour
{
    private List<ChargedObject> chargedObjects;
    private List<MovingChargedObject> movingChargedObjects;

    void Start()
    {
        FindChargedObjects();
        StartAllCoroutines();
    }

    private void FindChargedObjects()
    {
        chargedObjects = new List<ChargedObject>();
        movingChargedObjects = new List<MovingChargedObject>();
        foreach (GameObject go in ParentChildFunctions.GetAllChildren(gameObject, false))
        {
            ChargedObject co=go.GetComponent<ChargedObject>();
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
    }

    private IEnumerator Cycle(MovingChargedObject mco)
    {
        bool isFirst = true;
        while (true)
        {
            if (isFirst)
            {
                isFirst = false;
                yield return new WaitForSeconds(Random.Range(0, GameSettings.magnetPhysicsInterval));
            }

            ApplyMagneticForce(mco);
            yield return new WaitForSeconds(GameSettings.magnetPhysicsInterval);
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
            float force = 1000* mco.GetCharge() * chargedObject.charge / Mathf.Pow(distance, 2);
            Vector3 direction;

            direction = mco.transform.position - chargedObject.transform.position;
            direction.Normalize();

            newForce += force * direction * GameSettings.magnetPhysicsInterval;
        }

        mco.AddForce(newForce);
    }

}
