using UnityEngine;
using System;

public enum ChestState
{
    Locked,
    Unlocking,
    Unlocked,
    Collected
}

public class Chest
{
    public ChestConfig config;
    public ChestState State { get; set; }
    public float UnlockEndTime { get; set; }
    private int remainingMinutes;
    
    public void StartUnlock()
    {
        UnlockEndTime = Time.time + config.unlockTimeMinutes * 60;
        Debug.Log($"Time.time - {Time.time}, UnlockEndTime - {UnlockEndTime}");
        State = ChestState.Unlocking;
    }
    
    public int GetRemainingTime()
    {
        if (State != ChestState.Unlocking) return 0;
        return (int)(UnlockEndTime - Time.time);
    }

    public int GetUnlockCost()
    {
        int remainingMinutes = Mathf.CeilToInt((UnlockEndTime - Time.time) / 60);
        return Mathf.CeilToInt(remainingMinutes / 10.0f);
    }

    public bool IsReadyToCollect() => Time.time >= UnlockEndTime;

    public void CollectReward()
    {
        int coins = UnityEngine.Random.Range(config.minCoins, config.maxCoins);
        int gems = UnityEngine.Random.Range(config.minGems, config.maxGems);
        CurrencyManager.Instance.AddCoins(coins);
        CurrencyManager.Instance.AddGems(gems);
        State = ChestState.Collected;
    }
}