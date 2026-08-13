using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class CameraFollowBasic : MonoBehaviour
{
    #region ¿ŒΩ∫∆Â≈Õ
    [Header("1¿Œƒ™")]
    [SerializeField] private Vector3 _firstOffset = new Vector3(0f, 1.6f, 0.1f);
    [Min(0f)]
    [SerializeField] private float _firstSharpness = 20f;

    [Header("1¿Œƒ™ ø…º«")]
    [SerializeField] private bool _firstUseTargetRotation = true;
    #endregion

    private void InitFirst(bool snap)
    {
        Vector3 desiredPos;
        Quaternion desiredRot;

        BuildFirstPose(out desiredPos, out  desiredRot);

        ApplyPose(desiredPos, desiredRot, _firstSharpness, snap);
    }

    private void TickFirst()
    {
        Vector3 desiredPos;
        Quaternion desiredRot;

        BuildFirstPose(out desiredPos, out desiredRot);

        ApplyPose(desiredPos, desiredRot, _firstSharpness, false);
    }

    private void BuildFirstPose(out Vector3 desiredPos, out Quaternion desiredRot)
    {
        desiredPos = _target.position + (_target.rotation * _firstOffset);

        if (_firstUseTargetRotation)
        {
            desiredRot = _target.rotation;
        }

        else
        {
            desiredRot = _camTr.rotation;
        }

    }

}
