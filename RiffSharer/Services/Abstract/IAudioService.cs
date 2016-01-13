using System;
using System.Collections.Generic;

using Supermortal.Common.PCL.Enums;

using RiffSharer.Models;

namespace RiffSharer.Services.Abstract
{
    public interface IAudioService
    {
        Audio SaveAudio(string name, AudioFormat audioFormat, ChannelConfiguration channelConfiguration, int sampleRate, byte[] data);

        Audio SaveAudio(string name, string localPath);

        Audio GetAudio(string audioId);

        IEnumerable<Audio> GetAudiosForUser(string userId, int page, int pageSize);

        int GetAudioCountForUser(string userId);
    }
}

