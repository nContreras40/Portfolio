
//=======================================================================================
// Author:  Nathan Contreras  
// Version: 1.1
// Desc:    Simple 2D lerping script.  
//=======================================================================================

using UnityEngine;
using System.Collections;

public class Lerp2D : MonoBehaviour
{

    public Transform transformOne;
    public Transform transformTwo;

    [Tooltip("How quickly the Lerp will be performed")]
    public float speed = 1.0f;

    [Tooltip("Check box if you wish to set transformOne to this objects starting position")]
    public bool objectStartAsTransformOne = true;

    [Tooltip("Check box if you wish to only Lerp through the x values for this object")]
    public bool lerpOnlyXValue = false;

    [Tooltip("Check box if you wish to only Lerp through the y values for this object")]
    public bool lerpOnlyYValue = false;

    private bool  toPositionOne = false;
    private float timer = 0.0f;

    // Use this for initialization
    void Start()
    {
        if (objectStartAsTransformOne) { this.transformOne = this.transform; }
        if (transformOne == null) { Debug.LogError("Transform positionOne not assigned in Lerp.cs, obj " + this.name); }
        if (transformTwo == null) { Debug.LogError("Tansform positionTwo not assigned in Lerp.cs, obj " + this.name); }
    }

    // Update is called once per frame
    void Update()
    {
        if (this.toPositionOne)
        {
            this.timer += Time.deltaTime;
            float interpolator = ( this.timer / this.speed ) * Time.deltaTime;

            if (this.lerpOnlyXValue)
            {
                float x = Mathf.Lerp(this.transform.position.x, this.transformTwo.position.x, interpolator);
                this.transform.position = new Vector2(x, this.transform.position.y);
            }
            if (this.lerpOnlyYValue)
            {
                float y = Mathf.Lerp(this.transform.position.y, this.transformTwo.position.y, interpolator);
                this.transform.position = new Vector2(this.transform.position.x, y);
            }
            if (!this.lerpOnlyXValue && !this.lerpOnlyYValue)
            {
                float x = Mathf.Lerp(this.transform.position.x, this.transformTwo.position.x, interpolator);
                float y = Mathf.Lerp(this.transform.position.y, this.transformTwo.position.y, interpolator);
                this.transform.position = new Vector2(x, y);
            }
            
            if (this.timer >= this.speed)
            {
                this.timer = 0.0f;
                this.toPositionOne = false;
                Debug.Log(this.name);
            }
        }
        else
        {
            this.timer += Time.deltaTime;
            float interpolator = ( this.timer / this.speed ) * Time.deltaTime;

            if (this.lerpOnlyXValue)
            {
                float x = Mathf.Lerp(this.transform.position.x, this.transformOne.position.x, interpolator);
                this.transform.position = new Vector2(x, this.transform.position.y);
            }
            if (this.lerpOnlyYValue)
            {
                float y = Mathf.Lerp(this.transform.position.y, this.transformOne.position.y, interpolator);
                this.transform.position = new Vector2(this.transform.position.x, y);
            }
            if (!this.lerpOnlyXValue && !this.lerpOnlyYValue)
            {
                float x = Mathf.Lerp(this.transform.position.x, this.transformOne.position.x, interpolator);
                float y = Mathf.Lerp(this.transform.position.y, this.transformOne.position.y, interpolator);
                this.transform.position = new Vector2(x, y);
            }

            if (this.timer >= this.speed)
            {
                this.timer = 0.0f;
                this.toPositionOne = true;
            }
        }
    }
}
