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
    private void CreateImage(Texture2D texture)
    {
        resultImage.Configure(texture);
        onMessageReceived?.Invoke();
    }

}
