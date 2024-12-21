using UnityEngine;

public class RotateComponent : MonoBehaviour
{
    private float _speed;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, _speed * Time.deltaTime);
    }

    public void SetNewSpeed(float newSpeed) => _speed = newSpeed;
}
