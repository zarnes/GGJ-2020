using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndPanel : MonoBehaviour
{
    public float ScoreTime = 1.5f;
    public AnimationCurve ScoreCurve;
    public Text ScoreText;

    public Button NextLevelbutton;

    private int _finalScore;
    private Animator _anim;

    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();
    }

    public void Restart()
    {
        LevelManager.Instance.ReloadLevel();
    }

    public void NextLevel()
    {
        LevelManager.Instance.LoadNextLevel();
    }

    public void BackToMenu()
    {
        LevelManager.Instance.LoadMenu();
    }

    public void Show(int score, bool haveNextLevel)
    {
        if (!haveNextLevel)
            NextLevelbutton.interactable = false;

        _finalScore = score;
        _anim.SetTrigger("End");
    }

    public void ShowScore()
    {
        StartCoroutine(ShowScoreCoroutine());
    }

    private IEnumerator ShowScoreCoroutine()
    {
        float start = Time.time;
        float end = start + ScoreTime;
        float current = start;
        while (current <= end)
        {
            current = Time.time;
            float percentage = Mathf.InverseLerp(start, end, current);
            Debug.Log(percentage);
            int scoreToDisplay = (int) (ScoreCurve.Evaluate(percentage) * (float) _finalScore);
            ScoreText.text = scoreToDisplay.ToString();
            yield return null;
        }

        ScoreText.text = _finalScore.ToString();
        _anim.SetTrigger("Score Wobble");

        yield return new WaitForSeconds(1f);

        _anim.SetTrigger("Buttons");
    }
}
