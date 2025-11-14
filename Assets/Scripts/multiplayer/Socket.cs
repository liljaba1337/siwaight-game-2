using UnityEngine;
using System.Threading.Tasks;
using System.Net.WebSockets;
using System;
using System.Text;
using System.IO;
using System.Threading;

public class Socket : MonoBehaviour
{
    public Transform player2;
    private const string ip = "109.120.132.102";
    private ClientWebSocket client = new ClientWebSocket();

    // Start is called before the first frame update
    async void Start()
    {
        try
        {
            await client.ConnectAsync(new Uri($"ws://{ip}:7777"), CancellationToken.None);
        } catch(Exception e)
        {
            File.AppendAllText("log.txt", $"{e}\n");
        }
        _ = ReceiveLoop();
    }
    private async Task SendCoords()
    {
        try
        {
            Vector3 pos = transform.position;
            var bytes = Encoding.UTF8.GetBytes($"{pos.x},{pos.y},{pos.z},{transform.forward.x},{transform.forward.y},{transform.forward.z}");
            await client.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, CancellationToken.None);
        } catch(Exception e)
        {
            File.AppendAllText("log.txt", $"{e}\n");
        }
    }

    private float sendInterval = 0.05f;
    private float timer = 0f;

    private async Task ReceiveLoop()
    {
        var buffer = new byte[1024];

        try
        {
            while (client.State == WebSocketState.Open)
            {
                WebSocketReceiveResult result = await client.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await client.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
                    Debug.Log("Server closed connection");
                }
                else
                {
                    string message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    Debug.Log(message);
                    File.AppendAllText("log.txt", $"{message}\n");
                    string[] coords = message.Split(',');
                    float x = float.Parse(coords[0]);
                    float y = float.Parse(coords[1]);
                    float z = float.Parse(coords[2]);
                    float vx = float.Parse(coords[3]);
                    float vy = float.Parse(coords[4]);
                    float vz = float.Parse(coords[5]);
                    player2.position = new Vector3(x, y, z);
                    player2.rotation = Quaternion.LookRotation(new Vector3(vx, vy, vz));
                }
            }
        }
        catch (Exception e)
        {
            File.AppendAllText("log.txt", $"{e}\n\n\n");
            Debug.LogWarning($"Receive loop stopped: {e.Message}");
        }
    }

    // Update is called once per frame
    async void Update()
    {
        timer += Time.deltaTime;
        if (timer >= sendInterval)
        {
            timer = 0f;
            await SendCoords();
        }
    }
}
