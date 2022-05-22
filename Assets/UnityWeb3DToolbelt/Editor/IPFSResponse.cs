using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

[Serializable]
public class IPFSResponse
{
    [SerializeField]
    public bool ok;

    [SerializeField]
    public IPFSValueResponse value;

    /*  public IPFSResponse(string jsonString)
     {
         return JsonConvert.DeserializeObject<IPFSResponse>(jsonString);
     } */

}