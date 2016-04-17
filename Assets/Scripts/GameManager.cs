using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public ButtonSmash buttonSmasher;
    public Slider motivationSlider;
    public int maxFails = 3;
    public Animator animator;

    [Range(0, 1)]
    public int level;

    private int failedGroups;
    private float motivation = 1;
    private bool running = true;


    void Awake()
    {
        Instance = this;
        running = true;
    }

    private void CheckStatus()
    {
        if (motivation <= 0.01f)
        {
            Debug.Log("You lost!");
            running = false;
        }

        motivationSlider.value = motivation;
    }

    public void GroupFinished()
    {
        motivation += 0.2f;
        motivation = Mathf.Clamp(motivation, 0, 1);
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

    private IEnumerator FallingAnimation()
    {
        animator.SetTrigger("Fall");
        animator.SetTrigger("Run");
        yield return new WaitForSeconds(4.55f);
        running = true;
    }
}
