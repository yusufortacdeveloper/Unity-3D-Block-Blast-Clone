using UnityEngine;
using TMPro; 
public class ScoreManager : MonoBehaviour,IEventListener
{
    public int scoreValue = 0;
    [SerializeField] private TextMeshProUGUI scoreText;

    private void OnEnable()
    {
        EventManager.Instance.RegisterListener(this);
    }

    private void OnDisable()
    {
        EventManager.Instance.UnregisterListener(this); 
    }

    private void Update()
    {
        scoreText.text = "YOUR SCORE: " +  scoreValue.ToString();
    }

    public void IncreaseScore(int value)
    {
        scoreValue += value;
    }

}
