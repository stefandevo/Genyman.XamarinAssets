using System;
using System.Collections.Generic;
using Genyman.Core;

namespace Stefandevo.Genyman.XamarinAssets.Implementation
{
	[Documentation]
	public class Configuration
	{
		[Description("Relative path where the configuration is towards where the svg files are")]
		public string AssetsPath { get; set; }
		[Description("The platform to generate for")]
		public List<PlatformClass> Platforms { get; set; }
		[Description("Default asset settings")]
		public AssetDefaultClass AssetDefault { get; set; }
		[Description("List of assets to generate")]
		public List<AssetClass> Assets { get; set; }
	}

	[Documentation]
	public class PlatformClass
	{
		[Description("Platform")]
		public Platforms Type { get; set; }
		[Description("Relative path towards the project file of the platform (not the project iself, just the path)")]
		public string ProjectPath { get; set; }
		[Description("Specific Android options")]
		public AndroidOptions AndroidOptions { get; set; }
	}

	[Documentation]
	public class AndroidOptions
	{
		[Description("The folder where resources are")]
		public AndroidResourceFolder AssetFolderPrefix { get; set; }
	}

	[Documentation]
	public enum Platforms
	{
		[Description("iOS")]
		iOS,
		[Description("Android")]
		Android,
		[Description("UWP")]
		UWP
	}
	
	[Documentation]
	public enum AndroidResourceFolder
	{
		[Description("Use mipmap folders")]
		mipmap,
		[Description("Use drawable folders")]
		drawable
	}

	[Documentation]
	public class AssetDefaultClass
	{
		[Description("You can use a file pattern to include for example all files in the assetsPath, or you can leave this blank to specify individual files")]
		public string Pattern { get; set; }
		[Description("In the format 100x100 specifying width x height as default size for base resolution; can be specified individually")]
		public string Size { get; set; }
	}

	[Documentation]
	public class AssetClass
	{
		[Description("Name of the svg file (including extension)")]
		public string File { get; set; }
		[Description("Base resolution (format 100x100)")]
		public string Size { get; set; }

		internal string GetSafeFile()
		{
			// cannot contain - in Android resources
			var result = File.Replace("-", "_"); //more?
			// cannot start with number in Android resources
			if (char.IsDigit(result[0]))
				result = "_" + result;
			return result;
		}
	}
}