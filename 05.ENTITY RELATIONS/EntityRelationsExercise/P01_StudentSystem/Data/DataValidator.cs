namespace P01_StudentSystem.Data
{
    public static class DataValidator
    {
        public static class Student
        {
            public const int NameMaxLength = 100;

            public const int PhoneNumberLength = 10;
        }

        public static class Course
        {
            public const int NameMaxLength = 80; 
        } 

        public static class Resource
        {
            public const int NameMaxLength = 50;
        }
    }
}
