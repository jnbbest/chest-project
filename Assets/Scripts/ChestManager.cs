using UnityEngine;
using System.Collections.Generic;

public class ChestManager : MonoBehaviour
{
    public static ChestManager Instance { get; private set; }
    public List<ChestConfig> chestConfigs;
    public List<Chest> chests = new List<Chest>();
    private LinkedList<Chest> chestQueue = new LinkedList<Chest>();
    private Chest currentUnlockingChest;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        LoadChestSlots();
    }

    public void LoadChestSlots()
    {
        SaveSystem.LoadChestData(ref currentUnlockingChest, ref chestQueue, ref chests, chestConfigs);

        UIManager uim = GetComponent<UIManager>();
        for (int i = 0; i < uim.chestSlots.Length; i++)
        {
            if (i < chests.Count && chests[i] != null)
            {
                uim.chestSlots[i].AssignChest(chests[i]);
            }
            else
            {
                uim.chestSlots[i].AssignChest(null); // Clear any leftover slots
            }
        }
    }

    public void StartUnlockingChest(Chest chest)
    {
        if (currentUnlockingChest == null)
        {
            currentUnlockingChest = chest;
            chest.StartUnlock();
        }
        else
        {
            Chest previousChest = new();
            if (chestQueue.Count > 0)
                previousChest = chestQueue.Last.Value;
            else
                previousChest = currentUnlockingChest;

            chestQueue.AddLast(chest);
            chest.AddtoQueue(previousChest.UnlockEndTime);
        }
        SaveChestData();
    }

    public void OnChestUnlocked(Chest chest)
    {
        if (currentUnlockingChest == chest)
        {
            currentUnlockingChest = null; // Clear current chest

            // Start the next chest in the queue if available
            if (chestQueue.Count > 0)
            {
                var nextChest = chestQueue.First.Value;
                chestQueue.RemoveFirst();
                currentUnlockingChest = nextChest;
                nextChest.StartUnlock();
            }
        }
        SaveChestData();
    }

    public Chest CreateRandomChest()
    {
        if (chests.Count < 4) 
        {
            int randomIndex = Random.Range(0, (chestConfigs.Count-1) * 10);
            ChestConfig selectedConfig = chestConfigs[randomIndex / 10];

            Chest newChest = new Chest
            {
                config = selectedConfig,
                State = ChestState.Locked
            };
            chests.Add(newChest);

            SaveChestData();
            return newChest;
        }
        return null;
    }

    void SaveChestData()
    {
        SaveSystem.SaveChestData(currentUnlockingChest, chestQueue, chests);
    }
}