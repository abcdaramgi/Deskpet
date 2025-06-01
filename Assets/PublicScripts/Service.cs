using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Service : MonoBehaviour
{
    public static GameObject MainCanvas;

    private void Awake() {
        MainCanvas = GameObject.Find("Canvas");
    }

    public static double GetCurrentTime()
    {
        return ((double)(System.DateTimeOffset.Now.ToUnixTimeMilliseconds()) / 1000f);
    }
}
