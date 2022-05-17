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
    // 물리
    private Rigidbody aRigidbody;

    private void Start()
    {
        velocity = Vector3.zero;
        steering = new Steering();

        aRigidbody = GetComponent<Rigidbody>();
    }
    public void SetSteering(Steering steering, float weight)
    {
        this.steering.linear += (weight * steering.linear);
        this.steering.angular += (weight * steering.angular);
    }
    // 현재 값에 따라 이동 뼈대
    public virtual void Update()
    {
        if (aRigidbody == null)
            return;
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
        if (aRigidbody == null)
            return;
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

        // 물리 엔진 위에서 작업하고 있기 때문에
        // 오브젝트를 임의로 변환하는 대신 강체에 힘을 적용.
        Vector3 displacement = velocity * Time.deltaTime;
        orientation += rotation * Time.deltaTime;
        if (orientation < 0.0f)
            orientation += 360.0f;
        else if (orientation > 360.0f)
            orientation -= 360.0f;
        // 무엇을 하고 싶은지에 따라 ForceMode 값 설정
        // 여기서는 보여주는 용도로 VelocityChange를 사용.
        aRigidbody.AddForce(displacement, ForceMode.VelocityChange);
        Vector3 orientationVector = OriToVec(orientation);
        aRigidbody.rotation = Quaternion.LookRotation(orientationVector, Vector3.up);
    }
    public Vector3 OriToVec(float orientation)
    {
        Vector3 vector = Vector3.zero;
        vector.x = Mathf.Sin(orientation * Mathf.Deg2Rad) * 1.0f;
        vector.y = Mathf.Cos(orientation * Mathf.Deg2Rad) * 1.0f;
        return vector.normalized;
    }
}
