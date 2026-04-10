using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class db_illustrated_vo 
{
    public readonly string Illustrated_name;

    public readonly int Illustrated_type;

    public readonly int Illustrated_Shape;

    public readonly string Illustrated_need;

    public readonly string Illustrated_effect;

    public db_illustrated_vo(string illustrated_name, int illustrated_type, int illustrated_Shape, string illustrated_need, string illustrated_effect)
    { 
        Illustrated_name = illustrated_name;
        Illustrated_type = illustrated_type;
        Illustrated_Shape = illustrated_Shape;
        Illustrated_need = illustrated_need;
        Illustrated_effect = illustrated_effect;
    }
}
