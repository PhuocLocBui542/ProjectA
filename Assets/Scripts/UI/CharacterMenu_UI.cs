using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterMenu_UI : MonoBehaviour, ISaveManager
{
    [Header("End Screen")]
    [SerializeField] private UI_FadeScene fadeScene;
    [SerializeField] private GameObject endText;
    [SerializeField] private GameObject restartButton;
    [Space]

    [SerializeField] private GameObject characterUI;
    [SerializeField] private GameObject skilltreeUI;
    [SerializeField] private GameObject craftingUI;
    [SerializeField] private GameObject optionUI;
    [SerializeField] private GameObject inGameUI;

    [Header("Back To Menu")]
    [SerializeField] private string sceneName = "MainMenu";

    [Header("Tool Tip")]
    public UI_SkillToolTip skillToolTip;
    public UI_ItemToolTip itemToolTip;
    public UI_StatToolTip statToolTip;
    public UI_CraftWindow craftWindow;

    [SerializeField] private UI_VolumeSlider[] volumeSettings;

    private void Awake()
    {
        SwitchMenu(skilltreeUI);
        fadeScene.gameObject.SetActive(true);
    }

    void Start()
    {
        SwitchMenu(inGameUI);


        itemToolTip.gameObject.SetActive(false);
        statToolTip.gameObject.SetActive(false);
        skillToolTip.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
            SwitchWithKeyTo(characterUI);

        if (Input.GetKeyDown(KeyCode.U))
            SwitchWithKeyTo(craftingUI);

        if (Input.GetKeyDown(KeyCode.I))
            SwitchWithKeyTo(skilltreeUI);

        if (Input.GetKeyDown(KeyCode.O))
            SwitchWithKeyTo(optionUI);
    }

    public void SwitchMenu(GameObject _menu)
    {

        for (int i = 0; i < transform.childCount; i++)
        {
            bool fadeScreen = transform.GetChild(i).GetComponent<UI_FadeScene>() != null;

            if (fadeScreen == false)
                transform.GetChild(i).gameObject.SetActive(false);
        }

        if (_menu != null)
        {
            AudioManager.instance.PlaySFX(7, null);
            _menu.SetActive(true);
        }

        if (GameManager.instance != null)
        {
            if (_menu == inGameUI)
                GameManager.instance.PauseGame(false);
            else
                GameManager.instance.PauseGame(true);
        }
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(sceneName);
    }

    public void SwitchWithKeyTo(GameObject _menu)
    {
        if (_menu != null && _menu.activeSelf)
        {
            _menu.SetActive(false);
            CheckForInGameUI();
            return;
        }

        SwitchMenu(_menu);
    }

    private void CheckForInGameUI()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.activeSelf && transform.GetChild(i).GetComponent<UI_FadeScene>() == null)
                return;
        }

        SwitchMenu(inGameUI);
    }

    public void SwitchOnEndScrence()
    {
        fadeScene.FadeOut();
        StartCoroutine(EndScreenCorutione());
    }

    IEnumerator EndScreenCorutione()
    {
        yield return new WaitForSeconds(1);
        endText.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        restartButton.SetActive(true);
    }

    public void RestartGameButton() => GameManager.instance.RestartScene();

    public void LoadData(GameData _data)
    {
        foreach (KeyValuePair<string, float> pair in _data.volumeSettings)
        {
            foreach (UI_VolumeSlider item in volumeSettings)
            {
                if (item.parametr == pair.Key)
                    item.LoadSlider(pair.Value);
            }
        }
    }

    public void SaveData(ref GameData _data)
    {
        _data.volumeSettings.Clear();

        foreach (UI_VolumeSlider item in volumeSettings)
        {
            _data.volumeSettings.Add(item.parametr, item.slider.value);
        }
    }
}
