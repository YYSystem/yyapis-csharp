// See https://aka.ms/new-console-template for more information
using System.Text;
using CsharpSampleConsole;
using Microsoft.Extensions.Configuration;

var builder = new ConfigurationBuilder();
var config = builder.AddUserSecrets("9362d98c-35ba-437a-9e82-aebe549d7c24").Build();

string endpoint = config["YYAPIS_ENDPOINT"];
string port = config["YYAPIS_PORT"];
bool ssl = bool.Parse(config["YYAPIS_SSL"]);
string apiKey = config["YYAPIS_API_KEY"];

Console.OutputEncoding = Encoding.UTF8;
var client = new YYSystemClient (endpoint: endpoint, port: port, ssl: ssl, apiKey: apiKey);
await client.startStream();
Console.WriteLine("Press any key to exit...");
Console.ReadKey(intercept: true);

