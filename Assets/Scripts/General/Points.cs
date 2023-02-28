using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Points : MonoBehaviour
{
    public Text text;

    public void SetPoints(int points)
    {
        text.text = points.ToString();
    }
}
