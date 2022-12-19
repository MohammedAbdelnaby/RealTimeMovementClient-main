using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Player
{
    public Player(int id, GameObject ob)
    {
        ID = id;
        PlayerGameObject = ob;
        playerPositionInPercent = Vector2.zero;
        playerVelocityPercent = Vector2.zero;
    }
    public int ID;
    public Vector2 playerPositionInPercent;
    public Vector2 playerVelocityPercent;
    public GameObject PlayerGameObject;
}

public class GameLogic : MonoBehaviour
{
    public GameObject character;

    public Vector2 characterPositionInPercent;
    public Vector2 characterVelocityInPercent;
    const float CharacterSpeed = 0.25f;
    float DiagonalCharacterSpeed;
    public List<Player> Players;

    void Start()
    {
        Players = new List<Player>();
        DiagonalCharacterSpeed = Mathf.Sqrt(CharacterSpeed * CharacterSpeed + CharacterSpeed * CharacterSpeed) /2f;
        NetworkedClientProcessing.SetGameLogic(this);

        Sprite circleTexture = Resources.Load<Sprite>("Circle");

        character = new GameObject("Character");

        character.AddComponent<SpriteRenderer>();
        character.GetComponent<SpriteRenderer>().sprite = circleTexture;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D)
            || Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.D))
        {
            characterVelocityInPercent = Vector2.zero;

            if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D))
            {
                characterVelocityInPercent.x = DiagonalCharacterSpeed;
                characterVelocityInPercent.y = DiagonalCharacterSpeed;
            }
            else if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A))
            {
                characterVelocityInPercent.x = -DiagonalCharacterSpeed;
                characterVelocityInPercent.y = DiagonalCharacterSpeed;
            }
            else if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D))
            {
                characterVelocityInPercent.x = DiagonalCharacterSpeed;
                characterVelocityInPercent.y = -DiagonalCharacterSpeed;
            }
            else if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A))
            {
                characterVelocityInPercent.x = -DiagonalCharacterSpeed;
                characterVelocityInPercent.y = -DiagonalCharacterSpeed;
            }
            else if (Input.GetKey(KeyCode.D))
                characterVelocityInPercent.x = CharacterSpeed;
            else if (Input.GetKey(KeyCode.A))
                characterVelocityInPercent.x = -CharacterSpeed;
            else if (Input.GetKey(KeyCode.W))
                characterVelocityInPercent.y = CharacterSpeed;
            else if (Input.GetKey(KeyCode.S))
                characterVelocityInPercent.y = -CharacterSpeed;

            NetworkedClientProcessing.SendMovement();
        }

        characterPositionInPercent += (characterVelocityInPercent * Time.deltaTime);

        Vector2 screenPos = new Vector2(characterPositionInPercent.x * (float)Screen.width, characterPositionInPercent.y * (float)Screen.height);
        Vector3 characterPos = Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, 0));
        characterPos.z = 0;
        character.transform.position = characterPos;

    }

    public void MovePlayers(string list)
    {
        string[] PlayerInfo = list.Split(':');
        foreach (string sPlayer in PlayerInfo)
        {
            string[] info = sPlayer.Split(';');
            if (info.Length >= 3)
            {
                Player temp = Players.Find((Player player) => player.ID == int.Parse(info[0]));
                Players.Remove(temp);
                temp.playerPositionInPercent = new Vector2(float.Parse(info[1]), float.Parse(info[2]));
                Players.Add(temp);
            }
        }
    }

    public void AddNewPlayer(int ID, string pos)
    {
        Sprite circleTexture = Resources.Load<Sprite>("Circle");

        GameObject player = new GameObject("Character");

        player.AddComponent<SpriteRenderer>();
        player.GetComponent<SpriteRenderer>().sprite = circleTexture;
        Vector2 tempPos = StringToVector(pos);
        Vector2 screenPos = new Vector2(tempPos.x * (float)Screen.width, tempPos.y * (float)Screen.height);
        Vector3 characterPos = Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, 0));
        characterPos.z = 0;
        player.transform.position = characterPos;
        Player temp = new Player(ID, player);
        Players.Add(temp);
    }

    public void AddNewPlayerv(int ID, Vector2 tempPos)
    {
        Sprite circleTexture = Resources.Load<Sprite>("Circle");

        GameObject player = new GameObject("Character");

        player.AddComponent<SpriteRenderer>();
        player.GetComponent<SpriteRenderer>().sprite = circleTexture;
        Vector2 screenPos = new Vector2(tempPos.x * (float)Screen.width, tempPos.y * (float)Screen.height);
        Vector3 characterPos = Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, 0));
        characterPos.z = 0;
        player.transform.position = characterPos;
        Player temp = new Player(ID, player);
        Players.Add(temp);
    }

    private bool StringEqualsDirection(string String, string direction)
    {
        return (String == direction) ? true : false;
    }

    public void GameState(string msg)
    {
        string[] player = msg.Split(':');
        foreach (string Playerinfo in player)
        {
            string[] info = Playerinfo.Split(';');
            Debug.Log(Playerinfo);
            if (info.Length >= 3)
            {
                AddNewPlayerv(int.Parse(info[0]), new Vector3(float.Parse(info[1]), float.Parse(info[2])));
            }
        }
    }

    public Vector2 StringToVector(string msg)
    {
        string[] coord = msg.Split(';');
        Vector2 temp;
        temp = new Vector2(float.Parse(coord[0]), float.Parse(coord[1]));
        return temp;
    }

}

