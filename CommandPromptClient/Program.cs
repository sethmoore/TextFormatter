using System;
using System.IO;
using XamarinInterview;

namespace CommandPromptClient
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			if (args.Length >= 2) {
				using (StreamReader file = new StreamReader (args [0])) {
					string line;
					TextFormatter formatter = new TextFormatter ();
					while ((line = file.ReadLine ()) != null) {
						formatter.IssueCommand (line);
					}
					formatter.Save (args [1]);
					Console.WriteLine ("File written to {0}", args [1]);
				}
			} else {
				Console.WriteLine ("Requires 2 arguments: input file path and output file path");
			}
		}
	}
}
