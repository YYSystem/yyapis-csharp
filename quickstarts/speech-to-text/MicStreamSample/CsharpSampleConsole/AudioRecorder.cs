using NAudio.Wave;

namespace CsharpSampleConsole
{
    internal class AudioRecorder
    {
        public AudioRecorder(Action<byte[]> onData)
        {
            recording = false;
            _waveIn = new WaveInEvent
            {
                WaveFormat = new WaveFormat(16000, 16, 1)
            };
            _waveIn.DataAvailable += (s, a) =>
            {
                // Process recording stream on data event
                onData(a.Buffer);
            };
            _waveIn.RecordingStopped += (s, a) =>
            {
                // Process recording stream on stopped event
            };
        }
        private WaveInEvent _waveIn;
        public bool recording { get; private set; }
        public void startRecording()
        {
            Console.WriteLine("Start recording");
            _waveIn.StartRecording();
            recording = true;
        }

        public void stopRecording()
        {
            Console.WriteLine("Stop recording");
            _waveIn.StopRecording();
            recording= false;
        }
    }
}
