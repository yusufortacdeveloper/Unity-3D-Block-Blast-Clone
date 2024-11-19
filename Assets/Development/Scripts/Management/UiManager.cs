using System.Collections.Generic;
using UnityEngine;

public class UiManager : MonoBehaviour, IEventListener
{
    public enum PanelID
    {
        MainMenu,
        Settings,
        GameOver,
        PauseMenu,
        Game
    }

    private Dictionary<PanelID, Panel> panels = new Dictionary<PanelID, Panel>();

    [SerializeField] private List<Panel> panelList; 

    private void OnEnable()
    {
        EventManager.Instance.RegisterListener(this);
    }

    private void OnDisable()
    {
        EventManager.Instance.UnregisterListener(this);
    }

    void Awake()
    {
        foreach (Panel panel in panelList)
        {
            panels[panel.panelID] = panel;
        }
    }
    public void TogglePanel(PanelID panelID)
    {
        foreach (var panel in panels.Values)
        {
            panel.gameObject.SetActive(false);
        }

        if (panels.TryGetValue(panelID, out Panel targetPanel))
        {
            targetPanel.gameObject.SetActive(true);
        }
    }

}
