using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    [SerializeField] Image heart1, heart2, heart3;

    public void SwitchHeart(int index)
    {
        switch (index)
        {
            case 1:
                heart1.enabled = heart1.isActiveAndEnabled ? false : true;
                break;
            case 2:
                heart2.enabled = heart2.isActiveAndEnabled ? false : true;
                break;
            case 3:
                heart3.enabled = heart3.isActiveAndEnabled ? false : true;
                break;
        }
    }
}
