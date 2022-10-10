using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Utility;

public class PlayerInteraction : MonoBehaviour
{
    private Animator animator;
    private PlayerStats playerStats;
    public bool animation1Finished = false;
    public bool animation2Finished = false;
    [SerializeField] private SFXManager sFXManager;

    [SerializeField] GameObject equipGUI;

    [Header("Key Binding")]
    [SerializeField] private KeyCode takeItemInput;
    [SerializeField] private KeyCode dropItemInput;
    [SerializeField] private KeyCode attackInput;

    [Header("Weapon Positioning")]
    [SerializeField] private Transform weaponPositionL;
    [SerializeField] private Transform weaponPositionR;

    [Header("Attacking")]
    [SerializeField] private GameObject starterWeapon;
    [SerializeField] private SphereCollider attackRangeDetection;

    [Header("Displayed for Debug Purpose")]
    [SerializeField] private WeaponProperty usedWeapon;
    [SerializeField] private List<GameObject> pickupTargets;
    [SerializeField] private List<GameObject> enemyTargets;

    // attack property
    [SerializeField] private bool attack = false;
    [SerializeField] private bool attackAgain = false;
    [SerializeField] private bool cooldown = false;
    [SerializeField] private float startCooldown;
    [SerializeField] private int lastAttackCount = 0;

    public void AddInteractableTarget(GameObject target)
    {
        pickupTargets.Add(target);
        if(pickupTargets.Count > 0) equipGUI.SetActive(true);
    }
    public void RemoveInteractableTarget(GameObject target)
    {
        pickupTargets.Remove(target);
        if (pickupTargets.Count == 0) equipGUI.SetActive(false);
    }

    public void AddEnemyTarget(GameObject target)
    {
        if(!enemyTargets.Contains(target))
            enemyTargets.Add(target);
    }

    public void RemoveEnemyTarget(GameObject target) => enemyTargets.Remove(target);

    private void Awake()
    {
        pickupTargets = new List<GameObject>();
        enemyTargets = new List<GameObject>();
        animator = GetComponent<Animator>();
        usedWeapon = starterWeapon.GetComponent<WeaponProperty>();
        playerStats = GetComponent<PlayerStats>();
        equipGUI.GetComponentInChildren<TextMeshProUGUI>().text = string.Format("Press {0} to equip the item", takeItemInput.ToString());
        equipGUI.SetActive(false);
    }

    private void Update()
    {
        if (Cursor.visible) return;
        // take item region
        #region TAKE ITEM
        if (Input.GetKeyDown(takeItemInput))
        {
            if(usedWeapon.Index != starterWeapon.GetComponent<WeaponProperty>().Index)
            {
                DropWeapon(usedWeapon.gameObject);
            }
            foreach (GameObject item in pickupTargets)
            {
                sFXManager.PlayEquip();
                GameObject wield = Instantiate(item, weaponPositionR);
                wield.GetComponent<WeaponFloating>().enabled = false;
                SetWieldProperty(ref wield);
                Destroy(item);
                RemoveInteractableTarget(item);
                break;
            }
        }
        #endregion

        // drop item region
        #region DROP ITEM
        if(Input.GetKeyDown(dropItemInput))
        {
            if(usedWeapon.Index != starterWeapon.GetComponent<WeaponProperty>().Index) DropWeapon(usedWeapon.gameObject);
        }
        #endregion

        // attack Region
        #region ATTACK
        attackAgain = animator.GetBool("AttackAgain");

        // if the attack isn't in cooldown and player isn't sliding or rolling, we can attack
        if(!cooldown && !animator.GetBool("Roll") && !animator.GetBool("Slide"))
        {
            // receive attack input
            if (Input.GetKeyDown(attackInput))
            {
                // if the character haven't attack yet, start attack animation
                if (attack == false)
                {
                    Attack();
                    sFXManager.PlayAttack();
                    attack = true;
                    animator.SetTrigger("Attack");
                }
                // if it have, then give queue to attack again
                else
                {
                    attackAgain = true;
                }
            }

            // if we give the attack command and the attack animation is over
            if (attack && (animation1Finished || animation2Finished))
            {
                // 1. set cooldown
                lastAttackCount++;
                attack = false;
                cooldown = true;
                startCooldown = Time.time;

                // if attack again, cancel the cooldown and the attack change state
                if (attackAgain)
                {
                    Attack();
                    if (lastAttackCount < usedWeapon.AttackTimes) sFXManager.PlayAttack();
                    attack = true;
                    cooldown = false;
                    attackAgain = false;
                }

                // if the attack count exceeds the maximum amount, force stop the further attack attempt
                if (lastAttackCount >= usedWeapon.AttackTimes)
                {
                    cooldown = true;
                    attackAgain = false;
                    attack = false;
                    startCooldown = Time.time;
                }
            }
        }
        else if (cooldown && Time.time - startCooldown >= usedWeapon.AttackCooldown)
        {
            lastAttackCount = 0;
            cooldown = false;
        }
        animator.SetBool("AttackAgain", attackAgain);
        #endregion
    }

