using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{


    [SerializeField] private Text scoreText;
    [SerializeField] private Text gameOverText;
    [SerializeField] private Text restartText;
    [SerializeField] private Image livesDisplayImage;
    [SerializeField] private Sprite[] livesSprites;
    private bool _isGameOver = false;

    //cached references
    private GameManager _gameManager;


    // Start is called before the first frame update
    void Start()
    {
        scoreText.text = "Score: " + 0;
        gameOverText.gameObject.SetActive(false);
        restartText.gameObject.SetActive(false);

        //init cached references
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        if (_gameManager == null)
        {
            Debug.LogError("GameManager is null.");
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void UpdateScoreText(int score)
    {
        scoreText.text = "Score: " + score;
    }

    public void UpdateLives(int currentLives)
    {
        livesDisplayImage.sprite = livesSprites[currentLives];
        if (currentLives < 1)
        {
            GameOverSequence();
        }
    }

    private void GameOverSequence()
    {
        _gameManager.GameOver();
        _isGameOver = true;
        restartText.gameObject.SetActive(true);
        StartCoroutine(FlickerGameOverRoutine());
    }

    private IEnumerator FlickerGameOverRoutine()
    {
        while (_isGameOver)
        {
            gameOverText.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            gameOverText.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
        }
    }
}
