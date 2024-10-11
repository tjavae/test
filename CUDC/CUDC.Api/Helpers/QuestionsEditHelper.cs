using CUDC.Api.Data;
using CUDC.Common.Dtos;
using CUDC.Common.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CUDC.Api.Helpers
{
    public class QuestionsEditHelper
    {
        private readonly SurveyContext _context;
        private int _surveyId;
        private List<Section> _oldSections;
        private List<Section> _newSections;
        private List<Question> _oldQuestions;
        private List<Question> _newQuestions;
        private List<QuestionRevision> _oldRevisions;
        private List<QuestionRevision> _newRevisions;
        private List<QuestionOption> _oldOptions;
        private bool _hasResponses;
        private DateTime _createdOn;
        private string _createdBy;
        private DateTime _modifiedOn;
        private string _modifiedBy;
        private List<QuestionReference> _oldReferences;

        public QuestionsEditHelper(SurveyContext context)
        {
            _context = context;
        }

        public void SaveChanges(QuestionsEdit questions)
        {
            _createdOn = _modifiedOn = (DateTime)questions.ModifiedOn;
            _createdBy = _modifiedBy = questions.ModifiedBy;
            LoadCurrentData(questions.SurveyId);
            SaveUpdatedSections(questions.Sections);
            SaveUpdatedQuestions(questions.Questions);
            SaveUpdatedRevisions(questions.Questions);
            SaveUpdatedOptions(questions.QuestionOptions);
            SaveUpdatedReferences(questions.QuestionReferences);
            RemoveOldOptions();
        }

        private void LoadCurrentData(Guid surveyGuid)
        {
            _surveyId = _context.Surveys.Single(x => x.UniqueId == surveyGuid).Id;
            _oldSections = _context.Sections.Where(x => x.SurveyId == _surveyId && x.Description != "SystemForcedDeletedzzxxyy").ToList();
            _oldQuestions = _context.Questions.Where(x => x.SurveyId == _surveyId && x.IsActive).ToList();
            _oldRevisions = (from q in _context.Questions
                             join qr in _context.QuestionRevisions on q.Id equals qr.QuestionId
                             where q.SurveyId == _surveyId && q.IsActive && qr.IsActive
                             select qr).ToList();
            _oldOptions = (from q in _context.Questions
                           join qr in _context.QuestionRevisions on q.Id equals qr.QuestionId
                           join qo in _context.QuestionOptions on qr.Id equals qo.QuestionRevisionId
                           where q.SurveyId == _surveyId && q.IsActive && qr.IsActive
                           select qo).ToList();

            _oldReferences = (from q in _context.Questions
                              join qr in _context.QuestionReferences on q.UniqueId equals qr.ReferenceQuestionId
                              where q.SurveyId == _surveyId && q.IsActive
                              select qr).ToList();
            _hasResponses = _context.Responses.Any(x => x.SurveyId == _surveyId);
        }

        private void SaveUpdatedSections(IEnumerable<SectionDto> sections)
        {
            _newSections = [];
            List<Section> oldSectionToDeactivate; //deactivate section that is being removed from survey
            List<Section> oldSectionToMatched = [];

            foreach (var sectionDto in sections)
            {
                var section = _oldSections.SingleOrDefault(x => x.UniqueId == sectionDto.Id);
                if (section != null)
                {
                    oldSectionToMatched.Add(section);
                    section.Title = sectionDto.Title;
                    section.Description = sectionDto.Description;
                    section.Position = sectionDto.Position;
                    section.ModifiedOn = _modifiedOn;
                    section.ModifiedBy = _modifiedBy;
                    _context.Sections.Update(section);
                }
                else
                {
                    section = new Section
                    {
                        UniqueId = sectionDto.Id,
                        SurveyId = _surveyId,
                        Title = sectionDto.Title,
                        Description = sectionDto.Description,
                        Position = sectionDto.Position,
                        CreatedOn = _createdOn,
                        CreatedBy = _createdBy
                    };
                    _newSections.Add(section);   // these new section saved for questions information

                    _context.Sections.Add(section);
                }
            }

            if (oldSectionToMatched.Count != 0)
            {
                oldSectionToDeactivate = (from q in _oldSections
                                          select q).Except(from qm in oldSectionToMatched select qm).ToList();


                foreach (var item in oldSectionToDeactivate)
                {
                    item.Description = "SystemForcedDeletedzzxxyy";
                    item.ModifiedOn = _modifiedOn;
                    item.ModifiedBy = _modifiedBy;
                    _context.Sections.Update(item);  // Records being removed, flag as inactive
                }
            }

            _context.SaveChanges();
        }

        private void SaveUpdatedQuestions(IEnumerable<QuestionDto> questions)
        {
            _newQuestions = []; // variable used for revisions, which is handled after this fn
            List<Question> oldQuestionsToDeactivate; //deactivate questions that is being removed from survey
            List<Question> oldQuestionsMatched = [];
            if (_newSections.Count == 0 && _oldSections.Count == 0)
            {
                var defaultSection = new Section
                {
                    UniqueId = Guid.NewGuid(),
                    SurveyId = _surveyId,
                    Title = "Default Section Name",
                    Position = 1,
                    CreatedOn = _createdOn,
                    CreatedBy = _createdBy
                };
                _newSections.Add(defaultSection);
                _context.Sections.Add(defaultSection);
                _context.SaveChanges();
            }
            foreach (var questionDto in questions)
            {
                var section = _newSections.FirstOrDefault(x => x.Position == questionDto.SectionPos);
                var section_old = _oldSections.FirstOrDefault(x => x.Position == questionDto.SectionPos);

                var question = _oldQuestions.SingleOrDefault(x => x.UniqueId == questionDto.Id);
                int? tempid = 0;

                if (section == null)
                {
                    if (section_old != null)
                    {
                        tempid = section_old.Id;
                    }   
                }
                else
                {
                    tempid = section.Id;
                }

                if (question != null)
                {
                    oldQuestionsMatched.Add(question);
                    question.SectionId = tempid;
                    question.Number = questionDto.Number;
                    question.Position = questionDto.Position;
                    question.IsRequired = questionDto.IsRequired;
                    question.ModifiedOn = _modifiedOn;
                    question.ModifiedBy = _modifiedBy;
                    _context.Questions.Update(question);
                }
                else
                {
                    question = new Question
                    {
                        UniqueId = questionDto.Id,
                        SurveyId = _surveyId,
                        SectionId = tempid,
                        Number = questionDto.Number,
                        Position = questionDto.Position,
                        IsRequired = questionDto.IsRequired,
                        IsActive = true,
                        CreatedOn = _createdOn,
                        CreatedBy = _createdBy
                    };
                    _newQuestions.Add(question);
                    _context.Questions.Add(question);
                }
            }

            if (oldQuestionsMatched.Count != 0)
            {
                oldQuestionsToDeactivate = (from q in _oldQuestions
                                            select q).Except(from qm in oldQuestionsMatched select qm).ToList();

                foreach (var item in oldQuestionsToDeactivate)
                {
                    item.IsActive = false;
                    item.ModifiedOn = _modifiedOn;
                    item.ModifiedBy = _modifiedBy;
                    _context.Questions.Update(item);  // Records being removed, flag as inactive
                }
            }
            _context.SaveChanges();
        }

        private void SaveUpdatedRevisions(IEnumerable<QuestionDto> questions)
        {           
            _newRevisions = [];
            List<QuestionRevision> oldRevisionsToDeactivate; //deactivate revisions that is being removed from survey
            List<QuestionRevision> oldRevisionsMatched = [];

            foreach (var questionDto in questions)
            {
                var type = _context.QuestionTypes.SingleOrDefault(x => x.Text == questionDto.Revision.Type.ToString());
                int typeId; 
                if (type != null)
                    typeId = type.Id;
                else
                    continue;

                var oldQuestions = _oldQuestions.SingleOrDefault(x => x.UniqueId == questionDto.Id && (x.Position == questionDto.Position) && (x.Section.UniqueId == questionDto.SectionId));       //comes from SaveUpdatedQuestions fn
                var revision = new QuestionRevision();

                if (oldQuestions != null)
                {
                    revision = _oldRevisions.SingleOrDefault(x => x.QuestionId == oldQuestions.Id && x.QuestionTypeId == typeId);
                }

                if (revision != null && ((revision.Id != 0 && !_hasResponses) || oldQuestions != null))
                {
                    oldRevisionsMatched.Add(revision);
                    revision.QuestionTypeId = typeId;
                    revision.Text = questionDto.Revision.Text;
                    revision.MaxLength = questionDto.Revision.MaxLength;
                    revision.ModifiedOn = _modifiedOn;
                    revision.ModifiedBy = _modifiedBy;
                    _context.QuestionRevisions.Update(revision);
                }
                else
                {
                    var question = _newQuestions.SingleOrDefault(x => x.Number.Equals(questionDto.Number) && (x.Position == questionDto.Position) && (x.Section.UniqueId == questionDto.SectionId));
                    if (question != null)
                    {
                        revision = new QuestionRevision
                        {
                            UniqueId = Guid.NewGuid(),
                            QuestionId = question.Id,
                            QuestionTypeId = typeId,
                            Text = questionDto.Revision.Text,
                            MaxLength = questionDto.Revision.MaxLength,
                            IsActive = true,
                            CreatedOn = _createdOn,
                            CreatedBy = _createdBy
                        };
                        _newRevisions.Add(revision);
                        _context.QuestionRevisions.Add(revision);
                    }                    
                }
            }

            if (oldRevisionsMatched.Count != 0)
            {
                oldRevisionsToDeactivate = (from q in _oldRevisions
                                            select q).Except(from qm in oldRevisionsMatched select qm).ToList();

                foreach (var item in oldRevisionsToDeactivate)
                {
                    item.IsActive = false;
                    item.ModifiedOn = _modifiedOn;
                    item.ModifiedBy = _modifiedBy;
                    _context.QuestionRevisions.Update(item);  // Records being removed, flag as inactive
                }
            }
            _context.SaveChanges();
        }

        private void SaveUpdatedOptions(IEnumerable<QuestionOptionDto> options)
        {
            var option = new QuestionOption();
            foreach (var optionDto in options)
            {
                QuestionRevision revision;

                if (_newQuestions.Count != 0)
                {
                    revision = (from q in _newQuestions
                                join r in _newRevisions on q.Id equals r.QuestionId
                                where q.UniqueId == optionDto.QuestionId
                                select r).FirstOrDefault();
                    if (revision != null && revision.Id != 0)
                    {
                        option = new QuestionOption
                        {
                            UniqueId = (Guid)optionDto.Id,
                            QuestionRevisionId = revision.Id,
                            Text = optionDto.Text,
                            Position = optionDto.Position,
                            CreatedOn = _createdOn,
                            CreatedBy = _createdBy
                        };
                        _context.QuestionOptions.Add(option);
                    }
                }

                var prevRevision = (from q in _oldQuestions
                                    join r in _oldRevisions on q.Id equals r.QuestionId
                                    where q.UniqueId == optionDto.QuestionId
                                    select r).FirstOrDefault();
                var prevOption = _oldOptions.FirstOrDefault(x => x.QuestionRevisionId == prevRevision.Id && (x.Text.ToString() == optionDto.Text.ToString()));

                if (prevRevision != null && prevOption == null)
                {
                    option = new QuestionOption
                    {
                        UniqueId = Guid.NewGuid(),
                        QuestionRevisionId = prevRevision.Id,
                        Text = optionDto.Text,
                        Position = optionDto.Position,
                        CreatedOn = _createdOn,
                        CreatedBy = _createdBy
                    };
                    _context.QuestionOptions.Add(option);
                }
                else
                {                    
                    _oldOptions.Remove(prevOption);
                }
            }
            _context.SaveChanges();
        }

        private void SaveUpdatedReferences(IEnumerable<QuestionReferenceDto> references)
        {
            var reference = new QuestionReference();

            foreach (var referenceDto in references)
            {
                var prevReference = _oldReferences.SingleOrDefault(x => x.UniqueId == referenceDto.Id);
                if (prevReference != null && prevReference.Id != 0)
                {
                    // Modify prevReference if there are any differences between prevReference and referenceDto
                    if (prevReference.QuestionId != referenceDto.QuestionId
                        || prevReference.QuestionOptionId != referenceDto.QuestionOptionId
                        || prevReference.ReferenceQuestionId != referenceDto.ReferenceQuestionId
                        || prevReference.ReferenceOptionId != referenceDto.ReferenceOptionId)
                    {
                        prevReference.QuestionId = referenceDto.QuestionId;
                        prevReference.QuestionOptionId = referenceDto.QuestionOptionId;
                        prevReference.ReferenceQuestionId = referenceDto.ReferenceQuestionId;
                        prevReference.ReferenceOptionId = referenceDto.ReferenceOptionId;
                        prevReference.ModifiedOn = _modifiedOn;
                        prevReference.ModifiedBy = _modifiedBy;
                        _context.QuestionReferences.Update(prevReference);
                    }
                }
                // add new reference
                else
                {
                    reference = new QuestionReference
                    {
                        UniqueId = (Guid)referenceDto.Id,
                        ReferenceQuestionId = referenceDto.ReferenceQuestionId,
                        ReferenceOptionId = referenceDto.ReferenceOptionId,
                        QuestionId = referenceDto.QuestionId,
                        QuestionOptionId = referenceDto.QuestionOptionId,
                        CreatedOn = _createdOn,
                        CreatedBy = _createdBy
                    };
                    _context.QuestionReferences.Add(reference);
                }
            }
            // Remove old references
            var refIdList = references.Select(r => r.Id).ToList();
            List<QuestionReference> refDiffLists = _oldReferences.Where(x => !refIdList.Contains(x.UniqueId)).ToList();
            foreach (var rDto in refDiffLists)
            {
                _context.QuestionReferences.Remove(rDto);
            }
            _context.SaveChanges();
        }

        private void RemoveOldOptions()
        {
            if (!_hasResponses)
            {
                if (_oldOptions != null)
                {
                    foreach (var option in _oldOptions)
                    {
                        _context.QuestionOptions.Remove(option);
                    }
                    _context.SaveChanges();
                }
            }
        }
    }
}
