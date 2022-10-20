using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    const float MAX_TIME = 120f;
    const int MAX_SCORE = 3;

    public Ghost[] ghosts;
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
    public int ghostMultiplier { get; private set; } = 1;

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
        player2.ResetState();
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
    }

    private void UpdateTimer()
    {
        if(timer >= MAX_TIME)
        {
            if(roundOngoing)
            {
                RoundOver();

                Invoke("NewRound", 5f);
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
            if(eatenByPlayer)
            {
                player2.SetScore(player2.score + player1.points);
            }
            player1.DeathSequence();
            Invoke("RespawnPlayer1", respawnTime);
        } else
        {
            if (eatenByPlayer)
            {
                player1.SetScore(player1.score + player2.points);
            }
            player2.DeathSequence();
            Invoke("RespawnPlayer2", respawnTime);
        }
    }

    public void GhostEaten(Ghost ghost, Pacman player)
    {
        int points = ghost.points * ghostMultiplier;
        player.SetScore(player.score + points);

        ghostMultiplier++;
    }

    public void PelletEaten(Pellet pellet, Pacman player)
    {
        pellet.gameObject.GetComponent<SpriteRenderer>().enabled = false;

        player.SetScore(player.score + pellet.points);
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
}
