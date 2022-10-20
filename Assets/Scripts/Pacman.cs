using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Movement))]
public class Pacman : MonoBehaviour
{
    public AnimatedSprite deathSequence;
    public SpriteRenderer spriteRenderer { get; private set; }
    public new Collider2D collider { get; private set; }
    public Movement movement { get; private set; }
    public Text scoreText;
    public bool isP1;
    public bool vulnerable = false;

    [HideInInspector]
    public int points = 500;
    public Vector3 startingPosition { get; set; }
    public int score { get; set; }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<Collider2D>();
        movement = GetComponent<Movement>();
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
        scoreText.text = score.ToString().PadLeft(2, '0');
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Pacman"))
        {
            Pacman otherPlayer = collision.gameObject.GetComponent<Pacman>();
            if (otherPlayer.vulnerable)
            {
                FindObjectOfType<GameManager>().PacmanEaten(true, otherPlayer.isP1);
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
}
