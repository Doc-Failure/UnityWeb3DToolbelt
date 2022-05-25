using System;
using System.Collections;
using System.Collections.Generic;

class Networks
{
    Dictionary<ENetworks, Network> networksList = new Dictionary<ENetworks, Network>();

    public Networks()
    {
        Network AuroraTestnet = new Network("Aurora Testnet", "https://testnet.aurora.dev/", 1313161555, "ETH", "https://explorer.testnet.aurora.dev/");
        Network PolygonTestnet = new Network("Polygon Testnet (Mumbai)", "https://matic-mumbai.chainstacklabs.com", 80001, "matic", "https://mumbai.polygonscan.com/");
        Network RynkebyTestnet = new Network("Rinkeby Testnet (Mumbai)", "https://rinkeby.infura.io/v3/", 4, "ETH", "https://rinkeby.etherscan.io");
        Network BscTestnet = new Network("Binance Testnet", "https://data-seed-prebsc-1-s1.binance.org:8545/", 97, "BNB", "https://testnet.bscscan.com");
        Network AvalancheTestnet = new Network("Avalanche FUJI C-Chain", "https://api.avax-test.network/ext/bc/C/rpc", 43113, "AVAX", "https://testnet.snowtrace.io/");

        networksList.Add(ENetworks.AuroraTestnet, AuroraTestnet);
        networksList.Add(ENetworks.PolygonTestnet, PolygonTestnet);
        networksList.Add(ENetworks.BscTestnet, BscTestnet);
        networksList.Add(ENetworks.AvalancheTestnet, AvalancheTestnet);
        networksList.Add(ENetworks.RynkebyTestnet, RynkebyTestnet);
    }

    public Network GetNetwork(ENetworks networkId)
    {
        return networksList[networkId];
    }

    public int GetChainId(ENetworks networkId)
    {
        Network network = networksList[networkId];
        return network.GetChainID();
    }
}