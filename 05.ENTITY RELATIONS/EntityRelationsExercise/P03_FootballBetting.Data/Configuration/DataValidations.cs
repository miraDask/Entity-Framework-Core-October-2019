namespace P03_FootballBetting.Data.Configuration
{
    public static class DataValidations
    {
        public static class Color
        {
            public const int NameMaxLength = 30;
        }

        public static class Country
        {
            public const int NameMaxLength = 30;
        }

        public static class Player
        {
            public const int NameMaxLength = 30;
        }

        public static class Position
        {
            public const int NameMaxLength = 30;
        }

        public static class Team
        {
            public const int NameMaxLength = 50;
            
            public const int InitialsMaxLength = 3;

            public const int LogoUrlMaxLength = 250;
        }

        public static class Town
        {
            public const int NameMaxLength = 30;
        }

        public static class User
        {
            public const int UsernameMaxLength = 50;

            public const int PasswordMaxLength = 30;

            public const int EmailMaxLength = 30;

            public const int NameMaxLength = 100;

        }
    }
}
