using UnityEngine;
using UnityEngine.UI;
public class GridStick : MonoBehaviour
{
    public StickDirections stickDirections;

    public bool isBuilded = false;
    public void SetBuilded()
    {
        isBuilded = true;
        GetComponent<Image>().color = Color.blue;
    }
    public void SetReadyForBuild()
    {
        isBuilded = false;
        GetComponent<Image>().color = Color.white;
    }

}

