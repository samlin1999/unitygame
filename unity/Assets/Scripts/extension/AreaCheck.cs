using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 擴充方法(檢測範圍)
/// </summary>
public static class AreaCheck {
    /// <summary>
    /// 擴充方法(檢測圓形範圍)
    /// </summary>
    /// <param name="center">中心</param>
    /// <param name="target">目標</param>
    /// <param name="range">範圍</param>
    /// <returns></returns>
    public static bool CircleChecker(this Transform center,Transform target,float range)//圓形範圍檢測
    {
        return Vector3.Distance(center.position, target.position) <= range;
    }
    /// <summary>
    /// 擴充方法(檢測扇形範圍)
    /// </summary>
    /// <param name="center">中心</param>
    /// <param name="target">目標</param>
    /// <param name="range">範圍</param>
    /// <param name="ang">角度</param>
    /// <returns></returns>
    public static bool CircleChecker(this Transform center, Transform target, float range, float ang)//扇形範圍檢測
    {
        float dis = Vector3.Distance(center.position, target.position);
        float angle = Vector3.Angle(center.forward, (target.position - center.position));
        return dis <= range && angle <= ang / 2f; 
    }



}
