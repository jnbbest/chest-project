using UnityEngine;

[CreateAssetMenu(fileName = "ChestConfig", menuName = "Game/ChestConfig")]
public class ChestConfig : ScriptableObject
{
    public string chestName;
    public int minCoins;
    public int maxCoins;
    public int minGems;
    public int maxGems;
    public float unlockTimeMinutes;
}