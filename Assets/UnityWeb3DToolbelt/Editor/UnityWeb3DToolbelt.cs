using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.ABI.Model;
using Nethereum.Contracts;
using Nethereum.Contracts.CQS;
using Nethereum.Contracts.Extensions;
using Nethereum.JsonRpc.UnityClient;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using Newtonsoft.Json;

// using Unity.EditorCoroutines.Editor;

public class UnityWeb3DToolbelt : EditorWindow
{
    string privateKey;
    string account;

    NFTDeployment deployContract;

    GameObject redeemObjTrigger;
    Texture2D texture;
    bool showMeTheNFT = false;
    int objectID = 1;
    string tokenName;
    string tokenSymbol;
    bool includeWalletLoginWidgetInGame = true;
    Texture2D objectToDeploy;
    byte[] rawData;
    [MenuItem("Web3D Toolbelt Tools/NFT Deployer")]
    public static void ShowWindow()
    {
        GetWindow(typeof(UnityWeb3DToolbelt), true, "NFT Deployer");      //GetWindow is a method inherited from the EditorWindow class
    }

    /*   [MenuItem("Tools/NFT Deployer")]
       static void Init()
       {
           var window = GetWindow<UnityWeb3DToolbelt>("Texture Previewer");
           //window.position = new Rect(0, 0, 1000, 1000);
           window.Show();
       }*/

    public class SmartContract
    {
        public string bytecodeSource;
        public string opcodes;
        public string sourceMap;
    }

    void OnGUI()
    {
        if (deployContract == null)
            showMeTheNFT = false;
        GUILayout.Label("Wallet Settings", EditorStyles.boldLabel);
        privateKey = EditorGUILayout.TextField("Private Key", privateKey);
        account = EditorGUILayout.TextField("Account Address", account);

        GUILayout.Label("");

        GUILayout.Label("In Game Features", EditorStyles.boldLabel);
        includeWalletLoginWidgetInGame = EditorGUILayout.Toggle("Wallet Login Widget", includeWalletLoginWidgetInGame);

        GUILayout.BeginVertical();
        GUILayout.Label("");
        GUILayout.Label("NFT Settings", EditorStyles.boldLabel);
        tokenName = EditorGUILayout.TextField("NFT Name", tokenName);
        tokenSymbol = EditorGUILayout.TextField("NFT Symbol", tokenSymbol);
        objectToDeploy = EditorGUILayout.ObjectField("Object to deploy", objectToDeploy, typeof(Texture2D), true, GUILayout.Height(EditorGUIUtility.singleLineHeight)) as Texture2D;

        GUILayout.EndVertical();

        if (GUILayout.Button("Deploy as NFT"))
        {
            Debug.Log("Deploy as NFT");
            // this.StartCoroutine(DeployObject());
        }
        GUILayout.Label("");
        if (deployContract != null && GUILayout.Button("Retrieve NFT"))
        {
            Debug.Log("RetrieveImage image on EdgeNode");
            // this.StartCoroutine(RetrieveImage());
        }
        if (deployContract != null && showMeTheNFT)
        {
            texture = new Texture2D(100, 100);
            //var rawData = System.IO.File.ReadAllBytes("Assets/aaa.png");
            texture.LoadImage(rawData);
            GUILayout.Label("NFT preview", EditorStyles.boldLabel);
            GUILayout.Label(texture);
        }
        if (deployContract != null)
        {
            redeemObjTrigger = EditorGUILayout.ObjectField("Redeem function trigger", redeemObjTrigger, typeof(GameObject), true, GUILayout.Height(EditorGUIUtility.singleLineHeight)) as GameObject;
            if (GUILayout.Button("Bind 'NFT Redeem' action to a game object"))
            {
                NFTReedemer();
            }

        }

    }

    private void NFTReedemer()
    {
        Debug.Log("NFT reedemer");
    }




