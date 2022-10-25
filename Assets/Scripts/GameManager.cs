using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    public GameObject character;

    public TMP_Text timeText;
    public int currentTime;
    public int currentLevel;

    IEnumerator TimerTick(){
        while (currentTime > 0)
        {
            timeText.text = "Time : " + currentTime.ToString();
            yield return new WaitForSeconds(1);
            currentTime--;
        }
        SceneManager.LoadScene("Tmp_Thomas");
    }

    private CinemachineBrain mainCamera;

    private void Awake()
    {
        mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<CinemachineBrain>();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        if(!timeText){
            Debug.Log("ALLO");
        }
        StartCoroutine(TimerTick());
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log((mainCamera.ActiveVirtualCamera as CinemachineVirtualCamera).transform.position.y);
        if(character.transform.position.y < -10.0){
            //Debug.Log((mainCamera.ActiveVirtualCamera as CinemachineVirtualCamera).transform.position.y);
            SceneManager.LoadScene("Tmp_Thomas");
        }
    }
}
