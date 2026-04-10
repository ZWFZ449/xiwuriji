using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class db_synthesis_vo 
{
    public readonly string synthesis_name;

    public readonly string synthesis_type;

    public readonly string synthesis_need;

    public db_synthesis_vo(string synthesis_name, string synthesis_type, string synthesis_need)
    { 
        this.synthesis_name = synthesis_name;
        this.synthesis_type = synthesis_type;
        this.synthesis_need = synthesis_need;
    }
}
