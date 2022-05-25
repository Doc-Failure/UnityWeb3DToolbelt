using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class ContractConsumer : MonoBehaviour
{
    public string storageLayout;
    Contract contract;
    List<DecodedABI> decodedABI = new List<DecodedABI>();

    public TextAsset ContractJson;

    void Start()
    {
        //var contract = JsonConvert.DeserializeObject(ABI.text);
        //contract = JsonUtility.FromJson<Contract>(ContractJson.text["abi"]);
        //List<DecodedABI> listaFunzioniContratto = JsonConvert.DeserializeObject<List<DecodedABI>>(ContractJson.text[]);

        var root = JObject.Parse(ContractJson.text);
        List<DecodedABI> objects = root["abi"].ToObject<List<DecodedABI>>();
        for (int i = 0; i < objects.Count; i++)
        {
            Debug.Log(objects[i].name);
        }
        //objects = JObject.Parse(< DecodedABI[] > ();
        //ERC1155ImagesCID = objects["value"]["cid"].ToString();
        //Debug.Log(contract.address);
        /* foreach (JObject o in root["abi"].Children<JObject>())1
        {
            foreach (JProperty p in o.Properties())
            {
                /* string name = p.Name;
                string value = (string)p.Value;
                Console.WriteLine(name + " -- " + value); */
        /*    }
       } */

        //Debug.Log(contract.transactionHash);
        /* List<DecodedABI> ObjOrderList = JsonUtility.FromJson<List<DecodedABI>>(contract.abi);
        for (int i = 0; i < ObjOrderList.Count; i++)
        {
            Debug.Log(ObjOrderList[i]);
        } */

        //"address"
        //Su start lancio funzione?
    }


    public async void web3Req()
    {
        //hardcoded endpoint
        var url = "https://eth-rpc-api-testnet.thetatoken.org/rpc";
        //privateKeyInserted
        //var account = new Account("5c5fe39a97019300e92794f387e14f6b6f0302857803ddc349e28111c0693dec");
        //var web3 = new Web3(account, url);
        //var contract = web3.Eth.GetContract(contract.abi, contract.address);

        //how to call a function
        //var function = contract.GetFunction("name");
        //var result = await function.CallAsync<string>();
        //name = result;
    }


    void Update()
    {
        //Se click faccio qualcosa
        //Se collizione faccio altro???
    }
}