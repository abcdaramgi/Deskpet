using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AnimationData
{
    public string animationName;
    public float speed;
    public int x_size;
    public int y_size;
}

[System.Serializable]
public class AnimationDataList
{
    public AnimationData[] animationDatas;
}