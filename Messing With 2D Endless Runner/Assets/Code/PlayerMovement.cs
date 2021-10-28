using gm = GameManager;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 3.0f;
    public float jumpHeight = 5f;

    private bool inAir = false;
    private bool transitioning = false;
    private bool upsideDown = false;
    private Rigidbody2D rb;
    
    private int score = 0;
    private int lives = 3;
    private bool gettingHit = false;
    private bool movementDisabled = false;
    private int hitCooldown = 0;

    [SerializeField] private GameObject heart1, heart2, heart3;
    [SerializeField] private GameObject life1, life2, life3;

    // Start is called before the first frame update
    void Start() 
    {
        rb = GetComponent<Rigidbody2D>();

        life1.SetActive(false);
        life2.SetActive(false);
        life3.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (!movementDisabled)
        {
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                transform.Translate(Vector2.right * speed * Time.fixedDeltaTime);
            }
            else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                transform.Translate(Vector2.right * -speed * Time.fixedDeltaTime);
            }

            if (Input.GetKey(KeyCode.Space) && !inAir)
            {
                if (!upsideDown)
                {
                    rb.velocity = new Vector2(0, jumpHeight);
                    inAir = true;
                }
                else
                {
                    rb.velocity = new Vector2(0, -jumpHeight);
                    inAir = true;
                }
            }

            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            {
                if (upsideDown && (inAir || transitioning))
                {
                    rb.velocity = new Vector2(0, jumpHeight * 2);
                }

                if (!transitioning)
                {
                    rb.gravityScale = -1;
                    upsideDown = true;
                    transitioning = true;
                }
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            {
                if (!upsideDown && (inAir || transitioning))
                {
                    rb.velocity = new Vector2(0, -jumpHeight * 2);
                }

                if (!transitioning)
                {
                    rb.gravityScale = 1;
                    upsideDown = false;
                    transitioning = true;
                }
            }

            if (hitCooldown > 0)
            {
                hitCooldown--;
            }
            else if (hitCooldown <= 0)
            {
                gettingHit = false;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) 
    {
        if (collision.gameObject.tag == "Ground")
        {
            inAir = false;
            transitioning = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "KillBlock" && gettingHit == false)
        {
            gettingHit = true;
            hitCooldown = 50;

            Destroy(collision.gameObject);
            WasHit();
        }
    }

    public int GetScore()
    {
        return score;
    }

    public void AddScore(int addedScore)
    {
        score += addedScore;

        if (score >= PlayerPrefs.GetInt("highScore"))
        {
            PlayerPrefs.SetInt("highScore", score);
        }
    }

    public int GetLives()
    {
        return lives;
    }

    private void WasHit()
    {
        if (lives == 3)
        {
            heart3.SetActive(false);
            life3.SetActive(true);
            life3.GetComponent<Animator>().Play("HeartBreakAnimation", 0, 0);
        }
        else if (lives == 2)
        {
            heart2.SetActive(false);
            life2.SetActive(true);
            life2.GetComponent<Animator>().Play("HeartBreakAnimation", 0, 0);
        }
        else if (lives == 1)
        {
            heart1.SetActive(false);
            life1.SetActive(true);
            life1.GetComponent<Animator>().Play("HeartBreakAnimation", 0, 0);
        }

        lives-= 1;
    }

    public void Reset()
    {
        lives = 3;
        score = 0;

        rb.gravityScale = 1;
        rb.velocity = new Vector2(0, -jumpHeight);
        upsideDown = false;
        transitioning = false;
        inAir = true;
    }

    public void Disable()
    {
        movementDisabled = true;
        transform.position = Vector3.zero;
    }

    public void Enable()
    {
        movementDisabled = false;

        heart1.SetActive(true);
        heart2.SetActive(true);
        heart3.SetActive(true);
    }
}
