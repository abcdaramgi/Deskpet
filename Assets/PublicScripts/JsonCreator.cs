using System.IO;
using UnityEngine;

public class JsonCreator : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AnimationDataList dataList = new AnimationDataList();
        dataList.animationDatas = new AnimationData[1];

        AnimationData data = new AnimationData();
        data.animationName = "Idle";
        data.speed = 1;
        data.x_size = 32;
        data.y_size = 32;

        dataList.animationDatas[0] = data;

        string json = JsonUtility.ToJson(dataList);

        string path = Application.persistentDataPath + "/animationData.json";
        File.WriteAllText(path, json);
    }
}
