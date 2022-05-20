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

using Unity.EditorCoroutines.Editor;

public class UnityWeb3DToolbelt : EditorWindow
{

    //string[] chain = { "Aurora testnet", "BSC testnet", "Polygon testnet (Mumbai)", "Polkadot testnet", "Avalanche testnet (FUJI)" };


    ENetworks index;
    string privateKey;
    string account;
    /* 
        NFTDeployment deployContract; */

    GameObject redeemObjTrigger;
    Texture2D texture;
    bool showMeTheNFT = false;
    int objectID = 1;
    string tokenName;
    string tokenSymbol;
    bool includeWalletLoginWidgetInGame = true;
    Texture2D objectToDeploy;
    Texture2D objtdeploy;

    Networks networksList = new Networks();

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
        //AppOptions options = AppOptions.LoadFromJsonConfig("networks.json");
        //Debug.Log(options[0]);
        index = (ENetworks)EditorGUILayout.EnumPopup("Deploy to:", index);
        GUILayout.Label("");

        //Debug.Log("NFT reedemer: " + networksList.GetNetwork(index).GetChainID());

        /*   if (deployContract == null)
              showMeTheNFT = false; */
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
        objtdeploy = EditorGUILayout.ObjectField("Object to aaaaaa", objtdeploy, typeof(Texture2D), true, GUILayout.Height(EditorGUIUtility.singleLineHeight)) as Texture2D;

        GUILayout.EndVertical();

        if (GUILayout.Button("Deploy as NFT"))
        {
            Debug.Log("Deploy as NFT");
            this.StartCoroutine(DeployNFT());
        }
        GUILayout.Label("");
        /*   if (deployContract != null && GUILayout.Button("Retrieve NFT"))
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
          } */

    }

    private void NFTReedemer()
    {
        Debug.Log("NFT reedemer");
    }

    //passare da DeployObject a DeployNFT
    private IEnumerator DeployNFT()
    {

        Dictionary<string, string> headers = new Dictionary<string, string>();
        headers.Add("accept", "application/json");
        headers.Add("Authorization", "Bearer TBD");

        var bytes = objectToDeploy.EncodeToPNG();
        var bytes2 = objtdeploy.EncodeToPNG();

        WWWForm form = new WWWForm();
        form.AddBinaryData("file", bytes, "alieno.png", "image/png");
        form.AddBinaryData("file", bytes2, "aaaa.png", "image/png");

        UnityWebRequest www = UnityWebRequest.Post("https://api.nft.storage/upload", form);
        www.SetRequestHeader("Authorization", "Bearer TBD");
        www.SetRequestHeader("accept", "application/json");
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Form upload complete!");
        }
    }
}
