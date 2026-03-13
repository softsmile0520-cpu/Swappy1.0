using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Tutorail4 : MonoBehaviour
{
    public GameObject Tip1;
    public GameObject Tip2;
    public GameObject Tip3;

    public Button Next;

    public GameObject Hand;
    public GameObject HandContainer;

    public GameObject swappy;
    public GameObject ScoreDiff1;
    public GameObject ScoreDiff2;
    public TextMeshProUGUI PlayerScore;
    public TextMeshProUGUI EnemyScore;


    //shape1Tiles
    public GameObject SwappyToPlace;
    public GameObject Tile1;
    public GameObject Tile2;
    public GameObject Tile3;
    public GameObject Tile4;

    public GameObject ShapeMade;

    //Extras
    int n = 0;
    int b = 0;

    public TextMeshProUGUI textToModify;
    public float duration = 0.5f;
    public float targetSize = 140f;
    public void NextButton()
    {
        if (n == 0)
        {
            SceneManager.LoadScene("Vertical 5");
            GameConfigration.instance.PlayerSound(0);
        }
        else if (n == 1)
        {

        }
    }

    private void Start()
    {
        //InvokeRepeating("HandShadking", 0f, 0.2f);

    }
    IEnumerator IncreaseTextSizeOverTime()
    {
        float startTime = Time.time;
        while (Time.time - startTime <= duration)
        {
            float t = (Time.time - startTime) / duration;
            textToModify.fontSize = (int)Mathf.Lerp(0f, targetSize, t);
            yield return null;
        }
        textToModify.fontSize = (int)targetSize;

        // Wait for 2 seconds before toggling off the text

        // Toggle off the text object
        textToModify.gameObject.SetActive(false);
    }
    public void HandShadking()
    {
        Hand.SetActive(!Hand.activeSelf);
    }
    public void MakingSahpe()
    {
        StartCoroutine(makeShape());
    }


    public IEnumerator makeShape()
    {
        HandContainer.SetActive(false);

        GameObject player = Instantiate(swappy);
        player.transform.position = SwappyToPlace.transform.position + new Vector3(0, 0, -1);
        player.transform.DOScale(new Vector3(1f, 1f, 1f), 0.09f);

        ShapeMade.SetActive(true);
        GameConfigration.instance.PlayerSound(2);
        yield return new WaitForSeconds(0.1f);

        GameObject player1 = Instantiate(swappy);
        player1.transform.position = Tile1.transform.position + new Vector3(0, 0, -1);
        player1.transform.DOScale(new Vector3(1f, 1f, 1f), 0.09f);

        GameConfigration.instance.PlayerSound(2);

        GameObject player2 = Instantiate(swappy);
        player2.transform.position = Tile2.transform.position + new Vector3(0, 0, -1);
        player2.transform.DOScale(new Vector3(1f, 1f, 1f), 0.09f);

        GameConfigration.instance.PlayerSound(2);

        PlayerScorePopUp.ShowUI();
        PlayerScorePopUp.instance.ShowScore(+5);
        //StartCoroutine(IncreaseTextSizeOverTime());

        GameObject player3 = Instantiate(swappy);
        player3.transform.position = Tile3.transform.position + new Vector3(0, 0, -1);
        player3.transform.DOScale(new Vector3(1f, 1f, 1f), 0.09f);

        GameConfigration.instance.PlayerSound(2);

        GameObject player4 = Instantiate(swappy);
        player4.transform.position = Tile4.transform.position + new Vector3(0, 0, -1);
        player4.transform.DOScale(new Vector3(1f, 1f, 1f), 0.09f);

        GameConfigration.instance.PlayerSound(2);


        yield return new WaitForSeconds(0.1f);

        ScoreDiff2.SetActive(true);
        PlayerScore.text = "9";
        Tip1.SetActive(false);
        Next.gameObject.SetActive(true);
    }
    public void BackTOMenu()
    {
        SceneManager.LoadScene("MenuScene");
        GameConfigration.instance.PlayerSound(0);
    }
}
