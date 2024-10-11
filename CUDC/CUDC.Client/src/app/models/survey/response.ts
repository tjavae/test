import { Answer } from './answer';

export class Response {
    id: string;
    surveyId: string;
    surveyTypeId: number;
    userId: string;
    cuNumber: number;
    joinNumber: number;
    answers: Answer[];
    createdOn: Date;
    createdBy: string;
    modifiedOn: Date;
    modifiedBy: string;
    submittedOn: Date;
    isRejected: boolean;
}
