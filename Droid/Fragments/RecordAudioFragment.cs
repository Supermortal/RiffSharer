
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        MediaRecorder _recorder;
        MediaPlayer _player;
        //        ImageView _play;
        ImageView _stop;
        ImageView _record;

        string _path = "/sdcard/test.3gpp";

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }



        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            return inflater.Inflate(Resource.Layout.RecordAudioFragment, container, false);
        }

        public static Android.Support.V4.App.Fragment newInstance(Context context)
        {
            RecordAudioFragment busrouteFragment = new RecordAudioFragment();
            return busrouteFragment;
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
                    
            _stop = _activity.FindViewById<ImageView>(Resource.Id.stop);
            _record = _activity.FindViewById<ImageView>(Resource.Id.record);
            //_play = _activity.FindViewById<ImageView>(Resource.Id.play);

            _record.Click += Click_Record;
            _stop.Click += Click_Stop;
        }

        public override void OnPause()
        {
            base.OnPause();

            _player.Release();
            _recorder.Release();
            _player.Dispose();
            _recorder.Dispose();
            _player = null;
            _recorder = null;
        }

        #region Events

        private void Click_Record(object sender, EventArgs e)
        {
            _stop.Visibility = ViewStates.Visible;
            _record.Visibility = ViewStates.Gone;

            _recorder.SetAudioSource(AudioSource.Mic);
            _recorder.SetOutputFormat(OutputFormat.ThreeGpp);
            _recorder.SetAudioEncoder(AudioEncoder.AmrNb);
            _recorder.SetOutputFile(_path);
            _recorder.Prepare();
            _recorder.Start();
        }

        private void Click_Stop(object sender, EventArgs e)
        {
            _stop.Visibility = ViewStates.Gone;
            _record.Visibility = ViewStates.Visible;

            _recorder.Stop();
            _recorder.Reset();

            _player.SetDataSource(_path);
            _player.Prepare();
            _player.Start();
        }

        #endregion
    }
}

