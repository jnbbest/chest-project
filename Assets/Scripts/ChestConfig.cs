using UnityEngine;

[CreateAssetMenu(fileName = "ChestConfig", menuName = "Chest/ChestConfig")]
public class ChestConfig : ScriptableObject
{
    public string chestName;
    public int minCoinsReward;
    public int maxCoinsReward;
    public int minGemsReward;
    public int maxGemsReward;
    public int unlockTimeInMinutes;
}