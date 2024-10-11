import { BasicInfo } from '../search/basic-info';
import { Question } from './question';
import { QuestionOption } from './question-option';
import { QuestionReference } from './question-reference';
import { Response } from './response';
import { Section } from './section';
import { Survey } from './survey';

export class SurveyInfo {
    basicInfo: BasicInfo;
    survey: Survey;
    sections: Section[];
    questions: Question[];
    questionOptions: QuestionOption[];
    questionReferences: QuestionReference[];
    answers: Response;
    hasPreSubmitedSurvey: Boolean;
}
