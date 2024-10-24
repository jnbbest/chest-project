using UnityEngine;

public class ChestUnlocker
{
    private Chest chest;
    private float remainingTime;
    private bool isUnlocking;

    public ChestUnlocker(Chest chest)
    {
        this.chest = chest;
    }

    public void StartUnlocking()
    {
        if (chest.state == ChestState.Locked)
        {
            chest.StartUnlocking();
            isUnlocking = true;
            remainingTime = chest.remainingTimeInSeconds;
        }
    }

    public void UpdateUnlocking(float deltaTime)
    {
        if (isUnlocking && remainingTime > 0)
        {
            remainingTime -= deltaTime;
            if (remainingTime <= 0)
            {
                chest.state = ChestState.UnlockedNotCollected;
                isUnlocking = false;
            }
        }
    }

    public void UnlockImmediately()
    {
        int gemsCost = Mathf.CeilToInt(remainingTime / 600f); // 1 gem per 10 mins
        // Deduct gems from the player
        remainingTime = 0;
        chest.state = ChestState.UnlockedNotCollected;
        isUnlocking = false;
    }

    public bool IsFinished()
    {
        return !isUnlocking || chest.state == ChestState.UnlockedNotCollected;
    }

    public float GetRemainingTime()
    {
        return remainingTime;
    }
}