using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Supermortal.Common.PCL.Helpers;
using Supermortal.Common.PCL.Enums;

using RiffSharer.Services.Abstract;
using RiffSharer.Repositories.Abstract;
using RiffSharer.Models;

namespace RiffSharer.Services.Concrete
{
    public class DefaultAudioService : IAudioService
    {

        private readonly AAudioRepository _ar;
        private readonly ANonTableQueryAudioRepository _nar;

        public DefaultAudioService()
            : this(IoCHelper.Instance.GetService<AAudioRepository>(), IoCHelper.Instance.GetService<ANonTableQueryAudioRepository>())
        {
        }

        public DefaultAudioService(AAudioRepository ar, ANonTableQueryAudioRepository nar)
        {
            _ar = ar;
            _nar = nar;
        }

        public Audio SaveAudio(string name, AudioFormat audioFormat, ChannelConfiguration channelConfiguration, int sampleRate, byte[] data)
        {
            var audio = new Audio();

            audio.Name = name;
            audio.SetAudioFormat(audioFormat);
            audio.SetChannelConfiguration(channelConfiguration);
            audio.SampleRate = sampleRate;
            audio.Data = data;

            return _ar.Insert(audio);
        }

        public Audio SaveAudio(string name, string localPath)
        {
            throw new NotImplementedException();
        }

        public Audio GetAudio(string audioId)
        {
            return _ar.Get(audioId);
        }

        public async Task<IEnumerable<Audio>> GetAudiosForUser(string userId, int page, int pageSize)
        {
            return _nar.GetAllForUserPaged(userId, page, pageSize).AsEnumerable();
        }

        public async Task<int> GetAudioCountForUser(string userId)
        {
            return _nar.GetCountForUser(userId);
        }
    }
}

