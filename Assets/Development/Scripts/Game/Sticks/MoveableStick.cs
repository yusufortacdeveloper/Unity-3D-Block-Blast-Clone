using UnityEngine;
using UnityEngine.UI;

public class MoveableStick : MonoBehaviour
{
    public StickDirections stickDirections;

    public bool isBuildable = false;
    public GameObject tmpGridStick;

    public float raycastRadius = 0.5f;  
    public LayerMask buildableLayer;    
    private Collider2D _collider;       

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();

        buildableLayer = 1 << 6;
    }

    private void Update()
    {
        CheckForBuildableObjects();
    }

    private void CheckForBuildableObjects()
    {
        Vector2 center = _collider.bounds.center;

        Collider2D hit = Physics2D.OverlapCircle(center, raycastRadius, buildableLayer);

        if (hit != null && hit.gameObject != this.gameObject) 
        {
            if (hit.gameObject.layer == 6 && hit.gameObject.GetComponent<GridStick>().stickDirections == stickDirections)  
            {
                if(tmpGridStick == null)
                tmpGridStick = hit.gameObject;

                if(hit.GetComponent<GridStick>() != null)
                {
                    if(!hit.GetComponent<GridStick>().isBuilded)
                    {
                        isBuildable = true;
                        hit.GetComponent<Image>().color = Color.gray; 
                    }
                }
                Debug.Log("Buildable object detected.");
            }
            else
            {
                ResetBuildableStatus();
            }
        }
        else
        {
            ResetBuildableStatus();
        }
    }

    private void ResetBuildableStatus()
    {
        if(tmpGridStick)
        {
            if(!tmpGridStick.GetComponent<GridStick>().isBuilded)
            {
                tmpGridStick.GetComponent<Image>().color = Color.white;
            }
            else
            {
                tmpGridStick.GetComponent<Image>().color = Color.blue;
            }
        }
        tmpGridStick = null;
        isBuildable = false;
    }

    private void OnDrawGizmosSelected()
    {
        if (_collider == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_collider.bounds.center, raycastRadius);
    }
}
