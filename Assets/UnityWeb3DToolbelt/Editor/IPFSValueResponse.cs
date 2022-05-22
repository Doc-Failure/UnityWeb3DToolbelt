using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

[Serializable]
public class IPFSValueResponse
{
    [SerializeField]
    public string cid;
    [SerializeField]
    public string created;
    [SerializeField]
    public string type;
    [SerializeField]
    public string scope;
    [SerializeField]
    public string files;
    [SerializeField]
    public string size;
    [SerializeField]
    public string name;
    [SerializeField]
    public string pin;
    [SerializeField]
    public string deals;

    /*  public IPFSValueResponse(string jsonString)
     {
         return JsonConvert.DeserializeObject<IPFSValueResponse>(jsonString);
     } */

}