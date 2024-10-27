using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    public ChestSlot[] chestSlots;
    public Text coinsText;
    public Text gemsText;

    private CurrencyManager currencyManager;

    private void Start()
    {
        currencyManager = GetComponent<CurrencyManager>();
        LoadChestSlots();
    }

    public void LoadChestSlots()
    {
        // Load saved chests and assign them to the UI slots
        List<Chest> chests;
        int coins, gems;
        SaveSystem.LoadGameData(out chests, out coins, out gems);

        CurrencyManager.Instance.AddCoins(coins);
        CurrencyManager.Instance.AddGems(gems);

        for (int i = 0; i < chestSlots.Length; i++)
        {
            if (i < chests.Count && chests[i] != null)
            {
                chestSlots[i].AssignChest(chests[i]);
            }
            else
            {
                chestSlots[i].AssignChest(null); // Clear any leftover slots
            }
        }
    }

    public void GenerateRandomChest()
    {
        if (ChestManager.Instance.chests.Count < chestSlots.Length)
        {
            Chest newChest = ChestManager.Instance.CreateRandomChest();
            foreach (ChestSlot slot in chestSlots)
            {
                if (slot.chest == null)
                {
                    slot.AssignChest(newChest);
                    break;
                }
            }
        }
    }

    public void UpdateCurrencyUI()
    {
        coinsText.text = "Coins: " + currencyManager.Coins.ToString();
        gemsText.text = "Gems: " + currencyManager.Gems.ToString();
    }
}
