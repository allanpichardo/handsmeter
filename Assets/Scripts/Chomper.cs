using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Chomper : MonoBehaviour
{
    private Animator animator;
    private Vector3 awayTarget;
    private bool isMoving;
    private NavMeshAgent navMeshAgent;
    private float rewardTotal;
    private float elapsedTime;
    
    public bool isDebug = true;
    public GameObject player;
    public Text rewardsText;
    
    void Start()
    {
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        RefreshAwayTarget();
        UpdateRewardsText();
    }

    void Update()
    {
        if (isDebug)
        {
            WatchActionKeys();
        }
        WatchRewardKeys();

        TrackTime();
        
        UpdateRewardsText();
    }

    private void TrackTime()
    {
        elapsedTime += Time.deltaTime;
    }

    private void WatchRewardKeys()
    {
        if (Input.GetKeyDown(KeyCode.Equals))
        {
            //give reward
            rewardTotal++;
        }

        if (Input.GetKeyDown(KeyCode.Minus))
        {
            //give punishment
            rewardTotal--;
        }
    }

    private void UpdateRewardsText()
    {
        if (rewardsText != null)
        {
            rewardsText.text = "Rewards: " + rewardTotal;
        }
    }

    private void WatchActionKeys()
    {
        //run towards player
        if (Input.GetKeyDown(KeyCode.S))
        {
            ApproachPlayer(2.0f);
        } else if (Input.GetKeyUp(KeyCode.S))
        {
            Stop();
        }
        //walk towards player
        if (Input.GetKeyDown(KeyCode.A))
        {
            ApproachPlayer(1.0f);
        } else if (Input.GetKeyUp(KeyCode.A))
        {
            Stop();
        }
        //run away from player
        if (Input.GetKeyDown(KeyCode.D))
        {
            RetreatFromPlayer(2.0f);
        } else if (Input.GetKeyUp(KeyCode.D))
        {
            Stop();
        }
        //get angry
        if (Input.GetKeyDown(KeyCode.F))
        {
            SetAggressive(true);
        } else if (Input.GetKeyUp(KeyCode.F))
        {
            SetAggressive(false);
        }
        //get happy
        if (Input.GetKeyDown(KeyCode.G))
        {
            SetHappy(true);
        } else if (Input.GetKeyUp(KeyCode.G))
        {
            SetHappy(false);
        }
        //do flip
        if (Input.GetKeyDown(KeyCode.H))
        {
            DoFlip();
        }
        //get hurt
        if (Input.GetKeyDown(KeyCode.J))
        {
            GetHurt();
        }
    }

    public void LookAtPlayer()
    {
        if (player != null)
        {
            transform.LookAt(player.transform);
        }
    }

    public void GetHurt()
    {
        animator.SetTrigger("getHurt");
    }

    public void DoFlip()
    {
        animator.SetTrigger("doFlip");
    }

    public void ApproachPlayer(float speed)
    {
        if (player != null)
        {
            MoveTowards(player.transform.position, speed);
        }
    }

    public void RetreatFromPlayer(float speed)
    {
        MoveTowards(awayTarget, speed);
    }

    public void Stop()
    {
        isMoving = false;
        animator.SetFloat("speed", 0.0f);
        navMeshAgent.isStopped = true;
        RefreshAwayTarget();
    }

    public void SetAggressive(bool isAggressive)
    {
        animator.SetBool("isHappy", false);
        animator.SetBool("isAggressive", isAggressive);
    }

    public void SetHappy(bool isHappy)
    {
        animator.SetBool("isHappy", isHappy);
        animator.SetBool("isAggressive", false);
    }

    private void MoveTowards(Vector3 target, float speed)
    {
        if (player != null)
        {
            isMoving = true;
            transform.LookAt(target);
            navMeshAgent.speed = speed;
            navMeshAgent.SetDestination(target);
            animator.SetFloat("speed", speed);
            navMeshAgent.isStopped = false;
        }
    }

    private void RefreshAwayTarget()
    {
        awayTarget = new Vector3(Random.Range(-25, 25), Random.Range(-25,25), 0.0f);
        if (awayTarget.Equals(player.transform.position))
        {
            RefreshAwayTarget();
        }
    }
}
