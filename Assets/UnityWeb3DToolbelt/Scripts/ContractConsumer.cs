using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.ABI.Model;
using Nethereum.Web3;
using Nethereum.Hex.HexTypes;
using Nethereum.Web3.Accounts;
using Nethereum.Web3.Accounts.Managed;
using Nethereum.Contracts;
using Nethereum.Contracts.CQS;
using Nethereum.Contracts.Extensions;
using Nethereum.JsonRpc.UnityClient;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Unity.EditorCoroutines.Editor;

[ExecuteInEditMode]
public class ContractConsumer : MonoBehaviour
{
    List<DecodedABI> decodedABI = new List<DecodedABI>();

    public TextAsset ContractJson;
    public InputField privateKey;
    public bool onStart, onUpdate, onClick, onCollision = false;
    public string onStartFunction, onUpdateFunction, onClickFunction, onCollisionFunction = "";
    public string onClickParameters = "";
    public string abi = "";
    ENetworks index;
    public Contract contract;

    void Start()
    {

        contract = JsonUtility.FromJson<Contract>(ContractJson.text);
        //Decode the ABI
        var root = JObject.Parse(ContractJson.text);
        decodedABI = root["abi"].ToObject<List<DecodedABI>>();
        abi = JsonConvert.SerializeObject(root["abi"]);
    }

    void Update()
    {
        if (onClick && Input.GetMouseButtonDown(0))
        {
            Debug.Log(onClickFunction);
            web3Req(onClickFunction);
        }
        //Se click faccio qualcosa
        //Se collizione faccio altro???
    }


    public async void web3Req(string functionName)
    {
        //hardcoded endpoint
        var url = "https://data-seed-prebsc-2-s3.binance.org:8545/";
        //privateKeyInserted
        var account = new Account(privateKey.text);
        var web3 = new Web3(account, url);
        var contractConnector = web3.Eth.GetContract(abi, contract.address);
        var function = contractConnector.GetFunction(functionName);
        var result = await function.CallAsync<string>();
        Debug.Log(result);
    }

    void OnGUI()
    {/* 
        if (GUI.Button(new Rect(10, 10, 150, 100), "I am a button"))
        {
            print("You clicked the button!");
        } */
    }

}