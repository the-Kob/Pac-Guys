using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    const float MAX_TIME = 120f;
    const int MAX_SCORE = 3;

    public Ghost[] ghosts;
    [Range(5, 30)] // Between 5 and 30 seconds
    public int ghostChangeTargetInterval;
    public int ghostMultiplier { get; private set; } = 1;

    public Pacman player1;
    public Pacman player2;
    public List<Vector3> startPositionsP1;
    public List<Vector3> startPositionsP2;
    public float respawnTime;

    public Transform pellets;

    public Text roundOverText;
    public Text gameOverText;
    public Text scoreText;
    public Text timerText;

    float timer;
    bool roundOngoing;

    public enum RoundState
    {
        p1,
        p2,
        none
    }

    #region Global Score information
    public GlobalScore score;

    public class GlobalScore {
        public int p1Score { get; set; }
        public int p2Score { get; set; }

        public GlobalScore()
        {
            ResetScore();
        }

        public void UpdateScore(RoundState state)
        {
            if(state == RoundState.p1)
            {
                p1Score++;
            } else if(state == RoundState.p2)
            {
                p2Score++;
            } else if(state == RoundState.none)
            {
                p1Score++;
                p2Score++;
            }
        }

        public void ResetScore()
        {
            p1Score = 0;
            p2Score = 0;
        }

        public String WinMessage()
        {
            String msg;
            if(p1Score > p2Score)
            {
                msg = "PLAYER 1 WINS!";
            } else if(p2Score > p1Score)
            {
                msg = "PLAYER 2 WINS!";
            } else
            {
                msg = "TIE!";
            }

            return msg;
        }
    }

    #endregion

    private void Start()
    {
        score = new GlobalScore();
        NewGame();
    }

    private void Update()
    {
        UpdateTimer();

        if(timer % ghostChangeTargetInterval == 0)
        {
            UpdateGhostsTarget();
        }

        // Check if max global score has been reached by a player
        if ((score.p1Score >= MAX_SCORE || score.p2Score >= MAX_SCORE) && Input.anyKeyDown) {
            // In the future can take the players to a menu screen  
            NewGame();
        }
    }

    private void NewGame()
    {
        gameOverText.enabled = false;
        NewRound();
    }

    private void NewRound()
    {
        roundOngoing = true;
        // Reset timer
        timer = 0f;

        roundOverText.enabled = false;

        RefillBoard();

        ResetState();
    }

    private void ResetState()
    {
        for (int i = 0; i < ghosts.Length; i++) {
            ghosts[i].ResetState();
        }

        int posIndex = Random.Range(0, startPositionsP1.Count);

        player1.startingPosition = startPositionsP1[posIndex];
        player1.SetScore(0);
        RespawnPlayer1();

        player2.startingPosition = startPositionsP2[posIndex];
        player2.SetScore(0);
        RespawnPlayer2();
    }

    private void RespawnPlayer1()
    {
        player1.ResetState();
    }

    private void RespawnPlayer2()
    {
        player2.ResetState();
    }

    private void RoundOver()
    {
        roundOngoing = false;
        roundOverText.enabled = true;

        for (int i = 0; i < ghosts.Length; i++) {
            ghosts[i].gameObject.SetActive(false);
        }

        player1.gameObject.SetActive(false);
        player2.gameObject.SetActive(false);
        
        RoundState state;
        if(player1.score > player2.score)
        {
            state = RoundState.p1;
        } else if(player1.score < player2.score)
        {
            state = RoundState.p2;
        } else
        {
            state = RoundState.none;
        }
        
        score.UpdateScore(state);

        UpdateGlobalScore();
    }

    private void GameOver()
    {
        gameOverText.enabled = true;
        gameOverText.text = score.WinMessage();

        for (int i = 0; i < ghosts.Length; i++)
        {
            ghosts[i].gameObject.SetActive(false);
        }

        player1.gameObject.SetActive(false);
        player2.gameObject.SetActive(false);

        // After 5 seconds, go back to the main menu
        Invoke("GoBackToMainMenu", 5f);
    }

    private void UpdateTimer()
    {
        if(timer >= MAX_TIME)
        {
            if(roundOngoing)
            {
                RoundOver();
                if(score.p1Score < 3 && score.p2Score < 3)
                {
                    Invoke("NewRound", 5f);
                } else
                {
                    GameOver();
                }
            }
        } else
        {
            timer += Time.deltaTime;
        }

        timerText.text = ((int)timer).ToString();
    }

    private void UpdateGlobalScore()
    {
        scoreText.text = score.p1Score.ToString() + " / " + score.p2Score.ToString();
    }

    public void PacmanEaten(bool eatenByPlayer, bool isP1)
    {
        if (isP1)
        {
            if(!player1.invulnerable)
            {
                if (eatenByPlayer)
                {
                    int result = player2.score + player1.points;
                    player2.SetScore(result);
                }
                else
                {
                    // Ghosts subtract their points if they kill you
                    int result = player1.score - 200;
                    if (result < 0)
                    {
                        result = 0;
                    }
                    player1.SetScore(result);
                }

                player1.DeathSequence();
                Invoke("RespawnPlayer1", respawnTime);
            }

            ChangeTargetOnPacmanDeath(player1);
        } else
        {
            if(!player2.invulnerable)
            {
                if (eatenByPlayer)
                {
                    int result = player1.score + player2.points;
                    player1.SetScore(result);
                }
                else
                {
                    // Ghosts subtract their points if they kill you
                    int result = player2.score - 200;
                    if (result < 0)
                    {
                        result = 0;
                    }
                    player2.SetScore(result);
                }

                player2.DeathSequence();
                Invoke("RespawnPlayer2", respawnTime);
            }

            ChangeTargetOnPacmanDeath(player2);
        }
    }

    public void GhostEaten(Ghost ghost, Pacman player)
    {
        int points = ghost.points * ghostMultiplier;

        int result = player.score + points;
        player.SetScore(result);

        ghostMultiplier++;
    }

    public void PelletEaten(Pellet pellet, Pacman player)
    {
        pellet.gameObject.GetComponent<SpriteRenderer>().enabled = false;

        int result = player.score + pellet.points;
        player.SetScore(result);
    }
 
    public void PelletRefill(Pellet pellet)
    {
        pellet.gameObject.GetComponent<SpriteRenderer>().enabled = true;
    }

    public void PowerPelletEaten(PowerPellet pellet, Pacman player)
    {
        if (player1 == player)
        {
            player1.backpack.PickupPowerup(pellet);
        }
        else if (player2 == player)
        {
            player2.backpack.PickupPowerup(pellet);
        }

        PelletEaten(pellet, player);
    }

    private void ResetGhostMultiplier()
    {
        ghostMultiplier = 1;
    }

    public void ActivateStarPowerup(PowerPellet powerup, Pacman player)
    {
        for (int i = 0; i < ghosts.Length; i++)
        {
            ghosts[i].frightened.Enable(powerup.duration);
        }

        if (player1 == player)
        {
            player2.EnableVulnerable(powerup.duration);
        }
        else if (player2 == player)
        {
            player1.EnableVulnerable(powerup.duration);
        }

        CancelInvoke(nameof(ResetGhostMultiplier));
        Invoke(nameof(ResetGhostMultiplier), powerup.duration);
    }

    public void RefillBoard()
    {
        foreach (Transform pellet in pellets)
        {
            pellet.gameObject.GetComponent<SpriteRenderer>().enabled = true;
        }
    }

    public void ActivateFreezePowerup(PowerPellet powerup, Pacman player)
    {
        for (int i = 0; i < ghosts.Length; i++)
        {
            ghosts[i].SlowMovementSpeed(powerup.power, powerup.duration);
        }

        if (player1 == player)
        {
            player2.ChangeMovementSpeed(powerup.power, powerup.duration);
        }
        else if (player2 == player)
        {
            player1.ChangeMovementSpeed(powerup.power, powerup.duration);
        }
    }

    public void ActivateSpeedPowerup(PowerPellet powerup, Pacman player)
    {
        if (player1 == player)
        {
            player1.ChangeMovementSpeed(powerup.power, powerup.duration);
        }
        else if (player2 == player)
        {
            player2.ChangeMovementSpeed(powerup.power, powerup.duration);
        }
    }

    private void UpdateGhostsTarget()
    {
        for (int i = 0; i < ghosts.Length; i++)
        {
            ghosts[i].RandomlyChangeTarget();
        }
    }

    private void GoBackToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    private void ChangeTargetOnPacmanDeath(Pacman deadPlayer)
    {
        for (int i = 0; i < ghosts.Length; i++)
        {
            ghosts[i].ChangeTarget(deadPlayer);
        }
    }
}
