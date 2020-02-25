using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 3f;
    public Sprite[] tankSprite;
    public GameObject bulletPrefab;
    public GameObject explodePrefab;
    public GameObject shieldPrefab;
    public GameObject bornPrefab;

    public List<AudioClip> EngineClip;
   

    [HideInInspector]
    public bool isStop = false;

    private SpriteRenderer sr;
    private GameObject shieldObject;
    private Vector3 bulletEulerAngles;
    private float timeVal = 0;
    private float shieldVal = 3f;
    private bool isDefended = false;
    private bool isBorn = false;
    private GameObject bornObject;
    private float v = 0;
    private float h = 0;
    private AudioSource moveAudio;


    private void Awake()
    {
        gameObject.SetActive(false);
        sr = GetComponent<SpriteRenderer>();
        bornObject = Instantiate(bornPrefab, transform.position, Quaternion.identity);
        Invoke("Create", 1f);
        moveAudio = gameObject.GetComponent<AudioSource>();
    }

    private void Create()
    {
        shieldObject = Instantiate(shieldPrefab, transform);
        isDefended = true;
        gameObject.SetActive(true);
        isBorn = true;
        Destroy(bornObject);
    }

    void Start()
    {

    }

    void Update()
    {
        if (isStop) return;
        if (!isBorn) return;
        if (timeVal >= 0.4f)
        {
            Attack();
        }
        else
        {
            timeVal += Time.deltaTime;
        }

        if (shieldVal <= 0)
        {
            shieldObject.SetActive(false);
            isDefended = false;
        }
        else
        {
            isDefended = true;
            shieldVal -= Time.deltaTime;
        }
    }
    
    private void FixedUpdate()
    {
        if (isStop) return;
        if (!isBorn) return;
        Move();  
    }

    private void Attack()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.Euler(transform.eulerAngles + bulletEulerAngles));
            bullet.GetComponent<Bullet>().isPlayerBullet = true;
            timeVal = 0;
        }
    }

    private void Die()
    {
        if (isDefended) return;
        Instantiate(explodePrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
        GameManager.Instance.isDead = true;
    }


    private void Move()
    {
        v = Input.GetAxisRaw("Vertical");
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

        if (Mathf.Abs(v) > 0.05f)
        {
            if (!moveAudio.isPlaying)
            {
                moveAudio.clip = EngineClip[0];
                moveAudio.Play();
            }
        }

        if (v != 0)
        {
            return;
        }
        h = Input.GetAxisRaw("Horizontal");
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

        if (Mathf.Abs(h) > 0.05f)
        {
            if (!moveAudio.isPlaying)
            {
                moveAudio.clip = EngineClip[1];
                moveAudio.Play();
            }          
        }
        else
        {
            if (moveAudio.isPlaying)
            {
                //moveAudio.clip = EngineClip[1];
                //moveAudio.Play();
                moveAudio.Pause();
            }
        }
    }
}
