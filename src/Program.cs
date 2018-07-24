using Stefandevo.Genyman.XamarinAssets.Implementation;
using Genyman.Core;

namespace Stefandevo.Genyman.XamarinAssets
{
	internal class Program
	{
		public static void Main(string[] args)
		{
			GenymanApplication.Run<Configuration, NewTemplate, Generator>(args);
		}
	}
}