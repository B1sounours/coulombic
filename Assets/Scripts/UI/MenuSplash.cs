using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MenuSplash : MonoBehaviour
{
    public GameObject cameraCenter;

    private GameObject cubePrefab, spherePrefab;
    private List<GameObject> allChargedObjects;
    private GameObject splashObjectsContainer;
    private int splashObjectCount = 50;

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

    private MovingChargedObject MakeMovingChargedObject(float charge, float mass, Vector3 position)
    {
        return MakeMovingChargedObject(charge, mass, position, spherePrefab);
    }

    private MovingChargedObject MakeMovingChargedObject(float charge, float mass, Vector3 position, GameObject prefab)
    {
        GameObject go = MakeChargedObject(spherePrefab, charge, position);
        MovingChargedObject mco = go.AddComponent<MovingChargedObject>();
        mco.mass = mass;
        return mco;
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
        
        int r = Random.Range(0, 6);

        Vector3 center = transform.position;
        if (r == 0)
            MakeMolecules(center);
        else if (r == 1)
            MakeChaos(center);
        else if (r == 2)
            MakeNetDisruption(center);
        else if (r == 3)
            MakeRandomBlanket(center);
        else if (r == 4)
            MakeSnake(center);
        else if (r == 5)
            MakeModuloBlanket(center);
    }

    private void MakeModuloBlanket(Vector3 center)
    {
        Debug.Log("MakeModuloBlanket");
        int size = Mathf.FloorToInt(Mathf.Sqrt((float)splashObjectCount));
        float charge = 3;
        int moduloOffset = Random.Range(2, 5);
        float spreadScale = 3;
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                if ((i + j) % moduloOffset == 0)
                    charge *= -1;
                Vector3 position = center + new Vector3(i - size / 2, 0, j - size / 2) * spreadScale;
                MakeMovingChargedObject(charge, 1, position);
            }
        }
    }

    private void MakeMolecules(Vector3 center)
    {
        Debug.Log("MakeMolecules");
        int makeCounter = 0;
        int sign=Random.Range(1,3)*2-3;
        int orbitSpacing = 2;
        while (makeCounter < splashObjectCount)
        {
            Vector3 newOffset = GetRandomOffset();

            int orbitRows=Random.Range(1,5);
            makeCounter++;
            MakeMovingChargedObject(-sign * Random.Range(10, 15), Random.Range(10, 30), center + newOffset);
            for (int i=-2;i<5;i++)
                for(int j=-1;j<2;j+=2)
                    for (int k = -1; k < 2; k += 2)
                    {
                        Vector3 point = center + newOffset + new Vector3(i * orbitSpacing, j * orbitSpacing, k * orbitSpacing);
                        if (makeCounter < splashObjectCount)
                        {
                            makeCounter++;
                            MakeMovingChargedObject(sign, 1, point);
                        }
                    }
        }
        MakeMovingChargedObject(sign * Random.Range(5, 60), Random.Range(30, 50), center+GetRandomOffset());

    }

    private void MakeChaos(Vector3 center)
    {
        Debug.Log("MakeChaos");
        int width = 4;
        int height = splashObjectCount/( width*width);
        int spread = 5;
        Vector3 loopOffset = new Vector3((float)width / 2, (float)height / 2, (float)width / 2) * spread;

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < width; j++)
            {
                for (int k = 0; k < height; k++)
                {
                    float charge = Random.Range(-5, 5);
                    if (Random.Range(1, 5) == 1)
                        charge *= 3;

                    float mass = Random.Range(1, 5);
                    if (Random.Range(1, 5) == 1)
                        charge *= 5;

                    Vector3 position = center + new Vector3(i * spread, j * spread, k * spread) - loopOffset;

                    if (Random.Range(1, 10) == 1)
                        MakeChargedObject(cubePrefab, charge, position);
                    else
                        MakeMovingChargedObject(charge, mass, position);
                }
            }
        }
    }

    private void MakeNetDisruption(Vector3 center)
    {
        Debug.Log("MakeNetDisruption");
        MakeNet(center, 5, 5, 3);

        int spread = 5;
        for (int i = -1; i < 2; i += 2)
        {
            for (int j = -1; j < 2; j += 2)
            {
                int height = Random.Range(60, 130);
                Vector3 position = center + new Vector3(i * spread, height, j * spread);
                MovingChargedObject mco = MakeMovingChargedObject(GetRandomNonZero(5) * 15, Random.Range(20, 100), position);
                mco.startVelocity = new Vector3(0, -Random.Range(2, 5), 0);
            }
        }
    }

    private void MakeSnake(Vector3 center)
    {
        Debug.Log("MakeSnake");
        List<Vector3> usedPoints = new List<Vector3>();
        Vector3 headPoint = Vector3.zero;
        int makeCounter = 0;
        int whileCounter = 0;
        float charge = 3;

        while (makeCounter < splashObjectCount && whileCounter < splashObjectCount * 100)
        {
            whileCounter++;
            Vector3 newPoint = headPoint + GetRandomDirection();
            if (!usedPoints.Contains(newPoint))
            {
                headPoint = newPoint;
                MakeMovingChargedObject(charge, 1, headPoint + center);
                charge *= -1;
                makeCounter++;
                usedPoints.Add(newPoint);
            }
        }

        MakeMovingChargedObject(30, 10, headPoint * -1 + center);
    }

    private float GetRandomNonZero(int max)
    {
        float charge = Random.Range(-max, max + 1);
        if (charge == 0)
            charge = 1;
        return charge;
    }

    private void MakeRandomBlanket(Vector3 center)
    {
        Debug.Log("MakeRandomBlanket");
        //makes a 2D grid with random charges
        int size = Mathf.FloorToInt(Mathf.Sqrt((float)splashObjectCount));
        float charge = 0;
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                charge = GetRandomNonZero(4);
                Vector3 position = center + new Vector3(i - size / 2, 2, j - size / 2);
                MakeMovingChargedObject(charge, 1, position);
            }
        }
    }

    private void MakeNet(Vector3 center, float chargeMagnitude, float mass, float spreadScale)
    {
        //makes a coherent grid that will stick together
        Debug.Log("MakeNet");
        int size = Mathf.FloorToInt(Mathf.Sqrt((float)splashObjectCount));
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                float charge = (i + j) % 2 == 0 ? chargeMagnitude : -chargeMagnitude;
                Vector3 position = center + new Vector3(i - size / 2, 0, j - size / 2) * spreadScale;
                MakeMovingChargedObject(charge, mass, position);
            }
        }
    }

    private Vector3 GetRandomDirection()
    {
        int sign = Random.Range(1, 3) * 2 - 3;
        int r = Random.Range(1, 4);
        if (r == 1)
            return new Vector3(sign, 0, 0);
        if (r == 2)
            return new Vector3(0, sign, 0);
        return new Vector3(0, 0, sign);
    }

    private Vector3 GetRandomOffset()
    {
        int maximum = 5;
        Vector3 offset = new Vector3(Random.Range(-maximum, maximum), Random.Range(-maximum, maximum), Random.Range(-maximum, maximum));
        return offset;
    }

    void Update()
    {
        cameraCenter.transform.Rotate(gameObject.transform.position, Time.deltaTime * 7);
    }
}
