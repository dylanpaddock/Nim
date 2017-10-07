using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils {
    //Utility functions for unit selection and converting between coordinate
    //systems. Based on hyunkell.com/blog/rts-style-unit-selection-in-unity-5/

    public static void DrawScreenRect(Rect rect, Color color){
        GUI.color = color;
        GUI.DrawTexture(rect, Texture2D.whiteTexture);
        GUI.color = Color.white;
    }

    public static void DrawScreenRectBorder(Rect rect, float thickness, Color color){
        DrawScreenRect(new Rect(rect.xMin, rect.yMin, rect.width, thickness), color); //top
        DrawScreenRect(new Rect(rect.xMin, rect.yMin, thickness, rect.height), color); //left

        DrawScreenRect(new Rect(rect.xMin, rect.yMax - thickness, rect.width, thickness), color); //bottom
        DrawScreenRect(new Rect(rect.xMax - thickness, rect.yMin, thickness, rect.height), color); //right
    }

    public static Rect GetScreenRect(Vector3 pos1, Vector3 pos2){
        pos1.y = Screen.height - pos1.y;
        pos2.y = Screen.height - pos2.y;
        return Rect.MinMaxRect(Mathf.Min(pos1.x, pos2.x), Mathf.Min(pos1.y, pos2.y),
                               Mathf.Max(pos1.x, pos2.x), Mathf.Max(pos1.y, pos2.y));
    }

    public static Bounds GetViewportBounds(Camera camera, Vector3 screenPos1, Vector3 screenPos2){
        Vector3 v1 = camera.ScreenToViewportPoint(screenPos1);
        Vector3 v2 = camera.ScreenToViewportPoint(screenPos2);
        Vector3 min = Vector3.Min(v1,v2);
        Vector3 max = Vector3.Max(v1,v2);
        min.z = camera.nearClipPlane;
        max.z = camera.farClipPlane;

        Bounds b = new Bounds();
        b.SetMinMax(min,max);
        return b;
    }

    public static Bounds GetViewportBoundsWorldspace(Camera camera, Vector3 screenPos1, Vector3 screenPos2){
        Vector3 v1 = camera.WorldToViewportPoint(screenPos1);
        Vector3 v2 = camera.WorldToViewportPoint(screenPos2);
        Vector3 min = Vector3.Min(v1,v2);
        Vector3 max = Vector3.Max(v1,v2);
        min.z = camera.nearClipPlane;
        max.z = camera.farClipPlane;

        Bounds b = new Bounds();
        b.SetMinMax(min,max);
        return b;
    }
}
