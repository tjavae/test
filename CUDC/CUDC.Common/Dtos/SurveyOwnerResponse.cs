namespace CUDC.Common.Dtos
{
    public class SurveyOwnerResponse
    {
        public bool HasOwner { get; set; }

        public bool OwnerIsMe { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
