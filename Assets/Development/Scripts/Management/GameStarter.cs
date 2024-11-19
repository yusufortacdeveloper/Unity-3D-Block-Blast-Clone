using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStarter : MonoBehaviour, IEventListener
{
    [SerializeField] private GameObject gameLogo, inGameLogos;
    [SerializeField] private Transform mainMenuPanel;
    [SerializeField] private Transform gameMenuPanel;
    [SerializeField] private float delayStartTime;



    public Vector3 gameMenuInitPos, mainMenuInitPos,gameLogoInitPoz;
    private void OnEnable()
    {
        EventManager.Instance.RegisterListener(this);
        gameMenuInitPos = gameMenuPanel.position;
        mainMenuInitPos = mainMenuPanel.position;
        gameLogoInitPoz = gameLogo.transform.position;
    }

    private void OnDisable()
    {
        EventManager.Instance.UnregisterListener(this);
    }

    public void AdventureModeStart()
    {
        StartCoroutine(StartGameWithDelay(true));
    }
    public void ClassicModeStart()
    {
        StartCoroutine(StartGameWithDelay(false));
    }

    public void CloseGame()
    {
        StartCoroutine(EndGameWithDelay());
    }

    public IEnumerator StartGameWithDelay(bool isAdventure)
    {
        inGameLogos.SetActive(false);
        gameLogo.transform.DOMove(new Vector3(gameLogo.transform.position.x, gameLogo.transform.position.y + 1000f, gameLogo.transform.position.z), 0.5f)
            .OnComplete(() => gameLogo.SetActive(false));

        mainMenuPanel.DOMove(gameMenuInitPos, 0.5f);

        yield return new WaitForSeconds(delayStartTime);

        GridManager gridManager = EventManager.Instance.GetListener<GridManager>();
        ItemsManager itemsManager = EventManager.Instance.GetListener<ItemsManager>();

        yield return new WaitForSeconds(delayStartTime);

        gameMenuPanel.DOMove(mainMenuInitPos, 0.2f);
        inGameLogos.SetActive(true);

        if (gridManager)
            gridManager.GenerateGrid(isAdventure);

        if (itemsManager)
            itemsManager.RandomItemSpawn();
    }

    public IEnumerator EndGameWithDelay()
    {
        inGameLogos.SetActive(false);

        gameMenuPanel.DOMove(gameMenuInitPos, 0.5f);

        yield return new WaitForSeconds(delayStartTime);

        GridManager gridManager = EventManager.Instance.GetListener<GridManager>();
        ItemsManager itemsManager = EventManager.Instance.GetListener<ItemsManager>();

        yield return new WaitForSeconds(delayStartTime);

        mainMenuPanel.DOMove(mainMenuInitPos, 0.2f);
        gameLogo.SetActive(true);
        gameLogo.transform.DOMove(gameLogoInitPoz, 0.2f)
              .OnComplete(() => gameLogo.SetActive(true));

        if (gridManager)
            gridManager.ClearGridDatas();

        if (itemsManager)
            itemsManager.ClearItems();
    }
}
