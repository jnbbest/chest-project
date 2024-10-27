using System.Collections.Generic;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    public static void SaveGameData(List<IChest> chests, int coins, int gems)
    {
        PlayerPrefs.SetInt("Coins", coins);
        PlayerPrefs.SetInt("Gems", gems);

        for (int i = 0; i < chests.Count; i++)
        {
            PlayerPrefs.SetInt($"Chest{i}_State", (int)chests[i].State);
            float remainingTime = chests[i].State == ChestState.Unlocking ? chests[i].UnlockEndTime - Time.time : 0;
            PlayerPrefs.SetFloat($"Chest{i}_RemainingTime", remainingTime);
        }

        PlayerPrefs.Save();
    }

    public static void LoadGameData(out List<IChest> chests, out int coins, out int gems)
    {
        coins = PlayerPrefs.GetInt("Coins", 0);
        gems = PlayerPrefs.GetInt("Gems", 0);

        chests = new List<IChest>();
        for (int i = 0; i < 4; i++)
        {
            ChestState state = (ChestState)PlayerPrefs.GetInt($"Chest{i}_State", 0);
            float remainingTime = PlayerPrefs.GetFloat($"Chest{i}_RemainingTime", -1);

            if (state != ChestState.Collected)
            {
                Chest chest = new Chest(); // Assuming the config is assigned appropriately
                chest.State = state;
                if (state == ChestState.Unlocking && remainingTime > 0)
                {
                    chest.UnlockEndTime = Time.time + remainingTime;
                }
                chests.Add(chest);
            }
        }
    }
}
