using UnityEngine;
// 주요 컴퍼넌트
// 지능적인 움직임을 만들기 위한 행위들을 활용
public class Agent : MonoBehaviour
{
    public float maxSpeed;
    public float maxAccel;
    public float orientation;
    public float rotation;
    public Vector3 velocity;

    protected Steering steering;

    private void Start()
    {
        velocity = Vector3.zero;
        steering = new Steering();
    }
    public void SetSteering(Steering steering)
    {
        this.steering = steering;
    }
    // 현재 값에 따라 이동 뼈대
    public virtual void Update()
    {
        Vector3 displacement = velocity * Time.deltaTime;
        orientation += rotation * Time.deltaTime;
        // 회전 값들의 범위를 0에서 360사이로 제한해야 함
        if(orientation < 0.0f)
            orientation += 360.0f;
        else if( orientation > 360.0f)
            orientation -= 360.0f;
        transform.Translate(displacement, Space.World);
        transform.rotation = new Quaternion();
        transform.Rotate(Vector3.up, orientation);
    }
    // 현재 프레임의 계산에 따라 다음 프레임의 움직임 갱신.
    public virtual void LateUpdate()
    {
        velocity += steering.linear * Time.deltaTime;
        rotation += steering.angular * Time.deltaTime;
        if(velocity.magnitude > maxSpeed)
        {
            velocity.Normalize();
            velocity = velocity * maxSpeed;
        }
        if (steering.angular == 0.0f)
        {
            rotation = 0.0f;
        }
        if(steering.linear.sqrMagnitude == 0.0f)
        {
            velocity = Vector3.zero;
        }
        steering = new Steering();
    }
}
