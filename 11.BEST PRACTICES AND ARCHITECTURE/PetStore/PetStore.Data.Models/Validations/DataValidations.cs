namespace PetStore.Data.Models.Validations
{
    public static class DataValidations
    {
        public const int NameMaxLength = 30;
        public const int DescriptionMaxLength = 1000;

        public static class User
        {
            public const int EmailMaxLength = 100;
        }
    }
}
