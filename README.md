# Genyman.XamarinAssets

The idea is to have a single folder with vector based SVG files. With this generator it will convert these SVG files to all necessary png formats for iOS, Android and UWP.

Use [genyman](https://genyman.github.io/docs/) to create a new configuration template:

```
genyman new Stefandevo.Genyman.XamarinAssets
```

A sample configuration file:

```
{
    "genyman": {
        "packageId": "Stefandevo.Genyman.XamarinAssets",
        "version": "",
        "nugetSource": "",
        "info": "This is a Genyman configuration file - https://genyman.github.io/docs/"
    },
    "configuration": {
        "assetsPath": "Assets",
        "platforms": [
            {
                "type": "iOS",
                "projectPath": "XamarinAssetsApp/iOS"
            },
            {
                "type": "Android",
                "projectPath": "XamarinAssetsApp/Droid",
                "androidOptions": {
                    "assetFolderPrefix": "mipmap"
                }
            },
            {
                "type": "UWP",
                "projectPath": "XamarinAssetsApp/UWP"
            }
        ],
        "assetDefault": {
            "pattern": "*.*",
            "size": "24x24"
        },
        "assets": [
            {
                "file": "house.svg",
		"size": "32x32"
            },
            {
                "file": "vlc.svg",
		"size": "64x64"
            }
        ]
    }
}

```

Configuration options:

- `assetsPath`: relative path where the configuration is towards where the svg files are.
- `platforms`:
  - `type`: can be `iOS`, `Android` or `UWP`
  - `projectPath`: relative path towards the project file of the platform (do not include the project file itself!)
  - `androidOptions`: extra options for Android; `assetFolderPrefix` can be `mipmap` (default) or `drawable`
- `assetDefault`:
  - `pattern`: you can use a file pattern to include for example all files in the `assetsPath`, or you can leave this blank to specify individual files
  - `size`: in the format `100x100` specifying `width` x `height` as default size for base resolution; can be specified individually
- `assets`:
  - `file`: name of the svg file (including extension)
  - `size`: base resolution (see above for format)

If no `size` is set for individual file, the default from `assetDefault` is used.
If individual asset is set, it's not duplicated when `pattern` is set for `assetDefault`.

Upon generation, all png sizes for all resolutions are created in the correct folder for each platform project, and the `.csproj` for each platform project is updated.
