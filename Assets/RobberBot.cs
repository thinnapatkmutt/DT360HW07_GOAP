using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class RobberBot : MonoBehaviour
{
    NavMeshAgent agent;
    public GameObject target;
    public GameObject[] hidingSpots;
    public GameObject _bloodParticle;
    public GameObject _knife;
    public GameObject _light;
    public GameObject _cv;
    bool stab = false;

    // Start is called before the first frame update
    void Start()
    {
        _cv.SetActive(false);
        agent = this.GetComponent<NavMeshAgent>();

        hidingSpots = GameObject.FindGameObjectsWithTag("hide");
    }

    void Seek(Vector3 location)
    {
        agent.SetDestination(location);
    }

    #region NotUse
    void Flee(Vector3 location)
    {
        Vector3 targetDir = location- agent.transform.position;
        agent.SetDestination(agent.transform.position - targetDir);
    }

    void Pursue()
    {
        Vector3 targetDir = target.transform.position - agent.transform.position;

        float predictionTime = targetDir.magnitude / (agent.speed + target.GetComponent<Drive>().speed);
        Vector3 intercept = target.transform.position + predictionTime * target.transform.forward * 5;

        Seek(intercept);
    }

    void Arrival(Vector3 location)
    {
        float slowdownDistance = 10.0F;
        float slowdownRate = 0.90F;
        
        Vector3 targetDir = location - agent.transform.position;
        if (targetDir.magnitude < slowdownDistance)
        {
            // slowdown takes effect here.
            location = agent.transform.position + slowdownRate * targetDir;
        }
        Seek(location);
    }

    Vector3 wanderTarget = Vector3.zero;
    void Wander()
    {
        float wanderRadius = 10;
        float wanderDistance = 20;
        float wanderJitter = 1;

        wanderTarget += new Vector3(Random.Range(-1.0f, 1.0f) * wanderJitter,
                                    0,
                                    Random.Range(-1.0f, 1.0f) * wanderJitter);
        wanderTarget.Normalize();
        wanderTarget *= wanderRadius;

        Vector3 targetLocal = wanderTarget + new Vector3(0, 0, wanderDistance);
        Vector3 targetWorld = this.gameObject.transform.InverseTransformVector(targetLocal);

        Seek(targetWorld);
    }
    #endregion

    void Hide()
    {
        float nearest = Mathf.Infinity;
        Vector3 chosenSpot = Vector3.zero;

        for(int i=0; i<hidingSpots.Length; i++)
        {
            Vector3 hidingDir = hidingSpots[i].transform.position - target.transform.position;
            Vector3 hidingPos = hidingSpots[i].transform.position + hidingDir.normalized * 5;

            float dist = Vector3.Distance(this.transform.position, hidingPos); 
            if (dist < nearest)
            {
                chosenSpot = hidingPos;
                nearest = dist;
            }
        }

        Seek(chosenSpot);
    }

    void Sneaking()
    {
        if (CanBeSeen())
        {
            _knife.SetActive(false);
            Hide();
        }
        else
        {
            _knife.SetActive(true);
            Seek(target.transform.position);
        }
    }

    bool CanBeSeen()
    {
        bool isSeen = false;
        Vector3 targetSee = transform.position - target.transform.position;
        float angle = Vector3.Angle(targetSee, target.transform.forward);
        //Debug.Log(angle);
        if(angle < 60)
        {
            isSeen = true;
        }
        return isSeen;
    }

    void Stab()
    {
        if(Vector3.Distance(transform.position, target.transform.position) < 0.5f && !stab)
        {
            stab = true;
            GameObject.Find("Cop").GetComponent<Drive>().enabled = false;
            
            StartCoroutine(bloodSplash());
        }
    }

    private IEnumerator bloodSplash()
    {
        Instantiate(_bloodParticle, target.transform.position, Quaternion.identity);
        yield return new WaitForSeconds(1.0f);
        _light.SetActive(false);
        _cv.SetActive(true);
        GameObject.Find("Cop").SetActive(false);
        yield return new WaitForSeconds(5.0f);
        SceneManager.LoadScene(0);
    }

    // Update is called once per frame
    void Update()
    {

        // Seek(target.transform.position);
        // Flee(target.transform.position); 
        // Pursue();
        // Arrival(target.transform.position);
        // Wander();
        // Hide();
        //Move closer when Out of Sight
        Sneaking();

        //Distance - Out of Range - No respond from Cop sight
        //Collide - Trigger Blood Particle
        Stab();
    }
}
