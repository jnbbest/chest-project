using UnityEngine;
using System.Collections.Generic;

public class ChestManager : MonoBehaviour
{
    public static ChestManager Instance { get; private set; }
    public List<Chest> chests = new List<Chest>();
    private Queue<Chest> chestQueue = new Queue<Chest>();
    public List<ChestConfig> chestConfigs;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void GenerateChest()
    {
        if (chests.Count < 4) {
            var newChest = CreateRandomChest();
            chests.Add(newChest);
        }
    }

    public void StartUnlocking(Chest chest)
    {
        if (chests.Exists(c => c.State == ChestState.Unlocking)) chestQueue.Enqueue(chest);
        else chest.StartUnlock();
    }

    public void OnChestCollected(ChestSlot slot)
    {
        if (slot != null) slot.AssignChest(null); // Clear slot if chest was collected

        if (chestQueue.Count > 0)
        {
            Chest nextChest = chestQueue.Dequeue();
            nextChest.StartUnlock();
        }
    }

    public Chest CreateRandomChest()
    {
        int randomIndex = Random.Range(0, (chestConfigs.Count-1) * 10);
        ChestConfig selectedConfig = chestConfigs[randomIndex / 10];

        Chest newChest = new Chest
        {
            config = selectedConfig,
            State = ChestState.Locked
        };
        return newChest;
    }

}