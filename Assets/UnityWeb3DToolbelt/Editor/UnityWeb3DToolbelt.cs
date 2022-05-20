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

    string NFTStorageBearerApi;
    GameObject redeemObjTrigger;
    Texture2D texture;
    bool showMeTheNFT = false;
    int objectID = 1;
    string tokenName;
    string tokenSymbol;
    bool includeWalletLoginWidgetInGame = true;
    Texture2D objectToDeploy;
    Texture2D objtdeploy;
    List<ERC1155Metadata> tokenList = new List<ERC1155Metadata>();

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
        GUILayout.Label("NFT.storage", EditorStyles.boldLabel);
        NFTStorageBearerApi = EditorGUILayout.TextField("Bearer API Key", NFTStorageBearerApi);

        GUILayout.BeginVertical();
        GUILayout.Label("");
        GUILayout.Label("NFT Builder", EditorStyles.boldLabel);

        if (GUILayout.Button("Add Asset", GUILayout.ExpandWidth(false)))
        {
            Debug.Log("Asset Added");
            tokenList.Add(new ERC1155Metadata());
        }
        GUILayout.Label("");
        for (int i = 0; i < tokenList.Count; i++)
        {
            tokenList[i].name = EditorGUILayout.TextField("Token name", tokenList[i].name);
            tokenList[i].description = EditorGUILayout.TextField("Token description", tokenList[i].description);
            tokenList[i].image = EditorGUILayout.ObjectField("Token Image", tokenList[i].image, typeof(Texture2D), true, GUILayout.Height(EditorGUIUtility.singleLineHeight)) as Texture2D;
            GUILayout.Label("Attributes");
            /*  if (GUILayout.Button("Add Attribute", GUILayout.ExpandWidth(false)))
             {
                 Debug.Log("Attribute Added");
                 tokenList[i].attributes.Add("string", "string");
             } */
            string stringToEdit = "";
            stringToEdit = GUILayout.TextArea(stringToEdit, 200);
            /* foreach (KeyValuePair<string, string> entry in tokenList[i].attributes)
            {
                GUILayout.Label("Attributes Name");
                tokenList[i].attributes.name = EditorGUILayout.TextField("Attributes name", tokenList[i].attributes.name);
                GUILayout.Label("Attributes Value");
                /* tokenList[i].attributes.values = EditorGUILayout.TextField("Attributes Value", tokenList[i].attributes.values);
            } */
            if (GUILayout.Button("Delete Asset", GUILayout.ExpandWidth(false)))
            {
                Debug.Log("Asset Deleted");
                tokenList.RemoveAt(i);
            }
            GUILayout.Label("");

        }

        GUILayout.EndVertical();

        if (GUILayout.Button("Deploy as NFT"))
        {
            Debug.Log("Deploy as NFT");
            this.StartCoroutine(DeployNFT());
        }
        GUILayout.Label("");
        GUILayout.Label("In Game Features", EditorStyles.boldLabel);
        includeWalletLoginWidgetInGame = EditorGUILayout.Toggle("Wallet Login Widget", includeWalletLoginWidgetInGame);

    }

    private void NFTReedemer()
    {
        Debug.Log("NFT reedemer");
    }

    //passare da DeployObject a DeployNFT
    private IEnumerator DeployNFT()
    {
        var bytes = objectToDeploy.EncodeToPNG();
        var bytes2 = objtdeploy.EncodeToPNG();

        WWWForm form = new WWWForm();
        form.AddBinaryData("file", bytes, "alieno.png", "image/png");
        form.AddBinaryData("file", bytes2, "aaaa.png", "image/png");

        UnityWebRequest www = UnityWebRequest.Post("https://api.nft.storage/upload", form);
        www.SetRequestHeader("Authorization", "Bearer " + NFTStorageBearerApi);
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
