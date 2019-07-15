using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

public class StatsVisualManager : MonoBehaviour
{
    public Animator StatsPlayerAnimator;

    public RawImage CharacterImage;

    public TextMeshPro Text_AttackSpeed;
    public TextMeshPro Text_MoveSpeed;
    public TextMeshPro Text_Health;
    public TextMeshPro Text_Damage;


    private string _contentAttackSpeed;
    private string _contentMoveSpeed;
    private string _contentHealth;
    private string _contentDamage;

    private PlayerData.Stats _stats;

    private void Awake()
    {
        _contentAttackSpeed = Text_AttackSpeed.text;
        _contentMoveSpeed = Text_MoveSpeed.text;
        _contentHealth = Text_Health.text;
        _contentDamage = Text_Damage.text;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void ShowStats()
    {
        Sequence seq = DOTween.Sequence();

        seq.Append(CharacterImage.DOFade(1, 0.5f));
        seq.Append(Text_AttackSpeed.DOFade(1, 0.18f));
        seq.Append(Text_MoveSpeed.DOFade(1, 0.18f));
        seq.Append(Text_Health.DOFade(1, 0.18f));
        seq.Append(Text_Damage.DOFade(1, 0.18f));

    }

    public void HideStats()
    {
        Sequence seq = DOTween.Sequence();

        seq.Append(CharacterImage.DOFade(0, 0));
        seq.Append(Text_AttackSpeed.DOFade(0, 0));
        seq.Append(Text_MoveSpeed.DOFade(0, 0));
        seq.Append(Text_Health.DOFade(0, 0));
        seq.Append(Text_Damage.DOFade(0, 0));
    }

    public void SetStats(PlayerData.Stats stats)
    {
        _stats = stats;


        string temp_aS = _contentAttackSpeed.Replace("§", "" + _stats.AttackSpeed);
        string temp_mS = _contentMoveSpeed.Replace("§", "" + _stats.MoveSpeed);
        string temp_h = _contentHealth.Replace("§", "" + _stats.MaxHealth);
        string temp_d = _contentDamage.Replace("§", "" + _stats.Damage);

        Text_AttackSpeed.text = temp_aS;
        Text_MoveSpeed.text = temp_mS;
        Text_Health.text = temp_h;
        Text_Damage.text = temp_d;
    }

    public void ShowBonus(ItemData.ItemType type, float value)
    {
        SetStats(_stats);

        if(type == ItemData.ItemType.ATTACKSPEED)
        {
            Text_AttackSpeed.text = Text_AttackSpeed.text + "<color=green> (+" + value + ")";
        }
        else if (type == ItemData.ItemType.MOVESPEED)
        {
            Text_MoveSpeed.text = Text_MoveSpeed.text + "<color=green> (+" + value + ")";
        }
        else if (type == ItemData.ItemType.HEALTH)
        {
            Text_Health.text = Text_Health.text + "<color=green> (+" + value + ")";
        }
        else if (type == ItemData.ItemType.DAMAGE)
        {
            Text_Damage.text = Text_Damage.text + "<color=green> (+" + value + ")";
        }
    }

    public void ApplyBonus(PlayerData.Stats stats)
    {
        SetStats(stats);
        StatsPlayerAnimator.SetTrigger("End");
    }
}
