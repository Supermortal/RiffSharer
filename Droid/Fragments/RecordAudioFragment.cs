﻿
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

using Supermortal.Common.Droid.Helpers;

namespace RiffSharer.Droid
{
    public class RecordAudioFragment : Android.Support.V4.App.Fragment
    {
        private Activity _activity;
        private AudioRecord _audioRecord;
        private AudioTrack _audioTrack;
        private ImageView _play;
        private ImageView _stop;
        private ImageView _record;

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

            SetViews();

            _record.Click += Click_Record;
            _stop.Click += Click_Stop;
            _play.Click += Click_Play;
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
            _play = _activity.FindViewById<ImageView>(Resource.Id.play);
        }

        protected void DisposeAll()
        {
            if (_audioRecord != null)
            {
                _audioRecord.Release();
                _audioRecord.Dispose();
                _audioRecord = null;
            }

            if (_audioTrack != null)
            {
                _audioTrack.Release();
                _audioTrack.Dispose();
                _audioTrack = null;
            }

            _audioDataBuffer = null;
            _isAudioRecording = false;
            _bufferLength = 0;
            _audioData = null;
            _isPlaying = false;
        }

        #endregion

        #region Events

        private void Click_Record(object sender, EventArgs e)
        {
            _stop.Visibility = ViewStates.Visible;
            _record.Visibility = ViewStates.Gone;
            _play.Visibility = ViewStates.Gone;

            _audioRecord = AudioHelper.FindAudioRecord(ref _sampleAudioBitRate, ref _audioFormat, ref _channelConfig, ref _bufferLength);
            _audioDataBuffer = new byte[_bufferLength];
            _audioData = new List<byte>();

            _audioRecord.StartRecording();
            _isAudioRecording = true;

            (new TaskFactory()).StartNew(() =>
                {
                    while (_isAudioRecording)
                    {
                        _audioRecord.Read(_audioDataBuffer, 0, _audioDataBuffer.Length);
                        _audioData.AddRange(_audioDataBuffer);
                    } 
                });
        }

        private void Click_Stop(object sender, EventArgs e)
        {
            _stop.Visibility = ViewStates.Gone;
            _record.Visibility = ViewStates.Visible;
            _play.Visibility = ViewStates.Visible;

            _isAudioRecording = false;
            _audioRecord.Stop();
        }

        private volatile bool _isPlaying = false;

        private void Click_Play(object sender, EventArgs e)
        {
            if (_isPlaying)
            {          
                _audioTrack.Stop();
                _isPlaying = false;
                _play.SetImageResource(Resource.Drawable.play);
                return;
            }

            _play.SetImageResource(Resource.Drawable.stop);

            var byteArr = _audioData.ToArray();

            int sampleRate = 0;
            Android.Media.Encoding audioFormat = Android.Media.Encoding.Pcm16bit;
            ChannelOut channelConfig = ChannelOut.Stereo;
            int bufferLength = 0;
            _audioTrack = AudioHelper.FindAudioTrack(ref sampleRate, ref audioFormat, ref channelConfig, ref bufferLength);

            _isPlaying = true;

            (new TaskFactory()).StartNew(() =>
                {
                    _audioTrack.Play();
                    _audioTrack.Write(byteArr, 0, byteArr.Length);
                    _activity.RunOnUiThread(() =>
                        {
                            _isPlaying = false;
                            _play.SetImageResource(Resource.Drawable.play);
                        });
                });
        }

        #endregion
    }
}

