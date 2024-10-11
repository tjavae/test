namespace CUDC.Common.Dtos
{
    public class UserTypeDef
    {
        public string UserType { get; set; }
        public bool CanAccess { get; set; }
        public string Region { get; set; }
        public int District { get; set; }
        public string Se { get; set; }

        public string[] Regions { get; set; }

        public string[] Ses { get; set; }

        public string[] Districts { get; set; }
        public bool ReviewCat { get; set; } = true;
        public bool EditCat { get; set; } = false;
        public bool ReviewSe { get; set; } = true;
        public bool EditSe { get; set; } = false;
        public bool ReviewDos { get; set; } = true;
        public bool EditDos { get; set; } = false;
    }
}
