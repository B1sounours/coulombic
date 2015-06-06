﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RegionManager : MonoBehaviour
{
    public bool isSplashMenu = false;
    private List<ChargedObject> chargedObjects;
    private List<MovingChargedObject> movingChargedObjects;
    private bool isInitialized = false;
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
            isInitialized = true;
        }
        if (!hasAppliedStartVelocity && (isSplashMenu || !GameManager.GetGameManager().GetIsPaused()))
            ApplyStartVelocities();
    }

    private List<ChargedObject> GetChargedObjects()
    {
        if (chargedObjects == null)
            chargedObjects = new List<ChargedObject>();
        return chargedObjects;
    }

    private List<MovingChargedObject> GetMovingChargedObjects()
    {
        if (movingChargedObjects == null)
            movingChargedObjects = new List<MovingChargedObject>();
        return movingChargedObjects;
    }

    private void ApplyStartVelocities()
    {
        hasAppliedStartVelocity = true;
        foreach (MovingChargedObject mco in GetMovingChargedObjects())
            mco.ApplyStartVelocity();
    }

    public void AddChargedObject(ChargedObject chargedObject)
    {
        GetChargedObjects().Add(chargedObject);
        chargedObject.UpdateAppearance();
        MovingChargedObject mco=chargedObject.gameObject.GetComponent<MovingChargedObject>();
        if (mco != null)
        {
            GetMovingChargedObjects().Add(mco);
            if (hasAppliedStartVelocity)
                mco.ApplyStartVelocity();
            StartCoroutine(Cycle(mco));
        }
    }

    public void DeleteChargedObject(ChargedObject chargedObject)
    {
        GameObject go = chargedObject.gameObject;
        MovingChargedObject mco = go.GetComponent<MovingChargedObject>();
        GetChargedObjects().Remove(chargedObject);
        if (mco != null)
            GetMovingChargedObjects().Remove(mco);
        Destroy(go);
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

        return regionManager;
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
            if (GetMovingChargedObjects().Contains(movingChargedObject))
                GetMovingChargedObjects().Remove(movingChargedObject);
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
        foreach (GameObject go in ParentChildFunctions.GetAllChildren(gameObject, false))
        {
            ChargedObject co = go.GetComponent<ChargedObject>();
            if (co != null)
                AddChargedObject(co);
        }
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
                if (isSplashMenu || !GameManager.GetGameManager().GetIsPaused())
                    ApplyMagneticForce(mco);
                yield return new WaitForSeconds(GameSettings.magnetInterval);
            }
        }
    }

    private void ApplyMagneticForce(MovingChargedObject mco)
    {
        Vector3 newForce = new Vector3(0, 0, 0);

        foreach (ChargedObject chargedObject in GetChargedObjects())
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
