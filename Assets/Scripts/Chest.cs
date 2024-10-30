using UnityEngine;
using System;

public enum ChestState
{
    Locked,
    Queue,
    Unlocking,
    Unlocked,
    Collected
}

[System.Serializable]
public class Chest
{
    public ChestConfig config;
    public ChestState State;
    public DateTime UnlockEndTime;
    
    public void StartUnlock()
    {
        UnlockEndTime = DateTime.Now.AddMinutes(config.unlockTimeMinutes);
        Debug.Log($"Time.time - {Time.time}, UnlockEndTime - {UnlockEndTime}");
        State = ChestState.Unlocking;
    }

    public void AddtoQueue(DateTime previousUnlockEndTime)
    {
        UnlockEndTime = previousUnlockEndTime.AddMinutes(config.unlockTimeMinutes);
        Debug.Log($"Time.time - {Time.time}, UnlockEndTime - {UnlockEndTime}");
        State = ChestState.Queue;
    }
    
    public int GetRemainingTime()
    {
        if (State != ChestState.Unlocking) return 0;
            return (int)(UnlockEndTime - DateTime.Now).TotalSeconds;
    }

    public int GetUnlockCost()
    {
        int remainingMinutes = Mathf.CeilToInt(((int)(UnlockEndTime - DateTime.Now).TotalMinutes) / 60);
        return Mathf.CeilToInt(remainingMinutes / 10.0f);
    }

    // if UnlockEndTime is greater then DateTime.Now it's ready to collect
    public bool IsReadyToCollect() => DateTime.Compare(UnlockEndTime, DateTime.Now) >= 0;// => Time.time >= UnlockEndTime;

    public void CollectReward()
    {
        int coins = UnityEngine.Random.Range(config.minCoins, config.maxCoins);
        int gems = UnityEngine.Random.Range(config.minGems, config.maxGems);
        CurrencyManager.Instance.AddCoins(coins);
        CurrencyManager.Instance.AddGems(gems);
        State = ChestState.Collected;
    }
}