    private IEnumerator RetrieveImage()
    {
        Debug.Log("deployContract.TokenUri: " + deployContract.TokenUri);

        UnityWebRequest request = UnityWebRequestTexture.GetTexture(deployContract.TokenUri);
        yield return request.SendWebRequest();
        if (request.isNetworkError || request.isHttpError)
            Debug.Log(request.error);
        else
        {
            rawData = ((DownloadHandlerTexture)request.downloadHandler).data;
        }
        /* var request = new UnityWebRequest("http://localhost:19888/rpc", "POST");
	string bodyJsonString = "{\"jsonrpc\":\"2.0\",\"method\":\"edgestore.GetFile\",\"params\":[{\"key\": \"0xdacc9a23035a458f21aa0cb51189d715cb5c43d7ff4c0227cca5c25eeef3d5b4\"}],\"id\":1}";
        byte[] bodyRaw = Encoding.UTF8.GetBytes(bodyJsonString);
        request.uploadHandler = (UploadHandler) new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();
        Debug.Log("Status Code: " + request.responseCode);*/
        showMeTheNFT = true;
    }



    private IEnumerator DeployObject()
    {
        var request = new UnityWebRequest("http://localhost:19888/rpc", "POST");
        string filePath = Path.Combine(Directory.GetCurrentDirectory(), AssetDatabase.GetAssetPath(objectToDeploy));
        string bodyJsonString = "{\"jsonrpc\":\"2.0\",\"method\":\"edgestore.PutFile\",\"params\":[{\"path\": \"" + filePath + "\"}],\"id\":2}";
        byte[] bodyRaw = Encoding.UTF8.GetBytes(bodyJsonString);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();

        var data = request.downloadHandler.text;
        Debug.Log("data: " + data);

        dynamic objects = JsonConvert.DeserializeObject(data); // parse as array  
        string tokenUri = "http://localhost:8080/api/v1/file?key=" + objects.result.key + "&relpath=" + objects.result.relpath;
        /*  if(objectToDeploy == null)
            {
                Debug.LogError("Error: Please choose wich object you want to deploy as NFT.");
                return;
            } */
        var url = "https://eth-rpc-api-testnet.thetatoken.org/rpc";
        //initialising the transaction request sender
        var transactionRequest = new TransactionSignedUnityRequest(url, privateKey, 365);
        // transactionRequest.UseLegacyAsDefault = true;


        deployContract = new NFTDeployment()
        {
            TokenUri = tokenUri,
            TokenName = tokenName,
            TokenSymbol = tokenSymbol
        };

        //deploy the contract and True indicates we want to estimate the gas
        yield return transactionRequest.SignAndSendDeploymentContractTransaction<NFTDeploymentBase>(deployContract);

        if (transactionRequest.Exception != null)
        {
            Debug.Log(transactionRequest.Exception.Message);
            yield break;
        }

        var transactionHash = transactionRequest.Result;

        Debug.Log("Deployment transaction hash:" + transactionHash);

        //create a poll to get the receipt when mined
        var transactionReceiptPolling = new TransactionReceiptPollingRequest(url);
        //checking every 2 seconds for the receipt
        yield return transactionReceiptPolling.PollForReceipt(transactionHash, 2);
        var deploymentReceipt = transactionReceiptPolling.Result;

        Debug.Log("Deployment contract address:" + deploymentReceipt.ContractAddress);

        /*  if (objectBaseName == string.Empty) {
                Debug.LogError("Error: Please enter a base name for the object.");
                return;
            } */
        /* 
                Vector3 spawnPos = new Vector3(spawnCircle.x, 0f, spawnCircle.y);

                GameObject newObject = Instantiate(objectToDeploy, spawnPos, Quaternion.identity); */
    }




    public partial class NFTDeployment : NFTDeploymentBase
    {
        public NFTDeployment() : base(BYTECODE) { }

        public NFTDeployment(string byteCode) : base(byteCode) { }
    }

    public class NFTDeploymentBase : ContractDeploymentMessage
    {

        public static string BYTECODE = @"608.......BYTECODE......736574206f66206e6f6e6578697374656e7420746f6b656e";



        public NFTDeploymentBase() : base(BYTECODE) { }

        public NFTDeploymentBase(string byteCode) : base(byteCode) { }

        [Parameter("string", "name", 1)]
        public string TokenName { get; set; }
        [Parameter("string", "symbol", 2)]
        public string TokenSymbol { get; set; }
        [Parameter("string", "uri", 3)]
        public string TokenUri { get; set; }
    }
}
