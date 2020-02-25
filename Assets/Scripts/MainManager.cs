using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour
{
    public GameObject Tank;
    public Transform Pos1;
    public Transform Pos2;

    private int num = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Tank.transform.position = Pos1.position;
            num = 1;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow)){
            Tank.transform.position = Pos2.position;
            num = 2;
        }

        if (Input.GetKeyDown(KeyCode.Space)&& num == 1)
        {
            PlayerPrefs.SetString("playerNum", num.ToString());
            SceneManager.LoadScene(1);
        }
    }
}
