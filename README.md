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

```
Class: Configuration
--------------------

Name              Type                   Req      Description                                                             
--------------------------------------------------------------------------------------------------------------------------
AssetsPath        String                          Relative path where the configuration is towards where the svg files are
Platforms         PlatformClass[]                 The platform to generate for                                            
AssetDefault      AssetDefaultClass               Default asset settings                                                  
Assets            AssetClass[]                    List of assets to generate                                              

Class: PlatformClass
--------------------

Name                Type                  Req      Description                                                                                  
------------------------------------------------------------------------------------------------------------------------------------------------
Type                Platforms (Enum)               Platform                                                                                     
ProjectPath         String                         Relative path towards the project file of the platform (not the project iself, just the path)
AndroidOptions      AndroidOptions                 Specific Android options                                                                     

Class: AndroidOptions
---------------------

Name                   Type                              Req      Description                   
------------------------------------------------------------------------------------------------
AssetFolderPrefix      AndroidResourceFolder (Enum)               The folder where resources are

Enum: Platforms
---------------

Name         Description
------------------------
iOS          iOS        
Android      Android    
UWP          UWP        

Enum: AndroidResourceFolder
---------------------------

Name          Description         
----------------------------------
mipmap        Use mipmap folders  
drawable      Use drawable folders

Class: AssetDefaultClass
------------------------

Name         Type        Req      Description                                                                                                                           
------------------------------------------------------------------------------------------------------------------------------------------------------------------------
Pattern      String               You can use a file pattern to include for example all files in the assetsPath, or you can leave this blank to specify individual files
Size         String               In the format 100x100 specifying width x height as default size for base resolution; can be specified individually                    

Class: AssetClass
-----------------

Name      Type        Req      Description                               
-------------------------------------------------------------------------
File      String               Name of the svg file (including extension)
Size      String               Base resolution (format 100x100)     
```