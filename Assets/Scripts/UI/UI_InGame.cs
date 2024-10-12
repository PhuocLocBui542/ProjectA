
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_InGame : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private Slider slider;

    [SerializeField] private Image dashImage;
    [SerializeField] private Image crystalImage;
    [SerializeField] private Image blackholeImage;
    [SerializeField] private Image flaskImage;

    private SkillManager skills;

    [Header("Souls info")]
    [SerializeField] private TextMeshProUGUI currentExpPoint;
    [SerializeField] private float soulsAmount;
    [SerializeField] private float increaseRate = 100;
    void Start()
    {
        if (playerStats != null)
            playerStats.onHealthChanged += UpdateHealthUI;

        skills = SkillManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSoulsUI();

        if (Input.GetKeyDown(KeyCode.Z) && skills.dash.dashUnlocked)
            SetCooldownOf(dashImage);
        if (Input.GetKeyDown(KeyCode.A) && skills.crystal.crystalUnlocked)
            SetCooldownOf(crystalImage);
        if (Input.GetKeyDown(KeyCode.V) && skills.blackhole.blackholeUnlocked)
            SetCooldownOf(blackholeImage);
        if (!Input.GetKeyDown(KeyCode.Alpha1) && Inventory.instance.GetEquipment(EquipmentType.Flask) != null)
            SetCooldownOf(flaskImage);

        CheckCDOf(dashImage, skills.dash.CD);
        CheckCDOf(crystalImage, skills.crystal.CD);
        CheckCDOf(blackholeImage, skills.blackhole.CD);
        CheckCDOf(flaskImage, Inventory.instance.flankCD);
    }

    private void UpdateSoulsUI()
    {
        if (soulsAmount < PlayerManager.instance.GetCurrency())
            soulsAmount += Time.deltaTime * increaseRate;
        else
            soulsAmount = PlayerManager.instance.GetCurrency();

        currentExpPoint.text = ((int)soulsAmount).ToString();
    }

    private void UpdateHealthUI()
    {
        slider.maxValue = playerStats.GetMaxHealthValue();
        slider.value = playerStats.currentHealth;

        /*if (playerStats.currentHealth < 0)
            gameObject.SetActive(false);*/
    }

    private void SetCooldownOf(Image _image)
    {
        if (_image.fillAmount <= 0)
            _image.fillAmount = 1;
    }

    private void CheckCDOf(Image _image, float _cd)
    {
        if (_image.fillAmount > 0)
            _image.fillAmount -= 1 / _cd * Time.deltaTime;
    }
}
