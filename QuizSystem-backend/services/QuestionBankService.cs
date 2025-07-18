﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using QuizSystem_backend.DTOs;
using QuizSystem_backend.Enums;
using QuizSystem_backend.Models;
using QuizSystem_backend.repositories;
using System.ComponentModel;
using System.Net.WebSockets;

namespace QuizSystem_backend.services
{
    public class QuestionBankService : IQuestionBankService
    {
        private readonly IQuestionBankRepository _questionBankRepository;
        private readonly IMapper _mapper;
        private readonly IChapterRepository _chapterRepository;
        private readonly QuizSystemDbContext _context;

        public QuestionBankService(IQuestionBankRepository questionBankRepository,IMapper mapper,IChapterRepository chapterRepository,QuizSystemDbContext context)
        {
            _questionBankRepository = questionBankRepository;
            _mapper = mapper;
            _context=context;
            _chapterRepository = chapterRepository;
        }
        public async Task<IEnumerable<QuestionBankDto>> GetQuestionBanksAsync(Guid userId)
        {
            var questionBanks = await _questionBankRepository.GetQuestionBanksAsync(userId);

            return questionBanks.Select(qb => new QuestionBankDto(qb));
        }
        public async Task<QuestionBankDto> GetQuestionBankByIdAsync(Guid id)
        {
            var questionBank = await _questionBankRepository.GetQuestionBankByIdAsync(id);

            return new QuestionBankDto(questionBank);
        }

        public async Task<QuestionBankDto> AddQuestionBankAsync(QuestionBankDto dto)
        {
            var questionBank = new QuestionBank(dto);

            //if (!string.IsNullOrEmpty(dto.Subject))
            //{
            //    var existingSubject = await _context.Subjects
            //        .FirstOrDefaultAsync(s => s.Name == dto.Subject);

            //    if (existingSubject != null)
            //    {
            //        qb.SubjectId = existingSubject.Id;
            //    }
            //    else
            //    {
            //        var newSubject = new Subject
            //        {
            //            Id = Guid.NewGuid(),
            //            Name = dto.Subject
            //        };
            //        _context.Subjects.Add(newSubject);
            //        await _context.SaveChangesAsync();

            //        qb.SubjectId = newSubject.Id;
            //    }
            //}
            //else if (dto.SubjectId.HasValue)
            //{
            //    qb.SubjectId = dto.SubjectId;
            //}

            await _questionBankRepository.AddAsync(questionBank);

            var newQuestionBank = await _questionBankRepository.GetQuestionBankByIdAsync(questionBank.Id);

            return new QuestionBankDto(newQuestionBank);
        }


        public async Task<QuestionBankDto> UpdateQuestionBankAsync(Guid id, QuestionBankDto dto)
        {
            var questionBank = await _questionBankRepository.GetQuestionBankByIdAsync(id);

            questionBank.Description = dto.Description!;
            questionBank.Name = dto.Name;
            questionBank.Status = dto.Status;

            await _questionBankRepository.SaveChangesAsync();

            return new QuestionBankDto(questionBank);
        }
        public async Task<IEnumerable<QuestionDto>> GetQuestionsByQuestionBankAsync(Guid id)
        {
            var questions = await _questionBankRepository.GetQuestionsByQuestionBankAsync(id);

            return questions.Select(q => new QuestionDto(q));
        }
        public async Task<bool> DeleteQuestionBankAsync(Guid id)
        {
            var questionBank = await _questionBankRepository.GetQuestionBankByIdAsync(id);
             
            if (questionBank.Status == Status.DELETED || questionBank == null)
            {
                return false;
            }

            questionBank.Status = Status.DELETED;

            await _questionBankRepository.SaveChangesAsync();

            return true;
        }
        

        public async Task<List<QuestionImportPreviewDto>> ImportQuestionsPreview(IFormFile file)
        {
            var listQuestion = new List<QuestionImportPreviewDto>();
            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);

                // Đặt ở đầu code dùng EPPlus
                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets[0];

                    var arrayCode = new string[] { "A", "B", "C", "D", "E", "F" };


                    int rowMax = 1; int colMax = 1;
                    while (true)
                    {
                        while (colMax <= 12 && worksheet.Cells[rowMax, colMax++].Text.Trim() == string.Empty) ;
                        if (colMax == 13) break;
                        rowMax++; colMax = 1;
                    }

