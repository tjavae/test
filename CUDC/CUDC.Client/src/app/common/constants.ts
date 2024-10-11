export class GroupName {
   public static readonly ADMIN='ADMIN';
   public static readonly DISTRICT_EXAMINER='DISTRICT EXAMINER';
   public static readonly SUPERVISORY_EXAMINER='SUPERVISORY EXAMINER';
   public static readonly DOS_DOT_RSM='DOS/DOT/RSM';
   public static readonly PCO='PCO';
   public static readonly DSA_SPECIALIST='DSA/SPECIALIST';
   public static readonly SURVEY_VIEWER='SURVEY VIEWER';
   public static readonly TESTER='CUDC TESTER';
}

export class Module {
   public static readonly SURVEYS_MANAGEMENT='Surveys Management';
   public static readonly CAT_SURVEY='CAT Survey';
   public static readonly SE_SURVEY='SE Survey';
   public static readonly DOS_SURVEY='DOS Survey';
   public static readonly RADAR_VIEW='RADAR View';
   public static readonly PCO_EDIT_CAT_SURVEY="PCO Edit CAT Survey";
   public static readonly DOS_EDIT_SURVEY='DOS Edit Survey';
}

export class Action {
    public static readonly VIEW='View';
    public static readonly EDIT='Edit';
}

export class SurveyStatus {
    public static readonly NOT_TAKE='Has not been taken';
    public static readonly SAVED='Saved';
    public static readonly SUBMITTED='Submitted';
}