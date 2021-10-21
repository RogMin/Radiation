using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Radiation
{
   public class Radiation : MonoBehaviour
    {
        RaycastHit[] hits;
        public float RadiationPowerRoentgHour;
       [Header("Don't Change:")]
        [SerializeField]
        private float AttenuationСoefficient;
        [SerializeField]
        private float RadiationPowerInNowPosition;
        public int AmountHits;
        [Header("")]

        public ReceivedDose receivedDose;
        public GameObject player;
        public RadiationCounter radiationCounter;
        private float PlayerDistanceToCollider;



       void ResetRayCast()
        {
            AttenuationСoefficient = 1;
            hits = null;
            AmountHits = 0;
        }
        void OnTriggerStay(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                ResetRayCast();
                PlayerDistanceToCollider = Vector3.Distance(player.transform.position, transform.position);
                AttenuationCoeff();
                if (PlayerDistanceToCollider >= 1)
                {
                    RadiationInNowPosition();
                }
                else
                {
                    RadiationPowerInNowPosition = RadiationPowerRoentgHour;
                }
                if (AttenuationСoefficient > 0)
                {
                    RadiationPowerInNowPosition /= AttenuationСoefficient;
                }
                RecieveRadInAnotherScripts();
            }
            
         
        }
        public float RadiationInNowPosition()
        {
            return RadiationPowerInNowPosition = RadiationPowerRoentgHour / (PlayerDistanceToCollider* PlayerDistanceToCollider);
        }
        public void RecieveRadInAnotherScripts() 
        {
            RadiationToCounter();
            RadRecievedDose();
            void RadiationToCounter()
            {
                radiationCounter.RoentgenHour += RadiationPowerInNowPosition;
            }
            void RadRecievedDose()
            {
                receivedDose.RecieveRoentgen = RadiationPowerInNowPosition;
            }
           
        }
        
        public float AttenuationCoeff()
        {
            int layerMask = LayerMask.GetMask("Wall");
            hits = Physics.RaycastAll(transform.position, player.transform.position - transform.position, PlayerDistanceToCollider, layerMask);

             foreach (var h in hits)
             {
                
                h.collider.gameObject.GetComponent<Renderer>().material.color = Color.yellow;
                try
                 {
                    AttenuationСoefficient += h.collider.gameObject.GetComponent<RadReduce>().AttenuationСoefficient;
                 }
                 catch
                 {
                    AttenuationСoefficient = 1;
                 }
                AmountHits++;
             }
            //Debug.DrawLine(transform.position, player.transform.position); 
            return AttenuationСoefficient;
        }
    }
}
