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
    public Text scoreText;

    public GameObject looseScreen;
    public Text looseScore;
    public Text looseHighscore;

    public GameObject boxing;
    public GameObject treadmill;
    public int fitnessSwitch = 500;
    public int levelSwitch = 250;

    private int level;
    private int failedGroups;
    private float motivation = 1;
    private bool running = true;
    private int score;
    private int highscore;

    private Animator animatorTreadmill;
    private Animator animatorBoxing;

    void Awake()
    {
        Instance = this;
        running = true;
    }

    void Start()
    {
        Cursor.visible = false;
        highscore = PlayerPrefs.GetInt("highscore");
        animatorTreadmill = treadmill.GetComponent<Animator>();
        animatorBoxing = boxing.GetComponent<Animator>();
    }

    private void CheckStatus()
    {
        if (motivation <= 0.01f)
        {
            Loose();
        }

        motivationSlider.value = motivation;

        if (score >= fitnessSwitch)
        {
            treadmill.SetActive(false);
            boxing.SetActive(true);
        }

        if (score >= levelSwitch)
        {
            level = 1;
        }
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
        Cursor.visible = true;
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
        StartCoroutine(FailAnimation());
        CheckStatus();
    }

    public bool IsRunning()
    {
        return running;
    }

    public int GetLevel()
    {
        return level;
    }

    public void OpenMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    private IEnumerator FailAnimation()
    {
        if (score < fitnessSwitch)
        {
            animatorTreadmill.SetTrigger("fail");
            yield return new WaitForSeconds(4.55f);
        }
        else
        {
            animatorBoxing.SetTrigger("fail");
            yield return new WaitForSeconds(5.25f);
        }

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
