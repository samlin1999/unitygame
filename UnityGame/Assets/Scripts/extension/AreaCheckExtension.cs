using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AreaCheckExtension  {
    /// <summary>
    /// 用座標點判定距離
    /// </summary>
    /// <param name="center">中心點</param>
    /// <param name="target">目標</param>
    /// <param name="distance">判定距離</param>
    /// <returns>是否於範圍內</returns>
	public static bool DistanceChecker(this Vector3 center,Vector3 target, float distance = 1f)
    {
        return Vector3.Distance(center, target) <= distance;
    }
    /// <summary>
    /// 用Transform物件判定距離
    /// </summary>
    /// <param name="center">中心點</param>
    /// <param name="target">目標</param>
    /// <param name="distance">判定距離</param>
    /// <returns>是否於範圍內</returns>
    public static bool DistanceChecker(this Transform center,Transform target,float distance = 1f)
    {
        return Vector3.Distance(center.position, target.position) <= distance;
    }
    /// <summary>
    /// 用Transform物件判定圓形範圍
    /// </summary>
    /// <param name="center">中心點</param>
    /// <param name="target">目標</param>
    /// <param name="range">半徑</param>
    /// <param name="angle">夾角</param>
    /// <returns>是否於目標前方範圍內</returns>
    public static bool CircleChecker(this Transform center, Transform target, float range = 1f, float angle = 360)
    {
        float dis = Vector3.Distance(center.position, target.position);
        float ang = Vector3.Angle(center.forward, target.position - center.position);

        return dis <= range && ang <= angle / 2;
    }
    /// <summary>
    /// 圓柱形檢測範圍
    /// </summary>
    /// <param name="center">中心物件</param>
    /// <param name="target">目標物件</param>
    /// <param name="range">半徑</param>
    /// <param name="angle">夾角</param>
    /// <param name="height">高度</param>
    /// <returns>是否於範圍內</returns>
    public static bool CircleChecker(this Transform center, Transform target, float range = 1f, float angle = 360,float height = 2)
    {
        Vector3 posOnPlane = Vector3.ProjectOnPlane(target.position, Vector3.up); //Debug.Log(posOnPlane.y);
        float dis = Vector3.Distance(center.position, posOnPlane);
        float ang = Vector3.Angle(center.forward, posOnPlane - center.position);

        return Mathf.Abs(posOnPlane.y) <= height && dis <= range && ang <= angle / 2;
    }

    public static bool RectangleChecker(this Transform center, Transform target, float range = 1f, float width = 2)
    {
        Vector3 posOnPlane = Vector3.ProjectOnPlane(target.position, Vector3.up);
        float rangeDot = Vector3.Dot(center.forward, posOnPlane - center.position);
        float widthDot = Vector3.Dot(center.right, posOnPlane - center.position);
        return rangeDot > 0 && rangeDot <= range && Mathf.Abs(widthDot) <= width/2;
    }
}
