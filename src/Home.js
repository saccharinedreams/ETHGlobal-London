import { Card, Modal } from 'antd';
import {
  DynamicContextProvider,
  DynamicUserProfile,
  DynamicWidget,
  useDynamicContext,
} from "@dynamic-labs/sdk-react-core";

import { AlgorandWalletConnectors } from "@dynamic-labs/algorand";
import { BloctoEvmWalletConnectors } from "@dynamic-labs/blocto-evm";
import { Button } from "antd";
import { CosmosWalletConnectors } from "@dynamic-labs/cosmos";
import { EthereumWalletConnectors } from "@dynamic-labs/ethereum";
import { FlowWalletConnectors } from "@dynamic-labs/flow";
import { IDKit } from "@worldcoin/idkit-standalone";
import { IDKitWidget } from "@worldcoin/idkit";
import { MagicWalletConnectors } from "@dynamic-labs/magic";
import { SolanaWalletConnectors } from "@dynamic-labs/solana";
import { StarknetWalletConnectors } from "@dynamic-labs/starknet";
import { useState } from "react";

function mintNFT(name, description, file_url, address) {
  alert(`Minting your ${name} NFT, please wait up to 30s...`)
  console.log(process.env.REACT_APP_NFTPORT_KEY);
  const options = {
    method: "POST",
    headers: {
      accept: "application/json",
      "content-type": "application/json",
      Authorization: process.env.REACT_APP_NFTPORT_KEY,
    },
    body: JSON.stringify({
      chain: "goerli",
      name: name,
      description: description,
      file_url: file_url,
      mint_to_address: address,
    }),
  };

  fetch("https://api.nftport.xyz/v0/mints/easy/urls", options)
    .then((response) => response.json())
    .then((response) => {
      console.log(response);
      alert(response.transaction_external_url);
      window.open(response.transaction_external_url);
    })
    .catch((err) => console.error(err));
}

const roles = [
  {
    name: "Pixel The NFT Collector",
    role: "Tactical Summoner",
    image_url:
      "https://media.discordapp.net/attachments/1182787621370478593/1216115889171070976/vr3.png?ex=65ff372e&is=65ecc22e&hm=25e5b3c57f358b97fe0f8c74058ee10075aadc68c8dbc91cce7b37547c266324&=&format=webp&quality=lossless&width=819&height=819",
    description:
      "A master of the digital canvas, Pixel wields the power of uniqueness to control the battlefield. By manifesting the essence of her collected artworks, she can summon clones to distract and overwhelm the enemy, turning the tide of battle with the stroke of a brush.      ",
    primaryAbility: {
      name: "Mint Condition",
      description:
        "Pixel summons a temporary NFT from the connected wallet that attracts enemy attention, providing strategic advantages or crucial moments to reposition and attack.         ",
    },
    ultimateAbility: {
      name: "Masterpiece",
      description:
        "Pixel releases a devastatingly powerful effect that sweeps across enemies in its path, afflicting enemies with Proof of Weakness directly correlated to the rarity of NFTs the user owns, embodying the rare and unmatched power of a true digital masterpiece.        ",
    },
  },
  {
    name: "Ledger The Market Master",
    role: "Tactical Economist",
    image_url:
      "https://media.discordapp.net/attachments/1182787621370478593/1216115889586573472/vr2.png?ex=65ff372e&is=65ecc22e&hm=a77fa649561061e06929fa25ae84bf48d76b58080f347d5e60b1297847a726f6&=&format=webp&quality=lossless&width=819&height=819",

    description:
      "A master of the financial fray, Ledger the Market Master harnesses the tumultuous power of market dynamics to dominate the battlefield. By channeling the volatile essence of cryptocurrency and the strategic depth of decentralized finance, he manipulates the field's economic undercurrents. With a trader's insight and a strategist's foresight, he crafts scenarios that bolster allies and confound foes, turning the tide of battle with the flick of a coin.      ",
    primaryAbility: {
      name: "Market Momentum",
      description:
        "Ledger taps into the current state of real-world financial markets to dynamically alter the battlefield. If the market is bullish, he unleashes a forceful wave that propels enemies away, embodying the surge of a rising economy. In a bearish market, he ensnares foes in a decelerating trap, mirroring the slowdown of a declining market.         ",
    },
    ultimateAbility: {
      name: "Circuit Breaker",
      description:
        "Ledger slows and weakens enemies while enhancing his own resilience, simulating the effects of a market downturn and strategic asset lock-in for an overwhelming battlefield advantage.        ",
    },
  },
  {
    name: "Block The Builder",
    role: "Defensive Engineer",
    image_url:
      "https://media.discordapp.net/attachments/1182787621370478593/1216115889854877736/vr1.png?ex=65ff372e&is=65ecc22e&hm=c72ac85af1f0a4cd7f60ff9b069949247bf6185ea051f811c26e9be737717db4&=&format=webp&quality=lossless&width=819&height=819",

    description:
      "The architects of the digital world, Builders construct the very fabric of the battlefield. With a blueprint for every occasion, they erect barriers and structures that mend themselves over time, providing relentless defense against the onslaught of enemies.",
    primaryAbility: {
      name: "Code Deploy",
      description:
        "Block quickly constructs defensive barriers or structures that automatically repair themselves, symbolizing the ongoing development and maintenance of blockchain projects.",
    },
    ultimateAbility: {
      name: "Genesis Block",
      description:
        "Block summons a colossal blockchain structure that temporarily nullifies all incoming attacks, offering a brief sanctuary in the heat of battle.",
    },
  },
];

