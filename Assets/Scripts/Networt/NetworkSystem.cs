using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenAI;
using OpenAI.Chat;
using OpenAI.Models;
using Cysharp.Threading.Tasks;
using System;
using OpenAI.Images;
using System.Net.Http;
using System.Text;
using OpenAI.Audio;
public class NetworkSystem : MonoBehaviour
{
    private static NetworkSystem _instance;
    public static NetworkSystem Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<NetworkSystem>();
            }
            return _instance;
        }
    }

    [SerializeField]
    private OpenAIConfiguration openAIConfiguration;
    private OpenAIClient openAIClient;
    void Awake()
    {
        _instance = this;
        openAIClient = new OpenAIClient(openAIConfiguration);
    }

    // 채팅 응답 받기
    public async UniTask<string> GetChatResponse(List<Message> messages)
    {
        var chatRequest = new ChatRequest(messages, Model.GPT3_5_Turbo, 0.2);
        var response = await openAIClient.ChatEndpoint.GetCompletionAsync(chatRequest);
        return response.FirstChoice; // 첫번째 응답 반환
    }


    // 오디오 스피치 생성
    public async UniTask<AudioClip> CreateSpeech(string text)
    {
        var request = new SpeechRequest(text);
        var (path, clip) = await openAIClient.AudioEndpoint.CreateSpeechAsync(request);
        return clip; // 생성된 오디오 클립 반환
    }

    // 오디오 트랜스크립션
    public async UniTask<string> CreateTranscription(AudioClip audioClip)
    {
        var request = new AudioTranscriptionRequest(audioClip, "en");
        var result = await openAIClient.AudioEndpoint.CreateTranscriptionAsync(request);
        return result; // 트랜스크립션 텍스트 반환
    }
    
    // 이미지 생성
    public async UniTask<Texture2D> CreateImage(string prompt, int numberOfResults = 1)
    {
        var request = new ImageGenerationRequest(prompt, Model.DallE_3, numberOfResults);
        var imageResults = await openAIClient.ImagesEndPoint.GenerateImageAsync(request);
        return imageResults[0].Texture; // 첫번째 이미지 반환
    }

    // 이미지 편집
    public async UniTask<Texture2D> EditImage(string imageAssetPath, string prompt, int imageCount)
    {
        var request = new ImageEditRequest(imageAssetPath, prompt, imageCount, ImageSize.Small);
        var imageResults = await openAIClient.ImagesEndPoint.CreateImageEditAsync(request);
        return imageResults[0].Texture; // 편집된 이미지 반환
    }

    // 이미지 변형
    public async UniTask<Texture2D> CreateImageVariation(string imagePath, int imageCount)
    {
        var request = new ImageVariationRequest(imagePath, imageCount, ImageSize.Small);
        var imageResults = await openAIClient.ImagesEndPoint.CreateImageVariationAsync(request);
        return imageResults[0].Texture; // 변형된 이미지 반환
    }
}