    private void DropWeapon(GameObject weapon)
    {
        GameObject drop = Instantiate(weapon, weaponPositionR.position, weaponPositionR.rotation);
        SetWorldProperty(ref drop);
        Destroy(weapon);
        usedWeapon = starterWeapon.GetComponent<WeaponProperty>();
        animator.SetInteger("WeaponIndex", usedWeapon.Index);
    }

    private void SetWorldProperty(ref GameObject @object)
    {
        Rigidbody rb = @object.GetComponent<Rigidbody>();
        rb.useGravity = true;
        rb.isKinematic = false;

        MeshCollider col = @object.GetComponent<MeshCollider>();

        col.isTrigger = false;
        col.enabled = true;
        @object.tag = "Weapon";
    }

    private void SetWieldProperty(ref GameObject @object)
    {
        WeaponProperty weaponProperty = @object.GetComponent<WeaponProperty>();
        attackRangeDetection.radius = weaponProperty.Range;
        usedWeapon = weaponProperty;
        animator.SetInteger("WeaponIndex", usedWeapon.Index);

        if(!weaponProperty.Fist)
        {
            Rigidbody rb = @object.GetComponent<Rigidbody>();
            rb.useGravity = false;
            rb.isKinematic = true;

            MeshCollider col = @object.GetComponent<MeshCollider>();

            col.isTrigger = true;
            col.enabled = false;

            @object.transform.localPosition = Vector3.zero;
            @object.transform.localRotation = Quaternion.identity;
            @object.tag = "Untagged";
        }
    }

    private void Attack()
    {
        if (animator.GetBool("Dead")) return;
        if (usedWeapon.Fist && enemyTargets.Count > 0) sFXManager.PlayPunchHit();
        if (!usedWeapon.Fist && enemyTargets.Count == 0) sFXManager.PlaySwordWoosh();
        if (!usedWeapon.Fist && enemyTargets.Count > 0) sFXManager.PlaySwordHit();

        List<GameObject> diedTargets = new List<GameObject>();
        foreach (GameObject target in enemyTargets)
        {
            EnemyStats enemy = target.GetComponent<EnemyStats>();
            enemy.ReceiveDamage((int) (usedWeapon.Damage * playerStats.AttackMultiplier));
            Debug.Log(enemy.HP + "/" + enemy.MaxHP);
            if (!enemy.Alive) diedTargets.Add(target);
        }
        foreach (GameObject enemy in diedTargets)
        {
            enemy.GetComponent<EnemyInteraction>().PlayDeathAudio();
            playerStats.AddXP(enemy.GetComponent<EnemyStats>().XP);
            enemyTargets.Remove(enemy);
            Animator temp = enemy.GetComponent<Animator>();
            temp.SetBool("Death", true);
            
            StartCoroutine(UtilityScript.DoAction(3, () =>
            {
                enemy.transform.parent = null;
                enemy.SetActive(false);
            }));
        }

        if (playerStats.IsLevelUp) sFXManager.PlayLevelUp();
    }
}
