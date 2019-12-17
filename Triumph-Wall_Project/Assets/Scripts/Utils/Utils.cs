using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static RaycastHit ScreenRayPicking(Vector2 mousePos)
    {
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            //Debug.Log(hit.transform.tag);
            return hit;
        }

        return hit;     
    }

	public static float Remap (float value, float from1, float from2, float to1, float to2)
	{
		return (value - from1) / (from2 - from1) * (to2 - to1) + to1;
	}

}

