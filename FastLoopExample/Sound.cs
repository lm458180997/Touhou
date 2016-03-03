using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tao.OpenAl;

namespace FastLoopExample
{
    /// <summary>
    /// 引入OpenAl
    /// </summary>

    public class Sound
    {
        public int Channel { get; set; }
        public bool FailedToPlay
        {
            get
            {
                //minus is an error state.
                return (Channel == -1);
            }
        }

        public Sound(int channel)
        {
            Channel = channel;
        }
    }
    public class SoundManager
    {
        //音源
        struct SoundSource
        {
            public int _bufferId;
            public string _filePath;
            public SoundSource(int buffered, string filePath)
            {
                _bufferId = buffered;
                _filePath = filePath;
            }
        }

        readonly int MaxSoundChannels = 256;   //现代的声音硬件一般可以同时播放256种声音

        /// <summary>
        /// OpenAL找出可用声道的数量的方法是，不断的请求声道，当硬件不能提供声道时，就得到了最大声道数
        /// </summary>
        
        List<int> _soundChannels = new List<int>(); //记录声道数

        /// <summary>
        /// 声音数据
        /// </summary>
        Dictionary<string, SoundSource> _soundIdentifiler = new Dictionary<string, SoundSource>();
        
        public SoundManager()
        {
            Alut.alutInit();
            DicoverSoundChannels();
        }

        private void DicoverSoundChannels()
        {
            while (_soundChannels.Count < MaxSoundChannels)
            {
                int src;
                Al.alGenSources(1, out src);
                if (Al.alGetError() == Al.AL_NO_ERROR)
                {
                    _soundChannels.Add(src);
                }
                else
                {
                    break;  //there's been an error - we've filled al the channel
                }
            }
        }

        public void LoadSound(string soundId, string path)    //读取声音资源
        {
            //Generate a buffer
            int buffer = -1;
            Al.alGenBuffers(1, out buffer);
            int errorCode = Al.alGetError();
            if (errorCode != Al.AL_NO_ERROR)
                return;
            
            int format;
            float frequency;
            int size;
            if(!System.IO.File.Exists(path))
                return ;
            IntPtr data = Alut.alutLoadMemoryFromFile(path, out format, out size, out frequency);
            if(data == IntPtr.Zero)
                return ;
            //Load wav data into the generated buffer
            Al.alBufferData(buffer, format, data, size, (int)frequency);
            //Everything seems ok, add it to the libarary
            _soundIdentifiler.Add(soundId, new SoundSource(buffer, path));

            System.Diagnostics.Debug.WriteLine("LoadSound's buffer: " + buffer.ToString());
            System.Diagnostics.Debug.WriteLine(_soundIdentifiler.Count.ToString());

        }

        private bool IsChannelPlaying(int channel)   //是否此声道已被占用
        {
            int value = 0;
            Al.alGetSourcei(channel, Al.AL_SOURCE_STATE, out value);
            return (value == Al.AL_PLAYING);
        }

        private int FindNextFreeChannel()
        {
            foreach (int slot in _soundChannels)
            {
                if (!IsChannelPlaying(slot))
                    return slot;
            }
            return -1;
        }

        public Sound PlaySound(string soundId , bool loop = false)
        {
            int channel = FindNextFreeChannel();
            if (channel != -1)
            {
                Al.alSourceStop(channel);
                Al.alSourcei(channel, Al.AL_BUFFER, _soundIdentifiler[soundId]._bufferId);
                Al.alSourcef(channel, Al.AL_PITCH, 1.0f);
                Al.alSourcef(channel, Al.AL_GAIN, 1.0f);

                if (loop)
                {
                    Al.alSourcei(channel, Al.AL_LOOPING, 1); 
                }
                else
                {
                    Al.alSourcei(channel, Al.AL_LOOPING, 0);
                    System.Diagnostics.Debug.Write(channel.ToString());
                }
                Al.alSourcePlay(channel);
                return new Sound(channel);
            }
            else
                return new Sound(-1);
        }

        public bool IsSoundPlaying(Sound sound)
        {
            return false;
        }

        public void StopSound(Sound sound)
        {
        }

    }

}
