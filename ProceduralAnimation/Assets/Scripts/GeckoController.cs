using UnityEngine;

[ExecuteAlways]
public class GeckoController : MonoBehaviour
{
#pragma mark disable 649
    [SerializeField] private Transform target;
    [SerializeField] private Transform headBone;

    [SerializeField] private float headMaxTurnAngle;
    [SerializeField] private float headTrackingSpeed;
#pragma mark restore 649
    
    private void LateUpdate()
    {
        UpdateHeadTracking();
    }

    private void UpdateHeadTracking()
    {
        Quaternion currentLocalRotation = headBone.localRotation;
        headBone.localRotation = Quaternion.identity;

        Vector3 targetWorldLookDir = target.position - headBone.position;
        Vector3 targetLocalLookDir = headBone.InverseTransformDirection(targetWorldLookDir);

        targetLocalLookDir =
            Vector3.RotateTowards(Vector3.forward, targetLocalLookDir, Mathf.Deg2Rad * headMaxTurnAngle, 0);

        // Get the local rotation by using LookRotation on a local directional vector
        Quaternion targetLocalRotation = Quaternion.LookRotation(targetLocalLookDir, Vector3.up);

        // Apply smoothing
        headBone.localRotation = Quaternion.Slerp(
            currentLocalRotation,
            targetLocalRotation,
            1 - Mathf.Exp(-headTrackingSpeed * Time.deltaTime)
        );
    }
}
