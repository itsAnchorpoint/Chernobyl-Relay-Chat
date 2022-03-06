using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;

namespace Chernobyl_Relay_Chat
{
    public class CRCStrings
    {
        private const int GENERIC_CHANCE = 10;
        private const int REMARK_CHANCE = 25;

        private static readonly Random rand = new Random();
        private static Dictionary<string, List<string>> deathFormats, deathTimes, deathObservances, deathRemarks, deathGeneric;
        private static Dictionary<string, Dictionary<string, List<string>>> deathLevels, deathSections, deathClasses, fNames, sNames;
        private static Dictionary<string, Dictionary<string, string>> localization = new Dictionary<string, Dictionary<string, string>>();

        private static readonly Regex invalidNickRx = new Regex(@"[^a-zA-Z0-9_\-\\^{}|]");
        private static readonly Regex invalidNickFirstCharRx = new Regex(@"^[^a-zA-Z_\\^{}|]");

        private static readonly Dictionary<string, string> channelLangs = new Dictionary<string, string>()
        {
            ["#crcr_english"] = "eng",
            ["#crcr_english_rp"] = "eng",
            ["#crcr_english_shitposting"] = "eng",
            ["#crcr_tech_support"] = "eng",
            ["#crcr_russian"] = "rus",
            ["#crcr_russian_rp"] = "rus",
        };

        public static void Load()
        {
            XmlDocument xml = new XmlDocument();
            try
            {
                xml.Load(@"res\localization.xml");
                foreach (XmlNode keyNode in xml.DocumentElement.ChildNodes)
                {
                    string id = keyNode.Attributes["id"].Value;
                    foreach (XmlNode langNode in keyNode.ChildNodes)
                    {
                        string lang = langNode.Name;
                        if (!localization.ContainsKey(lang))
                            localization[lang] = new Dictionary<string, string>();
                        localization[lang][id] = langNode.InnerText;
                    }
                }
                xml.Load(@"res\localization_machine.xml");
                foreach (XmlNode keyNode in xml.DocumentElement.ChildNodes)
                {
                    string id = keyNode.Attributes["id"].Value;
                    foreach (XmlNode langNode in keyNode.ChildNodes)
                    {
                        string lang = langNode.Name;
                        if (!localization.ContainsKey(lang))
                            localization[lang] = new Dictionary<string, string>();
                        localization[lang][id] = langNode.InnerText;
                    }
                }
            }
            catch (Exception ex) when (ex is XmlException || ex is FileNotFoundException)
            {
                // Problems
                throw ex;
            }

            deathFormats = loadXmlList(@"res\death_formats.xml");
            deathTimes = loadXmlList(@"res\death_times.xml");
            deathObservances = loadXmlList(@"res\death_observances.xml");
            deathRemarks = loadXmlList(@"res\death_remarks.xml");
            deathLevels = loadXmlListDict(@"res\death_levels.xml");
            deathSections = loadXmlListDict(@"res\death_sections.xml");
            deathClasses = loadXmlListDict(@"res\death_classes.xml");
            deathGeneric = loadXmlList(@"res\death_generic.xml");

            fNames = loadXmlListDict(@"res\fnames.xml");
            sNames = loadXmlListDict(@"res\snames.xml");

            MergeLists(fNames, "actor_csky", "actor_stalker", "actor_ecolog");
            MergeLists(fNames, "actor_dolg", "actor_stalker", "actor_army");
            MergeLists(fNames, "actor_freedom", "actor_stalker", "actor_bandit");
            MergeLists(fNames, "actor_killer", "actor_stalker", "actor_bandit", "actor_ecolog");
            MergeLists(fNames, "actor_monolith", "actor_stalker", "actor_bandit", "actor_ecolog");
            MergeLists(fNames, "actor_zombied", "actor_stalker", "actor_bandit", "actor_ecolog", "actor_army");

            MergeLists(sNames, "actor_csky", "actor_stalker", "actor_ecolog");
            MergeLists(sNames, "actor_dolg", "actor_stalker", "actor_army");
            MergeLists(sNames, "actor_freedom", "actor_stalker", "actor_bandit");
            MergeLists(sNames, "actor_killer", "actor_stalker", "actor_bandit", "actor_ecolog");
            MergeLists(sNames, "actor_monolith", "actor_stalker", "actor_bandit", "actor_ecolog");
            MergeLists(sNames, "actor_zombied", "actor_stalker", "actor_bandit", "actor_ecolog", "actor_army");
        }

        public static string Localize(string id)
        {
            if (localization[CRCOptions.Language].ContainsKey(id)
                && localization[CRCOptions.Language][id] != string.Empty)
                return localization[CRCOptions.Language][id].Replace(@"\n", "\r\n");
            return id;
        }

        private static string PickRandom(List<string> list)
        {
            return list[rand.Next(list.Count)];
        }

