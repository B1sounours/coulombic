using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class ParentChildFunctions
{

    public static ArrayList GetAllChildren(GameObject parentGameObject, bool includeParent = false)
    {
        string[] excludeSubstrings = new string[0];
        return GetAllChildren(parentGameObject, excludeSubstrings, includeParent);
    }

    public static List<GameObject> GetAllParents(GameObject childGameObject)
    {
        List<GameObject> parents = new List<GameObject>();
        while (childGameObject.transform.parent != null)
        {
            GameObject parent = childGameObject.transform.parent.gameObject;
            parents.Add(parent);
            childGameObject = parent;
        }
        return parents;
    }

    public static void SetCollidersOfChildren(GameObject parentGameObject, bool isColliderEnabled, bool includeParent = false)
    {
        foreach (GameObject child in GetAllChildren(parentGameObject, includeParent))
        {
            if (child.GetComponent<MeshCollider>() != null)
                child.GetComponent<MeshCollider>().enabled = isColliderEnabled;
            if (child.GetComponent<Collider>() != null)
                child.GetComponent<Collider>().enabled = isColliderEnabled;
            if (child.GetComponent<SphereCollider>() != null)
                child.GetComponent<SphereCollider>().enabled = isColliderEnabled;
            if (child.GetComponent<BoxCollider>() != null)
                child.GetComponent<BoxCollider>().enabled = isColliderEnabled;
        }
    }

    public static ArrayList GetAllChildren(GameObject parentGameObject, string[] excludeSubstrings, bool includeParent = false)
    {
        //returns an arraylist of all children, grandchildren, etc.
        //excludes all objects and their children if their name contains any string in excludeSubstrings

        ArrayList children = new ArrayList();

        if (includeParent)
            children.Add(parentGameObject);

        for (int i = 0; i < parentGameObject.transform.childCount; i++)
        {
            GameObject child = parentGameObject.transform.GetChild(i).gameObject;
            bool excludeChild = false;
            foreach (string substring in excludeSubstrings)
            {
                if (child.name.Contains(substring))
                {
                    excludeChild = true;
                    break;
                }
            }
            if (excludeChild)
                continue;

            children.Add(child);
            if (child.transform.childCount > 0)
                children.AddRange(GetAllChildren(child, excludeSubstrings, false));
        }
        return children;
    }
}
