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
    ENetworks index;
    string privateKey;
    string ContractName;
    string account;
    /* 
        NFTDeployment deployContract; */

    string NFTStorageBearerApi;
    GameObject redeemObjTrigger;
    Texture2D texture;
    float MintPercentage = 0.0f;
    float MintQuantity = 0.0f;
    string tokenName;
    string tokenSymbol;
    bool includeWalletLoginWidgetInGame = true;
    Texture2D objectToDeploy;
    Texture2D objtdeploy;
    bool chainLinkRandomMinterOption = false;
    string tmp;

    List<ERC1155Metadata> tokenList = new List<ERC1155Metadata>();
    private string ERC1155ImagesCID = "";
    private string ERC1155CID = "";

    Networks networksList = new Networks();
    UnityEngine.Vector2 scrollPos;
    byte[] rawData;
    [MenuItem("Web3D Toolbelt Tools/NFT Deployer")]
    public static void ShowWindow()
    {
        GetWindow(typeof(UnityWeb3DToolbelt), true, "NFT Deployer");      //GetWindow is a method inherited from the EditorWindow class
    }

    void OnGUI()
    {
        GUILayout.BeginVertical();
        scrollPos = GUILayout.BeginScrollView(scrollPos);
        GUILayout.BeginVertical(EditorStyles.helpBox);
        index = (ENetworks)EditorGUILayout.EnumPopup("Deploy to:", index);
        GUILayout.EndVertical();
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
        ContractName = EditorGUILayout.TextField("Contract Name", ContractName);
        GUILayout.Label("");
        GUILayout.Label("ChainLink Integrated Features", EditorStyles.boldLabel);
        //GUILayout.BeginHorizontal(EditorStyles.toggleGroup);
        chainLinkRandomMinterOption = EditorGUILayout.BeginToggleGroup("VRF Token Minter", chainLinkRandomMinterOption);
        if (chainLinkRandomMinterOption)
        {
            MintPercentage = EditorGUILayout.FloatField("Probability to mint (%)", MintPercentage);
            MintQuantity = EditorGUILayout.FloatField("Quantity to mint", MintQuantity);
        }
        EditorGUILayout.EndToggleGroup();
        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.BeginToggleGroup("VRF Attributes Value", false);
        EditorGUILayout.EndToggleGroup();
        EditorGUI.EndDisabledGroup();
        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.BeginToggleGroup("Chainlink Data Feed", false);
        EditorGUILayout.EndToggleGroup();
        EditorGUI.EndDisabledGroup();

        GUILayout.Label("");
        if (GUILayout.Button("Add Asset", GUILayout.ExpandWidth(false)))
        {
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
                tokenList.RemoveAt(i);
            }

            GUILayout.EndVertical();
            GUILayout.Label("");
        }


        if (GUILayout.Button("Deploy as NFT"))
        {
            this.StartCoroutine(ImagesDeployer());
            this.StartCoroutine(MetadataDeployer());
            this.StartCoroutine(DeployToken());
        }
        if (GUILayout.Button("Download ABI"))
        { }
        GUILayout.EndVertical();

        GUILayout.Label("");
        EditorGUI.BeginDisabledGroup(true);
        GUILayout.BeginVertical(EditorStyles.helpBox);
        GUILayout.Label("Register new Upkeep", EditorStyles.boldLabel);
        EditorGUILayout.TextField("Email Address", tmp);
        EditorGUILayout.TextField("Upkeep name", tmp);
        EditorGUILayout.TextField("Upkeep address", tmp);
        EditorGUILayout.TextField("Admin address", tmp);
        EditorGUILayout.TextField("Gas limit address", tmp);
        EditorGUILayout.TextField("Starting Balance (Link)", tmp);
        EditorGUI.EndDisabledGroup();
        GUILayout.Label("");
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Register Upkeep"))
        { }
        if (GUILayout.Button("Cancel"))
        { }
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();

        GUILayout.EndScrollView();
        GUILayout.EndVertical();
    }

    //passare da DeployObject a DeployNFT
    private IEnumerator ImagesDeployer()
    {
        WWWForm form = new WWWForm();

        for (int i = 0; i < tokenList.Count; i++)
        {
            Texture2D decopmpresseTex = DeCompress(tokenList[i].image);
            var bytes = decopmpresseTex.EncodeToPNG();
            form.AddBinaryData("file", bytes, "image_ID_" + i + ".png", "image/png");
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
            dynamic objects = JsonConvert.DeserializeObject<dynamic>(www.downloadHandler.text);
            ERC1155ImagesCID = objects["value"]["cid"].ToString();
        }
    }

    private IEnumerator MetadataDeployer()
    {
        WWWForm form = new WWWForm();

        //'meta=\'{"image":null,"name":"Storing the Worlds Most Valuable Virtual Assets with NFT.Storage","description":"The metaverse is here. Where is it all being stored?","properties":{"type":"blog-post","origins":{"http":"https://nft.storage/blog/post/2021-11-30-hello-world-nft-storage/","ipfs":"ipfs://bafybeieh4gpvatp32iqaacs6xqxqitla4drrkyyzq6dshqqsilkk3fqmti/blog/post/2021-11-30-hello-world-nft-storage/"},"authors":[{"name":"David Choi"}],"content":{"text/markdown":"The last year has witnessed the explosion of NFTs onto the world’s mainstage. From fine art to collectibles to music and media, NFTs are quickly demonstrating just how quickly grassroots Web3 communities can grow, and perhaps how much closer we are to mass adoption than we may have previously thought. <... remaining content omitted ...>"}}}\''

        for (int i = 0; i < tokenList.Count; i++)
        {
            string tokenMeta = "{";
            tokenMeta += "\"name\":\"" + tokenList[i].name + "\",";
            tokenMeta += "\"description\":\"" + tokenList[i].description + "\",";
            tokenMeta += "\"image\":\"https://" + ERC1155ImagesCID + ".ipfs.nftstorage.link/image_ID_" + i + ".png\",";
            tokenMeta += "\"attributes\":" + tokenList[i].attributes + "}";
            byte[] bodyRaw = Encoding.UTF8.GetBytes(tokenMeta);
            form.AddBinaryData("file", bodyRaw, "meta_ID_" + i + ".json", "application/json'");
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
            dynamic objects = JsonConvert.DeserializeObject<dynamic>(www.downloadHandler.text);
            ERC1155CID = objects["value"]["cid"].ToString();
        }
    }

    //qui
    private IEnumerator DeployToken()
    {
        string nameTokenList = "";
        for (int i = 0; i < tokenList.Count; i++)
        {
            nameTokenList += tokenList[i].name;
            if (i < tokenList.Count - 1)
            {
                nameTokenList += ",";
            }
        }
        string request = "http://localhost:8080/generic-webhook-trigger/invoke?NAME=" + ContractName + "&TOKENS_LIST=" + nameTokenList + "&IPFS_ENDPOINT=https://" + ERC1155CID + ".ipfs.nftstorage.link&MINTING_PROBABILITY=" + MintPercentage + "&MINTING_QUANTITY=" + MintQuantity + "&CHAIN_ID=" + networksList.GetChainId(index);
        UnityWebRequest www = UnityWebRequest.Get(request);
        yield return www.SendWebRequest();

        if (www.isNetworkError)
        {
            Debug.Log("Error While Sending: " + www.error);
        }
        else
        {
            Debug.Log("NFT Generation in progress");
        }
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
