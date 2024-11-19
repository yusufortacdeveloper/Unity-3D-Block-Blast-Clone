using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class BlockStick : MonoBehaviour
{
    public LayerMask stickLayerMask;
    public float rayDistance = 5f;
    public List<GameObject> connectedSticks = new List<GameObject>();
    public bool isFilled = false;
    ScoreManager scoreManager;
    GridManager gridManager; 
    public int row; 
    public GameObject textPrefab;

    private void Start()
    {
        scoreManager = EventManager.Instance.GetListener<ScoreManager>();
        gridManager = EventManager.Instance.GetListener<GridManager>();
        DetectSticks();
        SticksControl();

    }

    private void DetectSticks()
    {
        Vector2 centerPosition = transform.position;
        RaycastInDirection(Vector2.right, centerPosition);
        RaycastInDirection(Vector2.left, centerPosition);
        RaycastInDirection(Vector2.up, centerPosition);
        RaycastInDirection(Vector2.down, centerPosition);
    }

    private void RaycastInDirection(Vector2 direction, Vector2 origin)
    {
        RaycastHit2D hit = Physics2D.Raycast(origin, direction, rayDistance, stickLayerMask);

        if (hit.collider != null)
        {
            GameObject stick = hit.collider.gameObject;
            connectedSticks.Add(stick);
            Debug.Log("Stick detected in direction " + direction + ": " + stick.name);
        }
    }

    public void SticksControl()
    {
        bool allSticksBuilded = true;

        for (int i = 0; i < connectedSticks.Count; i++)
        {
            if (connectedSticks[i])
            {
                if (connectedSticks[i].GetComponent<GridStick>() && !connectedSticks[i].GetComponent<GridStick>().isBuilded)
                {
                    allSticksBuilded = false;
                    break;
                }
            }
        }

        if (allSticksBuilded && !isFilled)
        {
            SetBlockFilled();
        }
    }

    public void SetBlockFilled()
    {
        isFilled = true;
        this.GetComponent<Image>().color = Color.blue;
        scoreManager.IncreaseScore(50);
        SoundManager soundManager = EventManager.Instance.GetListener<SoundManager>();
        soundManager.PlaySound(SoundManager.SoundID.Point);
        GameObject textObject = Instantiate(textPrefab, transform.position, Quaternion.identity, transform);
        TextMeshProUGUI textMeshPro = textObject.GetComponent<TextMeshProUGUI>();

        textMeshPro.text = "+50";
        textMeshPro.color = Color.green;
        textMeshPro.fontSize = 80;
        textMeshPro.alignment = TextAlignmentOptions.Center;

        Vector3 targetPosition = textObject.transform.position + new Vector3(0, 50, 0);
        textObject.transform.DOMove(targetPosition, 2f).SetEase(Ease.OutQuad);

        Destroy(textObject, 1f);

        CheckRowStatus();
    }

    public void SetBlockUnFilled()
    {
        isFilled = false;
        this.GetComponent<Image>().color = Color.white;
        scoreManager.IncreaseScore(50);
        SoundManager soundManager = EventManager.Instance.GetListener<SoundManager>();
        soundManager.PlaySound(SoundManager.SoundID.Point);
        GameObject textObject = Instantiate(textPrefab, transform.position, Quaternion.identity, transform);
        TextMeshProUGUI textMeshPro = textObject.GetComponent<TextMeshProUGUI>();

        textMeshPro.text = "+50";
        textMeshPro.color = Color.green;
        textMeshPro.fontSize = 80;
        textMeshPro.alignment = TextAlignmentOptions.Center;

        Vector3 targetPosition = textObject.transform.position + new Vector3(0, 50, 0);
        textObject.transform.DOMove(targetPosition, 2f).SetEase(Ease.OutQuad);

        Destroy(textObject, 1f);
        for (int i = 0; i < connectedSticks.Count; i++)
        {
            if(connectedSticks[i])
            {
                if(connectedSticks[i].GetComponent<GridStick>())
                {
                    connectedSticks[i].GetComponent<GridStick>().SetReadyForBuild();
                }
            }
        }
    }

    public void CheckRowStatus()
    {
        List<GameObject> allBlocks = gridManager.blocks;
        List<BlockStick> tmpRowList = new List<BlockStick>();

        int currentRow = this.row;

        foreach (GameObject block in allBlocks)
        {
            BlockStick blockStick = block.GetComponent<BlockStick>();

            if (blockStick != null && blockStick.row == currentRow)
            {
                tmpRowList.Add(blockStick);
            }
        }

        bool allBlocksFilled = true;
        foreach (BlockStick blockStick in tmpRowList)
        {
            if (!blockStick.isFilled)
            {
                allBlocksFilled = false;
                break;
            }
        }

        if (allBlocksFilled)
        {
            foreach (BlockStick blockStick in tmpRowList)
            {
                blockStick.ResetBlockData();
            }
        }
    }

    public void ResetBlockData()
    {
        isFilled = false;
        SetBlockUnFilled();
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, (Vector2)transform.position + Vector2.right * rayDistance);  // Sað
        Gizmos.DrawLine(transform.position, (Vector2)transform.position + Vector2.left * rayDistance);   // Sol
        Gizmos.DrawLine(transform.position, (Vector2)transform.position + Vector2.up * rayDistance);     // Yukarý
        Gizmos.DrawLine(transform.position, (Vector2)transform.position + Vector2.down * rayDistance);   // Aþaðý
    }
}