        private static void MergeLists(Dictionary<string, Dictionary<string, List<string>>> listDict, string target, params string[] sources)
        {
            foreach (string lang in listDict.Keys)
            {
                listDict[lang][target] = new List<string>();
                foreach (string source in sources)
                {
                    listDict[lang][target] = listDict[lang][target].Concat(listDict[lang][source]).ToList();
                }
            }
        }

        public static string RandomIrcName(string faction)
        {
            return RandomName("eng", faction).Replace(' ', '_');
        }

        public static string RandomName(string faction)
        {
            return RandomName(channelLangs[CRCOptions.Channel], faction);
        }

        private static string RandomName(string lang, string faction)
        {
            faction = ValidateFaction(faction);
            return PickRandom(fNames[lang][faction]) + " " + PickRandom(sNames[lang][faction]);
        }

        public static string ValidateFaction(string faction)
        {
            return validFactions.Contains(faction) ? faction : "actor_stalker";
        }

        public static string ValidateNick(string nick)
        {
            if (nick.Length < 1)
                return Localize("strings_nick_short");
            if (nick.Length > 30)
                return Localize("strings_nick_long");
            if (invalidNickRx.Match(nick).Success)
                return Localize("strings_nick_illegal");
            if (invalidNickFirstCharRx.Match(nick).Success)
                return Localize("strings_nick_first");
            return null;
        }

        public static string DeathMessage(string name, string level, string xrClass, string section)
        {
            string lang = channelLangs[CRCOptions.Channel];
            string levelText = deathLevels[lang].ContainsKey(level) ? PickRandom(deathLevels[lang][level]) : (Localize("strings_level_unknown") + " (" + level + ")");
            string deathText;
            if (rand.Next(101) < GENERIC_CHANCE)
                deathText = PickRandom(deathGeneric[lang]);
            else if (deathSections[lang].ContainsKey(section))
                deathText = PickRandom(deathSections[lang][section]);
            else if (deathClasses[lang].ContainsKey(xrClass))
                deathText = PickRandom(deathClasses[lang][xrClass]);
            else
                deathText = Localize("strings_death_unknown") + " (" + xrClass + "|" + section + ")";

            string message = PickRandom(deathFormats[lang]);
            message = message.Replace("$when", PickRandom(deathTimes[lang]));
            message = message.Replace("$level", levelText);
            message = message.Replace("$saw", PickRandom(deathObservances[lang]));
            message = message.Replace("$name", name);
            message = message.Replace("$death", deathText);
            message = message[0].ToString().ToUpper() + message.Substring(1);
            if (rand.Next(101) < REMARK_CHANCE)
                message += ' ' + PickRandom(deathRemarks[lang]);
            return message;
        }

        private static List<string> validFactions = new List<string>()
        {
            "actor_bandit",
            "actor_csky",
            "actor_dolg",
            "actor_ecolog",
            "actor_freedom",
            "actor_stalker",
            "actor_killer",
            "actor_army",
            "actor_monolith",
            "actor_zombied",
        };

        private static Dictionary<string, List<string>> loadXmlList(string path)
        {
            Dictionary<string, List<string>> list = new Dictionary<string, List<string>>();
            XmlDocument xml = new XmlDocument();
            try
            {
                xml.Load(path);
                foreach (XmlNode langNode in xml.DocumentElement.ChildNodes)
                {
                    list[langNode.Name] = new List<string>();
                    foreach (XmlNode stringNode in langNode.ChildNodes)
                    {
                        list[langNode.Name].Add(stringNode.InnerText);
                    }
                }
            }
            catch (Exception ex) when (ex is XmlException || ex is FileNotFoundException) { }
            return list;
        }

        private static Dictionary<string, Dictionary<string, List<string>>> loadXmlListDict(string path)
        {
            Dictionary<string, Dictionary<string, List<string>>> listDict = new Dictionary<string, Dictionary<string, List<string>>>();
            XmlDocument xml = new XmlDocument();
            try
            {
                xml.Load(path);
                foreach (XmlNode langNode in xml.DocumentElement.ChildNodes)
                {
                    string lang = langNode.Name;
                    listDict[lang] = new Dictionary<string, List<string>>();
                    foreach (XmlNode keyNode in langNode.ChildNodes)
                    {
                        string key = keyNode.Name;
                        listDict[lang][key] = new List<string>();
                        XmlNode clone = keyNode.Attributes["clone"];
                        if (clone != null)
                        {
                            listDict[lang][key] = listDict[lang][clone.Value];
                        }
                        else
                        {
                            foreach (XmlNode stringNode in keyNode.ChildNodes)
                            {
                                listDict[lang][key].Add(stringNode.InnerText);
                            }
                        }
                    }
                }
            }
            catch (Exception ex) when (ex is XmlException || ex is FileNotFoundException) { }
            return listDict;
        }
    }
}
