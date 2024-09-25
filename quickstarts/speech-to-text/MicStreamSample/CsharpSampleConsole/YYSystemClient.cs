using Google.Protobuf;
using Grpc.Core;
using Grpc.Net.Client;
using Yysystem;

namespace CsharpSampleConsole
{
    internal class YYSystemClient
    {
        public string endpoint { get; } = "";
        public string port { get; } = "";
        public string apiKey { get; } = "";
        public bool ssl { get; } = false;
        public YYSystemClient(string endpoint, string port, string apiKey, bool ssl)
        {
            this.endpoint = endpoint;
            this.port = port;
            this.apiKey = apiKey;
            this.ssl = ssl;
            var procotol = ssl ? "https" : "http";
            var credentials = ssl ? ChannelCredentials.SecureSsl : ChannelCredentials.Insecure;
            var address = $"{procotol}://{endpoint}:{port}";
            var channel = GrpcChannel.ForAddress(
                address,
                new GrpcChannelOptions { Credentials = credentials });
            _client = new YYSpeech.YYSpeechClient(channel);
        }

        private YYSpeech.YYSpeechClient _client;

        public async Task startStream()
        {
            var headers = new Metadata { new Metadata.Entry("x-api-key", apiKey) };
            using (var call = _client.RecognizeStream(headers))
            {
                var cts = new CancellationTokenSource();
                var requestStream = call.RequestStream;
                async void onData(byte[] bytes)
                {
                    // Process recording stream on data event
                    if (requestStream == null)
                    {
                        Console.WriteLine("requestStream null");
                        return;
                    }
                    try
                    {
                        cts.Token.ThrowIfCancellationRequested();
                        await requestStream.WriteAsync(new StreamRequest { Audiobytes = ByteString.CopyFrom(bytes, 0, bytes.Length) });
                    }
                    catch (OperationCanceledException e)
                    {
                        // Process request stream on cancel event
                        Console.WriteLine("Request stream cancelled");
                        Console.WriteLine(e);
                    }
                    catch (Exception e)
                    {
                        // Process request stream on error event
                        Console.WriteLine("Request stream error");
                        Console.WriteLine(e);
                    }
                }
                var recorder = new AudioRecorder(onData);
                var responseError = false;
                var connectionError = false;
                Console.WriteLine("Background task starts to listen response stream");
                var readTask = Task.Run(async () =>
                {
                    try
                    {
                        await foreach (var response in call.ResponseStream.ReadAllAsync())
                        {
                            // Process response stream on data event
                            if (response.Error != null)
                            {
                                // Process response stream data has error message
                                responseError = true;
                                Console.WriteLine(response.Error);
                                break;
                            }
                            if (!response.Result.IsFinal)
                            {
                                Console.WriteLine($"- {response.Result.Transcript}");
                                continue;
                            }
                            Console.WriteLine($"+ {response.Result.Transcript}");
                        }
                        // Process response stream on end event
                        Console.WriteLine("Response stream ended");
                    }
                    catch (RpcException e)
                    {
                        // Process response stream on error event
                        connectionError = true;
                        Console.WriteLine("Response stream Error");
                        Console.WriteLine(e);
                    }
                    finally
                    {
                        if (responseError || connectionError)
                        {
                            // if response stream has error, publish a cancel token and stop the recorder
                            cts.Cancel();
                            if (recorder.recording)
                            {
                                recorder.stopRecording();
                            }
                            Console.WriteLine("Press any key to finish streams...");
                        }
                    }
                    
                });
                Console.WriteLine("Press any key to start recoding...");
                while (true)
                {
                    var result = Console.ReadKey(intercept: true);
                    if (responseError || connectionError)
                    {
                        break;
                    }
                    if (recorder.recording)
                    {
                        recorder.stopRecording();
                        break;
                    }
                    // Send a StreamingConfig request, and then start the recorder
                    try
                    {
                        cts.Token.ThrowIfCancellationRequested();
                        await requestStream.WriteAsync(new StreamRequest { StreamingConfig = new StreamingConfig { EnableInterimResults = true, Model = 10 } });
                    }
                    catch (OperationCanceledException e)
                    {
                        Console.WriteLine("Request stream cancelled");
                        Console.WriteLine(e);
                    }
                    catch (RpcException e)
                    {
                        Console.WriteLine("Request stream error");
                        Console.WriteLine(e);
                    }
                    recorder.startRecording();
                    Console.WriteLine("Press any key to stop recoding...");
                }
                try
                {
                    // Complete request stream, and then finish the background task
                    await requestStream.CompleteAsync();
                    Console.WriteLine("Request stream completed");
                    await readTask;
                    Console.WriteLine("Backend task ended");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }

        }
    }

}

