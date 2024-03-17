using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MetaMask.Cryptography;
using MetaMask.Models;
using MetaMask.SocketIOClient;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.Networking;
namespace MetaMask.Unity.Samples
{

    public class MetaMaskDemo : MonoBehaviour
    {

        #region Events

        /// <summary>Raised when the wallet is connected.</summary>
        public event EventHandler onWalletConnected;
        /// <summary>Raised when the wallet is disconnected.</summary>
        public event EventHandler onWalletDisconnected;
        /// <summary>Raised when the wallet is ready.</summary>
        public event EventHandler onWalletReady;
        /// <summary>Raised when the wallet is paused.</summary>
        public event EventHandler onWalletPaused;
        /// <summary>Raised when the user signs and sends the document.</summary>
        public event EventHandler onSignSend;
        /// <summary>Occurs when a transaction is sent.</summary>
        public event EventHandler onTransactionSent;
        public event EventHandler openAlert;
        public event EventHandler onNFTFetch;
        public event EventHandler gameStartFailed;


        /// <summary>Raised when the transaction result is received.</summary>
        /// <param name="e">The event arguments.</param>
        public event EventHandler<MetaMaskEthereumRequestResultEventArgs> onTransactionResult;

        #endregion

        #region Fields

        /// <summary>The configuration for the MetaMask client.</summary>
        [SerializeField]
        protected MetaMaskConfig config;

        [SerializeField]
        protected string ConnectAndSignMessage = "Connect and Sign from Super Smash Bears using MetaMask Unity SDK!";

        public GameObject tokenList;
        public GameObject mainMenu;
        public GameObject nftList;
        
        private GameObject currentUI;

        private bool nftExists = false;

        #endregion

        #region Unity Methods

        /// <summary>Initializes the MetaMaskUnity instance.</summary>
        private void Awake()
        {
            this.currentUI = mainMenu;
            
            MetaMaskUnity.Instance.Initialize();
            MetaMaskUnity.Instance.Events.WalletAuthorized += walletConnected;
            MetaMaskUnity.Instance.Events.WalletDisconnected += walletDisconnected;
            MetaMaskUnity.Instance.Events.WalletReady += walletReady;
            MetaMaskUnity.Instance.Events.WalletPaused += walletPaused;
            MetaMaskUnity.Instance.Events.EthereumRequestResultReceived += TransactionResult;
            ConnectAndSign();
        }

        private void OnDisable()
        {
            MetaMaskUnity.Instance.Events.WalletAuthorized -= walletConnected;
            MetaMaskUnity.Instance.Events.WalletDisconnected -= walletDisconnected;
            MetaMaskUnity.Instance.Events.WalletReady -= walletReady;
            MetaMaskUnity.Instance.Events.WalletPaused -= walletPaused;
            MetaMaskUnity.Instance.Events.EthereumRequestResultReceived -= TransactionResult;
        }
        
        #endregion

        #region Event Handlers

        /// <summary>Raised when the transaction result is received.</summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        public void TransactionResult(object sender, MetaMaskEthereumRequestResultEventArgs e)
        {
            UnityThread.executeInUpdate(() => { onTransactionResult?.Invoke(sender, e); });
        }

        /// <summary>Raised when the wallet is connected.</summary>
        private void walletConnected(object sender, EventArgs e)
        {
            UnityThread.executeInUpdate(() =>
            {
                onWalletConnected?.Invoke(this, EventArgs.Empty);
            });
        }

