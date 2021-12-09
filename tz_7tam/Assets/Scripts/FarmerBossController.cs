using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FarmerBossController : MonoBehaviour
{
    [SerializeField] LayerMask playerLayer;
    [SerializeField] SpriteRenderer farmerSr;
    [SerializeField] Sprite farmerRight, farmerLeft, farmerUp, farmerDown, farmerDirtyRight, farmerDirtyLeft, farmerDirtyUp, farmerDirtyDown, farmerAngryRight, farmerAngryLeft, farmerAngryUp, farmerAngryDown;
    [SerializeField] LayerMask obstacleMask;
    int health = 3;
    float moveSpeed = 2f;
    bool isMoving = false;
    bool isDamagable = true;
    bool isDead = false;
    public bool IsDead => isDead;
    RaycastHit2D hit;
    Vector2 target;
    enum farmerDirection
    {
        RIGHT,
        LEFT,
        UP,
        DOWN
    }
    farmerDirection currentDirection;

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
            else if(isMoving && isDamagable)
            {
                transform.position = Vector2.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
            }
            else
            {
                transform.position = transform.position;
            }
            if(Vector2.Distance(transform.position, target) == 0)
                isMoving = false;
            switch (currentDirection)
            {
                case farmerDirection.RIGHT:
                    hit = Physics2D.BoxCast(new Vector2(transform.position.x, transform.position.y - 1), new Vector2(2,1), 0f, Vector2.right, 2f, playerLayer);                                                            
                    if(hit)
                        farmerSr.sprite = farmerAngryRight;
                    break;
                case farmerDirection.LEFT:
                    hit = Physics2D.BoxCast(new Vector2(transform.position.x, transform.position.y - 1), new Vector2(2,1), 0f, Vector2.left, 2f, playerLayer);                                        
                    if(hit)
                        farmerSr.sprite = farmerAngryLeft;
                    break;
                case farmerDirection.UP:
                    hit = Physics2D.BoxCast(new Vector2(transform.position.x, transform.position.y - 1), new Vector2(1,2), 0f, Vector2.up, 2f, playerLayer);                    
                    if(hit)
                        farmerSr.sprite = farmerAngryUp;
                    break;
                case farmerDirection.DOWN:
                    hit = Physics2D.BoxCast(new Vector2(transform.position.x, transform.position.y - 1), new Vector2(1,2), 0f, Vector2.down, 2f, playerLayer);
                    if(hit)
                        farmerSr.sprite = farmerAngryDown;
                    break;
            }
            if(!hit)
                FlipSprite(currentDirection);
        }
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.gameObject.CompareTag("Damage") && !isDead && isDamagable)
            StartCoroutine(IGetDamage(currentDirection));
    }

    void ChooseDirection()
    {
        do
        {
            switch (Random.Range(0,4))
            {
                case 0:
                    currentDirection = farmerDirection.RIGHT;
                    target = new Vector2(transform.position.x + 2.25f, transform.position.y);
                    break;
                case 1:
                    currentDirection = farmerDirection.LEFT;
                    target = new Vector2(transform.position.x - 2.25f, transform.position.y);
                    break;
                case 2:
                    currentDirection = farmerDirection.UP;
                    target = new Vector2(transform.position.x + .25f, transform.position.y + 2);
                    break;
                case 3:
                    currentDirection = farmerDirection.DOWN;
                    target = new Vector2(transform.position.x - .25f, transform.position.y - 2);
                    break;
            }
        } while (Physics2D.OverlapCircle(target, .5f, obstacleMask));
        
        FlipSprite(currentDirection);
        isMoving = true;
    }

    void FlipSprite(farmerDirection direction)
    {
        switch (direction)
        {
            case farmerDirection.RIGHT:
                farmerSr.sprite = farmerRight;
                break;
            case farmerDirection.LEFT:
                farmerSr.sprite = farmerLeft;
                break;
            case farmerDirection.UP:
                farmerSr.sprite = farmerUp;
                break;
            case farmerDirection.DOWN:
                farmerSr.sprite = farmerDown;
                break;
        }
    }

    IEnumerator IGetDamage(farmerDirection direction)
    {
        health--;
        if(health <= 0)
        {
            isDead = true;
            switch(direction)
            {
                case farmerDirection.RIGHT:
                    farmerSr.sprite = farmerDirtyRight;
                    break;
                case farmerDirection.LEFT:
                    farmerSr.sprite = farmerDirtyLeft;
                    break;
                case farmerDirection.UP:
                    farmerSr.sprite = farmerDirtyUp;
                    break;
                case farmerDirection.DOWN:
                    farmerSr.sprite = farmerDirtyDown;
                    break;
            }

            yield return new WaitForSecondsRealtime(.5f);

            transform.DOScale(Vector3.zero, .5f);
            yield return new WaitForSecondsRealtime(.5f);
            Destroy(gameObject);
        }
        else
        {
            isDamagable = false;
            Invulnerable();
            yield return new WaitForSecondsRealtime(1.5f);
            isDamagable = true;
        }
    }

    void Invulnerable()
    {
        Sequence _invulSequence = DOTween.Sequence();
        Color defaultColor = farmerSr.color;
        Color hurtColor = Color.red;
        _invulSequence.Append(farmerSr.DOColor(hurtColor, .25f));
        _invulSequence.Append(farmerSr.DOColor(defaultColor, .25f));
        _invulSequence.Append(farmerSr.DOColor(hurtColor, .25f));
        _invulSequence.Append(farmerSr.DOColor(defaultColor, .25f));
        _invulSequence.Append(farmerSr.DOColor(hurtColor, .25f));
        _invulSequence.Append(farmerSr.DOColor(defaultColor, .25f));
        
    }
}
