using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 3f;
    public Sprite[] tankSprite;
    public GameObject bulletPrefab;
    public GameObject explodePrefab;
    public GameObject bornPrefab;
    public float attackTime = 3f;
    public int hp = 1;
    public AudioClip HitClip;

    private SpriteRenderer sr;
    private Vector3 bulletEulerAngles;
    private float timeVal = 0;
    private float timeChangeDirection = 4f;
    private bool isBorn = false;
    private GameObject bornObject;
    private float v = 0;
    private float h = 0;

    private void Awake()
    {
        gameObject.SetActive(false);
        sr = GetComponent<SpriteRenderer>();
        bornObject = Instantiate(bornPrefab, transform.position, Quaternion.identity);
        Invoke("Create", 1f);
    }

    private void Create()
    {
        gameObject.SetActive(true);
        isBorn = true;
        Destroy(bornObject);
    }

    void Start()
    {

    }

    void Update()
    {
        if (!isBorn) return;
        if (timeVal >= attackTime)
        {
            Attack();
        }
        else
        {
            timeVal += Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        if (!isBorn) return;
        Move();
    }

    private void Attack()
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.Euler(transform.eulerAngles + bulletEulerAngles));
        bullet.GetComponent<Bullet>().isPlayerBullet = false;
        timeVal = 0;
    }

    private void Die()
    {
        hp -= 1;
        if (hp > 0) {
            AudioSource.PlayClipAtPoint(HitClip, transform.position);
            return;
        }
        Instantiate(explodePrefab, transform.position, Quaternion.identity);
        GameManager.Instance.destroyNum++;
        Destroy(gameObject);
    }


    private void Move()
    {
        if (timeChangeDirection >= 1.2f)
        {
            float n = Random.Range(0, 5.5f);
            if (n < 1)
            {
                h = 1;
                v = 0;
            }
            else if (n < 2)
            {
                h = -1;
                v = 0;
            }
            else if (n < 3)
            {
                v = 1;
                h = 0;
            }
            else
            {
                v = -1;
                h = 0;
            }
            timeChangeDirection = 0;
        }
        else
        {
            timeChangeDirection += Time.fixedDeltaTime;
        }
        transform.Translate(Vector3.up * v * speed * Time.fixedDeltaTime, Space.World);

        if (v == -1)
        {
            sr.sprite = sr.sprite != tankSprite[4] ? tankSprite[4] : tankSprite[5];
            bulletEulerAngles = new Vector3(0, 0, -180);
        }
        if (v == 1)
        {
            sr.sprite = sr.sprite != tankSprite[0] ? tankSprite[0] : tankSprite[1];
            bulletEulerAngles = new Vector3(0, 0, 0);
        }
        if (v != 0)
        {
            return;
        }

        transform.Translate(Vector3.right * h * speed * Time.fixedDeltaTime, Space.World);

        if (h == -1)
        {
            sr.sprite = sr.sprite != tankSprite[6] ? tankSprite[6] : tankSprite[7];
            bulletEulerAngles = new Vector3(0, 0, 90);
        }
        if (h == 1)
        {
            sr.sprite = sr.sprite != tankSprite[2] ? tankSprite[2] : tankSprite[3];
            bulletEulerAngles = new Vector3(0, 0, -90);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        switch (collision.gameObject.tag)
        {
            case "Enemy":
                v = 0;
                h = 0;
                timeChangeDirection = 1.2f;
                break;
            default:
                break;
        }
    }
}
    
