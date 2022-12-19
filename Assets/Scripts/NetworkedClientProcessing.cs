using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class NetworkedClientProcessing
{

    #region Send and Receive Data Functions
    static public void ReceivedMessageFromServer(string msg)
    {
        Debug.Log("msg received = " + msg + ".");

        string[] csv = msg.Split(',');
        switch (csv[0])
        {
            case "New Player":
                gameLogic.AddNewPlayer(int.Parse(csv[1]), csv[2]);
                break;
            case "Movement":
                gameLogic.MovePlayers(csv[1]);
                break;
            case "GameState":
                gameLogic.GameState(csv[1]);
                break;
            default:
                break;
        }
    }

    static public void SendMessageToServer(string msg)
    {
        networkedClient.SendMessageToServer(msg);
    }

    #endregion

    #region Connection Related Functions and Events
    static public void ConnectionEvent()
    {
        Debug.Log("Network Connection Event!");
        networkedClient.SendMessageToServer("New Player," + gameLogic.characterPositionInPercent.x + ";" + gameLogic.characterPositionInPercent.y);
    }

    static public void SendMovement()
    {
        SendMessageToServer("Movement," + gameLogic.characterPositionInPercent.x + ";" + gameLogic.characterPositionInPercent.y);
    }

    static public void SendInput(string input)
    {
        SendMessageToServer("Input," + input);
    }
    static public void DisconnectionEvent()
    {
        Debug.Log("Network Disconnection Event!");
    }
    static public bool IsConnectedToServer()
    {
        return networkedClient.IsConnected();
    }
    static public void ConnectToServer()
    {
        networkedClient.Connect();
    }
    static public void DisconnectFromServer()
    {
        networkedClient.Disconnect();
    }

    #endregion

    #region Setup
    static NetworkedClient networkedClient;
    static GameLogic gameLogic;

    static public void SetNetworkedClient(NetworkedClient NetworkedClient)
    {
        networkedClient = NetworkedClient;
    }
    static public NetworkedClient GetNetworkedClient()
    {
        return networkedClient;
    }
    static public void SetGameLogic(GameLogic GameLogic)
    {
        gameLogic = GameLogic;
    }

    #endregion

}

#region Protocol Signifiers
static public class ClientToServerSignifiers
{
    public const int asd = 1;
}

static public class ServerToClientSignifiers
{
    public const int asd = 1;
}

#endregion

