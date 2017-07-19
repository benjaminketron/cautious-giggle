using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace CautiousGiggle.Core.Config
{
    public class ConfigurationSettings
    {
        public ConfigurationSettings()
        {

        }

        public ConfigurationSettings(string filename)
        {
            var configurationSettings = LoadFileFromStorageAsync(filename).Result;

            this.DatabasePath = configurationSettings?.DatabasePath;
            this.TodoistApiUrl = configurationSettings?.TodoistApiUrl;
            this.Token = configurationSettings?.Token;
        }

        public string DatabasePath { get; set; }
        public string TodoistApiUrl { get; set; }

        /// <summary>
        /// TODO This could eventually be stored as ApplicationDate as a result of successfull OAuth.
        /// </summary>
        public string Token { get; set; }

        protected async Task<ConfigurationSettings> LoadFileFromStorageAsync(string filename)
        {
            var result = null as ConfigurationSettings;

            var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///config.json"));
            using (var inputStream = await file.OpenReadAsync())
            using (var classicStream = inputStream.AsStreamForRead())
            using (var streamReader = new StreamReader(classicStream))
            {
                result = JsonConvert.DeserializeObject<ConfigurationSettings>(streamReader.ReadToEnd());
            }

            return result;
        }
    }
}
