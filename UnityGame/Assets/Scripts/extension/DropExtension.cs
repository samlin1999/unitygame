using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DropExtension  {
    /// <summary>
    /// 掉落金幣數
    /// </summary>
    /// <param name="baseMoney"></param>
    /// <param name="range"></param>
    /// <returns></returns>
	public static int DropMoneyRandom(this int baseMoney, int range)
    {
        int dropMoney = baseMoney + Random.Range(-range, range);
        return dropMoney < 0 ? 0 : dropMoney; 
    }
    /// <summary>
    /// 掉落次數
    /// </summary>
    /// <param name="baseCount"></param>
    /// <param name="range"></param>
    /// <returns></returns>
    public static int DropItemCountRandom(this int baseCount, int range)
    {
        int dropCount = baseCount + Random.Range(0, range);
        return dropCount;
    }
}
