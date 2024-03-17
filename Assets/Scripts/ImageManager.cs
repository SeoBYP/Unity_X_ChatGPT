using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using OpenAI;
using OpenAI.Chat;
using OpenAI.Images;
using OpenAI.Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class ImageManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private ChatGPTResultImage resultImage;
    [SerializeField] private TMP_InputField inputField;

    [Header("Events")]
    public static Action onMessageReceived;

    public async void AskButtonCallback()
    {
        try
        {
            var results = await NetworkSystem.Instance.CreateImage(inputField.text);
            CreateImage(results);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    public async void AskEditImageButtonCallback()
    {
        try
        {
            await PickAndEditImage();
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }
    private void CreateImage(Texture2D texture)
    {
        resultImage.Configure(texture);
        onMessageReceived?.Invoke();
    }
    private void TakeScreenshotAndSaveButtonClicked()
    {
        TakeScreenshotAndSave().Forget();
    }

    private async UniTask TakeScreenshotAndSave()
    {
        await UniTask.WaitForEndOfFrame();

        Texture2D screenshot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        screenshot.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        screenshot.Apply();

        // 스크린샷을 갤러리에 저장
        NativeGallery.Permission permission = NativeGallery.SaveImageToGallery(screenshot, "GalleryTest", "Screenshot_{0}.png", (success, path) => Debug.Log($"Media save result: {success} Path: {path}"));

        // 메모리 누수 방지
        Destroy(screenshot);
    }
    private async UniTask PickAndEditImage()
    {
        // 갤러리에서 이미지를 선택합니다.
        NativeGallery.Permission permission = NativeGallery.GetImageFromGallery(async (path) =>
        {
            Debug.Log("Image path: " + path);
            if (!string.IsNullOrEmpty(path))
            {
                // 이 예시에서는 maskAssetPath와 imageCount는 예시 값으로 사용됩니다.
                // 실제 구현에서는 적절한 값으로 대체해야 합니다.
                int imageCount = 1; // 필요한 이미지 개수

                try
                {
                    // 갤러리에서 선택된 이미지의 경로를 사용하여 이미지 편집을 요청합니다.
                    Texture2D editedTexture = await NetworkSystem.Instance.EditImage(path, inputField.text, imageCount);

                    // 편집된 이미지를 UI에 표시합니다.
                    if (editedTexture != null)
                    {
                        CreateImage(editedTexture);
                    }
                    else
                    {
                        Debug.LogError("Failed to edit image.");
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError("EditImage error: " + e.Message);
                }
            }
        });

        Debug.Log("Permission result: " + permission);
    }

    private void PickImage(int maxSize)
    {
        // 갤러리에서 이미지를 선택
        NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
        {
            if (path != null)
            {
                // 선택된 이미지로 텍스처 생성
                Texture2D texture = NativeGallery.LoadImageAtPath(path, maxSize, markTextureNonReadable: false);
                if (texture != null)
                {
                    resultImage.Configure(texture);
                }
            }
        });
    }

    private void PickVideo()
    {
        // 갤러리에서 비디오를 선택
        NativeGallery.GetVideoFromGallery(path =>
        {
            Debug.Log($"Video path: {path}");
            if (!string.IsNullOrEmpty(path))
            {
                // 선택된 비디오 재생
                Handheld.PlayFullScreenMovie($"file://{path}");
            }
        });
    }
}
