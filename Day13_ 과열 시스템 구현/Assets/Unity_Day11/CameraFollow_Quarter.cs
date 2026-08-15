using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class CameraFollowBasic : MonoBehaviour
{
    [Header("ƒı≈Õ∫‰")]
    [SerializeField] private Vector3 _quarterOffset = new Vector3(8f, 10f, -8f);
    [SerializeField] private float _quarterLookAtHeight = 1.0f;
    [Min(0f)]
    [SerializeField] private float _quarterSharpness = 1.0f;

    private void InitQuarter(bool snap)
    {
        Vector3 desiredPos;
        Quaternion desiredRot;

        BuildQuarterPose(out desiredPos, out desiredRot);

        ApplyPose(desiredPos, desiredRot, _quarterSharpness, snap);
    }

    private void TickQuarter()
    {
        Vector3 desiredPos;
        Quaternion desiredRot;

        BuildQuarterPose(out desiredPos, out desiredRot);

        ApplyPose(desiredPos, desiredRot, _quarterSharpness, false);
    }

    private void BuildQuarterPose(out Vector3 desiredPos, out Quaternion desiredRot)
    {
        desiredPos = _target.position + _quarterOffset;

        Vector3 lookPos = _target.position + Vector3.up * _quarterLookAtHeight;

        desiredRot = Quaternion.LookRotation(lookPos - desiredPos, Vector3.up);
    }
}
