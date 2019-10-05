using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Teleportal;

public class Projectile : MonoBehaviour
{
    public float speed = 2f;
    public int destroyTime = 5000; // milliseconds (ms)

    // LIFECYCLE //

    void Start() {
        // Once the object has been initially transformed,
        // in the Realm, fire this projectile.
        TPObject.get(this.transform.parent.gameObject).OnTransform += delegate {
            this.FullSend();
        };
    }

    // helper method
    private void DestroyLaser() {
        GameObject laser = this.gameObject.transform.parent.gameObject;
        TPObject.get(laser).Delete();
    }

    private async void FullSend() {
        // Add force
        Vector3 v = transform.forward * speed;
        Rigidbody rb = this.gameObject.GetComponent<Rigidbody>();
        rb.AddForce(v, ForceMode.Impulse);

        // Wait time then destroy
        await Task.Delay(this.destroyTime); 
        this.DestroyLaser();
    }

    // COLLISIONS //

    void OnCollisionEnter(Collision c) {
        TPObject tpo = c.collider.gameObject.GetComponent<TPObject>();
        if (tpo != null) {
            tpo.LockGlobally();
            this.ResetLockDelayed(tpo);
        }
    }

    void OnCollisionExit(Collision c) {
        //this.DestroyLaser();
    }

    private async void ResetLockDelayed(TPObject tpo) {
        await Task.Delay(3000);

        if (tpo != null) {
            tpo.UnlockGlobally();
        }
    }
}
