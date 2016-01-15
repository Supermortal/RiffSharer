using System;
using System.Collections.Generic;
using System.Linq;

using Supermortal.Common.PCL.Enums;

using RiffSharer.Repositories.Abstract;
using RiffSharer.Models;

namespace RiffSharer.Repositories.Concrete
{
    public class TestAudioRepository : ANonTableQueryAudioRepository
    {

        public string TEST_USER_ID_GUID = "a1d9be8f-0b1c-4663-aecd-a9d76e11c124";

        public TestAudioRepository()
        {
            var list = new List<Audio>();

            for (var i = 0; i < 100; i++)
            {
                var a = new Audio()
                { 
                    AudioID = Guid.NewGuid().ToString(),
                    UserID = TEST_USER_ID_GUID,
                    Name = "TestRiff " + i,
                    DateCreated = DateTime.UtcNow,
                    SampleRate = 41000,
                    DurationSeconds = 60
                };

                a.SetAudioFormat(AudioFormat.PCM16Bit);
                a.SetChannelConfiguration(ChannelConfiguration.Stereo);

                list.Add(a);
            }
                
            DataSet = list.AsQueryable();
        }

        public override void DeleteAll()
        {
            throw new NotImplementedException();
        }

        public override RiffSharer.Models.Audio Insert(RiffSharer.Models.Audio obj)
        {
            throw new NotImplementedException();
        }

        public override RiffSharer.Models.Audio Update(RiffSharer.Models.Audio obj)
        {
            throw new NotImplementedException();
        }

        public override void Delete(string id)
        {
            throw new NotImplementedException();
        }
    }
}

