using UnityEngine;
using System.Collections;

public class LaserGun : MonoBehaviour
{

    int ticker = 0;
    public GameObject laserPrefab;
    public Transform laserPos;
    public Animator ani;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        ticker++;
        if (ticker == 80)
        {
            Instantiate(laserPrefab, laserPos.position, Quaternion.identity);
            ani.SetTrigger("shot");
            ticker = 0;
        }
    }
}
