using UnityEngine;
using UnityEngine.UI;

public class ChestSlot : MonoBehaviour
{
    public Text chestNameText;
    public Text chestStateText;
    public Chest chest;

    void Update()
    {
        if (chest != null && chest.State == ChestState.Unlocking)
        {
            int remainingSeconds = chest.GetRemainingTime();
            chestStateText.text = "Unlocking: " + $"{((int)remainingSeconds / 60)} : {((int)remainingSeconds % 60)} min";
            if (remainingSeconds <= 0)
            {
                chest.State = ChestState.Unlocked;
                UpdateUI();
            }
        }
    }

    public void AssignChest(Chest newChest)
    {
        chest = newChest;
        UpdateUI();
    }

    public void UpdateUI()
    {
        if (chest == null)
        {
            chestNameText.text = "";
            chestStateText.text = "Empty Slot";
        }
        else
        {
            chestNameText.text = chest.config.chestName;
            switch (chest.State)
            {
                case ChestState.Locked:
                    chestStateText.text = "Locked";
                    break;
                case ChestState.Unlocking:
                    chestStateText.text = "Unlocking: " + chest.GetRemainingTime().ToString("mm\\:ss");
                    break;
                case ChestState.Unlocked:
                    chestStateText.text = "Tap to Collect!";
                    break;
                case ChestState.Collected:
                    chestStateText.text = "Collected";
                    break;
            }
        }
    }

    public void OnChestClick()
    {
        if (chest == null) return;

        if (chest.State == ChestState.Locked)
        {
            ChestManager.Instance.StartUnlocking(chest);
        }
        else if (chest.State == ChestState.Unlocking)
        {
            PopupManager.Instance.ShowImmediateUnlockPopup(chest);
        }
        else if (chest.State == ChestState.Unlocked)
        {
            chest.CollectReward();
            ChestManager.Instance.OnChestCollected(this);
        }
        
        UpdateUI();
    }
}

