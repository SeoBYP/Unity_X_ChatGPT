using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using UnityEngine;
public interface IAuthenticationStrategy
{
    void Authenticate(HttpRequestMessage request);
}

public class AzureADAuthenticator : IAuthenticationStrategy
{
    private readonly string token;

    public AzureADAuthenticator(string clientId, string clientSecret)
    {
        // Azure AD 토큰 발급 로직 구현
        token = "Your_Token";
    }

    public void Authenticate(HttpRequestMessage request)
    {
        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
    }
}

public class OpenAIProxyAuthenticator : IAuthenticationStrategy
{
    private readonly string apiKey;

    public OpenAIProxyAuthenticator(string apiKey)
    {
        this.apiKey = apiKey;
    }

    public void Authenticate(HttpRequestMessage request)
    {
        request.Headers.Add("Authorization", $"Bearer {apiKey}");
    }
}
