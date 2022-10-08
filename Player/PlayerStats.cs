using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private bool showSPAGui;
    [SerializeField] private GameObject sPACanvas;
    [Header("HP Slider Properties")]
    [SerializeField] private TextMeshProUGUI hpTMP;
    [SerializeField] private Slider hpSlider;
    [SerializeField] private Image hpFill;
    [SerializeField] private Color highHPColor = Color.green;
    [SerializeField] private Color halfHPColor = new Color(255, 165, 0);
    [SerializeField] private Color lowHPColor = Color.red;
    [Header("XP Slider Properties")]
    [SerializeField] private TextMeshProUGUI xpTMP;
    [SerializeField] private Slider xpSlider;
    [SerializeField] private Image xpFill;
    [SerializeField] private Color xpColor = Color.yellow;
    [Header("Initialize Stats")]
    [SerializeField] private float maxHealth = 100;
    [SerializeField] private int levelUpXP = 50;
    [SerializeField] private int xpReqIncrease = 10;

    [Header("SPA Stats")]
    [SerializeField] private int skillPoint = 0;
    [SerializeField] private int strength = 10;
    [SerializeField] private int power = 10;
    [SerializeField] private int agility = 10;

    [Header("SPA UI")]
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI strengthText;
    [SerializeField] private TextMeshProUGUI powerText;
    [SerializeField] private TextMeshProUGUI agilityText;
    [SerializeField] private List<Button> buttons;

    [Header("Player Stats")]
    [SerializeField] private float hp;
    [SerializeField] private int xp;
    [SerializeField] private int level;

    private bool levelUp = false;

    // Property
    public int Strength { get => strength; }
    public int Power { get => power; }
    public int Agility { get => agility; }
    public float HP { get => hp; }
    public float MaxHP { get => maxHealth * strength / 10f; }
    public bool Alive { get => hp > 0; }
    public int XP { get => xp; }
    public int LevelUpXP { get => levelUpXP; }
    public int Level { get => level; }
    public float SpeedMultiplier { get => (agility - 10) / 20f + 1; }
    public float AttackMultiplier { get => power / 10f; }
    public bool IsLevelUp
    {
        get
        {
            bool output = levelUp;
            if (levelUp) levelUp = false;
            return output;
        }
    }
    private void Awake()
    {
        hp = MaxHP;
        level = 1;
        ResetUI();
    }
    public void ResetUI()
    {
        hpSlider.minValue = 0;
        hpSlider.maxValue = MaxHP;
        hpSlider.value = hp;

        xpSlider.minValue = 0;
        xpSlider.maxValue = levelUpXP;
        xpSlider.value = xp;

        if (hp / MaxHP > 0.5) hpFill.color = highHPColor;
        else if (hp / MaxHP > 0.2) hpFill.color = halfHPColor;
        else hpFill.color = lowHPColor;
        xpFill.color = xpColor;

        Debug.Log(string.Format("{0} / {1}", hp, MaxHP));
        Debug.Log(string.Format("{0} / {1}", xp, levelUpXP));
        hpTMP.text = string.Format("{0} / {1}", hp, MaxHP);
        xpTMP.text = string.Format("{0} / {1}", xp, levelUpXP);

        // Reset SPA stats
        levelText.text = string.Format("Level {0} ({1} pts)", Level, skillPoint);
        strengthText.text = strength.ToString();
        powerText.text = power.ToString();
        agilityText.text = agility.ToString();

        bool enable = skillPoint > 0;
        sPACanvas.SetActive(enable);
        foreach (Button b in buttons)
        {
            b.enabled = enable;
        }
    }

    public void AddXP(int xp)
    {
        this.xp += xp;
        if (this.xp >= levelUpXP) LevelUp();
        ResetUI();
    }

    private void LevelUp()
    {
        levelUp = true;
        while(xp >= levelUpXP)
        {
            hp = MaxHP;
            xp -= levelUpXP;
            levelUpXP += xpReqIncrease;
            skillPoint++;
            level++;
        }
        ResetUI();
    }

    public void AddHP(int amount)
    {
        hp = System.Math.Max(0, System.Math.Min(hp + amount, MaxHP));
        ResetUI();
    }

    public void AddStrength()
    {
        if (skillPoint <= 0) return;
        strength++;
        skillPoint--;
        ResetUI();
    }

    public void AddPower()
    {
        if (skillPoint <= 0) return;
        power++;
        skillPoint--;
        ResetUI();
    }

    public void AddAgility()
    {
        if (skillPoint <= 0) return;
        agility++;
        skillPoint--;
        ResetUI();
    }
}
