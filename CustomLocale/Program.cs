using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;

namespace CustomLocale
{
    class Program
    {
        static void Main(string[] args)
        {
            string cultureName = "la-IT";

            // A base culture that we can adapt for our own culture.
            CultureInfo ci = new CultureInfo("it-IT");

            // Grab the region info too.
            RegionInfo ri = new RegionInfo(ci.Name);

            // Create the custom culture.
            CultureAndRegionInfoBuilder builder = new CultureAndRegionInfoBuilder("la-IT", CultureAndRegionModifiers.None);
            builder.LoadDataFromCultureInfo(ci);
            builder.LoadDataFromRegionInfo(ri);

            // Customization. You can use any way you prefer to load the data, XML, json, INI, database... Or better, in LDML format that we'll save later.
            builder.CultureEnglishName = "Latin";
            builder.CultureNativeName = "Latina";
            builder.CurrencyEnglishName = "Lira";
            builder.CurrencyNativeName = "Lira";
            builder.RegionEnglishName = "Italy";
            builder.RegionNativeName = "Italia";
            builder.ThreeLetterISOLanguageName = "lat";
            builder.ThreeLetterISORegionName = "ita";
            builder.ThreeLetterWindowsLanguageName = "LAT";
            builder.ThreeLetterWindowsRegionName = "ITA";
            builder.TwoLetterISOLanguageName = "la";
            builder.TwoLetterISORegionName = "it";

            // Save the custom info in LDML (XML) format to share.
            string outputDir = DateTime.Now.Ticks.ToString();
            Directory.CreateDirectory(outputDir);
            string ldmlFileName = cultureName + ".LDML";
            builder.Save(Path.Combine(outputDir, ldmlFileName));

            // Save nlp file and registry tokens. Requires administrator priviledge.
            try
            {
                // Register in current system to make it work.
                builder.Register();
                string nlpFileName = cultureName + ".nlp";
                string nlpFilePath = Path.Combine(Environment.GetEnvironmentVariable("WinDir"), "Globalization", nlpFileName);
                if (File.Exists(nlpFilePath))
                {
                    File.Copy(nlpFilePath, Path.Combine(outputDir, nlpFileName));
                }
                else
                {
                    throw new FileNotFoundException("nlp file doesn't exist, custom culture register failed", nlpFilePath);
                }

                // TODO: export reg doesn't work as expected yet.
                string registryToken = @"HKLM\SYSTEM\CurrentControlSet\Control\Nls\CustomLocale";
                string registryFileName = cultureName + ".reg";
                string[] arguments = { "export", registryToken, Path.Combine(outputDir, registryFileName) };
                ProcessStartInfo process = new ProcessStartInfo("REG.exe", string.Join(" ", arguments));
                Process.Start(process);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                while (!Exception.Equals(e.InnerException, null))
                {
                    e = e.InnerException;
                    Console.WriteLine(e.Message);
                }
            }
            finally
            {
                // In fact, copy nlp file and import the registry tokens will enable the custom culture without the needs to run any program in administrator priviledge.
                // Once the necessary files are stored, feel free to revert the change to system.
                CultureAndRegionInfoBuilder.Unregister(cultureName);
            }
        }
    }
}
