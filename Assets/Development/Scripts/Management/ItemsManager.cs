using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsManager : MonoBehaviour,IEventListener
{
    [SerializeField] private ItemControls �temData;
    [SerializeField] private List<Transform> itemSpawnPositions;
    [SerializeField] private List<GameObject> spawnedItems;
    [SerializeField] public int spawnedItemsCount = 0;
    
    private void OnEnable()
    {
        EventManager.Instance.RegisterListener(this); 
    }

    private void OnDisable()
    {
        EventManager.Instance.UnregisterListener(this);
    }

    public void DecreaseItemsCount()
    {
        spawnedItemsCount--;
        RandomItemSpawn();
    }

    public void ClearItems()
    {
        for (int i = 0; i < spawnedItems.Count; i++)
        {
            if (spawnedItems[i])
            {
                Destroy(spawnedItems[i]);
            }
            spawnedItems.Remove(spawnedItems[i]);
        }

        spawnedItemsCount = 0;
    }

    public void RandomItemSpawn()
    {
        GameObject randomSpawnedItem;
        if (spawnedItemsCount <= 0)
        {
            for (int i = 0; i < itemSpawnPositions.Count; i++)
            {
                switch (i)
                {
                    case 0:
                         randomSpawnedItem = Instantiate(�temData.singleMoveableObjects[Random.Range(0, �temData.singleMoveableObjects.Count)], itemSpawnPositions[i].position, Quaternion.Euler(0f, 0f, -90f), itemSpawnPositions[i]);
                        spawnedItems.Add(randomSpawnedItem);
                        spawnedItemsCount++;
                        break;
                    case 1:
                         randomSpawnedItem = Instantiate(�temData.doubleMouveableObjects[Random.Range(0, �temData.doubleMouveableObjects.Count)], itemSpawnPositions[i].position, Quaternion.Euler(0f, 0f, -90f), itemSpawnPositions[i]);
                        spawnedItems.Add(randomSpawnedItem);
                        spawnedItemsCount++;
                        break;
                    case 2:
                         randomSpawnedItem = Instantiate(�temData.moreMoveableObjects[Random.Range(0, �temData.moreMoveableObjects.Count)], itemSpawnPositions[i].position, Quaternion.Euler(0f, 0f, -90f), itemSpawnPositions[i]);
                        spawnedItems.Add(randomSpawnedItem);
                        spawnedItemsCount++;
                        break;
                }
               
            }
        }
    }
}
