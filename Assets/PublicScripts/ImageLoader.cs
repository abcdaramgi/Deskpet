using UnityEngine;
using Kirurobo;
using System.IO;

public class ImageLoader : MonoBehaviour
{
    public SpriteRenderer targetRenderer;

    void Start()
    {
        OpenResourcesFolder();
        
        Sprite sprite = Resources.Load<Sprite>("Images/test"); // 확장자 없이 경로만
        if (sprite != null)
        {
            targetRenderer.sprite = sprite;
        }
        else
        {
            Debug.LogError("스프라이트 로드 실패");
        }
    }

    public void OpenResourcesFolder()
    {
        string path = Path.Combine(Application.dataPath, "Resources");
        System.Diagnostics.Process.Start(path);
        
    }

    public void OpenSingleFile()
    {
        FilePanel.Settings settings = new FilePanel.Settings();

        settings.filters = new FilePanel.Filter[]
        {
            new FilePanel.Filter("Image files (*.png;*.jpg;*.jpeg;)", "png", "jpg", "jpeg")
        };

        settings.title = "Open a file!";
        settings.initialDirectory = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyPictures);

        FilePanel.OpenFilePanel(settings, (files) =>
        {
            Debug.Log($"Open a file\n" + string.Join("\n", files));
        });
    }
}