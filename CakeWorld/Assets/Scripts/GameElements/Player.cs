using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : GravityBody {
    public float speed = 20;
    public float jumpingHeight = 2;
    public List<AudioClip> jumpSounds = new List<AudioClip>();
    public List<AudioClip> eatingSounds= new List<AudioClip>();
    public Vector2 jumpDampening = new Vector2(0.01f, 0.15f);
    private Vector2 moveDir;
    private bool canJump = true;
    private Rigidbody2D body;
    private bool jumping = false;
    private float jumpX = 0;
    private float jumpY = 0;

    private float jumpXStart = 0;
    private float jumpYStart = 0;

    private void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    protected override void Update()
    {
        moveDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical") >= 0 ? Input.GetAxisRaw("Vertical") : 0).normalized;
        base.Update();
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.CompareTag("MuncherHead"))
        {
            GameManager.Instance.RemoveMuncher(coll.gameObject.transform.parent.gameObject);
        }
        if (coll.gameObject.CompareTag("WorldJumpingArea"))
        {
            canJump = true;
        }
    }

    private void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.gameObject.CompareTag("WorldJumpingArea"))
        {
            canJump = false;

            AudioSource.PlayClipAtPoint(jumpSounds[Random.Range(0, jumpSounds.Count)], transform.position);
            jumping = true;
            jumpXStart = moveDir.x;
            jumpYStart = moveDir.y * jumpingHeight;
            jumpX = jumpXStart;
            jumpY = jumpYStart;
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.CompareTag("World"))
        {
            jumping = false;
        }
        if (coll.gameObject.CompareTag("Cake"))
        {
            Cake cake = coll.gameObject.GetComponent<Cake>();
            World.Instance.IncreaseSize(cake.growthFactor);
            GameManager.Instance.RemoveCake(coll.gameObject);
            AudioSource.PlayClipAtPoint(eatingSounds[Random.Range(0, eatingSounds.Count)], transform.position);
        }
    }
    
    // Update is called once per frame
    public void FixedUpdate()
    {
        if (moveDir.x >= 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
        else
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }

        
        if ((moveDir.x != 0 || moveDir.y != 0) && canJump)
        {
            moveDir.y = moveDir.y * jumpingHeight;
            body.MovePosition(body.position + (Vector2)transform.TransformDirection(moveDir) * speed * Time.deltaTime);
        }
        else if(jumping)
        {
            if (jumpXStart > 0)
            {
                jumpX = jumpX - jumpDampening.x < 0 ? 0 : jumpX - jumpDampening.x;
            }
            else if (jumpXStart < 0)
            {
                jumpX = jumpX + jumpDampening.x > 0 ? 0 : jumpX + jumpDampening.x;
            }
            if (jumpYStart > 0)
            {
                jumpY = jumpY - jumpDampening.y;
            }
            else if (jumpYStart < 0)
            {
                jumpY = jumpY + jumpDampening.y;
            }
            body.MovePosition(body.position + (Vector2)transform.TransformDirection(new Vector2(jumpX, jumpY)) * speed * Time.deltaTime);
        }
        // Force to keep player closer
        body.AddForce((World.Instance.transform.position - transform.position) * 30);
    }
}
