using UnityEngine;

// 대부분의 행위에 대한 템플릿 클래스
public class AgentBehavior : MonoBehaviour
{
    public GameObject target;
    protected Agent agent;

    public float maxSpeed;
    public float maxAccel;
    public float maxRotation;
    public float maxAngularAccel;

    public virtual void Awake()
    {
        agent = gameObject.GetComponent<Agent>();
    }
    public virtual void Update()
    {
        agent.SetSteering(GetSteering());
    }
    public  virtual Steering GetSteering()
    {
        return new Steering();
    }
    /// <summary>
    /// 두 방향 값을 뺀 후 실제 회전 방향을 찾는다.
    /// </summary>
    /// <param name="rotation">두 방향 값을 뺀 실제 회전 방향</param>
    /// <returns></returns>
    public float MapToRange(float rotation)
    {
        rotation %= 360.0f;
        if (Mathf.Abs(rotation) > 180.0f)
        {
            if (rotation < 0.0f)
                rotation += 360.0f;
            else
                rotation -= 360.0f;
        }
        return rotation;
    }
}
