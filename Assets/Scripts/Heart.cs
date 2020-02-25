using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour
{

    public Sprite heartDieSprite;
    public GameObject explodePrefab;

    private SpriteRenderer sr;

    public AudioClip DieClip;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Die()
    {
        Instantiate(explodePrefab, transform.position, Quaternion.identity);
        sr.sprite = heartDieSprite;
        AudioSource.PlayClipAtPoint(DieClip, transform.position);
        GameManager.Instance.GameOver();
    }
}
