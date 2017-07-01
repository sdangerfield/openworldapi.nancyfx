using System;

namespace OpenWorld.Model
{
    public static class owBaseConst
    {
        public const int LEN_SHORT = 10;
        public const int LEN_LONG = 42;
        public static DateTime minSQLDate = DateTime.Parse("1/1/1800");

        public const String sysTreeNameStatistic = "System Stat Tree";
        public const String sysTreeNameDevelopment = "Development Path Tree";
        public const String sysTreeNameItemType = "Item Type Tree";
        public const String sysTreeNameCombat = "Combat Stat Tree";
        public const String sysTreeNameRuleSet = "Rule Set Types";


        public static String[] TreeNames = { sysTreeNameStatistic, sysTreeNameDevelopment, sysTreeNameItemType };
    }
}
