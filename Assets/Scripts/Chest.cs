using UnityEngine;

public enum ChestState
{
    Locked,
    Unlocking,
    UnlockedNotCollected,
    Collected
}

public class Chest
{
    public ChestConfig config;
    public ChestState state = ChestState.Locked;
    public int remainingTimeInSeconds;
    public int coinsReward;
    public int gemsReward;

    public Chest(ChestConfig chestConfig)
    {
        config = chestConfig;
        GenerateRewards();
        remainingTimeInSeconds = config.unlockTimeInMinutes * 60;
    }

    private void GenerateRewards()
    {
        coinsReward = Random.Range(config.minCoinsReward, config.maxCoinsReward);
        gemsReward = Random.Range(config.minGemsReward, config.maxGemsReward);
    }

    public void StartUnlocking()
    {
        state = ChestState.Unlocking;
    }

    public void CollectRewards()
    {
        if (state == ChestState.UnlockedNotCollected)
        {
            // Grant the rewards to the player
            Debug.Log($"Collected {coinsReward} coins and {gemsReward} gems!");
            state = ChestState.Collected;
        }
    }
}