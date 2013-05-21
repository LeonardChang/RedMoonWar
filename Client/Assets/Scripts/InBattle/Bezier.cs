using UnityEngine;
using System.Collections;

public class Bezier
{
    public Vector3 p0;
    public Vector3 p1;
    public Vector3 p2;
    public Vector3 p3;

    public float ti = 0f;

    private Vector3 b0 = Vector3.zero;
    private Vector3 b1 = Vector3.zero;
    private Vector3 b2 = Vector3.zero;
    private Vector3 b3 = Vector3.zero;

    private float Ax;
    private float Ay;
    private float Az;
    private float Bx;
    private float By;
    private float Bz;
    private float Cx;
    private float Cy;
    private float Cz;

    public Bezier( Vector3 v0, Vector3 v1, Vector3 v2, Vector3 v3 )
    {
        this.p0 = v0;
        this.p1 = v1;
        this.p2 = v2;
        this.p3 = v3;
    }

    public Vector3 GetPointAtTime( float t )
    {
        CheckConstant();

        float t2 = t * t;
        float t3 = t * t * t;

        float x = this.Ax * t3 + this.Bx * t2 + this.Cx * t + p0.x;
        float y = this.Ay * t3 + this.By * t2 + this.Cy * t + p0.y;
        float z = this.Az * t3 + this.Bz * t2 + this.Cz * t + p0.z;

        return new Vector3( x, y, z );
    }

    private void SetConstant()
    {
        Cx = 3f * ( ( this.p0.x + this.p1.x ) - this.p0.x );
        Bx = 3f * ( ( this.p3.x + this.p2.x ) - ( this.p0.x + this.p1.x ) ) - this.Cx;
        Ax = this.p3.x - this.p0.x - this.Cx - this.Bx;
        Cy = 3f * ( ( this.p0.y + this.p1.y ) - this.p0.y );
        By = 3f * ( ( this.p3.y + this.p2.y ) - ( this.p0.y + this.p1.y ) ) - this.Cy;
        Ay = this.p3.y - this.p0.y - this.Cy - this.By;
        Cz = 3f * ( ( this.p0.z + this.p1.z ) - this.p0.z );
        Bz = 3f * ( ( this.p3.z + this.p2.z ) - ( this.p0.z + this.p1.z ) ) - this.Cz;
        Az = this.p3.z - this.p0.z - this.Cz - this.Bz;
    }

    private void CheckConstant()
    {
        if( this.p0 != this.b0 || this.p1 != this.b1 || this.p2 != this.b2 || this.p3 != this.b3 )
        {
	        SetConstant();
	        b0 = p0;
            b1 = p1;
	        b2 = p2;
	        b3 = p3;
        }
    }
}