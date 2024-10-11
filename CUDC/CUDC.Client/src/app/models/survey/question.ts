import { QuestionRevision } from './question-revision';

export class Question {
    id: string;
    sectionId: string;
    sectionPos: number;
    number: string;
    position: number;
    isRequired: boolean;
    revision: QuestionRevision;
    createdOn: Date;
    createdBy: string;
    modifiedOn: Date;
    modifiedBy: string;
}
