using System;
using System.Net;
using System.Threading.Tasks;
using UnityEngine;

namespace WebServer
{
    public class CustomWebServer
    {
        public Action<string> DataReceived;
        public bool IsServerRunning => _isServerRunning;
        
        private HttpListener _httpListener;
        private string _receivedData;
        private bool _isServerRunning;

        public void StartServer()
        {
            _isServerRunning = true;
            _httpListener = new HttpListener();
            _httpListener.Prefixes.Add("http://localhost:8080/callback/");
            _httpListener.Start();
            Debug.LogError($"IsListening: {_httpListener.IsListening}");

            Task.Run(async () =>
            {
                while (true)
                {
                    var context = await _httpListener.GetContextAsync();
                    var request = context.Request;
                    var response = context.Response;

                    if (request.HttpMethod == "POST")
                    {
                        var data = await new System.IO.StreamReader(request.InputStream).ReadToEndAsync();
                        Debug.LogError($"Response on CustomWebServer is {data}");
                        _receivedData = data;
                        OnDataReceived(_receivedData);
                    }

                    response.Close();
                }
            });
        }

        public void StopServer()
        {
            if (_httpListener != null)
            {
                _isServerRunning = false;
                _httpListener.Stop();
                _httpListener.Close();
            }
        }

        private void OnDataReceived(string data)
        {
            DataReceived?.Invoke(data);
        }
    }
}

