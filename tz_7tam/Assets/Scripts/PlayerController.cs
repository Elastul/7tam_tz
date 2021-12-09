using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

[System.Serializable]
public class OnDamageTake : UnityEvent<int>
{
}
public class PlayerController : MonoBehaviour
{
    [SerializeField] OnDamageTake onDamageTake;
    [SerializeField] Sprite pigGhost, pigUp, pigDown, pigLeft, pigRight;
    [SerializeField] SpriteRenderer pigRend;
    [SerializeField, Range(3,10)] int moveSpeed;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] GameObject bombPrefab;
    [SerializeField] PlayerStats playerStats;
    [SerializeField] Joystick joystick;
    

    int currentHealth;
    int CurrentHealth => currentHealth;
    float invTimer;

    bool isDead = false;
    bool isDamagable = true;
    bool isHandheld;
    Vector2 direction;
    float x,y;

    void Awake() => isHandheld = SystemInfo.deviceType == DeviceType.Handheld ? true : false;

    void Start()
    {
        currentHealth = playerStats.MaxHealth;
    }

    void Update()
    {
        if(!isDead)
        {
            if(!isHandheld)
            {
                x = Input.GetAxis("Horizontal");
                y = Input.GetAxis("Vertical");

                if(Input.GetKeyDown(KeyCode.Space)) PlaceBomb();
            }
            else
            {
                x = joystick.Horizontal;
                y = joystick.Vertical;
            }
            if(Mathf.Abs(joystick.Vertical) < Mathf.Abs(joystick.Horizontal))
            {
                if(x > 0)
                    pigRend.sprite = pigRight;
                else if(x < 0)
                    pigRend.sprite = pigLeft;
            }
            else if(Mathf.Abs(joystick.Vertical) > Mathf.Abs(joystick.Horizontal))
            {
                if(y > 0)
                    pigRend.sprite = pigUp;
                else if(y < 0)
                    pigRend.sprite = pigDown;
            }
                
            direction = new Vector2(x,y);

            if(invTimer > 0)
                invTimer -= Time.deltaTime;
            else if(invTimer <= 0) 
                isDamagable = true;

            Move();
        }
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        if(isDamagable)
        {
            if(other.gameObject.CompareTag("Damage"))
                GetDamage();
            if(other.gameObject.CompareTag("Enemy"))
                if(other.GetComponent<DogEnemyController>() != null && !other.GetComponent<DogEnemyController>().IsDead)
                    GetDamage();
                if(other.GetComponent<FarmerBossController>() != null && !other.GetComponent<FarmerBossController>().IsDead)
                    GetDamage();
        }
    }

    void GetDamage()
    {
        onDamageTake.Invoke(currentHealth);
        
        currentHealth--;
        isDamagable = false;
        invTimer = 1.5f;

        if(currentHealth > 0)
        {
            Invulnerable();
        }
        else
        {
            isDead = true;
            pigRend.sprite = pigGhost;
            Sequence _deathSequence = DOTween.Sequence();
            transform.DOMoveY(transform.position.y + 5f, 1.5f);
            pigRend.DOFade(0f, 1.5f);
        }
        
    }

    void Invulnerable()
    {
        Sequence _invulSequence = DOTween.Sequence();
        Color defaultColor = pigRend.color;
        Color hurtColor = Color.red;
        _invulSequence.Append(pigRend.DOColor(hurtColor, .25f));
        _invulSequence.Append(pigRend.DOColor(defaultColor, .25f));
        _invulSequence.Append(pigRend.DOColor(hurtColor, .25f));
        _invulSequence.Append(pigRend.DOColor(defaultColor, .25f));
        _invulSequence.Append(pigRend.DOColor(hurtColor, .25f));
        _invulSequence.Append(pigRend.DOColor(defaultColor, .25f));
        
    }

    public void PlaceBomb()
    {
        if(GameObject.FindGameObjectsWithTag("Bomb").Length < playerStats.MaxBombs)
        {
            GameObject bombTemp = Instantiate(bombPrefab, transform.position, Quaternion.Euler(Vector3.zero));
            bombTemp.GetComponent<BombExplosion>().Init(playerStats.BombRange);
        }
    }

    void Move()
    {
        rb.velocity = direction * moveSpeed;
    }
}
