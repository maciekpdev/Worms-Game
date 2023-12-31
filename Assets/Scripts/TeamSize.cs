using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;

using UnityEngine;

public class TeamSize : MonoBehaviour
{
    public PlayerManager playerManager;
    public GameStarter gameStarter;

    public TMP_Text textScoreTeam1;
    public TMP_Text textScoreTeam2;

    void Update() {
        if(playerManager.getTeam1Size() <= 0 || playerManager.getTeam2Size() <= 0) {
            gameStarter.LoadScene(0);
        }

        textScoreTeam1.text = playerManager.getTeam1Size().ToString();
        textScoreTeam2.text = playerManager.getTeam2Size().ToString();


    }
}
