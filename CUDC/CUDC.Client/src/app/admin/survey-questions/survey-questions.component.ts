import { Component, OnInit, Input, Output} from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { UserRole } from 'src/app/models/auth/user-role';
import { _Question, _QuestionReference, _Section } from 'src/app/models/survey/internal';
import { Question } from 'src/app/models/survey/question';
import { QuestionOption } from 'src/app/models/survey/question-option';
import { QuestionReference } from 'src/app/models/survey/question-reference';
import { QuestionRevision } from 'src/app/models/survey/question-revision';
import { QuestionType } from 'src/app/models/survey/question-type';
import { QuestionsEdit } from 'src/app/models/survey/questions-edit';
import { Section } from 'src/app/models/survey/section';
import { Survey } from 'src/app/models/survey/survey';
import { SurveyInfo } from 'src/app/models/survey/survey-info';
import { SurveyType } from 'src/app/models/survey/survey-type';
import { AdminService } from 'src/app/services/admin.service';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { SurveyService } from 'src/app/services/survey.service';
import { v4 as uuidv4 } from 'uuid';

declare const $: any;

@Component({
  selector: 'app-survey-questions',
  templateUrl: './survey-questions.component.html',
  styleUrls: ['./survey-questions.component.scss']
})
export class SurveyQuestionsComponent implements OnInit {
  survey: Survey;
  surveyTypes = SurveyType;
  questions: QuestionsEdit;
  questionTypes = QuestionType;
  sections: _Section[];
  loading = true;
  modalTitle: string;
  isEdit: boolean;
  section: _Section;
  question: _Question;
  options: string[];
  selectedRQuestion: _Question; 
  selectedQuestion: _Question;
  rQuestionHasOptions: _Question[];
  questionHasOptions: _Question[];
  selectedRAnswer: QuestionOption;
  selectedAnswer: QuestionOption;
  reference: _QuestionReference;
  userRole: UserRole;
  duplicatedReference: boolean;
  sectionTitle: string;

  constructor(
    private route: ActivatedRoute, 
    private router: Router, 
    private adminSvc: AdminService, 
    private surveySvc: SurveyService,
    private authSvc: AuthenticationService) {
  }

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      let surveyId: string = params.get('surveyId');
      this.adminSvc.getSurvey(surveyId).subscribe(survey => {
        this.survey = survey;
      });

      // get userRole
      this.authSvc.getUser().subscribe(userRole => {
        this.userRole = userRole;
      });

