import { Question } from './question';
import { QuestionOption } from './question-option';
import { QuestionReference } from './question-reference';
import { Section } from './section';

export class QuestionsEdit {
    surveyId: string;
    sections: Section[];
    questions: Question[];
    questionOptions: QuestionOption[];
    questionReferences: QuestionReference[];
}
