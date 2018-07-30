using System.Collections.Generic;

namespace Stefandevo.Genyman.XamarinAssets.Implementation
{
	public class NewTemplate : Configuration
	{
		public NewTemplate()
		{
			AssetsPath = "PathToYourAssets";
			Platforms = new List<PlatformClass>()
			{
				new PlatformClass()
				{
					Type = Implementation.Platforms.iOS,
					ProjectPath = "YourProject.iOS"
				},
				new PlatformClass()
				{
					Type = Implementation.Platforms.Android,
					ProjectPath = "YourProject.Droid",
					AndroidOptions = new AndroidOptions()
					{
						AssetFolderPrefix = "mipmap"
					}
				}
			};
			Assets = new List<AssetClass>()
			{
				new AssetClass()
				{
					File = "FirstAsset.svg",
					Size = "100x100"
				}
			};
		}
	}
}