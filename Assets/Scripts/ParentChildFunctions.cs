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
