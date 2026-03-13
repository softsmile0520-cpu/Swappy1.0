using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Tutorail3 : MonoBehaviour
{
    public GameObject Tip1;
    public GameObject Tip2;
    public GameObject Tip3;

    public Button Next;

    public GameObject Hand;
    public GameObject HandContainer;
    public Transform HandContainer2;

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
    public GameObject Tile5;
    public GameObject Tile6;
    public GameObject Tile7;
    public GameObject Tile8;
    public GameObject Tile9;
    public GameObject Tile10;

    public GameObject ShapeMade;
    public GameObject ShapeMade2;

    //Extras
    int n = 0;
    int b = 0;
    public GameObject Cube1;
    public GameObject Cube2;
    public GameObject Cube3;
    public GameObject Cube4;
    public GameObject Cube5;
    public GameObject Cube6;

    public TextMeshProUGUI textToModify;
    public float duration = 0.5f;
    public float targetSize = 140f;
    public void NextButton()
    {
        if (n == 0)
        {
            SceneManager.LoadScene("Vertical 4");
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
        ShapeMade2.SetActive(true);
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
        PlayerScorePopUp.instance.ShowScore(+11);
        //StartCoroutine(IncreaseTextSizeOverTime());

        Destroy(Cube1);
        GameObject player3 = Instantiate(swappy);
        player3.transform.position = Tile3.transform.position + new Vector3(0, 0, -1);
        player3.transform.DOScale(new Vector3(1f, 1f, 1f), 0.09f);

        GameConfigration.instance.PlayerSound(2);

        Destroy(Cube2);
        GameObject player4 = Instantiate(swappy);
        player4.transform.position = Tile4.transform.position + new Vector3(0, 0, -1);
        player4.transform.DOScale(new Vector3(1f, 1f, 1f), 0.09f);

        GameConfigration.instance.PlayerSound(2);

        Destroy(Cube3);
        GameObject player5 = Instantiate(swappy);
        player5.transform.position = Tile5.transform.position + new Vector3(0, 0, -1);
        player5.transform.DOScale(new Vector3(1f, 1f, 1f), 0.09f);

        GameConfigration.instance.PlayerSound(2);

        Destroy(Cube4);
        GameObject player6 = Instantiate(swappy);
        player6.transform.position = Tile6.transform.position + new Vector3(0, 0, -1);
        player6.transform.DOScale(new Vector3(1f, 1f, 1f), 0.09f);

        GameConfigration.instance.PlayerSound(2);

        Destroy(Cube5);
        GameObject player7 = Instantiate(swappy);
        player7.transform.position = Tile7.transform.position + new Vector3(0, 0, -1);
        player7.transform.DOScale(new Vector3(1f, 1f, 1f), 0.09f);

        GameConfigration.instance.PlayerSound(2);

        Destroy(Cube6);
        GameObject player8 = Instantiate(swappy);
        player8.transform.position = Tile8.transform.position + new Vector3(0, 0, -1);
        player8.transform.DOScale(new Vector3(1f, 1f, 1f), 0.09f);

        GameConfigration.instance.PlayerSound(2);

        GameObject player9 = Instantiate(swappy);
        player9.transform.position = Tile9.transform.position + new Vector3(0, 0, -1);
        player9.transform.DOScale(new Vector3(1f, 1f, 1f), 0.09f);

        GameConfigration.instance.PlayerSound(2);

        GameObject player10 = Instantiate(swappy);
        player10.transform.position = Tile10.transform.position + new Vector3(0, 0, -1);
        player10.transform.DOScale(new Vector3(1f, 1f, 1f), 0.09f);

        GameConfigration.instance.PlayerSound(2);

        yield return new WaitForSeconds(0.1f);

        ScoreDiff1.SetActive(true);
        ScoreDiff2.SetActive(true);
        PlayerScore.text = "27";
        EnemyScore.text = "19";
        Tip1.SetActive(false);
        Tip2.SetActive(true);
        Next.gameObject.SetActive(true);
    }
    public void BackTOMenu()
    {
        SceneManager.LoadScene("MenuScene");
        GameConfigration.instance.PlayerSound(0);
    }
}
