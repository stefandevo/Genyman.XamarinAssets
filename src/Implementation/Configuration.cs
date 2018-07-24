using System;
using System.Collections.Generic;
using Genyman.Core;

namespace Stefandevo.Genyman.XamarinAssets.Implementation
{
	public class Configuration
	{
		public string AssetsPath { get; set; }
		public List<PlatformClass> Platforms { get; set; }
		public AssetDefaultClass AssetDefault { get; set; }
		public List<AssetClass> Assets { get; set; }
	}

	public class PlatformClass
	{
		public Platforms Type { get; set; }
		public string ProjectPath { get; set; }
	}

	public enum Platforms
	{
		iOS,
		Android,
		UWP
	}

	public class AssetClass
	{
		public string File { get; set; }
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
	
	public class AssetDefaultClass
	{
		public string Pattern { get; set; }
		public string Size { get; set; }
	}
}		