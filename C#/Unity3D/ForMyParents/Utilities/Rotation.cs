
//===============================================================================
// Author:  Nathan Contreras
//
//   Rotation.cs should be used in situations where it is desirable to have
//   a GameObject continuously rotation in a set of axes at a set rate.  This
//   is great for setting a day/night cycle by applying this script to a
//   directional light within the scene
//   
//   Delta variables have been made public to allow easy access to values 
//   within the editor, and proper getters/setters have been implemented to
//   safely modify and retrieve values.
// 
//   Inversion functions have been provided to allow easy inverting of existing
//   directions in a safe environment.
//===============================================================================

using UnityEngine;
using System.Collections;

public class Rotation : MonoBehaviour {

    public float deltaX;
    public float deltaY;
    public float deltaZ;
	
    //===============================================================
    // Use this for initialization
	void Start () {

	}
	
    //===============================================================
	// Update is called once per frame
	void Update () {
        this.transform.Rotate(deltaX * Time.deltaTime, deltaY * Time.deltaTime, deltaZ * Time.deltaTime);
	}

    //===============================================================
    // Returns the currents change in x
    float getDeltaX()
    {
        return this.deltaX;
    }

    //===============================================================
    // Returns the currents change in y
    float getDeltaY()
    {
        return this.deltaY;
    }

    //===============================================================
    // Returns the currents change in z
    float getDeltaZ()
    {
        return this.deltaZ;
    }

    //===============================================================
    // Modifies the current change in x
    void setDeltaX( float newX )
    {
        this.deltaX = newX;
    }

    //===============================================================
    // Modifies the current change in y
    void setDeltaY( float newY )
    {
        this.deltaY = newY;
    }

    //===============================================================
    // Modifies the current change in z
    void setDeltaZ( float newZ )
    {
        this.deltaZ = newZ;
    }

    //===============================================================
    // Invert the current change in x
    void invertDeltaX()
    { 
        this.deltaX *= -1;
    }

    //===============================================================
    // Invert the current change in y
    void invertDeltaY()
    {
        this.deltaY *= -1;
    }
        
    //===============================================================
    // Invert the current change in z
    void invertDeltaZ()
    {
        this.deltaZ *= -1;
    }

}