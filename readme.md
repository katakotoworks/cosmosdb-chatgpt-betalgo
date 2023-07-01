---
page_type: sample
languages:
- csharp
products:
- azure-cosmos-db
- azure-openai
name: Sample chat app using Azure Cosmos DB for NoSQL and Azure OpenAI
urlFragment: chat-app
description: Sample application that implements multiple chat threads using the Azure OpenAI "text-davinci-003" model and Azure Cosmos DB for NoSQL for storage.
azureDeploy: https://raw.githubusercontent.com/katakotoworks/cosmosdb-chatgpt-betalgo/main/azuredeploy.json
---

# 本家からの変更点

 - OpenAI社のサービスをBetalgo経由で利用するように変更
 - リポジトリの参照先を変更

# Azureへのデプロイ方法(1)

 - Azureにアカウントとサブスクリプションを作成します。
 - リソースグループ"resource_group_1"をリージョン"(US) South Central US"に作成します
 - クライアントPCにAzuer CLIをセットアップします
 - 以下のコマンドを実行します
```
az deployment group create --resource-group resource_group_1 --template-file ./azuredeploy.bicep
```
 - Azureに作成されたApp Serviceリソースの「設定」→「構成」→"OPENAI_KEY"の値を、OpenAI社のChatGPTサービスから取得したAPIキーの値に変更します
 - Azureに作成されたApp Serviceリソースの「概要」→「既定のドメイン」をクリックし、システムにアクセスします

# Azureへのデプロイ方法(2)

 - Azureにアカウントとサブスクリプションを作成します。
 - 以下のDeploy to Azureボタンを押下して画面の指示に従います。

[![Deploy to Azure](https://aka.ms/deploytoazurebutton)](https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2Fkatakotoworks%2Fcosmosdb-chatgpt-betalgo%2Fmain%2Fazuredeploy.json)

 - Azureに作成されたApp Serviceリソースの「設定」→「構成」→"OPENAI_KEY"の値を、OpenAI社のChatGPTサービスから取得したAPIキーの値に変更します
 - Azureに作成されたApp Serviceリソースの「概要」→「既定のドメイン」をクリックし、システムにアクセスします

# ローカルでの起動方法

 - Azureにアカウントとサブスクリプションを作成します。
 - リソースグループ"resource_group_1"をリージョン"(US) South Central US"に作成します
 - クライアントPCにAzuer CLIをセットアップします
 - 以下のコマンドを実行します
```
az deployment group create --resource-group resource_group_1 --template-file ./azuredeploy.bicep
```
 - ローカルにVisual Studio 2022をインストールします(ASP.NETの開発環境を選ぶこと)。
 - チェックアウトしたリポジトリに含まれる`cosmoschatgpt.sln`を開きます
 - `appsettings.json`の`OpenAi.Key`の値を、OpenAI社のChatGPTサービスから取得したAPIキーの値に変更します
 - ツールバーの`[三角]cosmosdbchat`をクリックし、実行します。

Azure上にCosmosDBリソースが作成されています。また、利用しないApp Serviceリソースも作成されていますので適宜削除などしてください。

# 前提条件

 - サブスクリプション上にCosmosDBのインスタンスが存在しないこと(Azureの無料サブスクリプションだと、CosmosDBのインスタンスが1つしか作成できないため)
 - OpenAI社のChatGPTサービスのアカウントを所有していること


# Azure Cosmos DB + OpenAI ChatGPT

This sample application combines Azure Cosmos DB with OpenAI ChatGPT with a Blazor Server front-end for an intelligent chat bot application that shows off how you can build a 
simple chat application with OpenAi ChatGPT and Azure Cosmos DB.

![Cosmos DB + ChatGPT user interface](screenshot.png)

## Features

This application has individual chat sessions which are displayed and can be selected in the left-hand nav. Clicking on a session will show the messages that contain
human prompts and AI completions. 

When a new prompt is sent to the Azure OpenAI service, some of the conversation history is sent with it. This provides context allowing ChatGPT to respond 
as though it is having a conversation. The length of this conversation history can be configured from appsettings.json 
with the `OpenAiMaxTokens` value that is then translated to a maximum conversation string length that is 1/2 of this value. 

Please note that the "gpt-35-turbo" model used by this sample has a maximum of 4096 tokens. Token are used in both the request and reponse from the service. Overriding the maxConversationLength to values approaching maximum token value could result in completions that contain little to no text if all of it has been used in the request.

The history for all prompts and completions for each chat session is stored in Azure Cosmos DB. Deleting a chat session in the UI will delete it's corresponding data as well.

The application will also summarize the name of the chat session by asking ChatGPT to provide a one or two word summary of the first prompt. This allows you to easily
identity different chat sessions.

Please note this is a sample application. It is intended to demonstrate how to use Azure Cosmos DB and Azure OpenAI ChatGPT together. It is not intended for production or other large scale use

## Getting Started

### Prerequisites

- Azure Subscription
- Subscription access to Azure OpenAI service. Start here to [Request Acces to Azure OpenAI Service](https://customervoice.microsoft.com/Pages/ResponsePage.aspx?id=v4j5cvGGr0GRqy180BHbR7en2Ais5pxKtso_Pz4b1_xUOFA5Qk1UWDRBMjg0WFhPMkIzTzhKQ1dWNyQlQCN0PWcu)
- Visual Studio, VS Code, or some editor if you want to edit or view the source for this sample.

### Installation

1. Fork this repository to your own GitHub account.
1. Depending on whether you deploy using the ARM Template or Bicep, modify this variable in one of those files to point to your fork of this repository, "webSiteRepository": "https://github.com/katakotoworks/cosmosdb-chatgpt-betalgo.git" 
1. If using the Deploy to Azure button below, also modify this README.md file to change the path for the Deploy To Azure button to your local repository.
1. If you deploy this application without making either of these changes, you can update the repository by disconnecting and connecting an external git repository pointing to your fork.

The provided ARM or Bicep Template will provision the following resources:
1. Azure Cosmos DB account with database and container at 400 RU/s. This can optionally be configured to run on the Cosmos DB free tier if available for your subscription.
1. Azure App service. This will be configured for CI/CD to your forked GitHub repository. This service can also be configured to run on App Service free tier.
1. Azure Open AI account. You must also specify a name for the deployment of the "text-davinci-003" model which is used by this application.

Note: You must have access to Azure Open AI service from your subscription before attempting to deploy this application.

All connection information for Azure Cosmos DB and Open AI is zero-touch and injected as environment variables in the Azure App Service instance at deployment time. 

[![Deploy to Azure](https://aka.ms/deploytoazurebutton)](https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2Fkatakotoworks%2Fcosmosdb-chatgpt-betalgo%2Fmain%2Fazuredeploy.json)

### Quickstart

1. After deployment, go to the resource group for your deployment and open the Azure App Service in the Azure Portal. Click the web url to launch the website.
1. Click + New Chat to create a new chat session.
1. Type your question in the text box and press Enter.

## Clean up

To remove all the resources used by this sample, you must first manually delete the deployed model within the Azure AI service. You can then delete the resource group for your deployment. This will delete all remaining resources.

## Resources

- [Azure Cosmos DB + Azure OpenAI ChatGPT Blog Post Announcement](https://devblogs.microsoft.com/cosmosdb/)
- [Azure Cosmos DB Free Trial](https://aka.ms/TryCosmos)
- [Open AI Platform documentation](https://platform.openai.com/docs/introduction/overview)
- [Azure Open AI Service documentation](https://learn.microsoft.com/azure/cognitive-services/openai/)
