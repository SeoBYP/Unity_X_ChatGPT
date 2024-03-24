using Meta.WitAi.TTS.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TTSBridge : MonoBehaviour
{

    [Header(" Elements ")]
    [SerializeField] private TTSSpeaker speaker;

    // Start is called before the first frame update
    void Start()
    {
        DiscussionBubble.onVoiceButtonClicked += VoiceButtonClickedCallback;
        //DiscussionManager.onChatGPTMessageReceived += Speak;
    }

    private void OnDestroy()
    {
        DiscussionBubble.onVoiceButtonClicked -= VoiceButtonClickedCallback;
        //DiscussionManager.onChatGPTMessageReceived -= Speak;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void VoiceButtonClickedCallback(string message)
    {
        if(speaker.IsSpeaking)
        {
            Debug.Log("Stopping the speaker");
            speaker.Stop();
        }
        else
        {
            Debug.Log("Started Speakings");
            Speak(message);
        }
    }

    private void Speak(string message)
    {
        // Split ChatGPT answer
        string[] messages = message.Split('.');
        speaker.StartCoroutine(speaker.SpeakQueuedAsync(messages));

        //speaker.Speak(message);
    }
}
