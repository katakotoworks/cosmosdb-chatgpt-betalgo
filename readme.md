# 概要

 - Microsoftの[Azure Cosmos DB + OpenAI ChatGPT](https://github.com/Azure-Samples/cosmosdb-chatgpt)サンプルを、Azure OpenAIではなく、OpenAI社のサービスを利用するように変更したものです。
 - Azure OpenAIは、エンラープライズ契約がないと利用できないため、個人のAzureサブスクリプションで同サンプルを動かすために作成しました。
 - ソースコードとしては、Azure OpenAIへのアクセスを責務とするサービス`OpenAiService.cs`のみ修正しており、他の箇所の修正はありません。
 - OpenAI社のサービスにアクセスする方法として、[Betalgo.OpenAI](https://github.com/betalgo/openai)を利用しています。

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

# クライアントPCでの起動方法

 - Azureにアカウントとサブスクリプションを作成します。
 - リソースグループ"resource_group_1"をリージョン"(US) South Central US"に作成します
 - クライアントPCにAzuer CLIをセットアップします
 - 以下のコマンドを実行します
```
az deployment group create --resource-group resource_group_1 --template-file ./azuredeploy.bicep
```
 - クライアントPCにVisual Studio 2022をインストールします(ASP.NETの開発環境を選ぶこと)。
 - チェックアウトしたリポジトリに含まれる`cosmoschatgpt.sln`を開きます
 - `appsettings.json`の`OpenAi.Key`の値を、OpenAI社のChatGPTサービスから取得したAPIキーの値に変更します
 - ツールバーの`[三角]cosmosdbchat`をクリックし、実行します。

Azure上にCosmosDBリソースが作成されています。また、利用しないApp Serviceリソースも作成されていますので適宜削除などしてください。

# 前提条件

 - サブスクリプション上にCosmosDBのインスタンスが存在しないこと(Azureの無料サブスクリプションだと、CosmosDBのインスタンスが1つしか作成できないため)
 - OpenAI社のChatGPTサービスのアカウントを所有していること

