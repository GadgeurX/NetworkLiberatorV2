using AppKit;

namespace NetworkLiberator
{
	static class MainClass
	{
		static void Main(string[] args)
		{
			NetworkLiberator.Core.Core l_Core = new NetworkLiberator.Core.Core();
			NSApplication.Init();
			NSApplication.Main(args);
		}
	}
}
