using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Supermortal.Common.PCL.Enums;

using RiffSharer.Models;

namespace RiffSharer.Services.Abstract
{
    public interface IAudioService
    {
        Audio SaveAudio(string name, AudioFormat audioFormat, ChannelConfiguration channelConfiguration, int sampleRate, byte[] data);

        Audio SaveAudio(string name, string localPath);

        Audio GetAudio(string audioId);

        Task<IEnumerable<Audio>> GetAudiosForUser(string userId, int page, int pageSize);

        Task<int> GetAudioCountForUser(string userId);
    }
}

