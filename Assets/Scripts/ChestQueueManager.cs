using UnityEngine;
using System.Collections.Generic; 

public class ChestQueueManager
{
    private Queue<Chest> chestQueue = new Queue<Chest>();
    private ChestUnlocker currentUnlocker;

    public void AddChestToQueue(Chest chest)
    {
        chestQueue.Enqueue(chest);
        TryStartNextUnlock();
    }

    private void TryStartNextUnlock()
    {
        if (currentUnlocker == null || currentUnlocker.IsFinished())
        {
            if (chestQueue.Count > 0)
            {
                Chest nextChest = chestQueue.Dequeue();
                currentUnlocker = new ChestUnlocker(nextChest);
                currentUnlocker.StartUnlocking();
            }
        }
    }

    public void Update(float deltaTime)
    {
        if (currentUnlocker != null)
        {
            currentUnlocker.UpdateUnlocking(deltaTime);
            if (currentUnlocker.IsFinished())
            {
                TryStartNextUnlock();
            }
        }
    }
}