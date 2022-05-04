using System;

// using Unity.EditorCoroutines.Editor;

public class Network
{
    String networkName;
    String rpcUrl;
    int chainID;
    String symbol;
    String explorer;

    public Network(String networkName, String rpcUrl, int chainID, String symbol, String explorer)
    {
        this.networkName = networkName;
        this.rpcUrl = rpcUrl;
        this.chainID = chainID;
        this.symbol = symbol;
        this.explorer = explorer;
    }

    public String GetNetworkName()
    {
        return networkName;
    }
    public String GetRpcUrl()
    {
        return rpcUrl;
    }
    public String GetSymbol()
    {
        return symbol;
    }
    public String GetExplorer()
    {
        return explorer;
    }
    public int GetChainID()
    {
        return chainID;
    }


}
