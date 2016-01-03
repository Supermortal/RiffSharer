
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Media;

namespace RiffSharer.Droid
{
    public class RecordAudioFragment : Android.Support.V4.App.Fragment
    {
        private Activity _activity;
        private AudioRecord _audioRecord;
        private MediaRecorder _recorder;
        private MediaPlayer _player;
        //        ImageView _play;
        private ImageView _stop;
        private ImageView _record;

        private string _path = "/sdcard/test.3gpp";
        private int _sampleAudioBitRate;
        private byte[] _audioDataBuffer;
        private List<byte> _audioData;
        private volatile bool _isAudioRecording = false;
        private int _bufferLength;
        private Android.Media.Encoding _audioFormat;
        private ChannelIn _channelConfig;

        #region Lifecycle

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return inflater.Inflate(Resource.Layout.RecordAudioFragment, container, false);
        }

        public override void OnAttach(Android.App.Activity activity)
        {
            base.OnAttach(activity);
            _activity = activity;
        }

        public override void OnResume()
        {
            base.OnResume();

            _recorder = new MediaRecorder();
            _player = new MediaPlayer();

            _player.Completion += (sender, e) =>
            {
                _player.Reset();
                _record.Visibility = ViewStates.Visible;
                _stop.Visibility = ViewStates.Gone;
            };

            SetViews();

            _record.Click += Click_Record;
            _stop.Click += Click_Stop;
        }

        public override void OnPause()
        {
            base.OnPause();

            DisposeAll();
        }

        #endregion

        #region Static

        public static Android.Support.V4.App.Fragment newInstance(Context context)
        {
            RecordAudioFragment busrouteFragment = new RecordAudioFragment();
            return busrouteFragment;
        }

        #endregion

        #region Helpers

        protected void SetViews()
        {
            _stop = _activity.FindViewById<ImageView>(Resource.Id.stop);
            _record = _activity.FindViewById<ImageView>(Resource.Id.record);
            //_play = _activity.FindViewById<ImageView>(Resource.Id.play);
        }

        protected void DisposeAll()
        {
            if (_audioRecord != null)
            {
                _audioRecord.Release();
                _audioRecord.Dispose();
                _audioRecord = null;
            }

            if (_player != null)
            {
                _player.Release();
                _player.Dispose();
                _player = null;
            }

            if (_recorder != null)
            {
                _recorder.Release();
                _recorder.Dispose();
                _recorder = null;
            }

            _audioDataBuffer = null;
            _isAudioRecording = false;
            _bufferLength = 0;
            _audioData = null;
        }

        private static int[] _sampleRates = new int[] { 44100, 22050, 11025, 8000 };

        public AudioRecord FindAudioRecord(ref int sampleRate, ref Android.Media.Encoding audioFormat, ref ChannelIn channelConfig, ref int bufferSize)
        {
            foreach (int sr in _sampleRates)
            {
                foreach (var af in new Android.Media.Encoding[] { Android.Media.Encoding.Pcm16bit, Android.Media.Encoding.Pcm8bit })
                {
                    foreach (var cc in new ChannelIn[] { ChannelIn.Stereo, ChannelIn.Mono })
                    {
                        try
                        {
//                            Log.Debug(C.TAG, "Attempting rate " + rate + "Hz, bits: " + audioFormat + ", channel: "
//                                + channelConfig);
                            int bs = AudioRecord.GetMinBufferSize(sr, cc, af);

                            if (bs > 0)
                            {
                                // check if we can instantiate and have a success
                                AudioRecord recorder = new AudioRecord(AudioSource.Default, sr, cc, af, bs);

                                if (recorder.State == State.Initialized)
                                {
                                    bufferSize = bs;
                                    sampleRate = sr;
                                    audioFormat = af;
                                    channelConfig = cc;

                                    return recorder;
                                }      
                            }
                        }
                        catch (Exception e)
                        {
//                            Log.e(C.TAG, rate + "Exception, keep trying.", e);
                        }
                    }
                }
            }
            return null;
        }

        public AudioTrack FindAudioTrack(ref int sampleRate, ref Android.Media.Encoding audioFormat, ref ChannelOut channelConfig, ref int bufferSize)
        {
            foreach (var sr in _sampleRates)
            {
                foreach (var af in new Android.Media.Encoding[] { Android.Media.Encoding.Pcm16bit, Android.Media.Encoding.Pcm8bit })
                {
                    foreach (var cc in new ChannelOut[] { ChannelOut.Stereo, ChannelOut.Mono })
                    {
                        foreach (var atm in new AudioTrackMode[] { AudioTrackMode.Static, AudioTrackMode.Stream})
                        {
                            int bs = AudioTrack.GetMinBufferSize(sr, cc, af);

                            if (bs > 0)
                            {
                                var audioTrack = new AudioTrack(Stream.Music, sr, cc, af, bs, atm);

                                if (audioTrack.State == AudioTrackState.Initialized)
                                {
                                    sampleRate = sr;
                                    audioFormat = af;
                                    channelConfig = cc;
                                    bufferSize = bs;

                                    return audioTrack;
                                }
                            }
                        }
                    }
                }
            }

            return null;
        }

        #endregion

        #region Events

        private void Click_Record(object sender, EventArgs e)
        {
            _stop.Visibility = ViewStates.Visible;
            _record.Visibility = ViewStates.Gone;

            _audioRecord = FindAudioRecord(ref _sampleAudioBitRate, ref _audioFormat, ref _channelConfig, ref _bufferLength);
            _audioDataBuffer = new byte[_bufferLength];
            _audioData = new List<byte>();

            _audioRecord.StartRecording();
            _isAudioRecording = true;

            var t = (new TaskFactory()).StartNew(() =>
                {
                    while (_isAudioRecording)
                    {
                        var bufferReadResult = _audioRecord.Read(_audioDataBuffer, 0, _audioDataBuffer.Length);
                        _audioData.AddRange(_audioDataBuffer);
                    } 
                });

//            _recorder.SetAudioSource(AudioSource.Mic);
//            _recorder.SetOutputFormat(OutputFormat.ThreeGpp);
//            _recorder.SetAudioEncoder(AudioEncoder.AmrNb);
//            _recorder.SetOutputFile(_path);
//            _recorder.Prepare();
//            _recorder.Start();
        }

        private void Click_Stop(object sender, EventArgs e)
        {
            _stop.Visibility = ViewStates.Gone;
            _record.Visibility = ViewStates.Visible;

            _isAudioRecording = false;
            _audioRecord.Stop();

            var byteArr = _audioData.ToArray();

            int sampleRate = 0;
            Android.Media.Encoding audioFormat = Android.Media.Encoding.Pcm16bit;
            ChannelOut channelConfig = ChannelOut.Stereo;
            int bufferLength = 0;
            AudioTrack audioTrack = FindAudioTrack(ref sampleRate, ref audioFormat, ref channelConfig, ref bufferLength);

            audioTrack.Play();
            audioTrack.Write(byteArr, 0, byteArr.Length);

//            _recorder.Stop();
//            _recorder.Reset();
//
//            _player.SetDataSource(_path);
//            _player.Prepare();
//            _player.Start();
        }

        #endregion
    }
}

