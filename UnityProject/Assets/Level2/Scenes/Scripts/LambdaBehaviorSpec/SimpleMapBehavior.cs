using UnityEngine;
using System.Collections;

public class SimpleMapBehavior : LambdaBehavior
{

	public LambdaGrid.LambdaCube from, to;

    public SimpleMapBehavior(LambdaGrid.LambdaCube cubeA, LambdaGrid.LambdaCube cubeB) :
    base(Proc(cubeA, cubeB))
    {
        from = cubeA;
        to = cubeB;
    }

    static Fn Proc(LambdaGrid.LambdaCube cubeA, LambdaGrid.LambdaCube cubeB)
    {
        return new Fn(i => i.SimpleMap(cubeA, cubeB));
    }

}