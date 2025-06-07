using UnityEngine;
using System.IO;

public class ImageExporter : MonoBehaviour
{
    public Sprite spriteToExport;

    public void Start()
    {
        ExportSpriteToPNG();
    }
    
    public void ExportSpriteToPNG()
    {
        // Sprite의 텍스처 가져오기
        Texture2D texture = spriteToExport.texture;

        // 스프라이트 영역 크롭 (선택사항)
        Rect rect = spriteToExport.textureRect;
        Texture2D cropped = new Texture2D((int)rect.width, (int)rect.height);
        Color[] pixels = texture.GetPixels(
            (int)rect.x, (int)rect.y, (int)rect.width, (int)rect.height);
        cropped.SetPixels(pixels);
        cropped.Apply();

        // PNG 인코딩
        byte[] bytes = cropped.EncodeToPNG();

        // 저장
        string filePath = Path.Combine(Application.persistentDataPath, "exported_sprite.png");
        File.WriteAllBytes(filePath, bytes);
        Debug.Log("Exported to: " + filePath);
    }
}