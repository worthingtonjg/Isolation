using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Linq;

public class LevelGenerator : MonoBehaviour
{
    private static LevelGenerator _instance;
    public static LevelGenerator Instance
    {
        get 
        {
            return _instance;
        }
    }

    public int rows;
    public int cols;
    public List<GameObject> PlatformPrefabs;
    public List<int> PlatformFrequency;

    public GameObject TopPrefab;
    public GameObject TopPlayer;
    public GameObject LeftPrefab;
    public GameObject LeftPlayer;
    public GameObject BottomPrefab;
    public GameObject RightPrefab;

    public GameObject StartCanvas;
    public GameObject GameOverCanvas;
    public TMP_Text TimeText;

    public TMP_Text PlayersLostText;
    public TMP_Text PlayersHomeText;
    public TMP_Text PlayersIsolatedText;

    public List<Player> Players;

    public bool GameRunning;
    public float timeRemaining = 600;

    public List<AudioClip> Clips;

    private AudioSource audioSource;

    void Awake() {
        _instance = this;
        audioSource = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        InstantiateTops();
        InstantiateBottoms();
        InstantiateLefts();
        InstantiateRights();
        CreateBoard();
    }

    private void Update() 
    {
        if(GameRunning)    
        {
            timeRemaining -= Time.deltaTime; 
            UpdateUI();
            CheckForGameOver();
        }
    }

    private void InstantiateTops()
    {
        for (int col = 0; col < cols; col++)
        {
            GameObject.Instantiate(TopPrefab, new Vector3(-col * 3, 0f, (-rows * 3) + 1), Quaternion.identity);
            var player = GameObject.Instantiate(TopPlayer, new Vector3(-col * 3, 1f, (-rows * 3) + 1), Quaternion.identity);
            Players.Add(player.GetComponent<Player>());
        }
    }

    private void InstantiateBottoms()
    {
        for (int col = 0; col < cols; col++)
        {
            GameObject.Instantiate(BottomPrefab, new Vector3(-col * 3, 0f, 2), Quaternion.identity);
        }
    }

    private void InstantiateLefts()
    {
        for (int row = 0; row < rows; row++)
        {
            GameObject.Instantiate(LeftPrefab, new Vector3(2f, 0f, -row*3),  Quaternion.identity);
            var player = GameObject.Instantiate(LeftPlayer, new Vector3(2f, 1f, -row*3),  Quaternion.identity);
            Players.Add(player.GetComponent<Player>());
        }
    }

    private void InstantiateRights()
    {
        for (int row = 0; row < rows; row++)
        {
            GameObject.Instantiate(RightPrefab, new Vector3((-cols*3)+1, 0f, -row*3),  Quaternion.identity);
        }
    }    

    private void CreateBoard()
    {
        List<GameObject> pickList = FillPickList();

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                var p = Random.Range(0, pickList.Count);
                var a = Random.Range(0,3);
                var prefab = pickList[p];
                GameObject.Instantiate(prefab, new Vector3(-col * 3, 0f, -row * 3), Quaternion.Euler(new Vector3(0, 90*a, 0)));
            }
        }
    }

    private List<GameObject> FillPickList()
    {
        var pickList = new List<GameObject>();

        int index = 0;
        foreach (var f in PlatformFrequency)
        {
            for (int i = 0; i < f; i++)
            {
                pickList.Add(PlatformPrefabs[index]);
            }

            index++;
        }

        return pickList;
    }

    public void StartGame()
    {
        StartCanvas.SetActive(false);
        GameRunning = true;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void PlayClip(string name) 
    {
        
        audioSource.PlayOneShot(Clips.FirstOrDefault(c => c.name.ToLower().Contains(name.ToLower())));
    }

    public void CheckForGameOver()
    {
        if(timeRemaining <= 0 || Players.Count(p => p.state == "Lost") + Players.Count(p => p.state == "Home") >= Players.Count)
        {
            GameRunning = false;
            timeRemaining = 0;
            GameOverCanvas.SetActive(true);
        }
    }
    
    private void UpdateUI()
    {
        float minutes = Mathf.FloorToInt(timeRemaining / 60);  
        float seconds = Mathf.FloorToInt(timeRemaining % 60);

        if(timeRemaining <= 0)
        {
            minutes = 0;
            seconds = 0;
        }

        TimeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);        
        PlayersIsolatedText.text = Players.Count(p => p.state == "Isolated").ToString();
        PlayersHomeText.text = Players.Count(p => p.state == "Home").ToString();
        PlayersLostText.text = Players.Count(p => p.state == "Lost").ToString();
    }
}
