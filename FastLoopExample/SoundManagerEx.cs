using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Un4seen.Bass;                  //使用bass音频库

namespace FastLoopExample
{
    public struct SoundEx
    {
        public int stream;              //音频流句柄 ,记录某一种音乐
        public string name;
        public string file_path;
        public float volumn;
        public SoundEx(string name, string file_path, float volumn = 1)
            : this()
        {
            this.stream = 0; this.name = name; this.file_path = file_path;
            this.volumn = volumn;
        }
        public void setVolumn(float v)
        {
            volumn = v;
        }
    }

    public class SoundManagerEx : IDisposable
    {

        public class Tick
        {
            public int tick = 0;
            public Tick(int i)
            {
                tick = i;
            }

        }

        public Dictionary<string, Tick> soundRefreshTick = new Dictionary<string,Tick>(); //音乐的刷新周期计时（每3帧数只能重复播放一次）

        public Dictionary<string, SoundEx> soundlist = new Dictionary<string,SoundEx>();   //音乐资源列表
        public SoundManagerEx()
        {
            soundlist = new Dictionary<string, SoundEx>();
            soundRefreshTick = new Dictionary<string, Tick>();
        }
         ~SoundManagerEx()
        {
            Dispose();
        }
         public int Volume                            //音量0-100；
         {
             get
             {
                 return Bass.BASS_GetConfig(BASSConfig.BASS_CONFIG_GVOL_STREAM) / 100;
             }
             set
             {
                 Bass.BASS_SetConfig(BASSConfig.BASS_CONFIG_GVOL_STREAM, value * 100);
             }
         }

        public void AddSound(string name , string path)
        {
            //Bass.BASS_StreamCreateFile创建音频流，第一个参数是文件名，
            //第二个参数是文件流开始位置，第三个是文件流长度 0为使用文件整个长度，最后一个是流的创建模式。
            SoundEx sound = new SoundEx(name, path);
            sound.stream = Bass.BASS_StreamCreateFile(path, 0L, 0L, BASSFlag.BASS_SAMPLE_FLOAT);
            soundlist.Add(name, sound);
            soundRefreshTick.Add(name, new Tick(0));

        }
        public void AddSound(string name, IntPtr pointer ,long lenth)
        {
            SoundEx sound = new SoundEx(name, "none");
            sound.stream = Bass.BASS_StreamCreateFile(pointer, 0L, lenth, BASSFlag.BASS_SAMPLE_FLOAT);
            soundlist.Add(name, sound);
            soundRefreshTick.Add(name, new Tick(0));
        }
         
        public void Play(string name , bool replay = true)   //播放，第二个参数代表是否从头播放
        {
            if(soundRefreshTick[name].tick==0)               //防止过快的重复播放
                Bass.BASS_ChannelPlay(soundlist[name].stream, replay);
            soundRefreshTick[name].tick = 3;                       //刷新周期
        }

        //周期检查，  防止因任务过多而引起拥塞
        public void Update()
        {
            foreach (Tick t in soundRefreshTick.Values)
            {
                if (t.tick > 0)
                    t.tick--;
            }
        }

         public void Pause(string name)                       //暂停播放
        {
            Bass.BASS_ChannelPause(soundlist[name].stream);
        }

        public void Stop(string name)
        {
            Bass.BASS_ChannelStop(soundlist[name].stream);   // 停止播放
        }

        //if (Bass.BASS_ChannelIsActive(stream) == BASSActive.BASS_ACTIVE_PLAYING)  可以获取状态
        public float[] GetFFTData(string name)
        {
            float[] fft = new float[512]; 
            Bass.BASS_ChannelGetData(soundlist[name].stream, fft, (int)BASSData.BASS_DATA_FFT1024);
            return fft;
        }

        public bool GetFFTData(string name, float[] fft)
        {
            Bass.BASS_ChannelGetData(soundlist[name].stream, fft, (int)BASSData.BASS_DATA_FFT1024);
            return true;
        }

        public BASSActive getState(string name)           //获取歌曲状态
        {
            return Bass.BASS_ChannelIsActive(soundlist[name].stream);
        }

        public double GetCurrentTime(string name)         //当前运行的时间
        {
            return Bass.BASS_ChannelBytes2Seconds(soundlist[name].stream,
                Bass.BASS_ChannelGetPosition(soundlist[name].stream));
        }

        public double GetLength(string name)              //获取歌曲长度
        {
            return Bass.BASS_ChannelBytes2Seconds(soundlist[name].stream,
                Bass.BASS_ChannelGetLength(soundlist[name].stream));
        }

        public void SetVolumChannel(string name, float volume)
        {
            soundlist[name].setVolumn(volume);
            Bass.BASS_ChannelSetAttribute(soundlist[name].stream,
                BASSAttribute.BASS_ATTRIB_VOL, volume);
        }

        public void Dispose()
        {
            foreach (SoundEx s in soundlist.Values)
            {
                Bass.BASS_StreamFree(s.stream);               //释放资源
            }
            soundlist.Clear();
        }

        void IDisposable.Dispose()
        {
            foreach (SoundEx s in soundlist.Values)
            {
                Bass.BASS_StreamFree(s.stream);               //释放资源
            }
            soundlist.Clear();
        }
    }

}
