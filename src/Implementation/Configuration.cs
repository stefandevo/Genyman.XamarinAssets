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
	}
	
	public class AssetDefaultClass
	{
		public string Pattern { get; set; }
		public string Size { get; set; }
	}
}		