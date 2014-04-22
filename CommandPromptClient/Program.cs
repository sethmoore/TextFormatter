using System;
using System.IO;
using XamarinInterview;

namespace CommandPromptClient
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			using (StreamReader file = new StreamReader (@"C:\Projects\C#\TextFormatter\document.txt")) {
				string line;
				TextFormatter formatter = new TextFormatter ();
				while ((line = file.ReadLine ()) != null) {
					formatter.IssueCommand (file.ReadLine ());
				}
				formatter.Save (@"C:\Projects\C#\TextFormatter\document.pdf");
			}
		}
	}
}