        /// <summary>Raised when the wallet is disconnected.</summary>
        private void walletDisconnected(object sender, EventArgs e)
        {
            if (this.currentUI != null)
            {
                this.currentUI.SetActive(false);
            }
            
            onWalletDisconnected?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>Raised when the wallet is ready.</summary>
        private void walletReady(object sender, EventArgs e)
        {
            UnityThread.executeInUpdate(() =>
            {
                onWalletReady?.Invoke(this, EventArgs.Empty);
            });
        }
        
        /// <summary>Raised when the wallet is paused.</summary>
        private void walletPaused(object sender, EventArgs e)
        {
            UnityThread.executeInUpdate(() => { onWalletPaused?.Invoke(this, EventArgs.Empty); });
        }

        #endregion

        #region Public Methods

        public void OpenDeepLink() {
            if (MetaMask.Transports.Unity.UI.MetaMaskUnityUITransport.DefaultInstance != null)
            {
                MetaMask.Transports.Unity.UI.MetaMaskUnityUITransport.DefaultInstance.OpenConnectionDeepLink();
            }
        }

        /// <summary>Calls the connect method to the Metamask Wallet.</summary>
        public void Connect()
        {
            MetaMaskUnity.Instance.Connect();
        }

        public void ConnectAndSign()
        {
            MetaMaskUnity.Instance.ConnectAndSign("Connect and Sign from Super Smash Bears!");
        }

        /// <summary>Sends a transaction to the Ethereum network.</summary>
        /// <param name="transactionParams">The parameters of the transaction.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async void SendTransaction()
        {
            var transactionParams = new MetaMaskTransaction
            {
                To = MetaMaskUnity.Instance.Wallet.SelectedAddress,
                From = MetaMaskUnity.Instance.Wallet.SelectedAddress,
                Value = "0"
            };

            var request = new MetaMaskEthereumRequest
            {
                Method = "eth_sendTransaction",
                Parameters = new MetaMaskTransaction[] { transactionParams }
            };
            onTransactionSent?.Invoke(this, EventArgs.Empty);
            await MetaMaskUnity.Instance.Wallet.Request(request);

        }

        /// <summary>Signs a message with the user's private key.</summary>
        /// <param name="msgParams">The message to sign.</param>
        /// <exception cref="InvalidOperationException">Thrown when the application isn't in foreground.</exception>
        public async void Sign()
        {
            string msgParams = "{\"domain\":{\"chainId\":1,\"name\":\"Ether Mail\",\"verifyingContract\":\"0xCcCCccccCCCCcCCCCCCcCcCccCcCCCcCcccccccC\",\"version\":\"1\"},\"message\":{\"contents\":\"Hello, player!\",\"from\":{\"name\":\"Super Smash Bears\",\"wallets\":[\"0xCD2a3d9F938E13CD947Ec05AbC7FE734Df8DD826\",\"0xDeaDbeefdEAdbeefdEadbEEFdeadbeEFdEaDbeeF\"]},\"to\":[{\"name\":\"Bob\",\"wallets\":[\"0xbBbBBBBbbBBBbbbBbbBbbbbBBbBbbbbBbBbbBBbB\",\"0xB0BdaBea57B0BDABeA57b0bdABEA57b0BDabEa57\",\"0xB0B0b0b0b0b0B000000000000000000000000000\"]}]},\"primaryType\":\"Mail\",\"types\":{\"EIP712Domain\":[{\"name\":\"name\",\"type\":\"string\"},{\"name\":\"version\",\"type\":\"string\"},{\"name\":\"chainId\",\"type\":\"uint256\"},{\"name\":\"verifyingContract\",\"type\":\"address\"}],\"Group\":[{\"name\":\"name\",\"type\":\"string\"},{\"name\":\"members\",\"type\":\"Person[]\"}],\"Mail\":[{\"name\":\"from\",\"type\":\"Person\"},{\"name\":\"to\",\"type\":\"Person[]\"},{\"name\":\"contents\",\"type\":\"string\"}],\"Person\":[{\"name\":\"name\",\"type\":\"string\"},{\"name\":\"wallets\",\"type\":\"address[]\"}]}}";
            string from = MetaMaskUnity.Instance.Wallet.SelectedAddress;

            var paramsArray = new string[] { from, msgParams };

            var request = new MetaMaskEthereumRequest
            {
                Method = "eth_signTypedData_v4",
                Parameters = paramsArray
            };
            onSignSend?.Invoke(this, EventArgs.Empty);
            await MetaMaskUnity.Instance.Wallet.Request(request);
        }

        public async void BatchSend()
        {
            var metaMask = MetaMaskUnity.Instance.Wallet;
            
            var batch = metaMask.BatchRequests();
    
            string from = metaMask.SelectedAddress;
            List<Task<string>> requests = new List<Task<string>>();
            for (var i = 0; i < 10; i++)
            {
                var paramsArray = new string[] { Encoding.UTF8.GetBytes($"Some data {i+1}/10").ToHex(), from };
                requests.Add(batch.Request<string>("personal_sign", paramsArray));
            }

            await batch.Send();

            foreach (var task in requests)
            {
                var response = await task;
                Debug.Log(response);
            }
        }

        public void Disconnect()
        {
            MetaMaskUnity.Instance.Disconnect();
        }

        public void EndSession()
        {
            //MetaMaskUnity.Instance.Disconnect(true);
            MetaMaskUnity.Instance.EndSession();
        }

        public void ShowTokenList()
        {
            SetCurrentMenu(tokenList);
        }

        public void ShowMainMenu()
        {
            SetCurrentMenu(mainMenu);
        }

        public void ShowNftList()
        {
            SetCurrentMenu(nftList);
        }

        public void StartGame()
        {
            if (true) // change to nftExists
            {
                Debug.Log("Game started");
                SceneManager.LoadScene("Super Smash Bears");
            }
            else
            {
                Debug.Log("No Super Smash Bear NFTs found. Please obtain one to play the game.");
                gameStartFailed?.Invoke(this, EventArgs.Empty);

            }
        }

        public void GameStart()
        {
            SceneManager.LoadScene("Super Smash Bears");
        }

        public void StartGetNFTs()
        {
            StartCoroutine(GetNFTs());
        }

        IEnumerator GetNFTs()
        {
            // Replace 'YOUR_ALCHEMY_API_KEY' with your actual Alchemy API key
            string url = "https://polygon-mainnet.g.alchemy.com/nft/v2/374l9-eucheJf7r_lvnEZbEJ3dmtKRqn/getNFTs?owner=0x0E5d299236647563649526cfa25c39d6848101f5&withMetadata=true&pageSize=100";

            using (UnityWebRequest request = UnityWebRequest.Get(url))
            {
                request.SetRequestHeader("accept", "application/json");

                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError("Error: " + request.error);
                }
                else
                {
                    Debug.Log("Received: " + request.downloadHandler.text);
                    nftExists = true;
                    onNFTFetch?.Invoke(this, EventArgs.Empty);

                    // Here you can process the JSON response as needed
                }
            }
        }

        public void StartGetNFTs(Action onComplete)
        {
            StartCoroutine(GetNFTsCoroutine(onComplete));
        }

        private IEnumerator GetNFTsCoroutine(Action onComplete)
        {
            // Simulate asynchronous work, e.g., web request
            yield return new WaitForSeconds(2); // Simulating delay

            // Once done, invoke the callback
            onComplete?.Invoke();
        }

        #endregion

        #region Private Methods

        private void SetCurrentMenu(GameObject menu)
        {
            if (this.currentUI != null)
            {
                this.currentUI.SetActive(false);
            }
            
            menu.SetActive(true);
            this.currentUI = menu;
        }
        #endregion
    }

}