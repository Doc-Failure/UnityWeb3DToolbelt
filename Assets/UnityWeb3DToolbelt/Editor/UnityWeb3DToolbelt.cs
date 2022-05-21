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
using Newtonsoft.Json.Linq;
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
    string stringToEdit = "";
    bool chainLinkRandomMinterOption = true;


    List<ERC1155Metadata> tokenList = new List<ERC1155Metadata>();

    Networks networksList = new Networks();

    byte[] rawData;
    [MenuItem("Web3D Toolbelt Tools/NFT Deployer")]
    public static void ShowWindow()
    {
        GetWindow(typeof(UnityWeb3DToolbelt), true, "NFT Deployer");      //GetWindow is a method inherited from the EditorWindow class
    }

    void OnGUI()
    {

        index = (ENetworks)EditorGUILayout.EnumPopup("Deploy to:", index);
        GUILayout.Label("");

        GUILayout.BeginVertical(EditorStyles.helpBox);
        GUILayout.Label("Wallet Settings", EditorStyles.boldLabel);
        privateKey = EditorGUILayout.TextField("Private Key", privateKey);
        account = EditorGUILayout.TextField("Account Address", account);
        GUILayout.EndVertical();

        GUILayout.Label("");
        GUILayout.BeginVertical(EditorStyles.helpBox);
        GUILayout.Label("NFT.storage", EditorStyles.boldLabel);
        NFTStorageBearerApi = EditorGUILayout.TextField("Bearer API Key", NFTStorageBearerApi);
        GUILayout.EndVertical();

        GUILayout.Label("");
        GUILayout.BeginVertical(EditorStyles.helpBox);
        GUILayout.Label("NFT Builder", EditorStyles.boldLabel);
        GUILayout.BeginHorizontal();
        GUILayout.Label("ChainLink Integrated Features", EditorStyles.boldLabel);
        chainLinkRandomMinterOption = EditorGUILayout.Toggle("VRF Token Minter", chainLinkRandomMinterOption);
        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.Toggle("VRF Attributes Value", false);
        EditorGUI.EndDisabledGroup();
        GUILayout.EndHorizontal();

        if (GUILayout.Button("Add Asset", GUILayout.ExpandWidth(false)))
        {
            Debug.Log("Asset Added");
            tokenList.Add(new ERC1155Metadata());
        }
        GUILayout.Label("");
        for (int i = 0; i < tokenList.Count; i++)
        {
            GUILayout.BeginVertical(EditorStyles.helpBox);
            GUILayout.Label("Token #" + i);
            tokenList[i].name = EditorGUILayout.TextField("Name", tokenList[i].name);
            tokenList[i].description = EditorGUILayout.TextField("Description", tokenList[i].description);
            tokenList[i].image = EditorGUILayout.ObjectField("Image", tokenList[i].image, typeof(Texture2D), true, GUILayout.Height(EditorGUIUtility.singleLineHeight)) as Texture2D;

            EditorGUILayout.LabelField("Attributes");
            tokenList[i].attributes = EditorGUILayout.TextArea(tokenList[i].attributes);

            if (GUILayout.Button("Delete Asset", GUILayout.ExpandWidth(false)))
            {
                Debug.Log("Asset Deleted");
                tokenList.RemoveAt(i);
            }

            GUILayout.EndVertical();
            GUILayout.Label("");
        }


        if (GUILayout.Button("Deploy as NFT"))
        {
            Debug.Log("Deploy as NFT");
            this.StartCoroutine(ImagesDeployer());
            //DeployToken();
        }

        GUILayout.EndVertical();
        GUILayout.Label("");
        GUILayout.Label("In Game Features", EditorStyles.boldLabel);
        includeWalletLoginWidgetInGame = EditorGUILayout.Toggle("Wallet Login Widget", includeWalletLoginWidgetInGame);

    }

    private void NFTReedemer()
    {
        Debug.Log("NFT reedemer");
    }

    //passare da DeployObject a DeployNFT
    private IEnumerator ImagesDeployer()
    {

        WWWForm form = new WWWForm();

        for (int i = 0; i < tokenList.Count; i++)
        {
            Texture2D decopmpresseTex = DeCompress(tokenList[i].image);
            var bytes = decopmpresseTex.EncodeToPNG();
            form.AddBinaryData("file", bytes, "imageId" + i + ".png", "image/png");
        }

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
            Debug.Log(www.result);
            Debug.Log(www.downloadHandler.text);
            //dynamic objects = JsonConvert.DeserializeObject(www.downloadHandler.text); // parse as array  
            //Debug.Log(objects.value.cid);


        }
    }

    //qui
    private void DeployToken()
    {
        Debug.Log("NFT Deployer called");
        //Debug.Log(tmp);
    }

    static Texture2D DeCompress(Texture2D source)
    {
        RenderTexture renderTex = RenderTexture.GetTemporary(
                    source.width,
                    source.height,
                    0,
                    RenderTextureFormat.Default,
                    RenderTextureReadWrite.Linear);

        Graphics.Blit(source, renderTex);
        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = renderTex;
        Texture2D readableText = new Texture2D(source.width, source.height);
        readableText.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
        readableText.Apply();
        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(renderTex);
        return readableText;
    }
}
