using UnityEngine;
using UnityEditor;

public class ImportHelper : AssetPostprocessor
{

    /*This Script will autoimport all sprites as texture type advanced 
     * with the attribute readable.
     * This is needed for the sprite maker! When you don't use the sprite maker
     * you can deactivate the script
     * */
    void OnPreprocessTexture()
    {
        TextureImporter importer = assetImporter as TextureImporter;
      
        //set your default settings to the importer here
        importer.textureType = TextureImporterType.Advanced;
        importer.isReadable = true;
       

    }

}