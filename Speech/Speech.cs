using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;

namespace SpeechE
{
    public interface ISpeechMain
	{
        int GetVolume();
        void SetVolume(int volume);
        int GetRate();
        void SetRate(int rate);
        bool Say(string text);
        string[] GetSpeechList();
        string[] GetSpeechNameList();
        void SetSpeech(string name);
        Prompt sayAsync(string text);
        void sayAsyncSTOP(Prompt prompt);
        void sayAsyncStopAll();
        void saySsml(string textSsml);
        void saySsmlAsync(string textSsml);
        void Pause();
        void Resume();
        void AddLexicon(string uri, string mediaType);
        void RemoveLexicon(string uri);
        void OutToWaveFile(string path, string text);
    }
    [ClassInterface(ClassInterfaceType.None)]
    public class SpeechMain : ISpeechMain
    {
        SpeechSynthesizer sy = new SpeechSynthesizer();
        /// <summary>
        /// Volume 声音大小 0-100
        /// </summary>
        /// <returns></returns>
		public int GetVolume() => sy.Volume;
        /// <summary>
        /// Volume 声音大小 0-100
        /// </summary>
        /// <param name="value"></param>
		public void SetVolume(int value) => sy.Volume = value;
        /// <summary>
        /// Rate 语速 -10 - 10
        /// </summary>
        /// <returns></returns>
		public int GetRate() => sy.Rate;
        /// <summary>
        /// Rate 语速 -10 - 10
        /// </summary>
        /// <param name="value"></param>
		public void SetRate(int value) => sy.Rate = value;
		/// <summary>
		/// 说，同步的
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		public bool Say(string text)
		{
			try { 
            sy.SetOutputToDefaultAudioDevice();
            sy.Speak(text);
			}
			catch(InvalidCastException e)
			{
                return false;
			}
            return true;
        }
        /// <summary>
        /// 获取角色信息列表
        /// </summary>
        /// <returns></returns>
        public string[] GetSpeechList()
		{
            string[] sl = new string[0];
            List<string> list = new List<string>(sl);
            foreach (var voice in sy.GetInstalledVoices())
            {
                var info = voice.VoiceInfo;
                list.Add($"Id: {info.Id} | Name: {info.Name} |Age: { info.Age} | Gender: { info.Gender} | Culture: { info.Culture}");
            }
            sl = list.ToArray();
            return sl;
		}
        /// <summary>
        /// 获取角色名列表，同上不过方便SetSpeech
        /// </summary>
        /// <returns></returns>
        public string[] GetSpeechNameList()
		{
            string[] sl = new string[0];
            List<string> list = new List<string>(sl);
            foreach (var voice in sy.GetInstalledVoices())
            {
                var info = voice.VoiceInfo;
                list.Add(info.Name);
            }
            sl = list.ToArray();
            return sl;
        }
        /// <summary>
        /// 设置音频角色
        /// </summary>
        /// <param name="name"></param>
        public void SetSpeech(string name)
		{
            sy.SelectVoice(name);//试了几遍，不能出现中文角色，尝试了编码转换，应该不是编码的问题 (＠n＠;)
		}
        /// <summary>
        /// 异步说
        /// </summary>
        public Prompt sayAsync(string text)
		{
            return sy.SpeakAsync(text);
		}
        /// <summary>
        /// 传入异步说对象将其停止
        /// </summary>
        /// <param name="prompt"></param>
        public void sayAsyncSTOP(Prompt prompt)
		{
            sy.SpeakAsyncCancel(prompt);
		}
        /// <summary>
        /// 取消全部异步说
        /// </summary>
        public void sayAsyncStopAll()
		{
            sy.SpeakAsyncCancelAll();
		}
        /// <summary>
        /// 同步说包含SSML标记的
        /// </summary>
        /// <param name="textSsml"></param>
        public void saySsml(string textSsml)
		{
            sy.SpeakSsml(textSsml);
		}
        /// <summary>
        /// 异步说包含SSML标记的
        /// </summary>
        /// <param name="textSsml"></param>
        public void saySsmlAsync(string textSsml)
		{
            sy.SpeakSsmlAsync(textSsml);
		}
		/// <summary>
		/// 暂停
		/// </summary>
		public void Pause() => sy.Pause();
        /// <summary>
        /// 继续
        /// </summary>
        public void Resume() => sy.Resume();
        /// <summary>
        /// 添加词典，url可以为词典路径
        /// </summary>
        /// <param name="url"></param>
        /// <param name="mediatype"></param>
		public void AddLexicon(string url, string mediatype) => sy.AddLexicon(new Uri(url), mediatype);
        /// <summary>
        /// 移除词典
        /// </summary>
        /// <param name="url"></param>
		public void RemoveLexicon(string url) => sy.RemoveLexicon(new Uri(url));
        /// <summary>
        /// 输出音频到WAVE文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="text"></param>
        public void OutToWaveFile(string path,string text)
		{
            sy.SetOutputToWaveFile(path);
			sy.SpeakCompleted += new EventHandler<SpeakCompletedEventArgs>(synth_SpeakCompleted);
            PromptBuilder builder = new PromptBuilder();
            builder.AppendText(text);
            sy.Speak(builder);
            
        }
        static void synth_SpeakCompleted(object sender, SpeakCompletedEventArgs e)
		{

		}
	}
}
