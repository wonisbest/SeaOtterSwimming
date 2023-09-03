using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using GameControl;
using Random = UnityEngine.Random;
using System;

public class Boss : MonoBehaviour
{
    private int RanObs;
    private string Obstacle;
    private Vector3 ranPos;
    private Quaternion ranRot;
    [SerializeField] private Transform seaOtter;
    [SerializeField] private float startTime, repeatTime;
    [SerializeField] private float delay;

    [SerializeField] private float moveSpeed; // Player �ӵ��� ���� 1.3f

    [SerializeField] private ObstacleSpawner obstacleSpawner;
    [SerializeField] private GameObject Boss_Breathe;
    [SerializeField] private GameObject Boss_Attack;

    // Start is called before the first frame update
    void Start()
    {
        seaOtter = GameObject.Find("SeaOtter").transform;
        moveSpeed = seaOtter.GetComponent<SeaOtterController>().moveSpeed - 1f;

        obstacleSpawner = GameObject.Find("ObstacleSpawner").GetComponent<ObstacleSpawner>();
        obstacleSpawner.InvokeBossAttack();
    }

    // Update is called once per frame
    void Update()
    {
        // �÷��̾�� �ִ� ���� ���� (20f)
        if (seaOtter.position.x - transform.position.x > 45)
        {
            transform.position = new Vector3(seaOtter.position.x - 45, transform.position.y, transform.position.z);
        }
        else
        {
            transform.Translate(Vector3.right * (moveSpeed * Time.deltaTime));
        }
    }

    



    private void OnTriggerEnter2D(Collider2D collision)
    {
        //�÷��̾�� �浹 �� ���� ����
        if (collision.CompareTag("Player"))
        {
            GameManager.Instance.GameOver();
            Time.timeScale = 0f;
        }
    }
}