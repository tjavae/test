import { SurveyType } from './survey-type';

export class Survey {
    id: string;
    title: string;
    description: string;
    type: SurveyType;
    startDate: Date;
    endDate: Date;
    isActive: boolean;
    createdOn: Date;
    createdBy: string;
    modifiedOn: Date;
    modifiedBy: string;
    informationText: string;
}
