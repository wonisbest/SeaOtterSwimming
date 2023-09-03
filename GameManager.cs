using DG.Tweening;
using GameControl;
using ManagerControl;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    private int stage;

    [SerializeField] private ObstacleSpawner obstacleSpawner;
    [SerializeField] private Transform player;
    [SerializeField] private SpriteRenderer firstRenderer;
    [SerializeField] private SpriteRenderer secondRenderer;
    [SerializeField] private SpriteRenderer thirdRenderer;
    public float BackgroundMinY { get; private set; }
    [SerializeField] private SpriteRenderer background;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject gameClearPanel;
    [SerializeField] private Button reStartButton;
    [SerializeField] private Button reStartButton_GameClear;
    [SerializeField] private Button HomeButton;

    private void Awake()
    {
        reStartButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            Time.timeScale = 1;
        });

        reStartButton_GameClear.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            Time.timeScale = 1;
        });
    }

    private void Start()
    {
        SoundManager.Instance.PlayBGM(SoundManager.InGameBGM);
        gameOverPanel.SetActive(false);
        BackgroundMinY = background.bounds.min.y;
        stage = 0;
        Time.timeScale = 1;
    }

    public void GameOver()
    {
        SoundManager.Instance.PlaySound(SoundManager.FailEnding);
        Time.timeScale = 0f;
        gameOverPanel.SetActive(true);
    }

    public void GameClear()
    {
        SoundManager.Instance.PlaySound(SoundManager.ClearEnding);
        Time.timeScale = 0f;
        gameClearPanel.SetActive(true);
    }

    public void Func_HomeButton()
    {
        SceneManager.LoadScene("StartScene");
        Time.timeScale = 1;
    }


    public void BoxFadeIn()
    {
        player.GetComponent<Health>().decreaseAmount = 0.3f;
        player.GetComponent<Breathe>().decreaseAmount = 0.3f;

        obstacleSpawner.InvokeGlacier();
        stage = 1;
        secondRenderer.color = new Color(255, 255, 255);
        firstRenderer.DOColor(new Color(255, 255, 255, 0), 1f);
        secondRenderer.DOColor(new Color(255, 255, 255, 1), 0.7f);
    }

    public void Box2FadeIn()
    {
        player.GetComponent<Health>().decreaseAmount = 0.5f;
        player.GetComponent<Breathe>().decreaseAmount = 0.5f;
        obstacleSpawner.InvokeMonkFish();
        stage = 2;
        thirdRenderer.color = new Color(255, 255, 255, 0);
        secondRenderer.DOColor(new Color(255, 255, 255, 0), 0.5f);
        thirdRenderer.DOColor(new Color(255, 255, 255, 1), 0.5f);
    }
}