using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Http;
public static class APIRequestFactory
{
    public static HttpRequestMessage CreateGetRequest(string url, Dictionary<string, string> headers = null)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        AddHeaders(request, headers);
        return request;
    }

    public static HttpRequestMessage CreatePostRequest(string url, HttpContent content, Dictionary<string, string> headers = null)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, url)
        {
            Content = content
        };
        AddHeaders(request, headers);
        return request;
    }

    private static void AddHeaders(HttpRequestMessage request, Dictionary<string, string> headers)
    {
        if (headers != null)
        {
            foreach (var header in headers)
            {
                request.Headers.Add(header.Key, header.Value);
            }
        }
    }
}
