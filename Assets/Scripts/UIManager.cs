using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    public ChestSlot[] chestSlots;
    public Text coinsText;
    public Text gemsText;

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
        coinsText.text = "Coins: " + GetComponent<CurrencyManager>().Coins.ToString();
        gemsText.text = "Gems: " + GetComponent<CurrencyManager>().Gems.ToString();
    }
}
