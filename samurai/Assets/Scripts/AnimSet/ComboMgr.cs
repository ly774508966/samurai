using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboMgr : MonoBehaviour
{
    class ComboStep
    {
        public AttackType attackType;     
        public AnimAttackData data;
    }

    class Combo
    {        
        public ComboStep[] comboSteps;
    }

    AnimSetPlayer       _animSetPlayer;
    List<AttackType>    _comboProgress = new List<AttackType>();    // 连续攻击动作类型
    Combo[]             _playerComboAttacks = new Combo[6];         // 6种连招数据

    public void Reset()
    {
        _comboProgress.Clear();
    }

    void Start()
    {
        _animSetPlayer = GetComponent<AnimSetPlayer>();
        _playerComboAttacks[0] = new Combo() // FAST   Raisin Wave
        {            
            comboSteps = new ComboStep[]
            {
                new ComboStep(){attackType = AttackType.X, data = _animSetPlayer.AttackData[0]},
                new ComboStep(){attackType = AttackType.X, data = _animSetPlayer.AttackData[1]},
                new ComboStep(){attackType = AttackType.X, data = _animSetPlayer.AttackData[2]},                                
                new ComboStep(){attackType = AttackType.X, data = _animSetPlayer.AttackData[3]},
                //new ComboStep(){attackType = AttackType.None, data = _animSetPlayer.AttackData[3]},
                new ComboStep(){attackType = AttackType.O, data = _animSetPlayer.AttackData[4]},
            }
        };
        _playerComboAttacks[1] = new Combo() // BREAK BLOCK  half moon
        {         
            comboSteps = new ComboStep[]
            {
                new ComboStep(){attackType = AttackType.O, data = _animSetPlayer.AttackData[5]},
                new ComboStep(){attackType = AttackType.O, data = _animSetPlayer.AttackData[6]},
                new ComboStep(){attackType = AttackType.O, data = _animSetPlayer.AttackData[7]},
                new ComboStep(){attackType = AttackType.X, data = _animSetPlayer.AttackData[8]},
                new ComboStep(){attackType = AttackType.X, data = _animSetPlayer.AttackData[9]},
            }
        };
        _playerComboAttacks[2] = new Combo() // CRITICAL  cloud cuttin
        {            
            comboSteps = new ComboStep[]
            {               
                    new ComboStep(){attackType = AttackType.O, data = _animSetPlayer.AttackData[5]},
                    new ComboStep(){attackType = AttackType.O, data = _animSetPlayer.AttackData[6]},
                    new ComboStep(){attackType = AttackType.X, data = _animSetPlayer.AttackData[17]},
                    new ComboStep(){attackType = AttackType.X, data = _animSetPlayer.AttackData[18]},
                    new ComboStep(){attackType = AttackType.X, data = _animSetPlayer.AttackData[19]},
            }
        };

        _playerComboAttacks[3] = new Combo()  // flying dragon
        {            
            comboSteps = new ComboStep[]
            {
                new ComboStep(){attackType = AttackType.X, data = _animSetPlayer.AttackData[0]},
                new ComboStep(){attackType = AttackType.O, data = _animSetPlayer.AttackData[10]},
                new ComboStep(){attackType = AttackType.O, data = _animSetPlayer.AttackData[11]},
                new ComboStep(){attackType = AttackType.X, data = _animSetPlayer.AttackData[12]},
                new ComboStep(){attackType = AttackType.X, data = _animSetPlayer.AttackData[13]},
            }
        };
        _playerComboAttacks[4] = new Combo() // KNCOK //walking death
        {            
            comboSteps = new ComboStep[]
            {
                new ComboStep(){attackType = AttackType.X, data = _animSetPlayer.AttackData[0]},
                new ComboStep(){attackType = AttackType.X, data = _animSetPlayer.AttackData[1]},
                new ComboStep(){attackType = AttackType.O, data = _animSetPlayer.AttackData[14]},
                new ComboStep(){attackType = AttackType.X, data = _animSetPlayer.AttackData[15]},
                new ComboStep(){attackType = AttackType.X, data = _animSetPlayer.AttackData[16]},
            }
        };

        _playerComboAttacks[5] = new Combo() // HEAVY, AREA  shogun death
        {            
            comboSteps = new ComboStep[]
            {
                new ComboStep(){attackType = AttackType.O, data = _animSetPlayer.AttackData[5]},
                new ComboStep(){attackType = AttackType.X, data = _animSetPlayer.AttackData[20]},
                new ComboStep(){attackType = AttackType.O, data = _animSetPlayer.AttackData[21]},
                new ComboStep(){attackType = AttackType.O, data = _animSetPlayer.AttackData[22]},
                new ComboStep(){attackType = AttackType.O, data = _animSetPlayer.AttackData[23]},
            }
        };
    }    

    public AnimAttackData ProcessCombo(AttackType attackType)
    {
        if (attackType != AttackType.O && attackType != AttackType.X)
            return null;

        _comboProgress.Add(attackType);

        // 遍历每一种连招
        for (int i = 0; i < _playerComboAttacks.Length; ++i)
        {
            Combo combo = _playerComboAttacks[i];
            bool valid = _comboProgress.Count <= combo.comboSteps.Length;

            // 遍历出招记录
            for (int ii = 0; ii < _comboProgress.Count && ii < combo.comboSteps.Length; ++ii)
            {
                if (_comboProgress[ii] != combo.comboSteps[ii].attackType)
                {
                    valid = false;
                    break;
                }
            }

            if (valid)
            {
                combo.comboSteps[_comboProgress.Count-1].data.lastAttackInCombo = (NextAttackIsAvailable(AttackType.X) == false && NextAttackIsAvailable(AttackType.O) == false);
                combo.comboSteps[_comboProgress.Count-1].data.firstAttackInCombo = false;
                combo.comboSteps[_comboProgress.Count-1].data.comboIndex = i;
                combo.comboSteps[_comboProgress.Count-1].data.fullCombo = _comboProgress.Count == combo.comboSteps.Length;
                combo.comboSteps[_comboProgress.Count-1].data.comboStep = _comboProgress.Count;
                
                //GuiManager.Instance.ShowComboProgress(ComboProgress);
                return combo.comboSteps[_comboProgress.Count-1].data;
            }
        }

        _comboProgress.Clear();
        _comboProgress.Add(attackType);

        for (int i = 0; i < _playerComboAttacks.Length; i++)
        {
            if (_playerComboAttacks[i].comboSteps[0].attackType == attackType)
            {
                // Debug.Log(Time.timeSinceLevelLoad + " New combo " + i + " step " + ComboProgress.Count);
                _playerComboAttacks[i].comboSteps[0].data.lastAttackInCombo = false;
                _playerComboAttacks[i].comboSteps[0].data.firstAttackInCombo = true;                
                _playerComboAttacks[i].comboSteps[0].data.comboIndex = i;
                _playerComboAttacks[i].comboSteps[0].data.fullCombo = false;
                _playerComboAttacks[i].comboSteps[0].data.comboStep = 0;

                //GuiManager.Instance.ShowComboProgress(ComboProgress);
                return _playerComboAttacks[i].comboSteps[0].data;
            }
        }

        Debug.LogError("Could not find any combo attack !!! Some shit happens");

        return null;
    }

    bool NextAttackIsAvailable(AttackType attackType)
    {
        if (attackType != AttackType.O && attackType != AttackType.X)
            return false;

        if (_comboProgress.Count == 5)
            return false;

        List<AttackType> progress = new List<AttackType>(_comboProgress);
        progress.Add(attackType);

        for (int i = 0; i < _playerComboAttacks.Length; i++)
        {
            Combo combo = _playerComboAttacks[i];           

            bool valid = true;
            for (int ii = 0; ii < progress.Count; ii++)
            {
                if (progress[ii] != combo.comboSteps[ii].attackType)
                {
                    valid = false;
                    break;
                }
            }

            if (valid)
                return true;
        }
        return false;
    }
}
