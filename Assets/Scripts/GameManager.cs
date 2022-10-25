using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    [Header("Level components")] 
    [SerializeField] private TextMeshProUGUI m_timerText;
    [Header("Level settings")] 
    [SerializeField] private bool m_lightOff = true;
    [SerializeField] private float m_levelDuration = 30.0f;
    
    private float m_levelTimer;

    private void Awake()
    {
        m_levelTimer = m_levelDuration;
    }
    
    void Update()
    {
        m_levelTimer -= Time.deltaTime;
        m_timerText.text = $"{m_levelTimer:F2}";

        if (m_levelTimer <= 0.0f)
        {
            ReloadScene();
        }
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
