# YYAPIs Speech-to-Text

C# サンプル コンソールアプリ

## 実行環境

- \[Windows\] 10 以降

- \[[<u>Visual
  Studio</u>](https://visualstudio.microsoft.com/downloads/)\] 2022 以降

- \[[<u>.NET 7.0
  SDK</u>](https://dotnet.microsoft.com/en-us/download/visual-studio-sdks)\]

## 準備

**Visual StudioでC# サンプル コンソールアプリを開く**

任意のフォルダに **CsharpSampleConsole.zip** ファイルを解凍します。（例
. C:\Users\USERNAME\source）

Visual Studioを起動して、解答したフォルダの**CsharpSampleConsole.sln**
ファイルを開きます。

プロジェクトフォルダ CsharpSampleConsole に **Protos**
フォルダを作成します。

**protoファイルの設定**

\[[<u>開発者コンソール</u>](https://api-web.yysystem2021.com)\]から最新の
**yysystem.proto** ファイルをダウンロードします。

**フォルダの構造**
```
CsharpSampleConsole/ # ソリューションフォルダ
  CsharpSampleConsole.sln
  CsharpSampleConsole/　# プロジェクトフォルダ
    Protos/ # このフォルダを作成する
      yysystem.proto # ここに配置する
    …
  …
```

**ユーザーシークレットの設定**

ソリューションエクスプローラーで**CsharpSampleConsoleプロジェクトを右クリック**して、**ユーザーシークレットの管理**を選択します。エディタに
**secrets.json**
ファイルが開くので、次のテキストをコピーしてファイルに貼り付けます。

**secrets.json**

```
{
  "YYAPIS_ENDPOINT": "api-grpc-2.yysystem2021.com",
  "YYAPIS_PORT": "443",
  "YYAPIS_API_KEY": "YOUR_API_KEY",
  "YYAPIS_SSL": true
}
```

**YOUR_API_KEY** の値を開発者コンソールで発行した Speech-to-Text の API
キーに置き換えます。

## ビルドと実行

メニューバーからビルド \> ソリューションのビルド
を選択してビルドを実行します。

メニューバーからデバッグ \>
デバッグの開始を選択してアプリケーションを実行します。

コンソールアプリが起動し、YYAPIsのgRPCサーバーに接続されます。

コンソール画面に表示される指示に従って音声認識を実行します。

**コンソール画面**

```
Background task starts to listen response stream
Press any key to start recording...
```
