using System;
using System.Collections.Generic;
using System.Globalization;
using Genyman.Core;
using System.IO;
using System.Linq;
using Genyman.Core.Helpers;
using ServiceStack;
using SkiaSharp;
using SKSvg = SkiaSharp.Extended.Svg.SKSvg;

namespace Stefandevo.Genyman.XamarinAssets.Implementation
{
	public class Generator : GenymanGenerator<Configuration>
	{
		public override void Execute()
		{
			// get and write asset log
			var assetLogFile = Path.Combine(WorkingDirectory, "assets-log.json");
			if (!File.Exists(assetLogFile))
				File.WriteAllText(assetLogFile, string.Empty);

			var assetLogs = new List<AssetLog>();
			var assetLogsContents = File.ReadAllText(assetLogFile);
			if (!string.IsNullOrEmpty(assetLogsContents))
				assetLogs = assetLogsContents.FromJson<List<AssetLog>>();

			var needToUpdateLog = false;

			if (!string.IsNullOrEmpty(Configuration.AssetDefault?.Pattern))
			{
				var files = Directory.GetFiles(Path.Combine(WorkingDirectory, Configuration.AssetsPath), Configuration.AssetDefault.Pattern);
				foreach (var file in files)
				{
					var fileName = new FileInfo(file).Name;
					if (Configuration.Assets.FirstOrDefault(q => q.File == fileName) == null)
						Configuration.Assets.Add(new AssetClass() {File = fileName});
				}
			}

			if (!string.IsNullOrEmpty(Configuration.AssetDefault?.Size))
			{
				foreach (var asset in Configuration.Assets)
				{
					if (string.IsNullOrEmpty(asset.Size))
						asset.Size = Configuration.AssetDefault.Size;
				}
			}

			foreach (var asset in Configuration.Assets)
			{
				var path = Path.Combine(WorkingDirectory, Configuration.AssetsPath, asset.File);
				if (!File.Exists(path))
				{
					Log.Error($"Could not find {asset.File} in {Configuration.AssetsPath}");
					continue;
				}

				var assetFileInfo = new FileInfo(path);
				var found = assetLogs.FirstOrDefault(a => a.AssetFile == asset.File);

				if (found == null)
				{
					assetLogs.Add(new AssetLog()
					{
						AssetFile = asset.File,
						LastUpdated = assetFileInfo.LastWriteTimeUtc,
						FileSize = assetFileInfo.Length,
						Size = asset.Size
					});
				}
				else
				{
					if (found.FileSize == assetFileInfo.Length && found.LastUpdated == assetFileInfo.LastWriteTimeUtc && found.Size == asset.Size)
					{
						Log.Debug($"Skipping {asset.File} - No Change");
						continue;
					}

					found.LastUpdated = assetFileInfo.LastWriteTimeUtc;
					found.FileSize = assetFileInfo.Length;
					found.Size = asset.Size;
				}

				needToUpdateLog = true;
				Log.Information($"Processing {asset.File}");

				var svg = new SKSvg();
				svg.Load(path);

				var sourceActualWidth = svg.Picture.CullRect.Width;
				var sourceActualHeight = svg.Picture.CullRect.Height;

				var wantedBaseWidth = 0.0F;
				var wantedBaseHeight = 0.0F;

				if (!string.IsNullOrEmpty(asset.Size))
				{
					var sizeParts = asset.Size.Split('x');
					if (sizeParts.Length == 2)
					{
						var widthOk = float.TryParse(sizeParts[0], NumberStyles.Integer, new CultureInfo("en-US"), out wantedBaseWidth);
						var heightOk = float.TryParse(sizeParts[1], NumberStyles.Integer, new CultureInfo("en-US"), out wantedBaseHeight);

						if (!widthOk) wantedBaseWidth = sourceActualWidth;
						if (!heightOk) wantedBaseHeight = sourceActualHeight;
					}
				}
				else
				{
					wantedBaseWidth = sourceActualWidth;
					wantedBaseHeight = sourceActualHeight;
				}

				var nominalRatio = Math.Max((double) wantedBaseWidth / (double) sourceActualWidth, (double) wantedBaseHeight / (double) sourceActualHeight);

				foreach (var platform in Configuration.Platforms)
				{
					var configs = new List<Output>();
					switch (platform.Type)
					{
						case Platforms.iOS:
							configs.AddRange(new[]
							{
								new Output() {Ratio = 1},
								new Output() {Ratio = 2, Suffix = "@2x"},
								new Output() {Ratio = 3, Suffix = "@3x"}
							});
							break;
						case Platforms.Android:
							configs.AddRange(new[]
							{
								new Output() {Ratio = 1.0, Path = "drawable-mdpi"},
								new Output() {Ratio = 1.5, Path = "drawable-hdpi"},
								new Output() {Ratio = 2, Path = "drawable-xhdpi"},
								new Output() {Ratio = 3, Path = "drawable-xxhdpi"},
								new Output() {Ratio = 4, Path = "drawable-xxxhdpi"},
							});
							break;
						case Platforms.UWP:
							configs.AddRange(new[]
							{
								new Output() {Ratio = 1.0, Suffix = ".scale-100"},
								new Output() {Ratio = 1.25, Suffix = ".scale-125"},
								new Output() {Ratio = 1.50, Suffix = ".scale-150"},
								new Output() {Ratio = 2, Suffix = ".scale-200"},
								new Output() {Ratio = 4, Suffix = ".scale-400"},
							});
							break;
						default:
							throw new ArgumentOutOfRangeException();
					}

					foreach (var config in configs)
					{
						var destinationFolder = "Resources";
						if (platform.Type == Platforms.UWP)
							destinationFolder = "Assets";
						var destinationFile = Path.Combine(WorkingDirectory, platform.ProjectPath, destinationFolder);

						if (string.IsNullOrEmpty(config.Path))
							destinationFile = Path.Combine(destinationFile, $"{asset.GetSafeFile().Replace(".svg", "")}{config.Suffix}.png");
						else
							destinationFile = Path.Combine(destinationFile, config.Path, $"{asset.GetSafeFile().Replace(".svg", ".png")}");

						var destinationPath = new FileInfo(destinationFile);
						if (!Directory.Exists(destinationPath.DirectoryName))
							Directory.CreateDirectory(destinationPath.DirectoryName);

						var resizeRatio = config.Ratio;

						var adjustRatio = nominalRatio * resizeRatio;
						var scaledWidth = sourceActualWidth * adjustRatio;
						var scaledHeight = sourceActualHeight * adjustRatio;

						var bmp = new SKBitmap((int) scaledWidth, (int) scaledHeight);
						var canvas = new SKCanvas(bmp);

						// Make a matrix to scale the SVG
						var matrix = SKMatrix.MakeScale((float) adjustRatio, (float) adjustRatio);

						canvas.Clear(SKColors.Transparent);

						// Draw the svg onto the canvas with our scaled matrix
						canvas.DrawPicture(svg.Picture, ref matrix);

						// Save the op
						canvas.Save();

						// Export the canvas
						var img = SKImage.FromBitmap(bmp);

						var data = img.Encode(SKEncodedImageFormat.Png, 100);
						using (var fs = File.Open(destinationFile, FileMode.Create))
						{
							data.SaveTo(fs);
						}

						var platformProjectFolder = Path.Combine(WorkingDirectory, platform.ProjectPath);
						switch (platform.Type)
						{
							case Platforms.iOS:
								platformProjectFolder.AddXamarinIosResource(destinationFile);
								break;
							case Platforms.Android:
								platformProjectFolder.AddXamarinAndroidResource(destinationFile);
								break;
							case Platforms.UWP:
								platformProjectFolder.AddXamarinUWPResource(destinationFile);
								break;
						}
					}
				}
			}

			if (needToUpdateLog)
				File.WriteAllText(assetLogFile, assetLogs.ToJson());
		}
	}

	internal class Output
	{
		public double Ratio { get; set; }
		public string Path { get; set; }
		public string Suffix { get; set; }
	}
}