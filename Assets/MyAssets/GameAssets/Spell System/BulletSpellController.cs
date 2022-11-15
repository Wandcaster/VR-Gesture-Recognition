using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR.InteractionSystem;

public class BulletSpellController : MonoBehaviour
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
    [SerializeField]
    string gestureName;
    [SerializeField]
    private List<Image> ammoImages;
    private int currentAmmo;
    [SerializeField]
    float lifeTime;
    public void OnRecognition(List<RecognizeOutput> recognizeOutputs)
    {
        if (recognizeOutputs[0].recognizedGesture.gestureName != gestureName) return;
        Reload();
        CreateBullet();
    }

    private void ShootBullet(GameObject temp)
    {
        Vector3 velocitie;
        Vector3 anguarVelocitie;
        Player.instance.hands[0].GetEstimatedPeakVelocities(out velocitie,out anguarVelocitie);
        temp.transform.parent = null;
        temp.GetComponent<Rigidbody>().useGravity = true;
        temp.GetComponent<Rigidbody>().AddForce(-spawnPoint.up * velocityMultipler* velocitie.magnitude);
        isShooted = true;
    }
    private void Update()
    {
        if (!isShooted && bullet != null && (Camera.main.transform.position-Player.instance.hands[0].transform.position).magnitude > distanceToActive)
        {
            ShootBullet(bullet);
        }
        else
        {
            if(isShooted&&currentAmmo>0&& (Camera.main.transform.position - Player.instance.hands[0].transform.position).magnitude < distanceToActive)
            {
                CreateBullet();
            }
        }
    }

    private void CreateBullet()
    {
        isShooted = false;
        ammoImages[currentAmmo - 1].color = Color.gray;
        currentAmmo--;
        bullet = Instantiate(bulletPrefab, spawnPoint.position, Quaternion.identity, spawnPoint);
        Destroy(bullet, lifeTime);
    }

    private void Reload()
    {
        bullet = null;
        currentAmmo = ammoImages.Count;
        foreach (var item in ammoImages)
        {
            item.color = Color.white;
        }
    }
}
