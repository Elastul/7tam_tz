using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BombExplosion : MonoBehaviour
{
    [SerializeField] LayerMask obstacleLayer;
    [SerializeField] GameObject ExplosionPart;
    [SerializeField] Collider2D col;
    [SerializeField, Range(1, 5)] float bombTimer;
    int maxRange;
    float xOffset = 0.125f;
    bool exploded = false;

    bool spawnUp, spawnDown, spawnLeft, spawnRight;
    
    void Start()
    {
        DoBounce();
    }

    void Update()
    {
        if(bombTimer > 0) 
            bombTimer -= Time.deltaTime;
        else if(bombTimer <= 0 && !exploded) Explosion();
    }

    void OnTriggerExit2D(Collider2D other) 
    {
        col.isTrigger = false;
    }

    public void Init(int _maxRange)
    {
        maxRange = _maxRange;
    }

    void Explosion()
    {
        spawnUp = true;
        spawnDown = true;
        spawnLeft = true;
        spawnRight = true;
        exploded = true;
        Instantiate(ExplosionPart, transform.position, Quaternion.Euler(Vector3.zero));
        for (int i = 1; i <= maxRange; i++)
        {
            if(Physics2D.OverlapCircle(new Vector2(transform.position.x + i, transform.position.y), .2f, obstacleLayer))
                spawnRight = false;
            if(Physics2D.OverlapCircle(new Vector2(transform.position.x - i, transform.position.y), .2f, obstacleLayer))
                spawnLeft = false;
            if(Physics2D.OverlapCircle(new Vector2(transform.position.x + xOffset, transform.position.y + i), .2f, obstacleLayer))
                spawnUp = false;
            if(Physics2D.OverlapCircle(new Vector2(transform.position.x - xOffset, transform.position.y - i), .2f, obstacleLayer))
                spawnDown = false;
            if(spawnRight)
                Instantiate(ExplosionPart, new Vector3(transform.position.x + i, transform.position.y, 0), Quaternion.Euler(Vector3.zero));
            if(spawnLeft)
                Instantiate(ExplosionPart, new Vector3(transform.position.x - i, transform.position.y, 0), Quaternion.Euler(Vector3.zero));
            if(spawnUp)
                Instantiate(ExplosionPart, new Vector3(transform.position.x + xOffset, transform.position.y + i, 0), Quaternion.Euler(Vector3.zero));
            if(spawnDown)
                Instantiate(ExplosionPart, new Vector3(transform.position.x - xOffset, transform.position.y - i, 0), Quaternion.Euler(Vector3.zero));
            xOffset *= 2;
        }
        Destroy(transform.gameObject, .2f);
    }

    void DoBounce()
    {
        Sequence _bounceSequence = DOTween.Sequence();
        _bounceSequence.Append(transform.DOScale(new Vector3(0,0,0), 0f));
        _bounceSequence.Append(transform.DOScale(new Vector3(1.2f,1.2f,1.2f), .2f));
        _bounceSequence.Append(transform.DOScale(new Vector3(.8f,.8f,.8f), .05f));
        _bounceSequence.Append(transform.DOScale(new Vector3(1.2f,1.2f,1.2f), .05f));
    }
}
