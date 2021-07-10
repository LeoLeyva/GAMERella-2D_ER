using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
  // State 
  private enum State { idle, running, jumping, falling };
  private State st = State.running;

  // Constants 
  [SerializeField] private float mvSpeed;
  [SerializeField] private float jpForce;
  [SerializeField] private float jpPeak = 0.1f;

  private bool isGrounded;
  [SerializeField] private LayerMask ground;

  // Components 
  private Collider2D col;
  private Rigidbody2D rb;
  private Animator anim;



  // Start is called before the first frame update
  private void Start()
  {
    rb = GetComponent<Rigidbody2D>();
    col = GetComponent<Collider2D>();
    anim = GetComponent<Animator>();
  }

  // Update is called once per frame
  private void Update()
  {
    rb.velocity = new Vector2(mvSpeed, rb.velocity.y);
    isGrounded = Physics2D.IsTouchingLayers(col, ground);

    if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
    {
      rb.velocity = new Vector2(mvSpeed, jpForce);
      st = State.jumping;
    }
    ActionTransition();
    print(st);
    anim.SetInteger("state", (int)st);
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
    else
    {
      st = State.running;
    }
  }
}
