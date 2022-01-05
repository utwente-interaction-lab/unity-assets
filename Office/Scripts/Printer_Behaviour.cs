using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Office
{
    public class Printer_Behaviour : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] Transform Spawnpoint_Paper = null;
        [SerializeField] GameObject[] PaperPrefabs = null;
        [SerializeField] float SpawnInterval = 0.5f;
        [SerializeField] int MaximumPaperAmount = 10;
        [SerializeField] float PaperSpeed = 20f;
        [SerializeField] float CooldownTime = 5f;
        [SerializeField] float DespawnTime = 0f;
        private bool CooldownActive = false;


        private void OnTriggerEnter (Collider col)
        {
            if (!col.CompareTag("Printer") && !CooldownActive)
                StartCoroutine(StartPaperChaos(MaximumPaperAmount));
        }

        private IEnumerator StartPaperChaos(int amount)
        {
            CooldownActive = true;
            Debug.Log("<b> Warning. Printer activated. </b>");
            int counter = 0;
            while (counter < amount)
            {
                counter++;
                SpawnPaper();
                yield return new WaitForSeconds(SpawnInterval);
            }
            yield return new WaitForSeconds(CooldownTime);
            CooldownActive = false;
            yield return null;
        }


        private void SpawnPaper()
        {
            //get correct prefab
            int index = 0;
            if (PaperPrefabs.Length > 1)
                index = Random.Range(0, PaperPrefabs.Length);

            //give it correct proportions
            GameObject paper = Instantiate(PaperPrefabs[index], Spawnpoint_Paper.position, Spawnpoint_Paper.rotation, Spawnpoint_Paper);
            paper.transform.localScale = new Vector3(0.158f * 0.1f, 0.003f * 0.1f, 0.2249f * 0.1f);
            paper.transform.Rotate(0, 0, 0);

            //apply randomized force on instantiation
            float speed = Random.Range(PaperSpeed - 0.2f * PaperSpeed, PaperSpeed + 0.2f * PaperSpeed);
            var direction = Spawnpoint_Paper.forward;
            float x = Random.Range(direction.x - 0.1f, direction.x + 0.1f);
            float y = Random.Range(direction.y - 0.1f, direction.y + 0.1f);
            float z = Random.Range(direction.z - 0.1f, direction.z + 0.1f);
            paper.GetComponent<Rigidbody>().AddForce(new Vector3(x, y, z) * speed);
            paper.transform.parent = null;
            Destroy(paper, DespawnTime);
        }
    }
}
