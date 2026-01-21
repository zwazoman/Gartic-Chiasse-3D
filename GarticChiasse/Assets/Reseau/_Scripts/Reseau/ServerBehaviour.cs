using UnityEngine;
using Unity.Networking.Transport;
using Unity.Collections;
using System.Globalization;
using UnityEditor.Experimental.GraphView;


public class ServerBehaviour : MonoBehaviour
{
    public uint ChoosenNumber = 10;

    NetworkDriver m_Driver;
    NativeList<NetworkConnection> m_Connections;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_Driver = NetworkDriver.Create();
        m_Connections = new(16, Allocator.Persistent);

        var endPoint = NetworkEndpoint.AnyIpv4.WithPort(1000);
        if(m_Driver.Bind(endPoint) != 0)
        {
            Debug.LogError("NON");
            return;
        }
        m_Driver.Listen();
    }

    // Update is called once per frame
    void Update()
    {
        m_Driver.ScheduleUpdate().Complete();

        //clean up connections
        for (int i = 0; i < m_Connections.Length; i++)
        {
            if (!m_Connections[i].IsCreated)
            {
                m_Connections.RemoveAtSwapBack(i);
                i--;
            }
        }

        //Accept new connections
        NetworkConnection c;
        while ((c = m_Driver.Accept()) != default)
        {
            m_Connections.Add(c);
            Debug.Log("Accept a connection");
        }

        for(int i = 0; i < m_Connections.Length; i++)
        {
            DataStreamReader stream;
            NetworkEvent.Type cmd;
            while((cmd = m_Driver.PopEventForConnection(m_Connections[i], out stream)) != NetworkEvent.Type.Empty)
            {
                if(cmd == NetworkEvent.Type.Data)
                {
                    uint number = stream.ReadUInt();
                    Debug.Log($"Got {number} from a client, adding 2 to it.");

                    if(number == ChoosenNumber) { }
                        //réussite
                    else if(number > ChoosenNumber) { }
                        //trop grand
                    else { }
                        //trop petit
                    m_Driver.BeginSend(NetworkPipeline.Null, m_Connections[i], out var writer);
                    writer.WriteUInt(number);
                    m_Driver.EndSend(writer);
                }
                else if (cmd == NetworkEvent.Type.Disconnect)
                {
                    Debug.Log("Client disconnected from the server.");
                    m_Connections[i] = default;
                    break;
                }
            }
        }
    }

    void FonctionSmart()
    {

    }

    private void OnDestroy()
    {
        if (m_Driver.IsCreated)
        {
            m_Driver.Dispose();
            m_Connections.Dispose();
        }
    }
}