                    for (int row = 2; row < rowMax; row++)
                    {
                        var preview = new QuestionImportPreviewDto();

                        preview.RowIndex = row;

                        var typeQuestion = worksheet.Cells[row, 1].Text.Trim();
                        if (Enum.TryParse<TypeOfQuestion>(typeQuestion, true, out var type))
                            preview.Type = type;
                        else preview.Type = null;

                        preview.Subject = worksheet.Cells[row, 2].Text.Trim();
                        preview.Chapter = worksheet.Cells[row, 3].Text.Trim();
                        preview.Content = worksheet.Cells[row, 4].Text.Trim();

                        var difficulty = worksheet.Cells[row, 5].Text.Trim();

                        if (Enum.TryParse<Difficulty>(difficulty, true, out var value))
                            preview.Difficulty = value;
                        else preview.Difficulty = null;

                        var correctString = worksheet.Cells[row, 6].Text.Trim();
                        preview.CorrectAnswer = string.IsNullOrEmpty(correctString) ? Array.Empty<string>()
                            : correctString.Split(',').Select(x => x.Trim()).Where(x => !string.IsNullOrEmpty(x)).ToArray();

                        if (int.TryParse(worksheet.Cells[row, 7].Text.Trim(), out var selectionCount))
                            preview.SelectionCount = selectionCount;
                        else preview.SelectionCount = null;

                        if (int.TryParse(worksheet.Cells[row, 8].Text.Trim(), out var correctSelectionCount))
                            preview.CorrectSelectionCount = correctSelectionCount;
                        else preview.CorrectSelectionCount = null;

                        preview.Answer = new List<AnswerImportPreview>();
                        char code = 'A';

                        for (int i = 9; i <= 14; i++)
                        {
                            var answerImport = new AnswerImportPreview();

                            var cellValue = worksheet.Cells[row, i].Text.Trim();


                            if (cellValue != "")
                            {
                                answerImport.Content = cellValue;
                                answerImport.Code = code++.ToString();
                                preview.Answer.Add(answerImport);

                            }
                            else break;
                        }
                        //Validate question

                        var normalized = preview.Content.Trim().ToLower();
                        bool exists = _context.Questions
                            .Any(q => q.Content.ToLower() == normalized);

                        if (exists)
                            preview.ErrorMessages.Add("Nội dung câu hỏi đã tồn tại");

                        if (string.IsNullOrEmpty(preview.Content))
                            preview.ErrorMessages.Add("Nội dung câu hỏi không được để trống");

                        if (preview.Type == null)
                            preview.ErrorMessages.Add("Loại câu hỏi không hợp lệ");
                        else if (preview.Type == TypeOfQuestion.SingleChoice)
                        {
                            if (preview.CorrectAnswer.Length > 1)
                                preview.ErrorMessages.Add("Câu hỏi trắc nghiệm một lựa chọn chỉ có một đáp án đúng");
                        }
                        else if (preview.Type == TypeOfQuestion.MultipleChoice)
                        {
                            if (preview.CorrectAnswer.Length < 2)
                                preview.ErrorMessages.Add("Câu hỏi trắc nghiệm nhiều lựa chọn phải có ít nhất 2 đáp án đúng");
                            if (preview.Answer.Count < preview.CorrectAnswer.Length)
                                preview.ErrorMessages.Add("Số lượng đáp án đúng không được lớn hơn số lượng đáp án có trong câu hỏi");
                        }
                        else
                        {
                            if (preview.CorrectAnswer.Length != 1 || (preview.CorrectAnswer[0] != "A" && preview.CorrectAnswer[0] != "B"))
                                preview.ErrorMessages.Add("Câu hỏi đúng sai chỉ có một đáp án đúng là A hoặc B");
                        }


                        if (preview.CorrectSelectionCount == null)
                            preview.ErrorMessages.Add("Số lần chọn đúng là số nguyên");

                        if (preview.SelectionCount == null)
                            preview.ErrorMessages.Add("Số lần được lấy là số nguyên");

                        if (preview.Difficulty == null)
                            preview.ErrorMessages.Add("Độ khó không hợp lệ");

                        if (!preview.CorrectAnswer.Any())
                            preview.ErrorMessages.Add("Đáp án đúng không được để trống");
                        else if (preview.CorrectAnswer.Length > arrayCode.Length)
                            preview.ErrorMessages.Add("Số lượng đáp án đúng không hợp lệ");

                        else if (preview.CorrectAnswer.Any(code => !arrayCode.Contains(code)))
                            preview.ErrorMessages.Add("Mã đáp án đúng không hợp lệ");

                        if (preview.CorrectAnswer.Length == 0)
                            preview.ErrorMessages.Add("Câu hỏi phải có ít nhất một đáp án đúng");

                        foreach (var answer in preview.Answer)
                        {
                            answer.IsCorrect = preview.CorrectAnswer.Contains(answer.Code);
                            if (string.IsNullOrEmpty(answer.Content))
                                preview.ErrorMessages.Add("Nội dung đáp án không được để trống");
                            if (answer.Content.Length > 500)
                                preview.ErrorMessages.Add("Nội dung đáp án không được quá 500 ký tự");
                        }

                        var error = await _questionBankRepository.CheckErrorSubChap(preview.Subject, preview.Chapter);

                        if (error != null)
                        {
                            preview.ErrorMessages.Add(error);
                        }

                        preview.IsValid = !preview.ErrorMessages.Any();

                        listQuestion.Add(preview);
                    }

                }
            }
            return listQuestion;

        }


        public async Task<List<Question>> ImPortQuestionConfirm(Guid id, List<QuestionImportPreviewDto> listPreview)
        {
            var listQuestion = new List<Question>();

            foreach (var questionPreview in listPreview)
            {
                if (!questionPreview.IsValid)
                    continue;
                var question = new Question
                {
                    Id = Guid.NewGuid(),
                    Type = questionPreview.Type,
                    Content = questionPreview.Content,
                    Difficulty = questionPreview.Difficulty,
                    Status = Status.ACTIVE,
                    QuestionBankId = id
                };
                question.Chapter = await _chapterRepository.GetChapterByNameAsync(questionPreview.Chapter!);
               
                question.Answers = questionPreview.Answer.Select(a => new Answer
                {
                    Id = Guid.NewGuid(),
                    Content = a.Content,
                    IsCorrect = a.IsCorrect,
                    QuestionId = question.Id 
                }).ToList();

                listQuestion.Add(question);
            }
            await _questionBankRepository.AddListQuestionAsync(listQuestion);

            return listQuestion;
        }

        
    }
}
