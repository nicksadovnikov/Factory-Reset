﻿using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Ubh random lock on shot.
/// </summary>
[AddComponentMenu("UniBulletHell/Shot Pattern/Random Shot (Lock On)")]
public class UbhRandomLockOnShot : UbhRandomShot
{
    [Header("===== RandomLockOnShot Settings =====")]
    // "Set a target with tag name."
    [FormerlySerializedAs("_SetTargetFromTag")]
    public bool m_setTargetFromTag = true;
    // "Set a unique tag name of target at using SetTargetFromTag."
    [FormerlySerializedAs("_TargetTagName")]
    public string m_targetTagName = "Player";
    // "Flag to randomly select from GameObjects of the same tag."
    public bool m_randomSelectTagTarget;
    // "Transform of lock on target."
    // "It is not necessary if you want to specify target in tag."
    // "Overwrite RandomCenterAngle in direction of target to Transform.position."
    [FormerlySerializedAs("_TargetTransform")]
    public Transform m_targetTransform;
    // "Always aim to target."
    [FormerlySerializedAs("_Aiming")]
    public bool m_aiming;

    public override void Shot()
    {
        if (m_shooting)
        {
            return;
        }

        AimTarget();

        if (m_targetTransform == null)
        {
            Debug.LogWarning("Cannot shot because TargetTransform is not set.");
            return;
        }

        base.Shot();

        if (m_aiming)
        {
            StartCoroutine(AimingCoroutine());
        }
    }

    private void AimTarget()
    {
        if (m_targetTransform == null && m_setTargetFromTag)
        {
            m_targetTransform = UbhUtil.GetTransformFromTagName(m_targetTagName, m_randomSelectTagTarget);
        }
        if (m_targetTransform != null)
        {
            m_randomCenterAngle = UbhUtil.GetAngleFromTwoPosition(transform, m_targetTransform, shotCtrl.m_axisMove);
        }
    }

    private IEnumerator AimingCoroutine()
    {
        while (m_aiming)
        {
            if (m_shooting == false)
            {
                yield break;
            }

            AimTarget();

            yield return null;
        }

        yield break;
    }
}