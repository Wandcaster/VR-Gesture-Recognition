    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using VRGesureRecognition;

    public class MagicRopeController : MonoBehaviour
    {
        public GameObject object1;
        public GameObject object2;

        ConfigurableJoint joint;
        private LineRenderer lineRenderer;
        public int segments = 20;
        public float radius = 0.1f;
        public float frequency = 1.0f;
        public float amplitude = 1.0f;
        float ropeLength;
        float tempDistance;
        private Coroutine lineRendererFormWandToObject;
        [SerializeField] private float recognitionThreshold;

        [SerializeField] private string gestureName;


        [SerializeField] private WandModel wandModel;
        private void Start()
        {
            lineRenderer = GetComponent<LineRenderer>();
        }
        void Init()
        {
            lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.positionCount = segments + 1;
            ropeLength = Vector3.Distance(object1.transform.position, object2.transform.position);
            joint = object1.AddComponent<ConfigurableJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedBody = object2.GetComponent<Rigidbody>();
            joint.anchor = Vector3.zero;
            joint.connectedAnchor = Vector3.zero;
            joint.xMotion = ConfigurableJointMotion.Limited;
            joint.yMotion = ConfigurableJointMotion.Limited;
            joint.zMotion = ConfigurableJointMotion.Limited;
            joint.angularXMotion = ConfigurableJointMotion.Limited;
            joint.angularYMotion = ConfigurableJointMotion.Limited;
            joint.angularZMotion = ConfigurableJointMotion.Limited;
            joint.linearLimit = new SoftJointLimit() { limit = ropeLength };
            joint.enableCollision = true;
        }
        void Update()
        {
            if (object1 == null || object2 == null) return; 
            tempDistance = Vector3.Distance(object1.transform.position, object2.transform.position);
            amplitude = Mathf.InverseLerp(ropeLength,0,tempDistance);
            if (object1!=null&&object2!=null)
            // Calculate the position of each segment in the rope
            for (int i = 1; i <= segments+1; i++)
            {
                float t = (float)i / (float)segments;
                Vector3 point1 = object1.transform.position;
                Vector3 point2 = object2.transform.position;
                Vector3 point = Vector3.Lerp(point1, point2, t);
                point += new Vector3(0, -Mathf.Sin(t * Mathf.PI * frequency) * amplitude, 0);
                lineRenderer.SetPosition(i-1, point);
            }
        }
        public bool CastMagicRope(List<RecognizeOutput> recognizeOutputs)
        {
            if (!recognizeOutputs[0].recognizedGesture.gestureName.Equals(gestureName) || recognizeOutputs[0].probability > recognitionThreshold) return false;
            if (object1!= null && object2 != null|| wandModel.colliders.Count == 0) { object1= null; object2 = null; lineRenderer.positionCount = 0; Destroy(joint); }
            if (object1 == null)
            {
            if (wandModel.colliders[0].gameObject == null) return false;
                object1 = wandModel.colliders[0].gameObject;
                lineRendererFormWandToObject = StartCoroutine(LineFromObjectToWand());
                return true;
            }
            else
            {
                object2 = wandModel.colliders[0].gameObject;
            if (object1 == object2 || object1 == null || object2 == null)
            {
                object2 = null;
                return false;
            }
                StopCoroutine(lineRendererFormWandToObject);
                Init();
                return true;
            }
        }
        public IEnumerator LineFromObjectToWand()
        {
            while(true)
            {
                lineRenderer.positionCount = 2;
                lineRenderer.SetPosition(0, object1.transform.position);
                lineRenderer.SetPosition(1, wandModel.tip.position);
                yield return null;
            }
        }


    }
