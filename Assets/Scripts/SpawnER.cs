using UnityEngine;

public class Spawn : MonoBehaviour {

    // Grab our prefab
    public GameObject patientPrefab;
    int i;
    // Number of patients to spawn
    //public int numPatients;

    void Start() {

        //for (int i = 0; i < numPatients; ++i) {

        //    // Instantiate numPatients at the spawner
        //    Instantiate(patientPrefab, this.transform.position, Quaternion.identity);
        //}
        // Call the SpawnPatient method for the first time
        Invoke("SpawnPatient", 5.0f);
    }

    void SpawnPatient() {

        i = Random.Range(1, 10);
        // Instantiate numPatients at the spawner

        Instantiate(patientPrefab, this.transform.position, Quaternion.identity);
        //if (i == 10)
        //    Instantiate(patientPrefab[0], this.transform.position, Quaternion.identity);
        //else
        //    Instantiate(patientPrefab[1], this.transform.position, Quaternion.identity);
        // Invoke this method at random intervals
        Invoke("SpawnPatient", Random.Range(5.0f, 20.0f));
    }

    // Update is called once per frame
    void Update() {

    }
}
