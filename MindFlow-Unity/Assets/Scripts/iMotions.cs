using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System.Net;
using System.Net.Sockets;

public class iMotions : MonoBehaviour
{
    Socket sock;
    SocketAsyncEventArgs eventArgs;
    string data;
    float value;
    byte[] buffer;
    public Transform scaleObj;

    List<float> values = new List<float>();
    float maxValue = 0f;

    void Start()
    {
        sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPAddress serverAddr = IPAddress.Parse("127.0.0.1");
        IPEndPoint endPoint = new IPEndPoint(serverAddr, 8088);
        buffer = new byte[1024];
        sock.Connect(endPoint);
        eventArgs = new SocketAsyncEventArgs();
        eventArgs.SetBuffer(buffer, 0, buffer.Length);
        eventArgs.Completed += OnReceiveComplete;
        sock.ReceiveAsync(eventArgs);
    }

    void OnReceiveComplete(object sender, SocketAsyncEventArgs e)
    {
        data = System.Text.Encoding.ASCII.GetString(e.Buffer);
        for (int i = 0; i < buffer.Length;i++)
            buffer[i] = 0;
        sock.ReceiveAsync(eventArgs);
    }

    void OnApplicationQuit()
    {
        sock.Close();
    }

    void Update()
    {
        if (!string.IsNullOrEmpty(data) && data.Contains("Biopac"))
        {
            float.TryParse(data.Substring(data.LastIndexOf(';') + 1), out value);
            Debug.Log(value);
            values.Add(Mathf.Abs(value));
            if (values.Count > 90 * 30)
                values.RemoveAt(0);
            maxValue = 0f;
            for (int i=0; i<values.Count; i++)
            {
                if (values[i] > maxValue)
                    maxValue = values[i];
            }
            scaleObj.transform.localScale = (1f - 1f * Mathf.Clamp(value / maxValue, -1f, 1f)) * Vector3.one;
            scaleObj.transform.localPosition = new Vector3(0f, -0.1f, 0.3f - 0.25f * Mathf.Clamp(value / maxValue, -1f, 1f));
        }
        data = "";
    }
}
