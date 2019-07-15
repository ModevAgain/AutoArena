using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

//[CreateAssetMenu(fileName = "RobotAttackData", menuName = "Data/RobotAttackData")]
public class RobotAttack : EnemyBaseAttack
{
    public float AttackLength;
    public float RollSpeed;
    public float TelegraphTime;
    public float PrepareDelay;
    public GameObject RollAttackCollider;
    private Sequence _rollSequence;

    public void StartAttack(Vector3 origin, Vector3 target, AI_Robot robot)
    {
        StartRollAttack(robot, (target - origin).normalized);
        base.StartAttack(origin, target);
    }

    public void StartRollAttack(AI_Robot robot, Vector3 dir)
    {
        _rollSequence = DOTween.Sequence();

        GameObject colObj = Instantiate<GameObject>(RollAttackCollider, robot.transform);
        colObj.GetComponent<ColliderEventCatcher>().TriggerEnter = (other) =>
        {
            if (other.tag == "Player")
            {
                other.GetComponent<PlayerData>().GetDamage(Damage);

                robot.EndRollAttack();
                Destroy(colObj);

                _rollSequence.Kill();

            }
        };

        _rollSequence.SetDelay(PrepareDelay);

        _rollSequence.OnKill(() =>
        {
            robot.Telegraph.transform.DOScaleZ(0, 0);
            robot.EndRollAttack();
        });

        _rollSequence.AppendCallback(() => robot.AttackInProgress = true);
        _rollSequence.Append(robot.Telegraph.transform.DOScaleZ(AttackLength, TelegraphTime));
        _rollSequence.Insert(TelegraphTime - 0.2f, robot.Telegraph.Mat.DOColor(new Color(1, 0, 0, 0), 0.2f));
        _rollSequence.Append(robot.Telegraph.transform.DOScaleZ(0, 0));
        _rollSequence.Append(robot.transform.DOMove(robot.transform.position + dir * AttackLength, RollSpeed).SetEase(Ease.Linear));
        _rollSequence.Join(DOVirtual.DelayedCall(RollSpeed * 0.9f, () =>
        {
            robot.EndRollAttack();
            Destroy(colObj);
        }));

    }

}
