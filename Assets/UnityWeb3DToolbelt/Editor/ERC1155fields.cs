using System;

// using Unity.EditorCoroutines.Editor;

public class ERC1155fields
{
    String name;
    String description;
    String image;
    KeyValuePair<TKey, TValue> attributes;

    public ERC1155fields(String networkName, String rpcUrl, int chainID, String symbol, String explorer)
    {
        this.networkName = networkName;
        this.rpcUrl = rpcUrl;
        this.chainID = chainID;
        this.symbol = symbol;
        this.explorer = explorer;
    }
}