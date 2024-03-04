using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LaserEnemy : MonoBehaviour
{
    [SerializeField] private LineRenderer _AimLine;
    [SerializeField] private LineRenderer _LaserLine;
    [SerializeField] private Transform _PlayerPos;
    [SerializeField] private Animator _LaserRatAnimator;
    [SerializeField] private int LaserEnemyDamageToPlayer = 1;

    public EnemyHealth _enemyHealth;
    public GameObject _ObjectAimLine;
    public GameObject _ObjectLaserLine;
    public float AgentSpeedPlayerVisable = 0f;
    public float AgentSpeed = 3.5f;

    private FieldOfView _fieldOfView;
    private NavMeshAgent _agent;
    private PlayerHealth _playerHealth;
    private Gamemanager _gameManager;
    private float _aimTime = 1f;
    private float _currentAimTimer = 1f;

    void Start()
    {
        _fieldOfView = GetComponent<FieldOfView>();
        _agent = GetComponent<NavMeshAgent>();
        _playerHealth = FindObjectOfType<PlayerHealth>();
        _enemyHealth = GetComponent<EnemyHealth>();
        _gameManager = FindObjectOfType<Gamemanager>();
    }

    void Update()
    {
        if (_fieldOfView.IsPlayerVisable == true)
        {
            // Player is visible
            _agent.speed = AgentSpeedPlayerVisable; // Stop agent movement
            transform.LookAt(_PlayerPos.position); // Rotate agent to look at the player

            // Set positions for aiming lines
            _AimLine.SetPosition(0, this.gameObject.transform.position);
            _AimLine.SetPosition(1, _PlayerPos.position);

            _LaserRatAnimator.SetBool("shoot", true);

            _currentAimTimer -= Time.deltaTime;

            if (_currentAimTimer <= 0)
            {
                ShootLaser(); // Shoot the laser

                RaycastHit hit;
                if (Physics.Raycast(transform.position, _PlayerPos.position, out hit, Mathf.Infinity))
                {
                    _playerHealth.HurtPlayer(LaserEnemyDamageToPlayer); // Reduce player's health
                    Debug.Log("HIT!");
                }

                _currentAimTimer = _aimTime; // Reset the aim timer
                _LaserRatAnimator.SetBool("shoot", false);
            }
        }
        else
        {
            // Player is not visible
            _agent.speed = AgentSpeed; // Resume agent movement

            // Reset positions for aiming lines
            _AimLine.SetPosition(0, Vector3.zero);
            _AimLine.SetPosition(1, Vector3.zero);
            _ObjectAimLine.SetActive(true);

            // Reset positions for laser line
            _LaserLine.SetPosition(0, Vector3.zero);
            _LaserLine.SetPosition(1, Vector3.zero);

            _LaserRatAnimator.SetBool("shoot", false);
        }
    }

    private void ShootLaser()
    {
        // Reset positions for aiming lines and deactivate object aiming line
        _AimLine.SetPosition(0, Vector3.zero);
        _AimLine.SetPosition(1, Vector3.zero);
        _ObjectAimLine.SetActive(false);

        // Set positions for laser line
        _LaserLine.SetPosition(0, this.gameObject.transform.position);
        _LaserLine.SetPosition(1, _PlayerPos.position);
    }
}
