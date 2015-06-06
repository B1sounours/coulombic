using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MenuSplash : MonoBehaviour
{
    public GameObject cameraCenter;

    private GameObject cubePrefab, spherePrefab;
    private List<GameObject> allChargedObjects;
    private GameObject splashObjectsContainer;

    void Start()
    {
        cubePrefab = Resources.Load<GameObject>("prefabs/cube");
        spherePrefab = Resources.Load<GameObject>("prefabs/sphere");
        splashObjectsContainer = new GameObject("splash objects container");
        splashObjectsContainer.transform.parent = gameObject.transform.parent;

        RestartSplash();
    }

    private GameObject MakeChargedObject(GameObject prefab, float charge, Vector3 position)
    {
        GameObject go = Instantiate(prefab, position, Quaternion.Euler(Vector3.up)) as GameObject;
        allChargedObjects.Add(go);
        go.transform.parent = splashObjectsContainer.transform;
        ChargedObject co = go.AddComponent<ChargedObject>();
        co.charge = charge;
        co.UpdateAppearance();
        return go;
    }

    private void MakeMovingChargedObject(float charge, Vector3 position)
    {
        GameObject go = MakeChargedObject(spherePrefab, charge, position);
        go.AddComponent<MovingChargedObject>();
    }

    private void DestroyAllChargedObjects()
    {
        if (allChargedObjects != null)
            foreach (GameObject go in allChargedObjects)
                Destroy(go);
        allChargedObjects = new List<GameObject>();
    }

    private void RestartSplash()
    {
        DestroyAllChargedObjects();

        int likelyChargeSign = Random.Range(1, 3) == 1 ? 1 : -1;

        List<IntVector3> usedPoints = new List<IntVector3>();
        for (int i = 0; i < 30; i++)
        {
            float charge = GetRandomCharge(likelyChargeSign);

            IntVector3 newPoint = GetRandomPoint();
            while (usedPoints.Contains(newPoint))
                newPoint = GetRandomPoint();

            usedPoints.Add(newPoint);
            Vector3 newPosition = new Vector3(transform.position.x + newPoint.x, transform.position.y + newPoint.y, transform.position.z + newPoint.z);

            if (Random.Range(1, 50) == 1)
                MakeChargedObject(cubePrefab, charge, newPosition);
            else
                MakeMovingChargedObject(charge, newPosition);
        }
    }

    private float GetRandomCharge(int likelyChargeSign)
    {
        float charge = likelyChargeSign;
        if (Random.Range(1, 30) == 1)
            charge *= Random.Range(4, 50);
        else
            charge *= Random.Range(1, 4);
        if (Random.Range(1, 7) == 1)
            charge *= -1;

        return charge;
    }

    private IntVector3 GetRandomPoint()
    {
        int maximum = 8;
        return new IntVector3(Random.Range(-maximum, maximum), Random.Range(-maximum, maximum), Random.Range(-maximum, maximum));
    }

    void Update()
    {
        cameraCenter.transform.Rotate(gameObject.transform.position, Time.deltaTime * 7);
    }
}
