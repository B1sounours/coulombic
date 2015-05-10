using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public struct IntVector2 {
    public int x, y;
    public IntVector2(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
    public override string ToString()
    {
        return "IntVector2 ("+x+", "+y+")";
    }

    public static IntVector2 GetReversed(IntVector2 vector)
    {
        return new IntVector2(-vector.x, -vector.y);
    }

    public float GetMagnitude()
    {
        return new Vector2(x, y).magnitude;
    }
}

public struct RectangularPrism
{
    public float x, y, z;
    public float width, height, depth;
    public RectangularPrism(float x, float y, float z, float width, float height, float depth)
    {
        this.x = x;
        this.y = y;
        this.z = z;
        this.width = width;
        this.height = height;
        this.depth = depth;
    }

    public override string ToString()
    {
        return "RectangularPrism coord(" + x + ", " + y + "," + z + ") size(" + width + "," + height + "," + depth + ")";
    }

    public Vector3 GetSizeVector()
    {
        return new Vector3(width, height, depth);
    }

    public Vector3 GetPositionVector()
    {
        return new Vector3(x, y, z);
    }
}

public struct IntVector3
{
    public int x, y,z;
    public IntVector3(int x, int y,int z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }
    public IntVector3(float x, float y, float z)
    {
        this.x = Mathf.FloorToInt(x);
        this.y = Mathf.FloorToInt(y);
        this.z = Mathf.FloorToInt(z);
    }
    public override string ToString()
    {
        return "IntVector3 (" + x + ", " + y + ","+z+")";
    }
}
