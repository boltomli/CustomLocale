using System.Globalization;

namespace CustomLocale
{
    class Program
    {
        static void Main(string[] args)
        {
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

            // Save the custom info in LDML (XML) format.
            builder.Save("la-IT.LDML");

            // Register in current system to make it work.
            builder.Register();

            // TODO: Save nlp file and registry tokens.
            // In fact, copy nlp file and import the registry tokens will enable the custom culture without the needs to run any program in administrator priviledge.
            // Once the necessary files are stored, feel free to revert the change to system.
        }
    }
}
