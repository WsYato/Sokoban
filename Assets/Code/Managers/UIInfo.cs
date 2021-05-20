using UnityEngine;
using UnityEngine.UI;

public class UIInfo : MonoBehaviour
{
    [SerializeField] Text levelName;
    [SerializeField] Text levelCount;
    [SerializeField] Text playerMoveCount;
    [SerializeField] Button goBack;
    [SerializeField] Canvas hud;
    [SerializeField] Canvas levelDone;

    int m_currentPlayerMoveCount = 0;

    private void Awake()
    {
        Player.OnMove += UpdateUI;
        LevelManager.OnLevelStarted += LevelStarted;
        LevelManager.OnLevelComplete += EnableLevelDoneCanvas;

        playerMoveCount.text = m_currentPlayerMoveCount.ToString();
    }

    public void SetLevel(Level level)
    {
        levelName.text = level.m_levelName;
        levelCount.text = level.m_levelCount.ToString();
    }

    void UpdateUI()
    {
        m_currentPlayerMoveCount++;
        playerMoveCount.text = m_currentPlayerMoveCount.ToString();

        if(!goBack.interactable)
            goBack.interactable = true;
    }

    public void GoBack()
    {
        if(m_currentPlayerMoveCount <= 0)
            return;

        m_currentPlayerMoveCount--;
        playerMoveCount.text = m_currentPlayerMoveCount.ToString();

        MoveableManager.GoBack();

        if(m_currentPlayerMoveCount <= 0)
            goBack.interactable = false;
    }

    public void Restart()
    {
        ResetHUD();

        MoveableManager.Restart();
        LevelManager.Instance.Restart();
    }

    void ResetHUD()
    {
        m_currentPlayerMoveCount = 0;
        playerMoveCount.text = m_currentPlayerMoveCount.ToString();
        goBack.interactable = false;
    }

    void LevelStarted()
    {
        ResetHUD();

        hud.enabled = true;
        levelDone.enabled = false;
    }

    void EnableLevelDoneCanvas()
    {
        hud.enabled = false;
        levelDone.enabled = true;
    }

    public void NextLevel()
    {
        if(LevelManager.Instance.LoadNextLevel())
            return;

        print("No more levels!");
    }
}