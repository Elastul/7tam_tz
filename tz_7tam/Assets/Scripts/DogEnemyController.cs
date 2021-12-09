using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DogEnemyController : MonoBehaviour
{
    [SerializeField] LayerMask playerLayer;
    [SerializeField] SpriteRenderer dogSr;
    [SerializeField] Sprite dogRight, dogLeft, dogUp, dogDown, dogDirtyRight, dogDirtyLeft, dogDirtyUp, dogDirtyDown, dogAngryRight, dogAngryLeft, dogAngryUp, dogAngryDown;
    [SerializeField] LayerMask obstacleMask;
    float moveSpeed = 2f;
    bool isMoving = false;
    bool isDead = false;
    RaycastHit2D hit;
    public bool IsDead => isDead;
    Vector2 target;
    enum dogDirection
    {
        RIGHT,
        LEFT,
        UP,
        DOWN
    }
    dogDirection currentDirection;

    void Start()
    {
        ChooseDirection();
    }

    // Update is called once per frame
    void Update()
    {
        if(!isDead)
        {
            if(!isMoving)
                ChooseDirection();
            else
            {
                transform.position = Vector2.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
            }
            if(Vector2.Distance(transform.position, target) == 0)
                isMoving = false;
            switch (currentDirection)
            {
                case dogDirection.RIGHT:
                    hit = Physics2D.BoxCast(transform.position, new Vector2(2,1), 0f, Vector2.right, 2f, playerLayer);
                    if(hit)
                        dogSr.sprite = dogAngryRight;
                    break;
                case dogDirection.LEFT:
                    hit = Physics2D.BoxCast(transform.position, new Vector2(2,1), 0f, Vector2.left, 2f, playerLayer);
                    if(hit)
                        dogSr.sprite = dogAngryLeft;
                    break;
                case dogDirection.UP:
                    hit = Physics2D.BoxCast(transform.position, new Vector2(1,2), 0f, Vector2.up, 2f, playerLayer);
                    if(hit)
                        dogSr.sprite = dogAngryUp;
                    break;
                case dogDirection.DOWN:
                    hit = Physics2D.BoxCast(transform.position, new Vector2(1,2), 0f, Vector2.down, 2f, playerLayer);
                    if(hit)
                        dogSr.sprite = dogAngryDown;
                    break;
            }
            if(!hit)
                FlipSprite(currentDirection);
        }
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.gameObject.CompareTag("Damage") && !isDead)
            StartCoroutine(IGetDamage(currentDirection));
    }

    void ChooseDirection()
    {
        do
        {
            switch (Random.Range(0,4))
            {
                case 0:
                    currentDirection = dogDirection.RIGHT;
                    target = new Vector2(transform.position.x + 2.25f, transform.position.y);
                    break;
                case 1:
                    currentDirection = dogDirection.LEFT;
                    target = new Vector2(transform.position.x - 2.25f, transform.position.y);
                    break;
                case 2:
                    currentDirection = dogDirection.UP;
                    target = new Vector2(transform.position.x + .25f, transform.position.y + 2);
                    break;
                case 3:
                    currentDirection = dogDirection.DOWN;
                    target = new Vector2(transform.position.x - .25f, transform.position.y - 2);
                    break;
            }
        } while (Physics2D.OverlapCircle(target, .5f, obstacleMask));
        
        FlipSprite(currentDirection);
        isMoving = true;
    }

    void FlipSprite(dogDirection direction)
    {
        switch (direction)
        {
            case dogDirection.RIGHT:
                dogSr.sprite = dogRight;
                break;
            case dogDirection.LEFT:
                dogSr.sprite = dogLeft;
                break;
            case dogDirection.UP:
                dogSr.sprite = dogUp;
                break;
            case dogDirection.DOWN:
                dogSr.sprite = dogDown;
                break;
        }
    }

    IEnumerator IGetDamage(dogDirection direction)
    {
        isDead = true;
        switch(direction)
        {
            case dogDirection.RIGHT:
                dogSr.sprite = dogDirtyRight;
                break;
            case dogDirection.LEFT:
                dogSr.sprite = dogDirtyLeft;
                break;
            case dogDirection.UP:
                dogSr.sprite = dogDirtyUp;
                break;
            case dogDirection.DOWN:
                dogSr.sprite = dogDirtyDown;
                break;
        }

        yield return new WaitForSecondsRealtime(.5f);

        transform.DOScale(Vector3.zero, .5f);
        yield return new WaitForSecondsRealtime(.5f);
        Destroy(gameObject);
    }
}
