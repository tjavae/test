using CUDC.Api.Data;
using CUDC.Common.Dtos;
using CUDC.Common.Extensions;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CUDC.Api.Services
{
    public class SurveyService : ISurveyService
    {
        private readonly SurveyContext _context;
        private const int MAX_ANSWER_TEXT_LENGTH = 2000;
        private static readonly object lockObj = new();
        private readonly Logger fileLogger = LogManager.GetLogger("fileLogger");
        private readonly ILogService _databaseLogService;

        public SurveyService(SurveyContext context, ILogService logService)
        {
            _context = context;
            _databaseLogService = logService;
        }

        public IEnumerable<SurveyDto> GetSurveys()
        {
            try
            {
                var query = from s in _context.Surveys
                            join st in _context.SurveyTypes on s.SurveyTypeId equals st.Id
                            orderby s.Title
                            select new SurveyDto
                            {
                                Id = s.UniqueId,
                                Title = s.Title,
                                Description = s.Description,
                                TypeString = st.Name,
                                Type = (Common.Enums.SurveyType)(s.SurveyTypeId - 1),
                                StartDate = s.StartDate,
                                EndDate = s.EndDate,
                                IsActive = s.IsActive,
                                CreatedOn = s.CreatedOn,
                                CreatedBy = s.CreatedBy,
                                ModifiedOn = s.ModifiedOn,
                                ModifiedBy = s.ModifiedBy,
                                InformationText = s.InformationText
                            };
                List<SurveyDto> sortedList = query.OrderByDescending(o => o.IsActive).OrderByDescending(s => s.StartDate).ToList();
                return sortedList;
            }
            catch (Exception e)
            {
                _databaseLogService.NLog("Error", "GetSurveys error", e, MethodBase.GetCurrentMethod().Name);
                fileLogger.Error(e);
                throw;
            }

        }

        public IEnumerable<SectionDto> GetSections(Guid surveyId)
        {
            try
            {
                var query = from sur in _context.Surveys
                            join sec in _context.Sections on sur.Id equals sec.SurveyId
                            where sur.UniqueId == surveyId && sec.Description != "SystemForcedDeletedzzxxyy"
                            orderby sec.Position
                            select new SectionDto
                            {
                                Id = sec.UniqueId,
                                Title = sec.Title,
                                Description = sec.Description,
                                Position = sec.Position,
                                CreatedOn = sec.CreatedOn,
                                CreatedBy = sec.CreatedBy,
                                ModifiedOn = sec.ModifiedOn,
                                ModifiedBy = sec.ModifiedBy
                            };

                return query.ToList();
            }
            catch (Exception e)
            {
                string message = "GetSections(surveyId:" + surveyId + ") error";
                _databaseLogService.NLog("Error", message, e, MethodBase.GetCurrentMethod().Name);
                fileLogger.Error(e);
                throw;
            }

        }

        public IEnumerable<QuestionDto> GetQuestions(Guid surveyId)
        {
            try
            {
                var query = from sur in _context.Surveys
                            join q in _context.Questions on sur.Id equals q.SurveyId
                            join qr in _context.QuestionRevisions on q.Id equals qr.QuestionId
                            join qt in _context.QuestionTypes on qr.QuestionTypeId equals qt.Id
                            join _sec in _context.Sections on q.Section equals _sec into _sec2
                            from sec in _sec2.DefaultIfEmpty()
                            where sur.UniqueId == surveyId && q.IsActive == true && qr.IsActive == true
                            orderby sec.Position, q.Position
                            select new QuestionDto
                            {
                                Id = q.UniqueId,
                                SectionId = sec.UniqueId,
                                Number = q.Number,
                                Position = q.Position,
                                IsRequired = q.IsRequired,
                                Revision = new QuestionRevisionDto
                                {
                                    Id = qr.UniqueId,
                                    Type = (Common.Enums.QuestionType)(qr.QuestionTypeId - 1),
                                    TypeString = qt.Text,
                                    Text = qr.Text,
                                    MaxLength = qr.MaxLength,
                                    CreatedOn = qr.CreatedOn,
                                    CreatedBy = qr.CreatedBy,
                                    ModifiedOn = qr.ModifiedOn,
                                    ModifiedBy = qr.ModifiedBy
                                },
                                CreatedOn = q.CreatedOn,
                                CreatedBy = q.CreatedBy,
                                ModifiedOn = q.ModifiedOn,
                                ModifiedBy = q.ModifiedBy
                            };

                return query.ToList();
            }
            catch (Exception e)
            {
                _databaseLogService.NLog("Error", "GetQuestions error", e, MethodBase.GetCurrentMethod().Name);
                fileLogger.Error(e);
                throw;
            }
        }

        public IEnumerable<QuestionOptionDto> GetQuestionOptions(Guid surveyId)
        {
            try
            {
                var query = from s in _context.Surveys
                            join q in _context.Questions on s.Id equals q.SurveyId
                            join qr in _context.QuestionRevisions on q.Id equals qr.QuestionId
                            join qo in _context.QuestionOptions on qr.Id equals qo.QuestionRevisionId
                            where s.UniqueId == surveyId && q.IsActive == true && qr.IsActive == true
                            orderby q.Position, qo.Position
                            select new QuestionOptionDto
                            {
                                Id = qo.UniqueId,
                                QuestionId = q.UniqueId,
                                Text = qo.Text,
                                Position = qo.Position,
                                CreatedOn = qo.CreatedOn,
                                CreatedBy = qo.CreatedBy,
                                ModifiedOn = qo.ModifiedOn,
                                ModifiedBy = qo.ModifiedBy
                            };

                return query.ToList();
            }
            catch (Exception e)
            {
                _databaseLogService.NLog("Error", "GetQuestionOptions error", e, MethodBase.GetCurrentMethod().Name);
                fileLogger.Error(e);
                throw;
            }
        }

        public ResponseDto GetAnswers(AnswersRequest request)
        {
            try
            {
                var survey = GetSurveys().SingleOrDefault(x => x.Id == request.SurveyId);

                if (survey == null)
                {
                    return null;
                }

                ResponseDto response;

                if (survey.Type == Common.Enums.SurveyType.CAT || survey.Type == Common.Enums.SurveyType.SE || survey.Type == Common.Enums.SurveyType.DOS)
                {
                    response = GetCatSurveyResponse(request);
                }
                else
                {
                    response = GetSurveyResponse(request);
                }

                if (response == null)
                {
                    return null;
                }

                var answers = from r in _context.Responses
                              join a in _context.Answers on r.Id equals a.ResponseId
                              join qr in _context.QuestionRevisions on a.QuestionRevisionId equals qr.Id
                              join q in _context.Questions on qr.QuestionId equals q.Id
                              join _qo in _context.QuestionOptions on a.QuestionOption equals _qo into _qo2
                              from qo in _qo2.DefaultIfEmpty()
                              where r.UniqueId == response.Id
                              select new AnswerDto
                              {
                                  QuestionId = q.UniqueId,
                                  QuestionOptionId = qo.UniqueId,
                                  Text = a.LongText ?? a.Text,
                                  CreatedOn = a.CreatedOn,
                                  CreatedBy = a.CreatedBy,
                                  ModifiedOn = a.ModifiedOn,
                                  ModifiedBy = a.ModifiedBy
                              };

                response.Answers.AddRange(answers.ToList());

                return response;
            }
            catch (Exception e)
            {
                _databaseLogService.NLog("Error", "GetAnswers error", e, MethodBase.GetCurrentMethod().Name);
                fileLogger.Error(e);
                throw;
            }
        }

        private ResponseDto GetSurveyResponse(AnswersRequest request)
        {
            try
            {
                //Select user response
                return (from s in _context.Surveys
                        join r in _context.Responses on s.Id equals r.SurveyId
                        where s.UniqueId == request.SurveyId &&
                            r.UserId == request.UserId &&
                            r.CuNumber == request.CuNumber &&
                            r.JoinNumber == request.JoinNumber &&
                            (r.IsRejected == null || r.IsRejected == false)
                        select new ResponseDto
                        {
                            Id = r.UniqueId,
                            SurveyId = s.UniqueId,
                            UserId = r.UserId,
                            CuNumber = r.CuNumber,
                            JoinNumber = r.JoinNumber,
                            CreatedOn = r.CreatedOn,
                            CreatedBy = r.CreatedBy,
                            ModifiedOn = r.ModifiedOn,
                            ModifiedBy = r.ModifiedBy,
                            SubmittedOn = r.SubmittedOn
                        }).SingleOrDefault();
            }
            catch (Exception e)
            {
                _databaseLogService.NLog("Error", "GetSurveyResponse error", e, MethodBase.GetCurrentMethod().Name);
                fileLogger.Error(e);
                throw;
            }
        }

        private ResponseDto GetCatSurveyResponse(AnswersRequest request)
        {
            try
            {
                //Select the latest response
                var result = (from s in _context.Surveys
                              join r in _context.Responses on s.Id equals r.SurveyId
                              where s.UniqueId == request.SurveyId &&
                                  r.CuNumber == request.CuNumber &&
                                  r.JoinNumber == request.JoinNumber &&
                                  (r.IsRejected == null || r.IsRejected == false)
                              orderby r.CreatedOn descending
                              select new ResponseDto
                              {
                                  Id = r.UniqueId,
                                  SurveyId = s.UniqueId,
                                  UserId = r.UserId,
                                  CuNumber = r.CuNumber,
                                  JoinNumber = r.JoinNumber,
                                  CreatedOn = r.CreatedOn,
                                  CreatedBy = r.CreatedBy,
                                  ModifiedOn = r.ModifiedOn,
                                  ModifiedBy = r.ModifiedBy,
                                  SubmittedOn = r.SubmittedOn
                              }).FirstOrDefault();

                return result;

            }
            catch (Exception e)
            {
                _databaseLogService.NLog("Error", "GetCatSurveyResponse error", e, MethodBase.GetCurrentMethod().Name);
                fileLogger.Error(e);
                throw;
            }
        }

        public void SetAnswers(ResponseDto response)
        {
            try
            {
                var survey = _context.Surveys.Single(x => x.UniqueId == response.SurveyId);
                Response responseModel = null;

                lock (lockObj)
                {
                    responseModel = _context.Responses.Where(r => r.SurveyId == survey.Id &&
                                            r.UserId == response.UserId &&
                                            r.CuNumber == response.CuNumber &&
                                            r.JoinNumber == response.JoinNumber &&
                                            (r.IsRejected == null || r.IsRejected == false)).SingleOrDefault();

                    if (responseModel == null)
                    {
                        responseModel = new Response
                        {
                            UniqueId = Guid.NewGuid(),
                            SurveyId = survey.Id,
                            UserId = response.UserId,
                            CuNumber = response.CuNumber,
                            JoinNumber = response.JoinNumber,
                            CreatedOn = DateTime.Now,
                            CreatedBy = response.UserId
                        };
                        _context.Responses.Add(responseModel);
                    }
                    else
                    {
                        responseModel.ModifiedOn = DateTime.Now;
                        responseModel.ModifiedBy = response.UserId;
                        _context.Responses.Update(responseModel);
                    }
                    _context.SaveChanges();
                }

                var answersModel = _context.Answers.Where(x => x.ResponseId == responseModel.Id).ToList();

                var lookup = (from q in _context.Questions
                              join qr in _context.QuestionRevisions on q.Id equals qr.QuestionId
                              join _qo in _context.QuestionOptions on qr equals _qo.QuestionRevision into _qo2
                              from qo in _qo2.DefaultIfEmpty()
                              where q.SurveyId == survey.Id && q.IsActive == true && qr.IsActive == true
                              select new
                              {
                                  QuestionGuid = q.UniqueId,
                                  QuestionRevisionId = qr.Id,
                                  QuestionOptionId = qo == null ? 0 : qo.Id,
                                  QuestionOptionGuid = qo == null ? Guid.Empty : qo.UniqueId
                              }).ToList();

                foreach (var answer in response.Answers)
                {
                    var answerModel = (from a in answersModel
                                       join l in lookup on a.QuestionRevisionId equals l.QuestionRevisionId
                                       where l.QuestionGuid == answer.QuestionId
                                       select a).FirstOrDefault();
                    var questionOption = lookup.FirstOrDefault(x => x.QuestionOptionGuid == answer.QuestionOptionId);
                    var questionRevision = lookup.FirstOrDefault(x => x.QuestionGuid == answer.QuestionId);
                    int? questionOptionId = null;
                    int questionRevisionId = 0;
                    if (questionOption != null)
                    {
                        questionOptionId = questionOption.QuestionOptionId;
                    }
                    if (questionRevision != null) { questionRevisionId = questionRevision.QuestionRevisionId; }


                    if (answerModel == null)
                    {
                        answerModel = new Answer
                        {
                            UniqueId = Guid.NewGuid(),
                            ResponseId = responseModel.Id,
                            QuestionRevisionId = questionRevisionId,
                            QuestionOptionId = answer.QuestionOptionId.HasValue ? questionOptionId : null,
                            Text = answer.Text.Left(MAX_ANSWER_TEXT_LENGTH),
                            LongText = answer.Text?.Length > MAX_ANSWER_TEXT_LENGTH ? answer.Text : null,
                            CreatedOn = DateTime.Now,
                            CreatedBy = response.UserId
                        };
                        _context.Answers.Add(answerModel);
                    }
                    else
                    {
                        answerModel.QuestionOptionId = (answer.QuestionOptionId.HasValue ? questionOptionId : null);
                        answerModel.Text = answer.Text.Left(MAX_ANSWER_TEXT_LENGTH);
                        answerModel.LongText = (answer.Text?.Length > MAX_ANSWER_TEXT_LENGTH ? answer.Text : null);
                        answerModel.ModifiedOn = DateTime.Now;
                        answerModel.ModifiedBy = response.UserId;
                        _context.Answers.Update(answerModel);
                    }
                    _context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                _databaseLogService.NLog("Error", "SetAnswers error", e, MethodBase.GetCurrentMethod().Name);
                fileLogger.Error(e);
                throw;
            }
        }

        public void SubmitAnswers(SubmitRequest request)
        {
            try
            {
                var response = _context.Responses.SingleOrDefault(x => x.UniqueId == request.ResponseId && x.UserId == request.UserId);
                if (response != null)
                {
                    response.SubmittedOn = DateTime.Now;
                    _context.Responses.Update(response);
                    _context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                _databaseLogService.NLog("Error", "SubmitAnswers error", e, MethodBase.GetCurrentMethod().Name);
                fileLogger.Error(e);
                throw;
            }
        }

        public void UnlockSurvey(SubmitRequest request)
        {
            try
            {
                var response = _context.Responses.SingleOrDefault(x => x.UniqueId == request.ResponseId);
                if (response != null)
                {
                    response.SubmittedOn = null;
                    _context.Responses.Update(response);
                    _context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                _databaseLogService.NLog("Error", "UnlockSurvey error", e, MethodBase.GetCurrentMethod().Name);
                fileLogger.Error(e);
                throw;
            }
        }

        public void TransferOwnership(SubmitRequest request)
        {
            try
            {
                var response = _context.Responses.SingleOrDefault(x => x.UniqueId == request.ResponseId);
                if (response != null)
                {
                    response.UserId = request.UserId;
                    _context.Responses.Update(response);
                    _context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                _databaseLogService.NLog("Error", "TransferOwnership error", e, MethodBase.GetCurrentMethod().Name);
                fileLogger.Error(e);
                throw;
            }
        }

        public IEnumerable<QuestionReferenceDto> GetQuestionReferences(Guid surveyGuId)
        {
            try
            {
                var surveyId = _context.Surveys.Single(x => x.UniqueId == surveyGuId).Id;
                var query = from q in _context.Questions
                            join r in _context.QuestionReferences on q.UniqueId equals r.ReferenceQuestionId
                            where q.SurveyId == surveyId && q.IsActive == true
                            select new QuestionReferenceDto
                            {
                                Id = r.UniqueId,
                                ReferenceQuestionId = r.ReferenceQuestionId,
                                ReferenceOptionId = r.ReferenceOptionId,
                                QuestionId = r.QuestionId,
                                QuestionOptionId = r.QuestionOptionId,
                                CreatedOn = r.CreatedOn,
                                CreatedBy = r.CreatedBy,
                                ModifiedOn = r.ModifiedOn,
                                ModifiedBy = r.ModifiedBy
                            };
                return query.ToList();
            }
            catch (Exception e)
            {
                _databaseLogService.NLog("Error", "GetQuestionReferences error", e, MethodBase.GetCurrentMethod().Name);
                fileLogger.Error(e);
                throw;
            }
        }
    }
}
