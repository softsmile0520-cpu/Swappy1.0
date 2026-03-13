using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationFunctions : MonoBehaviour
{

    public void DestroyPopUp()
    {
        Destroy(this.transform.parent.gameObject);
    }
}
