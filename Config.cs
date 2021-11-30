using System.Text;

namespace Autowho
{
    static class Config
    {

        private static IDictionary<string, string> config;

        private static void CreateConfig()
        {
            config = new Dictionary<string, string>();

            string configFile = @"Autowho.txt";

            string configUrl = Path.GetFileName(configFile);
            if (!File.Exists(configUrl))
            {
                File.Copy(configFile, configUrl);
            }

            var fileStream = File.Open(configUrl, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
            using (var stream = fileStream)
            using (var reader = new StreamReader(stream, Encoding.UTF8))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] values = line.Split(": ");
                    if (!config.ContainsKey(values[0]))
                    {
                        config.Add(values[0], values[1]);
                    }
                }
            }
            fileStream.Close();
        }

        /*
        They key must be saved as scan code. I can't find a way to convert string to scan code and it doesnt matter since we can register the key event 
        when we make the GUI and by default, gives you the scan code. By default it is 75 which is 'k'. If you set it to default/27 (Escape) then it will do the manual /who
        */


        public static byte GetAutoWhoKey()
        {
            return Byte.Parse(config["key"]);
        }

        public static string getLogLocation()
        {
            if(config["log"] == "default")
            {
                return Directory.GetCurrentDirectory() + "/logs/latest.log";
            }

            return config["log"];
        }

        public static void Create()
        {
            CreateConfig();
        }
    }
}
