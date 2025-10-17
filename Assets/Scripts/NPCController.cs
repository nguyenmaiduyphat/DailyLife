using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// This is NPC. This NPC have automically move random places that I set up. If NPC is not normal
/// That NPC where player come to get quest and answer to get reward
/// </summary>
public class NPCController : MonoBehaviour
{
    public QuestionType questionType;
    public NPCTYPE nPCTYPE;

    public List<Question> questions;

    NavMeshAgent agent;
    private Animator animator;

    [SerializeField] List<Transform> waypoints;

    public AudioSource audioSource;

    [SerializeField] AudioClip stepClip;
    public AudioClip greetingClip;
    public AudioClip congratulationClip;
    public AudioClip wishGoodLuckClip;
    void Step()
    {
        audioSource.PlayOneShot(stepClip);
    }

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        if (nPCTYPE == NPCTYPE.Quest || nPCTYPE == NPCTYPE.Shop)
        {
            agent.enabled = false;
        }
        else
        {
            agent.avoidancePriority = Random.Range(30, 70);
            agent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
        }
    }

    private void Start()
    {
        if (nPCTYPE == NPCTYPE.Quest)
        {
            questions = Resources.LoadAll<Question>("Quest").ToList();
            questions = questions.Where(q => q.questionType == questionType).ToList();

            animator.SetFloat("Speed", 0f);
        }
        else
        {
            if (waypoints != null && waypoints.Count > 0)
            {
                agent.SetDestination(waypoints[Random.Range(0, waypoints.Count - 1)].position);
            }
        }
    }

    private void Update()
    {
        if (nPCTYPE == NPCTYPE.Quest || nPCTYPE == NPCTYPE.Shop)
        {
            audioSource.volume = SettingData.Instance.gameSetting.voice_Sound / 100;  
        }
        if (nPCTYPE == NPCTYPE.Normal)
        {
            audioSource.volume = SettingData.Instance.gameSetting.step_Sound / 100;
        }
        if (nPCTYPE == NPCTYPE.Quest || nPCTYPE == NPCTYPE.Shop) return;


        if (waypoints == null || waypoints.Count == 0) return;

        animator.SetFloat("Speed", agent.velocity.magnitude);

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            GoToNextPoint();
        }
    }

    private void GoToNextPoint()
    {
        if (waypoints.Count == 0) return;

        agent.SetDestination(waypoints[Random.Range(0, waypoints.Count - 1)].position);
    }
}

public enum NPCTYPE
{
    Normal,
    Quest,
    Shop
}
