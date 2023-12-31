using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public int teamSize;
    public GameObject playerObject;
    private GameObject currentPlayer;
    private int redPlayerTurnId;

    private int bluePlayerTurnId;
    public int minPlayerSpawnPositionX;
    public int maxPlayerSpawnPositionX;

    public int playerChangeTime = 2;
    private List<GameObject> team1Players = new List<GameObject>();
    private List<GameObject> team2Players = new List<GameObject>();
    private bool started = false;
    private bool team1 = true;
    public bool weaponAvailable = true;
    public bool changeWasCalled = false;

    private int team1Size;
    private int team2Size;

    public CameraManager cameraManager;
    void Start()
    {
        team1Size = teamSize;
        team2Size = teamSize;
        for (int i = 0; i < teamSize; i++)
        {
            team1Players.Add(setPlayerColor(spawn(), Color.red, 1));
            team2Players.Add(setPlayerColor(spawn(), Color.blue, 2));
        }

        currentPlayer = team1Players[0];
        redPlayerTurnId = 0;
        bluePlayerTurnId = 0;
        currentPlayer.GetComponentInChildren<WeaponManager>().setTurn(true);
        currentPlayer.GetComponent<PlayerInfo>().isCurrentPlayer = true;

        started = true;
    }

    public void changeTeamSize(int teamId)
    {
        if (teamId == 1)
        {
            team1Size -= 1;
        }
        else
        {
            team2Size -= 1;
        }
    }

    public int getTeam1Size()
    {
        return team1Size;
    }

    public int getTeam2Size()
    {
        return team2Size;
    }

    GameObject setPlayerColor(GameObject player, Color color, int teamId)
    {
        player.GetComponent<PlayerInfo>().teamId = teamId;
        Transform childTransform = player.transform.Find("Worm").Find("Body");
        GameObject bodyObject = childTransform.gameObject;
        bodyObject.GetComponent<Renderer>().material.SetColor("_Color", color);
        return player;
    }

    public bool playersExists()
    {
        return started;
    }

    GameObject spawn()
    {
        return Instantiate(playerObject, new Vector3(Random.Range(minPlayerSpawnPositionX, maxPlayerSpawnPositionX), 20, 0), playerObject.transform.rotation);
    }

    public Vector3 currentPlayerPosition()
    {
        return currentPlayer.transform.position;
    }
    public void changePlayer()
    {
        changeWasCalled = false;
        weaponAvailable = true;
        currentPlayer.GetComponentInChildren<WeaponManager>().setTurn(false);
        currentPlayer.GetComponent<PlayerInfo>().isCurrentPlayer = false;
        currentPlayer.GetComponent<PlayerMovement>().resetMoveCounter();

        setNextPlayer();
    }

    public void setNextPlayer()
    {
        if (!team1)
        {
            setNextPlayer(ref redPlayerTurnId, ref team1Players);
        }
        else
        {
            setNextPlayer(ref bluePlayerTurnId, ref team2Players);
        }

    }

    private void setNextPlayer(ref int playerId, ref List<GameObject> team)
    {
        playerId = ++playerId % team.Count;
        currentPlayer = team[playerId];

        int i = 0;
        while (!currentPlayer.activeSelf)
        {
            playerId = ++playerId % team.Count;
            currentPlayer = team[playerId];
            i++;
        }

        currentPlayer.GetComponent<PlayerInfo>().isCurrentPlayer = true;
        WeaponManager weaponManager = currentPlayer.GetComponentInChildren<WeaponManager>();
        weaponManager.setTurn(true);
    }

    public void move(PlayerMove movement)
    {
        if (started)
        {
            if (movement != PlayerMove.Jump)
            {
                var prevMove = currentPlayer.GetComponent<PlayerInfo>().prevPlayerMove;
                Debug.Log(prevMove);
                if (prevMove != movement && (movement == PlayerMove.Right || movement == PlayerMove.Left))
                {
                    rotateCurrentPlayer180();
                }
                currentPlayer.GetComponent<PlayerInfo>().prevPlayerMove = movement;
            }

            currentPlayer.GetComponent<PlayerMovement>().move(movement);

        }
    }

    public void activateWeapon()
    {
        if (weaponAvailable)
        {
            if (currentPlayer.activeSelf)
            {
                weaponAvailable = !currentPlayer.GetComponentInChildren<WeaponManager>().activate(postAttackCallback);
            }
            else
            {
                postAttackCallback();
            }
        }
    }

    public void postAttackCallback()
    {
        cameraManager.followPlayer = true;
        changePlayer();
        team1 = !team1;
        changeWasCalled = true;
    }

    public float currentPlayerMoveCounter()
    {

        return currentPlayer.GetComponent<PlayerMovement>().getMoveCounter();

    }

    public float getMoveLimit()
    {

        return currentPlayer.GetComponent<PlayerMovement>().getMoveLimit();

    }

    public void changeWeapon(int weaponId)
    {
        if (weaponAvailable)
        {
            if (currentPlayer.activeSelf)
            {
                WeaponManager weaponManager = currentPlayer.GetComponentInChildren<WeaponManager>();
                weaponManager.changeWeapon(weaponId);
            }
        }
    }

    public void rotateCurrentPlayer180()
    {
        Transform childTransform = currentPlayer.transform.Find("Worm");
        Vector3 currentRotation = childTransform.rotation.eulerAngles;

        float newYRotation = currentRotation.y + 180f;

        Quaternion newRotation = Quaternion.Euler(currentRotation.x, newYRotation, currentRotation.z);

        childTransform.rotation = newRotation;
    }
}
