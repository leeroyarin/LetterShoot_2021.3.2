using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calculate : IShape
{
    public Calculate()
    {
        
    }
    public Calculate(int g)
    {
        Debug.Log(g);
    }
    public float GetScale(LetterBehaviour scale)
    {
        return 2;
    }
}

public class IDONNO : IShape
{
 
    public float GetScale(LetterBehaviour scale)
    {
        return scale.transform.localScale.x;
    }
}
public interface IShape
{
    public float GetScale(LetterBehaviour scale);
}
