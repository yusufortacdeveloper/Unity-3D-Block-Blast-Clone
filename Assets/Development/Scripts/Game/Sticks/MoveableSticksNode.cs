using System.Collections.Generic;
using UnityEngine; 
using UnityEngine.EventSystems;

public class MoveableSticksNode : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public List<MoveableStick> moveableSticks; 
    public bool isGridReady = false;

    private Vector2 _offset;
    private RectTransform _rectTransform;
    private Vector3 _initialPosition;

    ItemsManager itemManager;
    GridManager gridManager;
    ScoreManager scoreManager;
    SoundManager soundManager;
    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _initialPosition = _rectTransform.localPosition;  
    }

    private void Start()
    {
         itemManager = EventManager.Instance.GetListener<ItemsManager>();
         gridManager = EventManager.Instance.GetListener<GridManager>();
         scoreManager = EventManager.Instance.GetListener<ScoreManager>();
         soundManager = EventManager.Instance.GetListener<SoundManager>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Down");
        soundManager.PlaySound(SoundManager.SoundID.Drag);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _rectTransform.parent as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out _offset);


    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("Drag");

        Vector2 localPoint;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _rectTransform.parent as RectTransform,
                eventData.position,
                eventData.pressEventCamera,
                out localPoint))
        {
            _rectTransform.localPosition = localPoint - _offset;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("Up");

        if (!ReturnGridReady())  
        {
            _rectTransform.localPosition = _initialPosition;
            soundManager.PlaySound(SoundManager.SoundID.ReturnPos);
        }
        else
        {
            

            foreach (MoveableStick item in moveableSticks)
            {
                if (item.tmpGridStick.GetComponent<GridStick>())
                {
                    item.tmpGridStick.GetComponent<GridStick>().SetBuilded();
                    soundManager.PlaySound(SoundManager.SoundID.Drop);
                    scoreManager.IncreaseScore(20);
                    soundManager.PlaySound(SoundManager.SoundID.Point);

                }
            }

            for (int i = 0; i < gridManager.blocks.Count; i++)
            {
                gridManager.blocks[i].GetComponent<BlockStick>().SticksControl();
            }

            if (itemManager)
                itemManager.DecreaseItemsCount();
            else
                Debug.Log("Dont found item manager");
            Destroy(this.gameObject);
        }
    }

    public bool ReturnGridReady()
    {
        isGridReady = true;

        foreach (MoveableStick item in moveableSticks)
        {
            if (!item.isBuildable)
            {
                isGridReady = false;
                break;
            }
        }

        return isGridReady;
    }

    public void BuildSticksOnGrid()
    {
        isGridReady = true;

        foreach (MoveableStick item in moveableSticks)
        {
            if (!item.isBuildable)
            {
                isGridReady = false;
                break;
            }
        }

        if (isGridReady)
        {
            // Ýnþa iþlemleri burada yapýlacak
        }
    }
}
