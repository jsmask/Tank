using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float bulletSpeed = 10f;
    public bool isPlayerBullet = false;
    public int power = 1;
    public AudioClip FireClip;
    public AudioClip HitClip;

    void Start()
    {
        if (isPlayerBullet)
        {
            AudioSource.PlayClipAtPoint(FireClip, transform.position);
        }
    }

    public void Hit()
    {
        try
        {
            AudioSource.PlayClipAtPoint(HitClip, transform.position);
        }
        catch
        {

        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(transform.up * Time.deltaTime * bulletSpeed,Space.World);
    }

    private void Run(Collider2D collision)
    {
        Bullet bt = collision.GetComponent<Bullet>();
        if (power <= bt.power)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log(collision.tag);
        switch (collision.tag)
        {
            case "Wall":
                Destroy(gameObject);
                Destroy(collision.gameObject);
                break;
            case "Heart":
                collision.SendMessage("Die");
                Destroy(gameObject);
                break;
            case "Tank":
                if (!isPlayerBullet)
                {
                    collision.SendMessage("Die");
                    Destroy(gameObject);
                }
                break;
            case "Barriar":
                collision.SendMessage("Hit");
                Destroy(gameObject);
                break;
            case "Enemy":
                if (isPlayerBullet)
                {
                    collision.SendMessage("Die");
                    Destroy(gameObject);
                } 
                break;
            case "Bullet":
                if (!isPlayerBullet)
                {
                    Hit();
                    collision.SendMessage("Run", collision);
                }
                break;
            default:
                break;
        }
    }
}
