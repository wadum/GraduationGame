using UnityEngine;
using System.Collections;

public class RiftScript : MonoBehaviour {

    public Vector3 OpenedScale, ClosedScale;
    public bool active;
    public RiftScript Partner;

    ElectrifiedRift electrified;
    private float ElapsedTime = 0;

    // Use this for initialization
    void Start () {
        electrified = GetComponentInChildren<ElectrifiedRift>();
        MultiTouch.RegisterTapHandlerByTag("PortalRift", hit => {
            if(hit.collider.gameObject == gameObject)
                FindObjectOfType<RiftMaster>().TagRift(gameObject);
            });

        if (active)
            transform.localScale = OpenedScale;
        else
            transform.localScale = ClosedScale;
    }

    // Update is called once per frame
    void Update () {
        if (!active)
        {
            if (ElapsedTime > 0)
                ElapsedTime -= Time.deltaTime;
        }
        else
        {
            if (Partner && electrified.Active)
            {
                Partner.GetComponentInChildren<LightningGenerator>().enabled = true;
            }
            if (ElapsedTime < 1f)
                ElapsedTime += Time.deltaTime;
        }
        if(!Partner)
            GetComponentInChildren<LightningGenerator>().enabled = false;
        float dist = ElapsedTime / 1f;
        transform.localScale = Vector3.Lerp(ClosedScale, OpenedScale, dist);
    }

    // OnTriggerStay is used when something is "resting" on a portal's location. If the portal then activates it should suck down whatever is on top.
    void OnTriggerStay(Collider collider)
    {
        // If rift is closed or there's no outgoing rift, do nothing.
        if (!Partner || !active)
            return;

        // If the collider is something that needs to be cloned, do the cloning and start the teleportation.
        if (collider.tag == "Elevator" || collider.tag == "Rock")
        {
            Cloneable cloneScript = collider.gameObject.GetComponent<Cloneable>();
            // It's cloneable
            if (cloneScript)
            {
                // If it doesn't already have a clone, or is a clone, then make a clone.
                if (!cloneScript.Clone)
                {
                    Rigidbody colliderBody = collider.GetComponent<Rigidbody>();

                    // If the elevator collides with stuff, make it so it doesn't.
       /*            if (!colliderBody.isKinematic)
                        colliderBody.isKinematic = true;
                    if (colliderBody.detectCollisions)
                        colliderBody.detectCollisions = false;*/

                    // Make the clone just below the Partner rift.
                    GameObject El = (GameObject)Instantiate(collider.gameObject, Partner.transform.position - Partner.transform.up * collider.gameObject.transform.localScale.y / 2, Quaternion.identity);
                    Cloneable cloned = El.GetComponent<Cloneable>();
                    El.GetComponent<Cloneable>().Clone = cloneScript;
                    El.GetComponent<Cloneable>().IsClone = true;
                    cloneScript.Clone = cloned;

                    // If we move the object manually, the new direction of the movement should be adjusted for the out rift.
                    ObjectMoveScript MoveScript = El.GetComponent<ObjectMoveScript>();
                    if (MoveScript)
                        MoveScript.SetDirection(Partner.transform.up);

                    // IF for some reason we don't move objects manually, but rather rely on RigidBody and their Velocity, it's also edited.
                    Rigidbody clonedBody = El.GetComponent<Rigidbody>();
                    Vector3 velocity = colliderBody.velocity;
                    if (velocity != Vector3.zero)
                    {
                        clonedBody.velocity = Vector3.zero;
                        clonedBody.velocity = Partner.transform.up * velocity.magnitude;
                    }
                    El.transform.localRotation = collider.transform.localRotation;
                }
                return;
            }
            return;
        }
    }

    // OnTriggerEnter is used when something collides only once, so when either the sidekick or something which is instantly teleported triggers.
    void OnTriggerEnter(Collider collider)
    {
        // If the sidekick is hitting the rift.
        if (collider.tag == "Sidekick")
        {
            // Check if SideKick is trying to open the portal.

            // Should play animation of the sidekick "opening" the rift.

            // Tell the sidekick that it should return to it's original state.
            return;
        }

        // If something collides, but there's no outgoing portal, nothing happens.
        if (!Partner || !active)
            return;
        
        // Player should be teleported
        if (collider.tag == "Player")
        {
            return;
        }

        // Objects which can be resting on the Portal, and which are gradually teleporting, are handled in OnTriggerStay.
        if (collider.tag == "Elevator" || collider.tag == "Rock") // || "Something Solid" || .. || .. and so on)
        {
            return;
        }

        float scale;
        if (collider.tag == "Wind")
            scale = collider.GetComponent<SphereCollider>().radius;
        else
            scale = collider.transform.localScale.y;

        // If ANYTHING else collides, it's teleported to the partner's location, but ahead of the partner by it's own scale.
        collider.transform.position = Partner.transform.position + Partner.transform.up * scale;//collider.transform.localScale.y;

        // If we move the object manually, the new direction of the movement should be adjusted for the out rift.
        ObjectMoveScript MoveScript = collider.GetComponent<ObjectMoveScript>();
        if (MoveScript)
            MoveScript.SetDirection(Partner.transform.up);

        // IF for some reason we don't move objects manually, but rather rely on RigidBody and their Velocity, it's also edited.
        Rigidbody colliderBody = collider.GetComponent<Rigidbody>();
        if (colliderBody)
        {
            Vector3 velocity = colliderBody.velocity;
            if(velocity != Vector3.zero)
            {
                colliderBody.velocity = Vector3.zero;
                colliderBody.velocity = Partner.transform.up * velocity.magnitude;
            }
        }
    }

    public void ClearPartner()
    {
        Partner = null;
    }

    public void AssignPartner(RiftScript pc)
    {
        Partner = pc;
    }

}
