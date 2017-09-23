using UnityEngine;

[AddComponentMenu("Advanced Platformer Controller/ Ladder Manager")]
public class LadderManager : MonoBehaviour {

    public TwoDController controller;

    private bool laddersEnabled;

    private bool isClimbingLadder;

    void Start ()
    {      
        controller = GetComponentInParent<TwoDController>();        

        Physics.IgnoreCollision(GetComponent<Collider>(), controller.GetComponent<Collider>() );        
    }   

    void FixedUpdate ()
    {
        if (controller.ladderType == TwoDController.LadderType.Activated)
        {
            laddersEnabled = true;
        } else
        {
            laddersEnabled = false;
        }        
    }

    void OnTriggerEnter (Collider other)
    {       
        if (laddersEnabled && other.GetComponent<Ladder>() != null)
        {          
            controller.isClimbing = true;            
        } else
        {
            controller.isClimbing = false;
        }
    }

    void OnTriggerExit ()
    {
        controller.isClimbing = false;
    }
}
