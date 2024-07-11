using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAutoKick : MonoBehaviour
{
    public void OnClick()
    {
        if (GameController.instant.isBallMoving)
            return;
        GameController.instant.AutoKick();
    }
}
