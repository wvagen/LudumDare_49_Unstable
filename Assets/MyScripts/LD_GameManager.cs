using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LD_GameManager : MonoBehaviour
{
    public GameObject clickGO;
    public GameObject notMeGO;
    public Transform spawnBtnsLocation;

    public Text scoreTxt,bestScoreTxt,bestScoreFinal,scoreFinal;

    public int clickedCount = 0;

    public Animator myAnim;
    public AudioSource musicSource;

    Vector2 screenEdge = new Vector2(7, 4);

    LD_ClickMeBtn spawnedBtn,notMeSpawnedBtn;

    int score = 0;
    int scoreFactor = 10;

    Coroutine myCour;

    bool isWin = false;
    float origPitchMusic;

    private void Start()
    {
        myCour = StartCoroutine(Level_Manager());
        bestScoreTxt.text = "Best : " + PlayerPrefs.GetInt("best", 0).ToString();
        isWin = false;
        origPitchMusic = musicSource.pitch;
    }

    private void Update()
    {
        if (spawnedBtn != null && !isWin)
        {
            if ((spawnedBtn.transform.position.x > screenEdge.x + 5) || (spawnedBtn.transform.position.x < -screenEdge.x - 5)
                || (spawnedBtn.transform.position.y > screenEdge.y + 5) || (spawnedBtn.transform.position.y < -screenEdge.y - 5))
            {
                isWin = true;
                myAnim.Play("Game_Over");
                int bestScore = PlayerPrefs.GetInt("best", 0);
                if (score > bestScore)
                {
                    bestScore = score;
                    PlayerPrefs.SetInt("best", bestScore);
                }

                scoreFinal.text = score.ToString();
                bestScoreFinal.text = bestScore.ToString();
                musicSource.pitch = origPitchMusic;
            }
        }
    }
    
    public void Retry_Game()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void Escape_Button()
    {
        Application.Quit();
    }

    public void Inc_Score()
    {
        if (spawnedBtn != null)
        {
            Destroy(spawnedBtn.gameObject);
        }

        if (notMeSpawnedBtn != null)
        {
            Destroy(notMeSpawnedBtn.gameObject);
        }

        score += scoreFactor;
        scoreTxt.text = score.ToString();
        clickedCount++;

        if (clickedCount == 1) myAnim.Play("Play_Game");
        if (clickedCount > 1) myAnim.Play("ScoreInc",-1,0);

        if (myCour != null)
        StopCoroutine(myCour);
        myCour = StartCoroutine(Level_Manager());

        if (musicSource.pitch < 2)
        musicSource.pitch += 0.01f;
    }

    IEnumerator Level_Manager()
    {
        yield return null;
        if (clickedCount == 0)
        {
            spawnedBtn = Instantiate(clickGO, Vector2.zero , Quaternion.identity, spawnBtnsLocation).GetComponent< LD_ClickMeBtn>();
            spawnedBtn.transform.localScale = Vector2.one * 2;

            score += 0;
            scoreTxt.text = score.ToString();
        }
        else if (clickedCount < 5)
        {
            spawnedBtn = Instantiate(clickGO, new Vector2(Random.Range(-screenEdge.x, screenEdge.x), Random.Range(-screenEdge.y, screenEdge.y)), Quaternion.identity, spawnBtnsLocation).GetComponent<LD_ClickMeBtn>();
            spawnedBtn.transform.localScale = Vector2.one * 1 / (clickedCount * 2);
        }else if (clickedCount < 10)
        {
            if (clickedCount % 2 == 0)
            {
                spawnedBtn = Instantiate(clickGO, new Vector2(-screenEdge.x - 2, Random.Range(-screenEdge.y, screenEdge.y)), Quaternion.identity, spawnBtnsLocation).GetComponent<LD_ClickMeBtn>();
                spawnedBtn.Go(Vector2.right * clickedCount * 2);
            }
            else
            {
                spawnedBtn = Instantiate(clickGO, new Vector2(screenEdge.x + 2, Random.Range(-screenEdge.y, screenEdge.y)), Quaternion.identity, spawnBtnsLocation).GetComponent<LD_ClickMeBtn>();
                spawnedBtn.Go(Vector2.left * clickedCount * 2);
            }
        }
        else if (clickedCount < 15)
        {
            if (Random.Range(0,2) % 2 == 0)
            {
                spawnedBtn = Instantiate(clickGO, new Vector2(Random.Range(-screenEdge.x, screenEdge.x), screenEdge.y + 2), Quaternion.identity, spawnBtnsLocation).GetComponent<LD_ClickMeBtn>();
                spawnedBtn.Go(Vector2.down * clickedCount);
            }
            else
            {
                spawnedBtn = Instantiate(clickGO, new Vector2(Random.Range(-screenEdge.x, screenEdge.x), -screenEdge.y - 2), Quaternion.identity, spawnBtnsLocation).GetComponent<LD_ClickMeBtn>();
                spawnedBtn.Go(Vector2.up * clickedCount);
            }
        }
        else if (clickedCount < 20)
        {
            if (clickedCount % 2 == 0)
            {
                spawnedBtn = Instantiate(clickGO, new Vector2(-screenEdge.x - 2, Random.Range(-screenEdge.y, screenEdge.y)), Quaternion.identity, spawnBtnsLocation).GetComponent<LD_ClickMeBtn>();
                spawnedBtn.Go(Vector2.right * 0.5f);
                yield return new WaitForSeconds(1);
                spawnedBtn.Go(Vector2.right * clickedCount * 5);
                yield return new WaitForSeconds(0.3f);
                spawnedBtn.Go(Vector2.right * 0.5f);
            }
            else
            {
                spawnedBtn = Instantiate(clickGO, new Vector2(screenEdge.x + 2, Random.Range(-screenEdge.y, screenEdge.y)), Quaternion.identity, spawnBtnsLocation).GetComponent<LD_ClickMeBtn>();
                spawnedBtn.Go(Vector2.left * 0.5f);
                yield return new WaitForSeconds(1);
                spawnedBtn.Go(Vector2.left * clickedCount * 5);
                yield return new WaitForSeconds(0.3f);
                spawnedBtn.Go(Vector2.left * 0.5f);
            }
        }
        else if (clickedCount < 21)
        {
            notMeSpawnedBtn = Instantiate(notMeGO, new Vector2(-screenEdge.x - 2, Random.Range(-screenEdge.y, screenEdge.y)), Quaternion.identity, spawnBtnsLocation).GetComponent<LD_ClickMeBtn>();
            notMeSpawnedBtn.Go(Vector2.right * 0.1f);
            yield return new WaitForSeconds(5);
            spawnedBtn = Instantiate(clickGO, new Vector2(screenEdge.x + 2, Random.Range(-screenEdge.y, screenEdge.y)), Quaternion.identity, spawnBtnsLocation).GetComponent<LD_ClickMeBtn>();
            spawnedBtn.Go(Vector2.left * 10);
        }else if (clickedCount < 25)
        {
            spawnedBtn = Instantiate(clickGO, new Vector2(-screenEdge.x - 2, 0), Quaternion.identity, spawnBtnsLocation).GetComponent<LD_ClickMeBtn>();
            spawnedBtn.Go(Vector2.right * clickedCount * 1.5f);
        }
        else if (clickedCount < 26)
        {
            for (int i = 0; i < 100; i++)
            {
                notMeSpawnedBtn = Instantiate(notMeGO, new Vector2(screenEdge.x + 2, Random.Range(-screenEdge.y, screenEdge.y)), Quaternion.identity, spawnBtnsLocation).GetComponent<LD_ClickMeBtn>();
                notMeSpawnedBtn.Go(Vector2.left * 10);
                Destroy(notMeSpawnedBtn.gameObject, 2);
                yield return new WaitForSeconds(Random.Range(0, 0.05f));
            }
            yield return new WaitForSeconds(5);

            spawnedBtn = Instantiate(clickGO, new Vector2(screenEdge.x + 2, Random.Range(-screenEdge.y, screenEdge.y)), Quaternion.identity, spawnBtnsLocation).GetComponent<LD_ClickMeBtn>();
            spawnedBtn.transform.localScale = Vector2.one * 0.05f;
            spawnedBtn.Go(Vector2.left * 1);
        }
        else
        {
            switch (Random.Range(0, 4))
            {
                case 0:
                    spawnedBtn = Instantiate(clickGO, new Vector2(-screenEdge.x - 2, Random.Range(-screenEdge.y, screenEdge.y)), Quaternion.identity, spawnBtnsLocation).GetComponent<LD_ClickMeBtn>();
                    spawnedBtn.Go(Vector2.right * clickedCount);
                    break;
                 case 1:
                    spawnedBtn = Instantiate(clickGO, new Vector2(screenEdge.x + 2, Random.Range(-screenEdge.y, screenEdge.y)), Quaternion.identity, spawnBtnsLocation).GetComponent<LD_ClickMeBtn>();
                    spawnedBtn.Go(Vector2.left * clickedCount);
                    break;
                case 2:
                    spawnedBtn = Instantiate(clickGO, new Vector2(Random.Range(-screenEdge.x, screenEdge.x), screenEdge.y + 2), Quaternion.identity, spawnBtnsLocation).GetComponent<LD_ClickMeBtn>();
                    spawnedBtn.Go(Vector2.down * clickedCount);
                    break;
                case 3:
                    spawnedBtn = Instantiate(clickGO, new Vector2(screenEdge.x + 2, Random.Range(-screenEdge.y, screenEdge.y)), Quaternion.identity, spawnBtnsLocation).GetComponent<LD_ClickMeBtn>();
                    spawnedBtn.Go(Vector2.left * clickedCount);
                    break;
            }
        }
    }
}
