using UnityEngine;
using System.Collections;
using Assets.Scripts.CloudBread;

public class ScoreManagerScript : MonoBehaviour {

    public static int Score { get; set; }

    private bool deadRefreshFlag = true;

	// Use this for initialization
	void Start () {
        (Tens.gameObject as GameObject).SetActive(false);
        (Hundreds.gameObject as GameObject).SetActive(false);
        deadRefreshFlag = true;
    }
	
	// Update is called once per frame
	void Update () {

        if (GameStateManager.GameState == GameState.Playing)
        {
            if (previousScore != Score) //save perf from non needed calculations
            {
                if (Score < 10)
                {
                    //just draw units
                    Units.sprite = numberSprites[Score];
                }
                else if (Score >= 10 && Score < 100)
                {
                    (Tens.gameObject as GameObject).SetActive(true);
                    Tens.sprite = numberSprites[Score / 10];
                    Units.sprite = numberSprites[Score % 10];
                }
                else if (Score >= 100)
                {
                    (Hundreds.gameObject as GameObject).SetActive(true);
                    Hundreds.sprite = numberSprites[Score / 100];
                    int rest = Score % 100;
                    Tens.sprite = numberSprites[rest / 10];
                    Units.sprite = numberSprites[rest % 10];
                }
            }

        }
        else if (GameStateManager.GameState == GameState.Dead)
        {
                
            (Units.gameObject as GameObject).SetActive(false);
            (Tens.gameObject as GameObject).SetActive(false);
            (Hundreds.gameObject as GameObject).SetActive(false);

            setScorewithSpirte(NewScoreUnit, Score);

            int bestScore = 0;

            if (PlayerPrefs.HasKey("bestScore"))
            {
                bestScore = PlayerPrefs.GetInt("bestScore");
            }
            else
            {
                PlayerPrefs.SetInt("bestScore", bestScore);
            }

            if (bestScore >= Score)
            {
                setScorewithSpirte(BestScoreUnit, bestScore);
            }
            else
            {
                PlayerPrefs.SetInt("bestScore", Score);
                setScorewithSpirte(BestScoreUnit, Score);
            }

            if (deadRefreshFlag)
            {
                deadRefreshFlag = false;
                CloudBread cb = new CloudBread();
                cb.CBComUdtMemberGameInfoes(Callback_Success);
            }
        }
        

    }

    public void Callback_Success(string id, WWW www)
    {
        print("[" + id + "] Success");
    }

    private void setScorewithSpirte(SpriteRenderer[] renders, int Score)
    {
        foreach(var render in renders)
        {
            (render.gameObject as GameObject).SetActive(false);
        }

        // 0-> 1의 자리, 1-> 10의 자리, 2> 100의자리
        if (Score >=0 )
        {
            (renders[0].gameObject as GameObject).SetActive(true);
            renders[0].sprite = smallNumberSprites[Score % 10];
        }

        if(Score >= 10)
        {
            (renders[1].gameObject as GameObject).SetActive(true);
            int rest = Score % 100;
            renders[1].sprite = smallNumberSprites[rest / 10];
        }

        if(Score >= 100)
        {
            (renders[2].gameObject as GameObject).SetActive(true);
            int rest = Score % 1000;
            renders[2].sprite = smallNumberSprites[rest / 100];
        }
    }

    int previousScore = -1;
    public Sprite[] numberSprites;
    public SpriteRenderer Units, Tens, Hundreds;

    public Sprite[] smallNumberSprites;
    public SpriteRenderer[] NewScoreUnit;
    public SpriteRenderer[] BestScoreUnit;

    public Sprite[] medalSprites;
    public SpriteRenderer medal;
}
