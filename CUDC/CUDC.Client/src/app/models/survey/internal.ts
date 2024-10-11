import { QuestionOption } from './question-option';
import { QuestionType } from './question-type';

export class _Section {
    id: string;
    title: string;
    description: string;
    position: number;
    questions: _Question[];
    references: _QuestionReference[];
}

export class _Question {
    id: string;
    number: string;
    type: QuestionType;
    text: string;
    maxLength: number;
    position: number;
    isRequired: boolean;
    options: QuestionOption[];
    answer: string;
}

export class _QuestionReference {
    id: string;
    referenceQuestionId: string;
    questionId: string;
    referenceOptionId: string;
    questionOptionId: string;    
    rNumber: string;
    qNumber: string;
    rOption: string;
    qOption: string;
}
