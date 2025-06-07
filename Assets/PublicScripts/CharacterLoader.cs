using UnityEngine;
using System.IO;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using System.Collections.Generic;
public class CharacterLoader : MonoBehaviour
{
    public Transform CharacterList;
    public GameObject CharacterButton;

    public Image testImage;
    void Start()
    {
        GetCharacterList();
    }

    private void GetCharacterList()
    {
        string persistentPath = Application.persistentDataPath + "/Characters/";

        if (Directory.Exists(persistentPath))
        {
            string[] directories = Directory.GetDirectories(persistentPath);

            foreach (string dir in directories)
            {
                string lastFolderName = Path.GetFileName(dir);
                HasAnimationPack(lastFolderName);

                GameObject chr = Instantiate(CharacterButton, CharacterList);
                chr.name = lastFolderName;
                chr.GetComponentInChildren<TMP_Text>().text = lastFolderName;

                chr.GetComponent<Button>().onClick.AddListener(() =>
                {
                    Debug.Log($"Character is {lastFolderName}");
                    HasAnimationPack(lastFolderName);
                });
            }
        }
        else
        {
            Debug.LogWarning("Persistent path does not exist!");
        }
    }

    private void HasAnimationPack(string characterName)
    {
        // 파일이 없으므로 생성
        CustomAnimationPack pack = ScriptableObject.CreateInstance<CustomAnimationPack>();

        // 올바른 방식
        CustomAnimation IdleAnimation = ScriptableObject.CreateInstance<CustomAnimation>();
        IdleAnimation.Key = "Idle";
        IdleAnimation.frameSpeed = 1f;

        string idleSprite = Application.persistentDataPath + "/Characters/" + characterName + "/Idle_Front.png";

        if (File.Exists(idleSprite))
        {
            byte[] imageData = File.ReadAllBytes(idleSprite);

            AnimationDataList loadedData = HasAnimationData(characterName);
            AnimationData IdleData = loadedData.animationDatas[0];
            AnimationData WalkData = loadedData.animationDatas[1];
            AnimationData PickUpData = loadedData.animationDatas[2];

            Texture2D texture = new Texture2D(2, 2);
            if (texture.LoadImage(imageData)) // PNG, JPG 가능
            {
                int width = texture.width;
                int height = texture.height;
                Debug.Log($"불러온 이미지의 크기: {width}x{height}");

                texture.filterMode = FilterMode.Point;

                int xSize = IdleData.x_size;
                int ySize = IdleData.y_size;

                List<Sprite> idleSprites = new List<Sprite>();

                int columns = width / xSize;
                int rows = height / ySize;

                for (int y = rows - 1; y >= 0; y--) // Unity는 Y축이 위로 증가
                {
                    for (int x = 0; x < columns; x++)
                    {
                        Rect rect = new Rect(x * xSize, y * ySize, xSize, ySize);
                        Sprite sprite = Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f));
                        idleSprites.Add(sprite);
                    }
                }

                pack.animations = new CustomAnimation[3];
                pack.animations[0] = ScriptableObject.CreateInstance<CustomAnimation>();
                pack.animations[0].sprites = new Sprite[idleSprites.Count];
                pack.animations[0].sprites = idleSprites.ToArray();

                if (testImage.GetComponent<PetAnimator>() == null)
                {
                    testImage.AddComponent<PetAnimator>().AnimationPack = pack;
                }
                else
                {
                    testImage.GetComponent<PetAnimator>().AnimationPack = pack;
                }

                

                Debug.Log($"Idle 스프라이트 수: {idleSprites.Count}");
            }
            else
            {
                Debug.LogError("이미지를 불러오는 데 실패했습니다.");
            }
        }
        else
        {
            Debug.Log($"{characterName} does not exist");
        }
    }

    private AnimationDataList HasAnimationData(string characterName)
    {
        string animationData = Application.persistentDataPath + "/Characters/" + characterName + "/animationData.json";

        if (File.Exists(animationData))
        {
            string loadedJson = File.ReadAllText(animationData);
            AnimationDataList loadedData = JsonUtility.FromJson<AnimationDataList>(loadedJson);

            return loadedData;
        }

        return null;
    }
}