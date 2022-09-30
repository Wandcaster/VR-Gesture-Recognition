using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellController : MonoBehaviour
{
    [SerializeField]
    float velocityMultipler;
    [SerializeField]
    Transform spawnPoint;
    [SerializeField]
    GameObject bulletPrefab;
    GameObject bullet;
    bool isShooted = false;
    [SerializeField]
    float distanceToActive;
    public void OnRecognition(List<RecognizeOutput> recognizeOutputs)
    {
        //if (recognizeOutputs[0].recognizedGesture.gestureID != 0) return;
        bullet = Instantiate(bulletPrefab, spawnPoint.position, Quaternion.identity, spawnPoint);
        isShooted = false;
    }

    private void ShootBullet(GameObject temp)
    {
        temp.transform.parent = null;
        temp.GetComponent<Rigidbody>().useGravity = true;
        temp.GetComponent<Rigidbody>().AddForce(-spawnPoint.up * velocityMultipler);
    }
    private void Update()
    {
        if (bullet != null&& !isShooted && (Camera.main.transform.position-bullet.transform.position).magnitude > distanceToActive)
        {
            ShootBullet(bullet);
            isShooted = true;
        }
    }
}
