# Stefandevo.Genyman.XamarinAssets
The idea is to have a single folder with vector based SVG files. With this generator it will convert these SVG files to all necessary png formats for iOS, Android and UWP.
## Getting Started
Stefandevo.Genyman.XamarinAssets is a **[genyman](http://genyman.net)** code generator. If you haven't installed **genyman** run following command:
```
dotnet tool install -g genyman
```
_Genyman is a .NET Core Global tool and thereby you need .NET Core version 2.1 installed._
## New Configuration file 
```
genyman new Stefandevo.Genyman.XamarinAssets
```
## Sample Configuration file 
```
{
    "genyman": {
        "packageId": "Stefandevo.Genyman.XamarinAssets",
        "info": "This is a Genyman configuration file - https://genyman.github.io/docs/"
    },
    "configuration": {
        "assetsPath": "PathToYourAssets",
        "platforms": [
            {
                "type": "iOS",
                "projectPath": "YourProject.iOS"
            },
            {
                "type": "Android",
                "projectPath": "YourProject.Droid",
                "androidOptions": {
                    "assetFolderPrefix": "mipmap"
                }
            }
        ],
        "assets": [
            {
                "file": "FirstAsset.svg",
                "size": "100x100"
            }
        ]
    }
}
```
## Documentation 
### Class Configuration
| Name | Type | Req | Description |
| --- | --- | :---: | --- |
| AssetsPath | String | * | Relative path where the configuration is towards where the svg files are |
| Platforms | PlatformClass[] |  | The platform to generate for |
| AssetDefault | AssetDefaultClass | * | Default asset settings |
| Assets | AssetClass[] | * | List of assets to generate |
### Class PlatformClass
| Name | Type | Req | Description |
| --- | --- | :---: | --- |
| Type | Platforms (Enum) | * | Platform |
| ProjectPath | String | * | Relative path towards the project file of the platform (not the project iself, just the path) |
| AndroidOptions | AndroidOptions |  | Specific Android options |
### Class AndroidOptions
| Name | Type | Req | Description |
| --- | --- | :---: | --- |
| AssetFolderPrefix | AndroidResourceFolder (Enum) |  | The folder where resources are |
### Enum Platforms
| Name | Description |
| --- | --- |
| iOS | iOS |
| Android | Android |
| UWP | UWP |
### Enum AndroidResourceFolder
| Name | Description |
| --- | --- |
| mipmap | Use mipmap folders |
| drawable | Use drawable folders |
### Class AssetDefaultClass
| Name | Type | Req | Description |
| --- | --- | :---: | --- |
| Pattern | String |  | You can use a file pattern to include for example all files in the assetsPath, or you can leave this blank to specify individual files |
| Size | String |  | In the format 100x100 specifying width x height as default size for base resolution; can be specified individually |
### Class AssetClass
| Name | Type | Req | Description |
| --- | --- | :---: | --- |
| File | String | * | Name of the svg file (including extension) |
| Size | String |  | Base resolution (format 100x100) |
