import { QuestionType } from './question-type';

export class QuestionRevision {
    id: string;
    type: QuestionType;
    text: string;
    maxLength: number;
    createdOn: Date;
    createdBy: string;
    modifiedOn: Date;
    modifiedBy: string;
}
