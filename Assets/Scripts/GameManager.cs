using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    public bool Running { get; private set; }

    [SerializeField]
    private GameObject gameOverPanel;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        Init();
    }

    private void OnDestroy()
    {
        PlayerController.OnPlayerHeathChange -= OnPlayerHealthChanged;
    }

    private void Init()
    {
        PlayerController.OnPlayerHeathChange += OnPlayerHealthChanged;

        Running = true;
    }

    private void OnPlayerHealthChanged(int health)
    {
        if (health <= 0)
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        gameOverPanel.SetActive(true);

        PlayerController.OnPlayerHeathChange -= OnPlayerHealthChanged;

        Running = false;
    }

    public void Reload()
    {
        gameOverPanel.SetActive(false);

        SceneManager.LoadScene("Main");

        Init();
    }
}
