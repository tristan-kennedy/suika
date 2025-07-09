using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    private int currentScore = 0;

    [Header("UI References")]
    [SerializeField] private TMP_Text scoreText;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        Instance = this;
    }
    void Start()
    {
        UpdateScoreDisplay();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void UpdateScoreDisplay()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + currentScore.ToString("N0");
    }

    public void ScoreFruit(Fruit fruit)
    {
        currentScore += fruit.points;
        UpdateScoreDisplay();
    }
}
