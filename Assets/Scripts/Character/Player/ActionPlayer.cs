using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ActionPlayer : MonoBehaviourPun
{
    public float movingSpeed = 4;
    private Joystick joystick;

    // Start is called before the first frame update
    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (this != null)
        {
            if (photonView.IsMine)
            {
                if (scene.name == "Game")
                {
                    joystick = GameObject.Find("Fixed Joystick").GetComponent<FixedJoystick>();
                }
            }

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            Move();
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Shoot();
            }
        }

       
    }

    private void Move()
    {
        if (joystick != null)
        {
            transform.Translate(0, joystick.Vertical * movingSpeed * Time.deltaTime, 0, Space.World);
            transform.Translate(joystick.Horizontal * movingSpeed * Time.deltaTime, 0, 0, Space.World);
            float angle = Mathf.Atan2(-(joystick.Horizontal), joystick.Vertical);
            transform.rotation = Quaternion.Euler(0f, 0f, angle * Mathf.Rad2Deg);

        }
    }


    public void Shoot()
    {
        // instancier un gameobject bullet
        GameObject bullet = PhotonNetwork.Instantiate("Bullet",this.transform.position, this.transform.rotation );
        float angle = Mathf.Atan2(-(joystick.Horizontal), joystick.Vertical);
        transform.rotation = Quaternion.Euler(0f, 0f, angle * Mathf.Rad2Deg);

        //
    }
}
