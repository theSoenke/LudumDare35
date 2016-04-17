using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public ButtonSmash buttonSmasher;
    public Slider motivationSlider;
    public int maxFails = 3;
    public Animator animator;
    public Text scoreText;

    public GameObject looseScreen;
    public Text looseScore;
    public Text looseHighscore;

    [Range(0, 1)]
    public int level;

    private int failedGroups;
    private float motivation = 1;
    private bool running = true;
    private int score;
    private int highscore;

    void Awake()
    {
        Instance = this;
        running = true;
    }

    void Start()
    {
        highscore = PlayerPrefs.GetInt("highscore");
    }

    private void CheckStatus()
    {
        if (motivation <= 0.01f)
        {
            Loose();
        }

        motivationSlider.value = motivation;
    }

    private void Loose()
    {
        Debug.Log("You lost!");
        running = false;

        SaveHighscore();
        looseScreen.SetActive(true);
        scoreText.gameObject.SetActive(false);

        looseScore.text = score.ToString();
        looseHighscore.text = highscore.ToString();
    }

    public void GroupFinished()
    {
        motivation += 0.2f;
        motivation = Mathf.Clamp(motivation, 0, 1);
        score += 10;
        scoreText.text = score.ToString();
        CheckStatus();
    }

    public void GroupFailed()
    {
        failedGroups++;
        motivation -= 1 / (float)maxFails;

        running = false;
        StartCoroutine(FallingAnimation());
        CheckStatus();
    }

    public bool IsRunning()
    {
        return running;
    }

    public void OpenMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    private IEnumerator FallingAnimation()
    {
        animator.SetTrigger("Fall");
        yield return new WaitForSeconds(4.55f);
        running = true;
    }

    private void SaveHighscore()
    {
        if (score > highscore)
        {
            Debug.Log("New highscore");
            highscore = score;
            PlayerPrefs.SetInt("highscore", highscore);
        }
    }
}
