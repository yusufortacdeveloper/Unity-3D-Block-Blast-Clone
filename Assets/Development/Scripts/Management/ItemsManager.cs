using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsManager : MonoBehaviour,IEventListener
{
    [SerializeField] private ItemControls ýtemData;
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
                         randomSpawnedItem = Instantiate(ýtemData.singleMoveableObjects[Random.Range(0, ýtemData.singleMoveableObjects.Count)], itemSpawnPositions[i].position, Quaternion.Euler(0f, 0f, -90f), itemSpawnPositions[i]);
                        spawnedItems.Add(randomSpawnedItem);
                        spawnedItemsCount++;
                        break;
                    case 1:
                         randomSpawnedItem = Instantiate(ýtemData.doubleMouveableObjects[Random.Range(0, ýtemData.doubleMouveableObjects.Count)], itemSpawnPositions[i].position, Quaternion.Euler(0f, 0f, -90f), itemSpawnPositions[i]);
                        spawnedItems.Add(randomSpawnedItem);
                        spawnedItemsCount++;
                        break;
                    case 2:
                         randomSpawnedItem = Instantiate(ýtemData.moreMoveableObjects[Random.Range(0, ýtemData.moreMoveableObjects.Count)], itemSpawnPositions[i].position, Quaternion.Euler(0f, 0f, -90f), itemSpawnPositions[i]);
                        spawnedItems.Add(randomSpawnedItem);
                        spawnedItemsCount++;
                        break;
                }
               
            }
        }
    }
}
