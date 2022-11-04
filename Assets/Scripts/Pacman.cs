using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Movement))]
public class Pacman : MonoBehaviour
{
    public AnimatedSprite deathSequence;
    public SpriteRenderer spriteRenderer { get; private set; }
    public new Collider2D collider { get; private set; }
    public Movement movement { get; private set; }
    public Backpack backpack { get; private set; }

    public Text scoreText;
    public bool isP1;
    public Vector3 startingPosition { get; set; }
    public int score { get; set; }
    public float invulnerabilityTime = 1.5f;

    public Transform launchOffset;
    public GameObject projectilePrefab;

    [HideInInspector]
    public bool invulnerable = false;
    [HideInInspector]
    public bool vulnerable = false;
    [HideInInspector]
    public int points = 100;
    
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<Collider2D>();
        movement = GetComponent<Movement>();
        backpack = GetComponent<Backpack>();
    }

    private void Update()
    {
        if (isP1)
        {
            // Set the new direction based on the current input
            if (Input.GetKeyDown(KeyCode.W))
            {
                movement.SetDirection(Vector2.up);
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                movement.SetDirection(Vector2.down);
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                movement.SetDirection(Vector2.left);
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                movement.SetDirection(Vector2.right);
            }
            else if(Input.GetKeyDown(KeyCode.Q))
            {
                if (backpack.CanUsePowerup())
                {
                    backpack.UsePowerup(this);
                }
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                movement.SetDirection(Vector2.up);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                movement.SetDirection(Vector2.down);
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                movement.SetDirection(Vector2.left);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                movement.SetDirection(Vector2.right);
            }
            else if (Input.GetKeyDown(KeyCode.Minus))
            {
                if (backpack.CanUsePowerup())
                {
                    backpack.UsePowerup(this);
                }
            }
        }

        // Rotate pacman to face the movement direction
        float angle = Mathf.Atan2(movement.direction.y, movement.direction.x);
        transform.rotation = Quaternion.AngleAxis(angle * Mathf.Rad2Deg, Vector3.forward);
    }

    public void ResetState()
    {
        if(!vulnerable)
        {
            ResetVulnerable();
        }
        EnableInvulnerable();
        enabled = true;
        spriteRenderer.enabled = true;
        collider.enabled = true;
        deathSequence.enabled = false;
        deathSequence.spriteRenderer.enabled = false;
        movement.startingPosition = startingPosition;
        movement.ResetState();
        gameObject.SetActive(true);
    }

    public void DeathSequence()
    {
        enabled = false;
        spriteRenderer.enabled = false;
        collider.enabled = false;
        movement.enabled = false;
        deathSequence.enabled = true;
        deathSequence.spriteRenderer.enabled = true;
        deathSequence.Restart();
    }

    public void SetScore(int score)
    {
        this.score = score;
        //scoreText.text = score.ToString().PadLeft(2, '0');
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Pacman"))
        {
            Pacman otherPlayer = collision.gameObject.GetComponent<Pacman>();
            if (otherPlayer.vulnerable)
            {
                FindObjectOfType<GameManager>().PacmanEaten(true, otherPlayer.isP1, false);
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Pacman"))
        {
            Pacman otherPlayer = collision.gameObject.GetComponent<Pacman>();
            if (otherPlayer.vulnerable)
            {
                FindObjectOfType<GameManager>().PacmanEaten(true, otherPlayer.isP1, false);
            }
        }
    }

    private void ResetVulnerable()
    {
        vulnerable = false;
    }

    public void EnableVulnerable(float duration)
    {
        vulnerable = true;

        Invoke("ResetVulnerable", duration);
    }

    private void ResetInvulnerable()
    {
        invulnerable = false;
    }

    private void EnableInvulnerable()
    {
        invulnerable = true;

        Invoke("ResetInvulnerable", invulnerabilityTime);
    }

    private void ResetMovementSpeed()
    {
        movement.ResetSpeedMultiplyer();
    }

    public void ChangeMovementSpeed(float speedMultiplier, float duration)
    {
        movement.speedMultiplier = speedMultiplier;

        Invoke("ResetMovementSpeed", duration);
    }

    public void ShootProjectile(float power)
    {
        GameObject fireball = Instantiate(projectilePrefab, launchOffset.position, transform.rotation);

        // Set the projectile's owner
        fireball.GetComponent<ProjectileBehaviour>().owner = this;

        // Apply a force to the projectile (powerup power works as the force)
        fireball.GetComponent<Rigidbody2D>().AddForce(launchOffset.right * power, ForceMode2D.Impulse);
    }
}
