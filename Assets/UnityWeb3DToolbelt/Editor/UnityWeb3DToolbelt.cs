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
    GameObject objtdeploy;

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
        objtdeploy = EditorGUILayout.ObjectField("Object to aaaaaa", objtdeploy, typeof(GameObject), true, GUILayout.Height(EditorGUIUtility.singleLineHeight)) as GameObject;

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




    /*  private IEnumerator RetrieveImage()
     {
         Debug.Log("deployContract.TokenUri: " + deployContract.TokenUri);

         UnityWebRequest request = UnityWebRequestTexture.GetTexture(deployContract.TokenUri);
         yield return request.SendWebRequest();
         if (request.isNetworkError || request.isHttpError)
             Debug.Log(request.error);
         else
         {
             rawData = ((DownloadHandlerTexture)request.downloadHandler).data;
         } */
    /* var request = new UnityWebRequest("http://localhost:19888/rpc", "POST");
string bodyJsonString = "{\"jsonrpc\":\"2.0\",\"method\":\"edgestore.GetFile\",\"params\":[{\"key\": \"0xdacc9a23035a458f21aa0cb51189d715cb5c43d7ff4c0227cca5c25eeef3d5b4\"}],\"id\":1}";
    byte[] bodyRaw = Encoding.UTF8.GetBytes(bodyJsonString);
    request.uploadHandler = (UploadHandler) new UploadHandlerRaw(bodyRaw);
    request.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer();
    request.SetRequestHeader("Content-Type", "application/json");
    yield return request.SendWebRequest();
    Debug.Log("Status Code: " + request.responseCode);*/
    /*       showMeTheNFT = true;
      } */



    /* private IEnumerator DeployObject()
    {
        var request = new UnityWebRequest("https://api.nft.storage/upload", "POST");
 */
    /*   string filePath = Path.Combine(Directory.GetCurrentDirectory(), AssetDatabase.GetAssetPath(objectToDeploy));
      string bodyJsonString = "{\"jsonrpc\":\"2.0\",\"method\":\"edgestore.PutFile\",\"params\":[{\"path\": \"" + filePath + "\"}],\"id\":2}"; */

    //byte[] bodyRaw = objectToDeploy.GetRawTextureData();
    //string filePath = Path.Combine(Directory.GetCurrentDirectory(), AssetDatabase.GetAssetPath(objectToDeploy));
    //byte[] bodyRaw = Encoding.UTF8.GetBytes(objectToDeploy.GetRawTextureData());
    /*   string filePath = Path.Combine(Directory.GetCurrentDirectory(), AssetDatabase.GetAssetPath(objectToDeploy));
      request.uploadHandler = (UploadHandler)new UploadHandlerRaw(filePath);
      request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
      request.SetRequestHeader("Authorization", "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJkaWQ6ZXRocjoweEI5RTEyMGFhRjQyMWNGNzJiNzAxOEU3ZUFlRDljNWYwOTBERDYxOGQiLCJpc3MiOiJuZnQtc3RvcmFnZSIsImlhdCI6MTY1MjI2MjQ3NDY0MSwibmFtZSI6Ik5GVC1VUCJ9.e8LUK0K_cabscL1gEkrgF7dekuDzP7Hn3QxZjPFHHPM");
      request.SetRequestHeader("accept", "application/json");
      request.SetRequestHeader("Content-Type", "multipart/form-data");
      yield return request.SendWebRequest();

      var data = request.downloadHandler.text;
      Debug.Log(data.ToString()); */

    //asdawer213edasd23erdasxcddasd1234awd

    //Loado immagini da Chain e interagisco in gioco x esempio chainlink.
    /* 
            dynamic objects = JsonConvert.DeserializeObject(data); // parse as array  
            string tokenUri = "http://localhost:8080/api/v1/file?key=" + objects.result.key + "&relpath=" + objects.result.relpath;

            var url = "https://eth-rpc-api-testnet.thetatoken.org/rpc";
     */
    //https://api.nft.storage

    /*   } */

    //passare da DeployObject a DeployNFT
    private IEnumerator DeployNFT()
    {

        Dictionary<string, string> headers = new Dictionary<string, string>();
        headers.Add("accept", "application/json");
        headers.Add("Authorization", "Bearer TBD");
        //headers.Add("Content-Type", "multipart/form-data");
        //WWW www = new WWW("https://example.com", null, headers);
        //string filePath = Path.Combine(Directory.GetCurrentDirectory(), AssetDatabase.GetAssetPath(objectToDeploy));
        WWWForm form = new WWWForm();
        byte[] bodyRaw = objectToDeploy.GetRawTextureData();
        string utf8String = Encoding.UTF8.GetBytes(bodyRaw);
        var bytes = Encoding.ASCII.GetBytes(utf8String);
        //byte[] textureBytes = objectToDeploy.EncodeToPNG());
        form.AddBinaryData("file", bytes, "foto_profilo.jpeg", "image/jpeg");
        //form.AddBinaryData("file", bytes);
        //form.AddField("file", @"/Users/conve/Documents/foto_profilo.jpeg");
        //this.bytes = Encoding.UTF8.GetBytes( JSON.JsonEncode( data ) );

        //UnityWebRequest www = UnityWebRequest.Post("https://api.nft.storage/upload", form, headers);
        //byte[] rawData = form.data;
        //byte[] bodyRaw = objectToDeploy.GetRawTextureData();
        //string filePath = Path.Combine(Directory.GetCurrentDirectory(), AssetDatabase.GetAssetPath(objectToDeploy));
        //byte[] bodyRaw = Encoding.UTF8.GetBytes(objectToDeploy.GetRawTextureData());
        //string filePath = Path.Combine(Directory.GetCurrentDirectory(), AssetDatabase.GetAssetPath(objectToDeploy));
        //request.uploadHandler = (UploadHandler)new UploadHandlerRaw(rawData);
        //request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();


        WWW www = new WWW("https://api.nft.storage/upload", form.data, headers);
        yield return www;
        Debug.Log(www.text);






        /*  List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
         formData.Add(new MultipartFormDataSection("file", "@/Users/conve/Documents/foto_profilo.jpeg")); */
        //("file=;type=image/jpeg"));
        //List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        //formData.Add(new MultipartFormDataSection("file=@/Users/conve/Documents/foto_profilo.jpeg;type=image/jpeg"));
        //formData.Add(new MultipartFormFileSection("my file data", "myfile.txt"));
        /* string filePath = Path.Combine(Directory.GetCurrentDirectory(), AssetDatabase.GetAssetPath(objectToDeploy)); */
        //form.AddField("file", "@CV-Europass-20190513-Convertino-EN.pdf;type=application/pdf");
        //var bytes = System.Text.Encoding.UTF8.GetBytes();
        //-F 'file=@CV-Europass-20190513-Convertino-EN.pdf;type=application/pdf' \
        //string bodyJsonString = "'file=@/Users/conve/Documents/foto_profilo.jpeg;type=images/*'";
        //UnityWebRequest www = UnityWebRequest("https://api.nft.storage/upload", "POST");
        /*   UnityWebRequest www = UnityWebRequest.Post("https://api.nft.storage/upload", formData); */

        //string bodyJsonString = "file=@/Users/conve/Documents/foto_profilo.jpeg;type=image/jpeg";
        /*  byte[] bodyRaw = Encoding.UTF8.GetBytes(bodyJsonString);
         www.uploadHandler = (UploadHandler)new UploadHandlerRaw(form); */

        /*   www.SetRequestHeader("Authorization", "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJkaWQ6ZXRocjoweEI5RTEyMGFhRjQyMWNGNzJiNzAxOEU3ZUFlRDljNWYwOTBERDYxOGQiLCJpc3MiOiJuZnQtc3RvcmFnZSIsImlhdCI6MTY1Mjk1ODY0MTE3NSwibmFtZSI6IlVuaXR5V2ViM0QifQ._SsnFEBT6Z1WjLQX7wPaD9dniy5LwhuBsfndo15ROu0");
          www.SetRequestHeader("accept", "application/json");
          www.SetRequestHeader("Content-Type", "multipart/form-data");
          //www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
          yield return www.SendWebRequest();

          Debug.Log("res: " + www.result);
          if (www.result != UnityWebRequest.Result.Success)
          {
              Debug.Log("error: " + www.error);
          }
          else
          {
              Debug.Log("Form upload complete!");
              Debug.Log("result: " + www.result);
          } */
    }
}
