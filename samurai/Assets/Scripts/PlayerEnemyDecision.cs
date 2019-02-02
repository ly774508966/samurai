using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnemyDecision : MonoBehaviour
{
    Agent _lastAttackTarget;

    public Agent GetBestTarget()
    {
        return null;
        /*if (Mission.Instance.CurrentGameZone == null)
            return null;

        List<Agent> enemies = Mission.Instance.CurrentGameZone.Enemies;

        float[] EnemyCoeficient = new float[enemies.Count];
        Agent enemy;
        Vector3 dirToEnemy;

        for (int i = 0; i < enemies.Count; i++)
        {
            EnemyCoeficient[i] = 0;
            enemy = enemies[i];

            if (hasToBeKnockdown && enemy.BlackBoard.MotionType != E_MotionType.Knockdown)
                continue;

            if (enemy.BlackBoard.Invulnerable)
                continue;

            dirToEnemy = (enemy.Position - Owner.Position);

            float distance = dirToEnemy.magnitude;

            if (distance > 5.0f)
                continue;

            dirToEnemy.Normalize();

            float angle = Vector3.Angle(dirToEnemy, Owner.Forward);

            if (enemy == LastAttacketTarget)
                EnemyCoeficient[i] += 0.1f;

            //Debug.Log("LastTarget " + Mission.Instance.CurrentGameZone.GetEnemy(i).name + " : " + EnemyCoeficient[i]); 

            EnemyCoeficient[i] += 0.2f - ((angle / 180.0f) * 0.2f);

            //  Debug.Log("angle " + Mission.Instance.CurrentGameZone.GetEnemy(i).name + " : " + EnemyCoeficient[i]);

            angle = Vector3.Angle(dirToEnemy, Owner.BlackBoard.DesiredDirection);
            EnemyCoeficient[i] += 0.5f - ((angle / 180.0f) * 0.5f);
            //    Debug.Log(" joy " + Mission.Instance.CurrentGameZone.GetEnemy(i).name + " : " + EnemyCoeficient[i]); 

            EnemyCoeficient[i] += 0.2f - ((distance / 5) * 0.2f);

            //      Debug.Log(" dist " + Mission.Instance.CurrentGameZone.GetEnemy(i).name + " : " + EnemyCoeficient[i]); 
        }

        float bestValue = 0;
        int best = -1;
        for (int i = 0; i < enemies.Count; i++)
        {
            //     Debug.Log(Mission.Instance.CurrentGameZone.GetEnemy(i).name + " : " + EnemyCoeficient[i]); 
            if (EnemyCoeficient[i] <= bestValue)
                continue;

            best = i;
            bestValue = EnemyCoeficient[i];
        }

        if (best >= 0)
            return enemies[best];

        return null;*/
    }

}
