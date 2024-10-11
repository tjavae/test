using CUDC.Api.Data;
using CUDC.Common.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CUDC.Api.Helpers
{
    public class CopySurveyHelper
    {
        private readonly SurveyContext _context;

        public CopySurveyHelper(SurveyContext context)
        {
            _context = context;
        }

        public void CopySurvey(CopySurveyRequest request)
        {
            var survey = _context.Surveys.Single(x => x.UniqueId == request.SurveyId);
            var sections = _context.Sections.Where(x => x.SurveyId == survey.Id).ToList();
            var questions = _context.Questions.Where(x => x.SurveyId == survey.Id && x.IsActive).ToList();
            var revisions = (from q in _context.Questions
                             join qr in _context.QuestionRevisions on q.Id equals qr.QuestionId
                             where q.SurveyId == survey.Id && q.IsActive && qr.IsActive
                             select qr).ToList();
            var options = (from q in _context.Questions
                           join qr in _context.QuestionRevisions on q.Id equals qr.QuestionId
                           join qo in _context.QuestionOptions on qr.Id equals qo.QuestionRevisionId
                           where q.SurveyId == survey.Id && q.IsActive && qr.IsActive
                           select qo).ToList();
            var references = (from q in _context.Questions
                              join qr in _context.QuestionReferences on q.UniqueId equals qr.ReferenceQuestionId
                              where q.SurveyId == survey.Id && q.IsActive
                              select qr).ToList();
            var createdOn = DateTime.Now;
            var createdBy = request.UserId;

            //Create copies to prevent foreign key issues
            survey = CreateCopy(survey);
            sections = CreateCopy(sections);
            questions = CreateCopy(questions);
            revisions = CreateCopy(revisions);
            options = CreateCopy(options);
            references = CreateCopy(references);

            //Survey
            survey.Id = 0;
            survey.UniqueId = Guid.NewGuid();
            survey.Title += $" (COPY {DateTime.Now:M/d/yy h:mm:ss tt})";
            survey.IsActive = false;
            survey.CreatedOn = createdOn;
            survey.CreatedBy = createdBy;
            survey.InformationText = null;
            var existingSurvey = _context.Surveys.Where(s => s.Title == survey.Title).FirstOrDefault();
            if (existingSurvey != null)
            {
                survey.Title = existingSurvey.Title + " DUPLICATED";
            }
            _context.Surveys.Add(survey);
            _context.SaveChanges();

            //Sections
            sections.ForEach(section =>
            {
                var oldSectionId = section.Id;
                section.Id = 0;
                section.UniqueId = Guid.NewGuid();
                section.SurveyId = survey.Id;
                section.CreatedOn = createdOn;
                section.CreatedBy = createdBy;
                _context.Sections.Add(section);
                _context.SaveChanges();
                questions.Where(question => question.SectionId == oldSectionId).ToList().ForEach(question =>
                {
                    question.SectionId = section.Id;
                });
            });

            //Questions
            questions.ForEach(question =>
            {
                var oldQuestionId = question.Id;
                var oldQuestionGuid = question.UniqueId;
                question.Id = 0;
                question.UniqueId = Guid.NewGuid();
                question.SurveyId = survey.Id;
                question.CreatedOn = createdOn;
                question.CreatedBy = createdBy;
                _context.Questions.Add(question);
                _context.SaveChanges();
                revisions.Where(revision => revision.QuestionId == oldQuestionId).ToList().ForEach(revision =>
                {
                    revision.QuestionId = question.Id;
                });
                // update references
                references.Where(r => r.ReferenceQuestionId == oldQuestionGuid).ToList().ForEach(r =>
                {
                    r.ReferenceQuestionId = question.UniqueId;
                });
                references.Where(r => r.QuestionId == oldQuestionGuid).ToList().ForEach(r =>
                {
                    r.QuestionId = question.UniqueId;
                });
            });

            //Revisions
            revisions.ForEach(revision =>
            {
                var oldRevisionId = revision.Id;
                revision.Id = 0;
                revision.UniqueId = Guid.NewGuid();
                revision.CreatedOn = createdOn;
                revision.CreatedBy = createdBy;
                _context.QuestionRevisions.Add(revision);
                _context.SaveChanges();
                options.Where(option => option.QuestionRevisionId == oldRevisionId).ToList().ForEach(option =>
                {
                    option.QuestionRevisionId = revision.Id;
                });
            });

            //Options
            options.ForEach(option =>
            {
                var oldOptionGuId = option.UniqueId;
                option.Id = 0;
                option.UniqueId = Guid.NewGuid();
                option.CreatedOn = createdOn;
                option.CreatedBy = createdBy;
                _context.QuestionOptions.Add(option);
                _context.SaveChanges();
                // update references
                references.Where(r => r.QuestionOptionId == oldOptionGuId).ToList().ForEach(r =>
                {
                    r.QuestionOptionId = option.UniqueId;
                });
                references.Where(r => r.ReferenceOptionId == oldOptionGuId).ToList().ForEach(r =>
                {
                    r.ReferenceOptionId = option.UniqueId;
                });
            });

            // References
            references.ForEach(reference =>
            {
                reference.Id = 0;
                reference.UniqueId = Guid.NewGuid();
                reference.CreatedOn = createdOn;
                reference.CreatedBy = createdBy;
                _context.QuestionReferences.Add(reference);
                _context.SaveChanges();
            });
        }

        private static Survey CreateCopy(Survey survey)
        {
            return new Survey
            {
                Id = survey.Id,
                UniqueId = survey.UniqueId,
                Title = survey.Title,
                Description = survey.Description,
                SurveyTypeId = survey.SurveyTypeId,
                StartDate = survey.StartDate,
                EndDate = survey.EndDate,
                IsActive = survey.IsActive,
                InformationText = survey.InformationText
            };
        }

        private static List<Section> CreateCopy(List<Section> sections)
        {
            return sections.Select(x => new Section
            {
                Id = x.Id,
                UniqueId = x.UniqueId,
                SurveyId = x.SurveyId,
                Title = x.Title,
                Description = x.Description,
                Position = x.Position
            }).ToList();
        }

        private static List<Question> CreateCopy(List<Question> questions)
        {
            return questions.Select(x => new Question
            {
                Id = x.Id,
                UniqueId = x.UniqueId,
                SurveyId = x.SurveyId,
                SectionId = x.SectionId,
                Number = x.Number,
                Position = x.Position,
                IsRequired = x.IsRequired,
                IsActive = x.IsActive
            }).ToList();
        }

        private static List<QuestionRevision> CreateCopy(List<QuestionRevision> revisions)
        {
            return revisions.Select(x => new QuestionRevision
            {
                Id = x.Id,
                UniqueId = x.UniqueId,
                QuestionId = x.QuestionId,
                QuestionTypeId = x.QuestionTypeId,
                Text = x.Text,
                MaxLength = x.MaxLength,
                IsActive = x.IsActive
            }).ToList();
        }

        private static List<QuestionOption> CreateCopy(List<QuestionOption> options)
        {
            return options.Select(x => new QuestionOption
            {
                Id = x.Id,
                UniqueId = x.UniqueId,
                QuestionRevisionId = x.QuestionRevisionId,
                Text = x.Text,
                Position = x.Position
            }).ToList();
        }

        private static List<QuestionReference> CreateCopy(List<QuestionReference> references)
        {
            return references.Select(x => new QuestionReference
            {
                Id = x.Id,
                UniqueId = x.UniqueId,
                ReferenceQuestionId = x.ReferenceQuestionId,
                ReferenceOptionId = x.ReferenceOptionId,
                QuestionId = x.QuestionId,
                QuestionOptionId = x.QuestionOptionId
            }).ToList();
        }
    }
}
