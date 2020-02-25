using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject mapBox;
    // 0 核心 1 墙 2 河 3 草 4 钢 5 空气墙 6 玩家A
    public GameObject[] mapPrefabList;
    // 0 灰普 1 红普通 2 灰速 
    public GameObject[] enemyPrefabList;

    public List<Vector3> enemyBirthPoint;

    public int enemyNum = 20;

    public int lifeNum = 3;

    public Text playerLifeNum;
    public Text enemyHasNum;
    public GameObject GameOverUI;

    public AudioClip startClip;

    [HideInInspector]
    public bool isDead = false;

    [HideInInspector]
    public int destroyNum = 0;

    private List<Vector3> itemList = new List<Vector3>();

    private int playerNum = 1;
    private int enemyMaxNum = 0;

    private GameObject player;
    private bool isEnd = false;

    private static GameManager instance;

    public static GameManager Instance { get => instance; set => instance = value; }

    private void Awake()
    {
        Instance = this;
        enemyMaxNum = enemyNum;
        try
        {
            playerNum = int.Parse(PlayerPrefs.GetString("playerNum"));
        }
        catch { }

        createHeartChunk();
        createPlayer();
        createAirChunk();

        createEnemy(0);
        createEnemy(1);
        createEnemy(2);
        InvokeRepeating("createNewEnemy", 4, 5);
        createMap();
        GameOverUI.SetActive(false);

        AudioSource.PlayClipAtPoint(startClip, transform.position);
    }

    private void Update()
    {
        if (isEnd) return;
        if (isDead)
        {
            Recover();
        }

        if (destroyNum >= enemyMaxNum)
        {
            YouWin(); 
        }

        playerLifeNum.text = lifeNum.ToString();
        enemyHasNum.text = destroyNum.ToString();
    }

    private void Recover()
    {
        lifeNum--;
        if (lifeNum > 0)
        {
            createPlayer();
        }
        else
        {
            GameOver();
        }      
    }

    public void YouWin()
    {
        
        isEnd = true;
        Debug.Log("You Win");
        SceneManager.LoadScene(1);
    }

    public void GameOver()
    {
        isEnd = true;
        try
        {
            player.GetComponent<Player>().isStop = true;
        }
        catch{}  
        Debug.Log("GameOver");
        Invoke("showGameOver", 1.2f);
    }

    private void showGameOver()
    {
        GameOverUI.SetActive(true);
    }

    private void createHeartChunk()
    {
        createGameObject(mapPrefabList[0], new Vector3(0, -8f, 0));
        createGameObject(mapPrefabList[1], new Vector3(-1, -8f, 0));
        createGameObject(mapPrefabList[1], new Vector3(1, -8f, 0));
        for(int i = -1; i < 2; i++)
        {
            createGameObject(mapPrefabList[1], new Vector3(i, -7f, 0));
        }
    }


    private void createPlayer()
    {
        isDead = false;
        player = createGameObject(mapPrefabList[6], new Vector3(-2, -8f, 0));
    }

    private void createNewEnemy()
    {
        if (enemyNum <= 0)
        {
            return;
        }
        enemyNum--;
        int type = Random.Range(0, enemyPrefabList.Length);
        int num = Random.Range(0, enemyBirthPoint.Count);
        createGameObject(enemyPrefabList[type], enemyBirthPoint[num]);
    }

    private void createEnemy(int num)
    {
        enemyNum--;
        int type = Random.Range(0, enemyPrefabList.Length);
        if (num == -1)
        {
            num = Random.Range(0, enemyBirthPoint.Count);
        }
        createGameObject(enemyPrefabList[type], enemyBirthPoint[num]);
    }

    private void createAirChunk()
    {
        for (int i = -11; i < 12; i++)
        {
            createGameObject(mapPrefabList[5], new Vector3(i, -9f, 0));
        }
        for (int i = -11; i < 12; i++)
        {
            createGameObject(mapPrefabList[5], new Vector3(i, 9f, 0));
        }
        for (int i = -8; i < 9; i++)
        {
            createGameObject(mapPrefabList[5], new Vector3(11.15f,i , 0));
        }
        for (int i = -8; i < 9; i++)
        {
            createGameObject(mapPrefabList[5], new Vector3(-11.15f, i, 0));
        }
    }

    private GameObject createGameObject(GameObject obj,Vector3 point)
    {
        GameObject item = Instantiate(obj, point, Quaternion.identity);
        item.transform.SetParent(mapBox.transform);
        itemList.Add(item.transform.position);
        return item;
    }

    private Vector3 createRandomPosition()
    {
        while (true)
        {
            Vector3 createPosition = new Vector3(Random.Range(-9, 10), Random.Range(-7, 8), 0);
            if (!HasThePosition(createPosition))
            {
                return createPosition;
            }
        }
        
    }

    private bool HasThePosition(Vector3 position)
    {
        for (int i = 0; i < itemList.Count; i++){
            if (itemList[i]==position)
            {
                return true;
            }
        }
        return false;
    }

    private void createMap()
    {
        for(int i = 0; i < 60; i++)
        {
            createGameObject(mapPrefabList[1], createRandomPosition());
        }

        for (int i = 0; i < 10; i++)
        {
            createGameObject(mapPrefabList[2], createRandomPosition());
        }

        for (int i = 0; i < 10; i++)
        {
            createGameObject(mapPrefabList[3], createRandomPosition());
        }

        for (int i = 0; i < 15; i++)
        {
            createGameObject(mapPrefabList[4], createRandomPosition());
        }
    }

}
