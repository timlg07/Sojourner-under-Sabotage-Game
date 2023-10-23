using UnityEngine;
using System.Collections;

namespace CreativeSpore.RpgMapEditor
{
    [AddComponentMenu("RpgMapEditor/AI/CompanionAI", 10)]
    [RequireComponent(typeof(PhysicCharBehaviour))]
    [RequireComponent(typeof(DirectionalAnimation))]
    [RequireComponent(typeof(IsoSpriteController))]
    public class CustomCompanionAI : MonoBehaviour
    {
        [Tooltip("Maximum waiting time between steps")]
        public float MaxStepDelay = 2f;
        [Tooltip("Minimum waiting time between steps")]
        public float MinStepDelay = 0.5f;
        [Tooltip("Distance (white) covered in each step")]
        public float MovingStepDist = 1f;
        [Tooltip("Moving Speed")]
        public float MovingSpeed = 1f;
        [Tooltip("Distance (yellow) the companion can see the player")]
        public float SightDistance = 1f;
        [Tooltip("Distance (green) in which the companion moves slower")]
        public float SlowMoveDistance = 1f;
        [Tooltip("Distance (red) in which the companion does not move")]
        public float NoMoveDistance = 1f;
        [Tooltip("If true, the companion flees from player instead following him")]
        public bool FleeFromTarget = false;
        [Tooltip("The companion will follow or flee from Target. If null, target will be the first player found.")]
        public GameObject Target;

        private PhysicCharBehaviour m_physicBhv;
        private DirectionalAnimation m_dirAnim;
        void Start()
        {
            m_physicBhv = GetComponent<PhysicCharBehaviour>();
            m_dirAnim = GetComponent<DirectionalAnimation>();
            StopAllCoroutines();
            StartCoroutine(DoLogic());
        }

#if UNITY_EDITOR
        void OnDrawGizmosSelected()
        {
            var transform0 = transform;
            var rotation = transform0.rotation;
            var position = transform0.position;
            UnityEditor.Handles.color = Color.white;
            EditorCompatibilityUtils.CircleCap(0, position, rotation, MovingStepDist);
            UnityEditor.Handles.color = Color.yellow;
            EditorCompatibilityUtils.CircleCap(0, position, rotation, SightDistance);
            UnityEditor.Handles.color = Color.green;
            EditorCompatibilityUtils.CircleCap(0, position, rotation, SlowMoveDistance);
            UnityEditor.Handles.color = Color.red;
            EditorCompatibilityUtils.CircleCap(0, position, rotation, NoMoveDistance);
            UnityEditor.Handles.color = Color.white;
        }
#endif

        void OnEnable()
        {
            Start();            
        }

        void OnDisable()
        {
            StopCoroutine(DoLogic());
        }

        void DoCollision(Vector2 normal)
        {
            Vector2 vReflect = Vector3.Reflect(m_velocity, normal);
            m_velocity = Vector2.Lerp(m_velocity, vReflect, Random.value);
        }

        void OnCollisionEnter(Collision other)
        {
            if(other.contacts.Length > 0)
                DoCollision(other.contacts[0].normal);
        }

        void OnCollisionEnter2D(Collision2D other)
        {
            if (other.contacts.Length > 0)
                DoCollision(other.contacts[0].normal);
        }

        Vector2 m_velocity;
        IEnumerator DoLogic()
        {
            m_physicBhv.Dir = Vector2.zero;
            m_dirAnim.IsPlaying = false;
            yield return new WaitForSeconds(Random.Range(MinStepDelay, MaxStepDelay));
            while (true)
            {
                var MaxActualDelay = MaxStepDelay;
                var MinActualDelay = MinStepDelay;
                m_velocity = Random.rotation * Vector2.right;
                m_velocity = m_velocity.normalized * MovingStepDist;
                if (!Target) Target = FindObjectOfType<PlayerController>().gameObject;
                if (Target)
                {
                    Vector2 vDist = FleeFromTarget ? transform.position - Target.transform.position : Target.transform.position - transform.position;
                    
                    if (vDist.magnitude <= NoMoveDistance)
                    {
                        m_dirAnim.IsPlaying = false;
                        yield return new WaitForSeconds(.25f);
                        continue;
                    }
                    m_dirAnim.IsPlaying = true;
                    
                    if (vDist.magnitude <= SightDistance)
                    {
                        m_velocity = Vector2.Lerp(vDist, m_velocity, Random.value / 8f);
                        
                        // if the companion is close to the player, it will move less often
                        MaxActualDelay = Mathf.Lerp(MaxStepDelay, 0, vDist.magnitude / SlowMoveDistance);
                        MinActualDelay = Mathf.Lerp(MinStepDelay, 0, vDist.magnitude / SlowMoveDistance);
                        Debug.Log("MaxActualDelay: " + MaxActualDelay + " MinActualDelay: " + MinActualDelay);
                    }
                }
                for (float dist = 0; dist < MovingStepDist; )
                {
                    yield return new WaitForFixedUpdate();
                    if (m_physicBhv.CollFlags != 0)
                    {
                        switch (m_physicBhv.CollFlags)
                        {
                            case PhysicCharBehaviour.eCollFlags.DOWN: DoCollision(Vector2.up); break;
                            case PhysicCharBehaviour.eCollFlags.UP: DoCollision(-Vector2.up); break;
                            case PhysicCharBehaviour.eCollFlags.RIGHT: DoCollision(-Vector2.right); break;
                            case PhysicCharBehaviour.eCollFlags.LEFT: DoCollision(Vector2.right); break;
                        }
                    }
                    Debug.DrawRay(transform.position, m_velocity);
                    Vector2 disp = m_velocity.normalized * MovingSpeed;
                    dist += disp.magnitude * Time.deltaTime;
                    m_physicBhv.MaxSpeed = MovingSpeed;
                    m_physicBhv.Dir = disp;
                    //transform.position += (Vector3)disp;
                    if (Time.frameCount % 4 == 0)
                    {
                        m_dirAnim.SetAnimDirection(disp);
                    }
                }
                m_physicBhv.Dir = Vector2.zero;
                m_dirAnim.IsPlaying = false;
                yield return new WaitForSeconds(Random.Range(MinActualDelay, MaxActualDelay));
            }
        }
    }
}