      this.adminSvc.getQuestions(surveyId).subscribe(questions => {
        this.questions = questions;
        let surveyInfo: SurveyInfo = {
          basicInfo: null,
          survey: null,
          sections: questions.sections,
          questions: questions.questions,
          questionOptions: questions.questionOptions,
          questionReferences: questions.questionReferences,
          answers: null,
          hasPreSubmitedSurvey: false
        };
        this.sections = this.surveySvc.getSections(surveyInfo);
        if(this.sections.length === 1 && this.sections[0].id == null){           
            this.sections[0].title = "Default Section Name";
            this.sections[0].id = uuidv4();
            this.modalTitle = 'Update Default Section Name';
            this.section = this.sections[0];         
            this.isEdit = true;
            this.sectionTitle = this.sections[0].title;
            $('#add-or-edit-section').modal();
        }
        this.loading = false;
      });
    }); 
  }

  addSection(): void {
    this.modalTitle = 'Create New Section';
    this.isEdit = false;        
    this.section = new _Section();
    this.section.id = uuidv4();
    this.section.position = Math.max(...this.sections.map(sec => sec.position)) + 1;
    this.section.questions = [];
    this.section.references = [];
    this.sectionTitle = this.section.title;
    $('#add-or-edit-section form').removeClass('was-validated').addClass('needs-validation');
    $('#add-or-edit-section').modal();
  }

  editSection(section: _Section): void {
    this.modalTitle = 'Update Section';
    this.isEdit = true;    
    this.sectionTitle = section.title;
    this.section = section;
    $('#add-or-edit-section form').removeClass('was-validated').addClass('needs-validation');
    $('#add-or-edit-section').modal();
  }

  addOrEditSection(): void {
    if ($('#add-or-edit-section form')[0].checkValidity() === false) {
      $('#add-or-edit-section form').addClass('was-validated');
      return;
    }
    $('#add-or-edit-section').modal('toggle');

    if (!this.isEdit) {
      this.section.title = this.sectionTitle;
      this.sections.push(this.section);
    } else {
      let sectionIndex = this.sections.findIndex(sec => sec.id == this.section.id);
      if (sectionIndex > -1){
        this.sections[sectionIndex].title = this.sectionTitle
      }
    }
  }

  deleteSection(section: _Section): void {   
    this.section = section;
    $('#delete-section-confirmation').modal();
  }

  deleteSectionConfirmation(): void {
    this.sections = this.sections.filter(section => section !== this.section);
  }

  moveSectionUp(section: _Section): void {
    let index1 = this.sections.indexOf(section);
    let index2 = index1 - 1;
    this.swap(this.sections, index1, index2);
  }

  moveSectionDown(section: _Section): void {
    let index1 = this.sections.indexOf(section);
    let index2 = index1 + 1;
    this.swap(this.sections, index1, index2);
  }

  addQuestion(section: _Section): void {
    this.modalTitle = 'Create New Question';
    this.isEdit = false;
    this.section = section;
    this.question = new _Question();
    this.question.id = uuidv4();
    this.options = ['', ''];
    $('#add-or-edit-question form').removeClass('was-validated').addClass('needs-validation');
    $('#add-or-edit-question').modal();
  }

  editQuestion(question: _Question): void {
    this.modalTitle = 'Update Question';
    this.isEdit = true;
    this.question = question;
    this.options = ['', ''];
    if (question.options) {
      this.options = question.options.map(x => x.text);
    }
    $('#add-or-edit-question form').removeClass('was-validated').addClass('needs-validation');
    $('#add-or-edit-question').modal();
  }

  addAnswer(): void {
    this.options.push('');
  }

  addOrEditQuestion(): void {
    if ($('#add-or-edit-question form')[0].checkValidity() === false) {
      $('#add-or-edit-question form').addClass('was-validated');
      return;
    }
    $('#add-or-edit-question').modal('toggle');
    this.question.type = parseInt(<any>this.question.type);
    this.question.isRequired = `${this.question.isRequired}` == 'true';

    if (this.question.type === QuestionType.DropDownList) {
      this.question.options = [];
      this.options.forEach(text => {
        if (text) {
          let option = new QuestionOption();
          option.id = uuidv4();
          option.text = text;
          this.question.options.push(option);
        }
      });
    }

    if (!this.isEdit) {
      this.section.questions.push(this.question);
    }
  }

  deleteQuestion(question: _Question): void {
    this.question = question;    
    $('#delete-question-confirmation').modal();
  }

  deleteQuestionConfirmation(): void {
    let section = this.findSection(this.question);
    section.questions = section.questions.filter(question => question !== this.question);
    // Delete references
    section.references = section.references.filter(ref => ref.referenceQuestionId !== this.question.id);
    section.references = section.references.filter(ref => ref.questionId !== this.question.id);
  }

  moveQuestionUp(question: _Question): void {
    let section = this.findSection(question);
    let index1 = section.questions.indexOf(question);
    let index2 = index1 - 1;
    this.swap(section.questions, index1, index2);
  }

  moveQuestionDown(question: _Question): void {
    let section = this.findSection(question);
    let index1 = section.questions.indexOf(question);
    let index2 = index1 + 1;
    this.swap(section.questions, index1, index2);
  }

  save(): void {
    this.questions.sections = [];
    this.questions.questions = [];
    this.questions.questionOptions = [];
    this.questions.questionReferences = [];   
    let sectionPosition = 0;
    this.sections.forEach(section => { 
      // add sections  
      let sectionId: string = null;
      ++sectionPosition;
      if (this.sections.length > 1 || section.title || section.description) {
        let sec = new Section();
        sec.id = sectionId = section.id || uuidv4();
        sec.title = section.title;
        sec.description = section.description;
        sec.position = sectionPosition;
        this.questions.sections.push(sec);
      } 

      // add querstions & options
      let questionPosition = 0;
      section.questions.forEach(question => {
        let questionId = question.id || uuidv4();
        let ques = new Question();   
        ques.id = questionId;
        ques.sectionId = sectionId;
        ques.sectionPos = sectionPosition;        
        ques.number = question.number;        
        ques.position = ++questionPosition;
        ques.isRequired = question.isRequired;
        ques.revision = new QuestionRevision();
        ques.revision.type = question.type;
        ques.revision.text = question.text;
        ques.revision.maxLength = question.maxLength;
        this.questions.questions.push(ques);
        if (question.type === QuestionType.DropDownList) {
          let optionPosition = 0;
          question.options.forEach(option => {
            let opt = new QuestionOption();
            opt.id = option.id;
            opt.questionId = questionId;
            opt.text = option.text;
            opt.position = ++optionPosition;
            this.questions.questionOptions.push(opt);
          });
        }
      });

      // Add references
      section.references.forEach(ref => {
        let reference = new QuestionReference();
        reference.id = ref.id || uuidv4();
        reference.referenceQuestionId = ref.referenceQuestionId;
        reference.questionId = ref.questionId;
        reference.referenceOptionId = ref.referenceOptionId;
        reference.questionOptionId = ref.questionOptionId;
        this.questions.questionReferences.push(reference);
      });
    });
        
    this.adminSvc.updateQuestions(this.questions).subscribe(() => {
      this.router.navigate(['../../../surveys'], { relativeTo: this.route });
    });
  }

  cancel(): void {
    this.router.navigate(['../../../surveys'], { relativeTo: this.route });
  }

  trackByFn(index: any, item: any) {
    return index;
  }

  private findSection(question: _Question): _Section {
    let section: _Section = null;
    this.sections.forEach(sec => {
      if (sec.questions.find(x => x === question)) {
        section = sec;
      }
    });
    return section;
  }
  
  private swap<T>(arr: T[], index1: number, index2: number): void {
    let temp: T = arr[index1];
    arr[index1] = arr[index2];
    arr[index2] = temp;
  }

  canAddQuestionReferences(section: _Section): boolean{
    let questionHasOptions = section.questions.filter(q => q.type == QuestionType.DropDownList);
    if (questionHasOptions.length > 1){
      return true;
    } else {
      return false;
    }
  }

  addReference(section: _Section): void {   
    this.modalTitle = 'Add a Reference';
    this.isEdit = false;   
    this.section = section;
    this.rQuestionHasOptions = section.questions.filter(q => q.type == QuestionType.DropDownList);
    if (this.rQuestionHasOptions.length > 1){
      this.selectedQuestion = null;
      this.selectedRQuestion = null;
      this.selectedRAnswer = null;
      this.selectedAnswer = null;
      this.duplicatedReference = false;
      $('#add-or-edit-reference form').removeClass('was-validated').addClass('needs-validation');
      $('#add-or-edit-reference').modal();
    }
  }
 
  getSelectedQuestion(): void{    
    this.questionHasOptions = this.rQuestionHasOptions.filter(q => q.id != this.selectedRQuestion.id)
  }

  onEditReference(id: string, section: _Section): void {
    this.modalTitle = 'Update Reference';   
    // load existing reference to form
    this.isEdit = true;
    this.section = section;
    let ref = section.references.filter(r => r.id == id)[0];
    this.rQuestionHasOptions = section.questions.filter(q => q.type == QuestionType.DropDownList);
    this.questionHasOptions = this.rQuestionHasOptions.filter(q => q.id != ref.referenceQuestionId)
    if (ref){
      this.selectedRQuestion = section.questions.filter(q => q.id == ref.referenceQuestionId)[0];      
      this.selectedRAnswer = this.selectedRQuestion.options.filter(opt => opt.id == ref.referenceOptionId)[0];
      this.selectedQuestion = section.questions.filter(q => q.id == ref.questionId)[0];
      this.selectedAnswer = this.selectedQuestion.options.filter(opt => opt.id == ref.questionOptionId)[0];
      this.reference = ref;
    }      
    $('#add-or-edit-reference form').removeClass('was-validated').addClass('needs-validation');
    $('#add-or-edit-reference').modal();
  }

  addOrEditReference(): void {
    if ($('#add-or-edit-reference form')[0].checkValidity() === false) {
      $('#add-or-edit-reference form').addClass('was-validated');
      return;
    }    
    if (this.isEdit){
      // update reference
      let itemIndex = this.section.references.findIndex(ref => ref.id == this.reference.id);
      if (itemIndex > -1){
        this.reference.referenceQuestionId = this.selectedRQuestion.id;
        this.reference.questionId = this.selectedQuestion.id;
        this.reference.referenceOptionId = this.selectedRAnswer.id;
        this.reference.questionOptionId = this.selectedAnswer.id;
        this.reference.rNumber = this.selectedRQuestion.number;
        this.reference.rOption = this.selectedRAnswer.text;
        this.reference.qNumber = this.selectedQuestion.number;
        this.reference.qOption = this.selectedAnswer.text;
        // validate
        let refIndex = this.section.references.findIndex(r => 
          r.id !== this.section.references[itemIndex].id
          && r.referenceQuestionId == this.selectedRQuestion.id
          && r.questionId == this.selectedQuestion.id
          && r.referenceOptionId == this.selectedRAnswer.id
        );
        if (refIndex > -1){
          this.duplicatedReference = true;
          $('#add-or-edit-reference form').addClass('was-validated');
          return;
        } else {
          this.section.references[itemIndex] = this.reference;
        }
      }
    } else {
      // Add new reference
      let ref = new _QuestionReference();
      ref.id = uuidv4();
      ref.referenceQuestionId = this.selectedRQuestion.id;
      ref.questionId = this.selectedQuestion.id;
      ref.referenceOptionId = this.selectedRAnswer.id;
      ref.questionOptionId = this.selectedAnswer.id;
      ref.rNumber = this.selectedRQuestion.number;
      ref.rOption = this.selectedRAnswer.text;
      ref.qNumber = this.selectedQuestion.number;
      ref.qOption = this.selectedAnswer.text;
      // validate
      let refIndex = this.section.references.findIndex(r => 
        r.referenceQuestionId == ref.referenceQuestionId
        && r.questionId == ref.questionId
        && r.referenceOptionId == ref.referenceOptionId
      );
      if (refIndex > -1){
        this.duplicatedReference = true;
        $('#add-or-edit-reference form').addClass('was-validated');
        return;
      } else {
        this.section.references.push(ref);
      }
    }
    $('#add-or-edit-reference').modal("toggle");
  }

  onDeleteReference(id: string, section: _Section): void {
    this.modalTitle = 'Delete Reference Confirmation';
    this.section = section;
    this.reference = section.references.filter(r => r.id == id)[0];
    $('#delete-reference-confirmation').modal();
  }

  deleteReferrenceConfirmation(): void {
    this.section.references = this.section.references.filter(ref => ref.id !== this.reference.id);
  }
}
