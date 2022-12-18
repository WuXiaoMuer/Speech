using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;
using SpeechE;
namespace SpeechTest
{
	class Program
	{
		static void Main(string[] args)
		{
			SpeechMain sm = new SpeechMain();
			sm.Say("你好");
			sm.saySsml(File.ReadAllText(System.Environment.CurrentDirectory+@"\ssml.txt"));
			sm.OutToWaveFile(System.Environment.CurrentDirectory + @"\test.wav", "你好，这是个WAV文件");
			foreach (string s in sm.GetSpeechList())
			{
				Console.WriteLine(s);
			}
			int i=0;
			Console.WriteLine(new string('=',50));
			foreach (string s in sm.GetSpeechNameList())
			{
				i++;
				Console.WriteLine("{0}."+s,i);
			}
			Console.Write("输入角色名：");
			sm.SetSpeech(Console.ReadLine());
			Console.Write("输入角色语音：");
			sm.Say(Console.ReadLine());
			Console.ReadKey();
		}
	}
}
