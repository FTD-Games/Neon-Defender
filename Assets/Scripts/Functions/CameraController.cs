using UnityEngine;

public class CameraController : MonoBehaviour
{
    private GameObject _followTarget;
    private bool _isFollowing;
    public float moveSpeed;
    public float rotationSpeed;
    private Vector3 _velocity = Vector3.zero;

    private void FixedUpdate()
    {
        if (!_isFollowing) return;

        Vector3 camPos = _followTarget.transform.position;
        Quaternion camRot = _followTarget.transform.rotation;

        // set camera to position
        var targetPos = Vector3.SmoothDamp(transform.position, camPos, ref _velocity, moveSpeed * Time.deltaTime);
        targetPos.z = transform.position.z;
        // set cameras rotation
        var rotation = Quaternion.Slerp(transform.rotation, camRot, rotationSpeed * Time.deltaTime);
        // set values
        transform.SetPositionAndRotation(targetPos, rotation);
    }

    /// <summary>
    /// Sets a new target the camera has to follow.
    /// </summary>
    public void SetNewTarget(GameObject newTarget) => _followTarget = newTarget;

    /// <summary>
    /// Start or stop the following of the camera.
    /// </summary>
    public void ControlFollowing(bool isFollowing) => _isFollowing = isFollowing;
}
