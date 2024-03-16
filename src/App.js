import {
  DynamicContextProvider,
  DynamicUserProfile,
  DynamicWidget,
  useDynamicContext,
} from "@dynamic-labs/sdk-react-core";

import { AlgorandWalletConnectors } from "@dynamic-labs/algorand";
import { BloctoEvmWalletConnectors } from "@dynamic-labs/blocto-evm";
import { CosmosWalletConnectors } from "@dynamic-labs/cosmos";
import { EthereumWalletConnectors } from "@dynamic-labs/ethereum";
import { FlowWalletConnectors } from "@dynamic-labs/flow";
import Home from "./Home";
import { MagicWalletConnectors } from "@dynamic-labs/magic";
import { SolanaWalletConnectors } from "@dynamic-labs/solana";
import { StarknetWalletConnectors } from "@dynamic-labs/starknet";

const App = () => {
  return (
    <DynamicContextProvider
      settings={{
        environmentId: "8c70b21d-72a6-4cc5-88cf-e7e48f772dd9",
        walletConnectors: [
          EthereumWalletConnectors,
          AlgorandWalletConnectors,
          SolanaWalletConnectors,
          FlowWalletConnectors,
          StarknetWalletConnectors,
          CosmosWalletConnectors,
          MagicWalletConnectors,
          BloctoEvmWalletConnectors,
        ],
      }}
    >
      <div
        style={{
          display: "flex",
          justifyContent: "center",
          alignItems: "center",
          height: "100vh",
        }}
      >
        <Home></Home>
      </div>
    </DynamicContextProvider>
  );
};

export default App;
