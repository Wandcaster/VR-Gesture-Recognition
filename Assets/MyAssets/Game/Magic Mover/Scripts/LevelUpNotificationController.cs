using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelUpNotificationController : MonoBehaviour
{
    [SerializeField]
    float offset;
    Camera main;
    public TextMeshProUGUI level;
    // Start is called before the first frame update
    void Start()
    {
        main=Camera.main;
        transform.position =main.transform.position+Vector3.up;
        Destroy(gameObject, 25);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position =Vector3.Lerp(transform.position,main.transform.position+ main.transform.forward *offset,Time.deltaTime);
        transform.LookAt(main.transform.position,main.transform.up);

    }
}
