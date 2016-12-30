using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System.Net;
using System.Net.Sockets;

public class iMotions : MonoBehaviour
{
    enum Mode { Off, Auto, Manual }
    enum Phase { BreathIn, HoldIn, BreathOut, HoldOut }
    Socket sock;
    SocketAsyncEventArgs eventArgs;
    string data;
    float value;
    byte[] buffer;
    public Transform scaleObj;

    List<float> values = new List<float>();
    float maxValue = 0f;

    Mode mode = Mode.Manual;

    public float BreathInTime = 3f;
    public float HoldInTime = 3f;
    public float BreathOutTime = 3f;
    public float HoldOutTime = 3f;
    Phase phase;
    float time;

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
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            mode = Mode.Off;
            scaleObj.gameObject.SetActive(false);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            mode = Mode.Auto;
            scaleObj.gameObject.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            mode = Mode.Manual;
            scaleObj.gameObject.SetActive(true);
        }

        if (mode == Mode.Manual && !string.IsNullOrEmpty(data) && data.Contains("Biopac"))
        {
            float.TryParse(data.Substring(data.LastIndexOf(';') + 1), out value);
            Debug.Log(value);
            values.Add(Mathf.Abs(value));
            if (values.Count > 90 * 30)
                values.RemoveAt(0);
            maxValue = 0f;
            for (int i = 0; i < values.Count; i++)
            {
                if (values[i] > maxValue)
                    maxValue = values[i];
            }
            scaleObj.transform.localScale = (1f - 1f * Mathf.Clamp(value / maxValue, -1f, 1f)) * Vector3.one;
            scaleObj.transform.localPosition = new Vector3(0f, -0.1f, 0.3f - 0.25f * Mathf.Clamp(value / maxValue, -1f, 1f));
        }
        else if (mode == Mode.Manual || mode == Mode.Auto)
        {
            time += Time.deltaTime;
            float newValue = 0f;
            if (phase == Phase.BreathIn)
            {
                newValue = Mathf.Lerp(1f, -1f, time / BreathInTime);
                if (time >= BreathInTime)
                {
                    phase = Phase.HoldIn;
                    time = 0;
                }
            }
            else if (phase == Phase.HoldIn)
            {
                newValue = -1f;
                if (time >= HoldInTime)
                { 
                    phase = Phase.BreathOut;
                    time = 0;
                }
            }
            else if (phase == Phase.BreathOut)
            {
                newValue = Mathf.Lerp(-1f, 1f, time / BreathOutTime);
                if (time >= BreathOutTime)
                { 
                    phase = Phase.HoldOut;
                    time = 0;
                }
            }
            else if (phase == Phase.HoldOut)
            {
                newValue = 1f;
                if (time >= HoldOutTime)
                { 
                    phase = Phase.BreathIn;
                    time = 0;
                }
            }
            scaleObj.transform.localScale = (1f - 1f * newValue) * Vector3.one;
            scaleObj.transform.localPosition = new Vector3(0f, -0.1f, 0.3f - 0.25f * newValue);
        }

        data = "";
    }
}
