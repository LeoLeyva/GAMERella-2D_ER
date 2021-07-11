using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
  // State 
  private enum State { idle, running, jumping, falling, justWon, justLost };
  private State st = State.running;

  // Constants 
  [SerializeField] private float mvSpeed;
  [SerializeField] private float jpForce;
  [SerializeField] private float jpPeak = 0.1f;

  private bool isGrounded;
  [SerializeField] private LayerMask ground;

  [SerializeField] private Transform groundCheck;
  [SerializeField] private float groundCheckRad;


  // Components 
  // private Collider2D col;
  private Rigidbody2D rb;
  private Animator anim;
  private BoxCollider2D bc;

  // Jumping 
  [SerializeField] private float jpTime;
  private float jpTimeCount;

  // Collectibles 
  private int junkCollected = 0;
  // private TextMeshProUGUI junkText;
  private int goalAmount = 10;

  public int junkMissed = 0;
  // private TextMeshProUGUI missedText;
  private int missAmount = 10;
  private bool slowDown = false;

  // Scenes 
  [SerializeField] private string winScene;
  [SerializeField] private string loseScene;





  // Start is called before the first frame update
  private void Start()
  {
    rb = GetComponent<Rigidbody2D>();
    // col = GetComponent<Collider2D>();
    anim = GetComponent<Animator>();
    bc = GetComponent<BoxCollider2D>();
    jpTimeCount = jpTime;
  }

  // Update is called once per frame
  private void Update()
  {
    EndingCheck();

    isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRad, ground);

    Movement();
    Jump();

    ActionTransition();
    anim.SetInteger("state", (int)st);
  }

  private void Movement()
  {
    if (slowDown)
    {
      rb.velocity = Vector2.zero;
      // rb.velocity -= new Vector2((rb.velocity.x / 5f) * Time.deltaTime, 0);
      // rb.AddForce(mvSpeed - (mvSpeed / 10), ForceMode.VelocityChange);
    }
    else
    {
      rb.velocity = new Vector2(mvSpeed, rb.velocity.y);
    }
  }

  private void Jump()
  {
    if (!isGrounded && st == State.falling)
    {
      jpTimeCount = 0;
    }
    if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
    {
      rb.velocity = new Vector2(mvSpeed, jpForce);
      st = State.jumping;
    }
    if (Input.GetKey(KeyCode.Space) && jpTimeCount > 0)
    {
      rb.velocity = new Vector2(rb.velocity.x, jpForce);
      jpTimeCount -= Time.deltaTime;
      st = State.jumping;
    }
    if (Input.GetKeyUp(KeyCode.Space))
    {
      jpTimeCount = 0;
    }
    if (isGrounded)
    {
      jpTimeCount = jpTime;
    }
  }

  private void EndingCheck()
  {
    if (junkCollected == goalAmount || junkMissed == missAmount)
    {
      slowDown = true;
      if (rb.velocity.x <= 0.1f)
      {
        rb.velocity = new Vector2(0f, rb.velocity.y);
        if (junkCollected == goalAmount)
        {
          st = State.justWon;
          StartCoroutine(WaitForWinScene());
        }
        else if (junkMissed == missAmount)
        {
          st = State.justLost;
          float ySize = 0.7969806f - 0.185f;
          bc.size = new Vector2(bc.size.x, ySize);
          StartCoroutine(WaitForLoseScene());
        }
      }
    }
  }

  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (collision.tag == "Collectible")
    {
      collision.gameObject.SetActive(false);
      junkCollected += 1;
      // junkText.text = junkCollected.ToString();
    }
  }

  private void ActionTransition()
  {
    if (st == State.jumping)
    {
      if (rb.velocity.y < jpPeak)
      {
        st = State.falling;
      }
    }
    else if (!isGrounded && st != State.jumping)
    {
      st = State.falling;
    }
    else if (st == State.falling)
    {
      if (isGrounded)
      {
        st = State.running;
      }
    }
    else if (st == State.justWon)
    {

    }
    else if (st == State.justLost)
    {

    }
    else
    {
      st = State.running;
    }
  }


  private IEnumerator WaitForWinScene()
  {
    yield return new WaitForSeconds(3f);
    SceneManager.LoadScene(winScene);
  }

  private IEnumerator WaitForLoseScene()
  {
    yield return new WaitForSeconds(3f);
    SceneManager.LoadScene(loseScene);
  }

}