const Home = () => {
  const { primaryWallet } = useDynamicContext();

  const [isVerified, setIsVerified] = useState(false);

  function selectClass(classNumber) {
    if (isVerified === false) {
      alert("Please verify with WorldID first to ensure uniqueness!");
    } else {
      mintNFT(
        roles[classNumber].name,
        roles[classNumber].description,
        roles[classNumber].image_url,
        primaryWallet.address
      );
    }
  }
  const onSuccess = (data) => {
    setIsVerified(true);
  };
  const [isModalVisible, setIsModalVisible] = useState(false);
  const [selectedRole, setSelectedRole] = useState(null);
  const showModal = (role) => {
    setSelectedRole(role);
    setIsModalVisible(true);
  };

  const handleOk = () => {
    setIsModalVisible(false);
  };

  const handleCancel = () => {
    setIsModalVisible(false);
  };
  return (
    <div
      style={{
        display: "flex",
        flexDirection: "column",
        alignItems: "center",
      }}
    >
      <img style={{borderRadius: 50}} src="https://cdn.discordapp.com/attachments/1182787621370478593/1218590354920702073/benliang_Cartoony_bear_wearing_VR_headset_biting_ethereum_shard_2b02bbf6-af84-4e54-872c-fd0713ba223a.png?ex=660837b4&is=65f5c2b4&hm=593b03364b22ce76c71ab672d38b1f7d5b4d3a90c2ec67cacbfc07271f7442d6&" alt="Welcome Image" height={200} />
      <div>
        <p>Welcome to Super Smash Bears!</p>
      </div>
      <DynamicWidget />
      {primaryWallet && primaryWallet.address && (
        <div
          style={{
            display: "flex",
            flexDirection: "column",
            alignItems: "center",
          }}
        >
          <br />

          <p>Thanks for logging in!</p>
          <p>Connected wallet: {primaryWallet.address}</p>

          <IDKitWidget
            app_id="app_staging_dfbbb6c8ef7b17bae66db523a50c4b30" // obtained from the Developer Portal
            action="ethlondon_verify_uniqueness_7375" // this is your action name from the Developer Portal
            signal="user_value" // any arbitrary value the user is committing to, e.g. a vote
            onSuccess={onSuccess}
            verification_level="device" // minimum verification level accepted, defaults to "orb"
          >
            {({ open }) => (
              <Button onClick={open}>
                {isVerified ? "✅ You are verified!" : "Verify with World ID"}
              </Button>
            )}
          </IDKitWidget>
          <br />

          {isVerified && (
            <div>
              <p>Choose your class below:</p>

              <div style={{ display: "flex" }}>
                {roles.map((role, index) => (
                  <div
                    key={index}
                    onClick={() => showModal(role)}
                    style={{
                      maxWidth: "200px",
                      marginLeft: "10px",
                      cursor: "pointer",
                      flexDirection: "column",
                      justifyContent: "center",
                      border: "1px solid black",
                      padding: "10px",
                      borderRadius: "30px",
                    }}
                  >
                    <img
                      src={role.image_url}
                      alt={role.name}
                      style={{
                        maxWidth: "200px",
                        borderRadius: "100px",
                      }}
                    />
                    <div
                      style={{
                        display: "flex",
                        flexDirection: "column",
                        justifyContent: "center",
                      }}
                    >
                      <p>{role.name}</p>

                      <Button onClick={() => selectClass(index)}>Mint NFT</Button>
                    </div>
                  </div>
                ))}
                <Modal
                  title={selectedRole?.name}
                  visible={isModalVisible}
                  onOk={handleOk}
                  onCancel={handleCancel}
                >
                  <Card
                    hoverable
                    cover={<img alt={selectedRole?.name} src={selectedRole?.image_url} />}
                  >
                    <Card.Meta
                      title={selectedRole?.role}
                      description={selectedRole?.description}
                    />
                    <p>
                      Primary Ability: {selectedRole?.primaryAbility.name} -{" "}
                      {selectedRole?.primaryAbility.description}
                    </p>
                    <p>
                      Ultimate Ability: {selectedRole?.ultimateAbility.name} -{" "}
                      {selectedRole?.ultimateAbility.description}
                    </p>
                  </Card>
                </Modal>
              </div>
            </div>
          )}
        </div>
      )}
    </div>
  );
};

export default Home;
