using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;

public class Cross : MonoBehaviour
{
    public GameObject swappy;

    //public GameObject Hand1st;
    //public GameObject Hand2nd;
    //public GameObject Hand3rd;
    //public GameObject Hand4th;
    public GameObject Hand5th;


    //public Transform Tile1st;
    //public Transform Tile2nd;
    //public Transform Tile3rd;
    //public Transform Tile4th;
    public Transform Tile5th;

    public Transform Tile6th;
    public Transform Tile7th;
    public Transform Tile8th;
    public Transform Tile9th;
    //public Transform Tile10th;
    //public Transform Tile11th;

    public Image shape;
    public GameObject EndText;
    public GameObject BTN;

    public void FirstSwappy()
    {
        GameObject player = Instantiate(swappy);
        player.transform.position = Tile5th.transform.position + new Vector3(0, 0, -1);
        player.transform.DOScale(new Vector3(1f, 1f, 1f), 0.09f);
        Hand5th.SetActive(false);
        Invoke("Shape", 0.9f);
        Invoke("LineComplete", 0.5f);
    }

    public void LineComplete()
    {
        GameObject player = Instantiate(swappy);
        player.transform.position = Tile6th.transform.position + new Vector3(0, 0, -1);
        player.transform.DOScale(new Vector3(1f, 1f, 1f), 0.09f);

        player = Instantiate(swappy);
        player.transform.position = Tile7th.transform.position + new Vector3(0, 0, -1);
        player.transform.DOScale(new Vector3(1f, 1f, 1f), 0.09f);

        player = Instantiate(swappy);
        player.transform.position = Tile8th.transform.position + new Vector3(0, 0, -1);
        player.transform.DOScale(new Vector3(1f, 1f, 1f), 0.09f);

        player = Instantiate(swappy);
        player.transform.position = Tile9th.transform.position + new Vector3(0, 0, -1);
        player.transform.DOScale(new Vector3(1f, 1f, 1f), 0.09f);
    }

    public void OKBTN()

    {
        GameConfigration.instance.PlayerSound(0);

        TextEnd();
        Destroy(BTN);
    }

    public void Shape()
    {
        GameConfigration.instance.PlayerSound(8);

        shape.gameObject.SetActive(true);
    }

    public void TextEnd()
    {
        EndText.SetActive(true);

    }

    public void NextBTN()
    {
        GameConfigration.instance.PlayerSound(0);

        SceneManager.LoadScene("RightAngle");
    }
}
