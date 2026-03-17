using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;


public class DragAndDrop : MonoBehaviour
{
    public Tiles MyTile = null;

    public Renderer SwappyMesh;


    private void OnMouseDown()
    {
        Gamemanager.instance.SelectNSwap(this);

    }


}
