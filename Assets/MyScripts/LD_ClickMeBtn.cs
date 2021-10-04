using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LD_ClickMeBtn : MonoBehaviour
{

    LD_GameManager gameOverMan;

    public AudioSource myAudioSource;
    public AudioClip fartSound;
    public Rigidbody2D myRig;


    // Start is called before the first frame update
    void Start()
    {
        gameOverMan = FindObjectOfType<LD_GameManager>();
        StartCoroutine(Spawn_Me());
    }

    IEnumerator Spawn_Me()
    {
        Vector2 myScale = transform.localScale;
        Vector2 currentScale = Vector2.zero;

        transform.localScale = Vector2.zero;
        while (transform.localScale.x < myScale.x) {
            currentScale.x += Time.deltaTime * myScale.x * 5;
            currentScale.y += Time.deltaTime * myScale.x * 5;
            transform.localScale = currentScale;
            yield return new WaitForEndOfFrame();
        }
    }

    public void Go(Vector2 velocity)
    {
        myRig.velocity = velocity;
    }


    public void Click_Me()
    {
        gameOverMan.Inc_Score();
    }

    public void Not_Click_Me()
    {
        myAudioSource.PlayOneShot(fartSound);
    }




}
