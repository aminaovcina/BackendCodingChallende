﻿using System.Collections.Generic;
using System.Data;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using QuizService.Domain.Entities;
using QuizService.Domain.Models;
using ApplicationCore.IServices;
using System;
using Microsoft.Extensions.Logging;

namespace QuizService.Controllers;

[Route("api/quizzes")]
public class QuizController : Controller
{
    //TO DO: Refactor Every Method On Clean Arhitecture
    //TO DO: Implement Try-Catch Blocks with Global Exception Handling
    //TO DO: Implement Global Logger Handler
    private readonly IDbConnection _connection;
    public readonly IQuizService _quizService;
    private readonly ILogger _logger;

    public QuizController(IDbConnection connection,
        IQuizService quizService,
        ILogger<QuizController> logger)
    {
        _connection = connection;
        _quizService = quizService;
        _logger = logger;
    }

    // GET api/quizzes
    [HttpGet]
    public IEnumerable<QuizResponseModel> Get()
    {
        const string sql = "SELECT * FROM Quiz;";
        var quizzes = _connection.Query<Quiz>(sql);
        return quizzes.Select(quiz =>
            new QuizResponseModel
            {
                Id = quiz.Id,
                Title = quiz.Title
            });
    }

    // GET api/quizzes/5
    [HttpGet("{id}")]
    public object Get(int id)
    {
        try
        {
            var result = _quizService.Get(id);
            if (result == null)
                return NotFound();
            return Ok(result);
        }
        catch(Exception ex)
        {
            _logger.LogError("QuizController - Get ERROR: " + ex.Message);
            return BadRequest(ex.Message);
        }
    }

    // POST api/quizzes
    [HttpPost]
    public IActionResult Post([FromBody]QuizCreateModel value)
    {
        var sql = $"INSERT INTO Quiz (Title) VALUES('{value.Title}'); SELECT LAST_INSERT_ROWID();";
        var id = _connection.ExecuteScalar(sql);
        return Created($"/api/quizzes/{id}", null);
    }

    // PUT api/quizzes/5
    [HttpPut("{id}")]
    public IActionResult Put(int id, [FromBody]QuizUpdateModel value)
    {
        const string sql = "UPDATE Quiz SET Title = @Title WHERE Id = @Id";
        int rowsUpdated = _connection.Execute(sql, new {Id = id, Title = value.Title});
        if (rowsUpdated == 0)
            return NotFound();
        //TO DO: Return Updated Data.
        return NoContent();
    }

    // DELETE api/quizzes/5
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        const string sql = "DELETE FROM Quiz WHERE Id = @Id";
        int rowsDeleted = _connection.Execute(sql, new {Id = id});
        if (rowsDeleted == 0)
            return NotFound();
        return NoContent();
    }

    // POST api/quizzes/5/questions
    [HttpPost]
    [Route("{id}/questions")]
    public IActionResult PostQuestion(int id, [FromBody] QuestionCreateModel value)
    {
        const string quizSql = "SELECT * FROM Quiz WHERE Id = @Id;";
        var quiz = _connection.QueryFirstOrDefault<Quiz>(quizSql, new { Id = id });
        if (quiz == null)
            return NotFound();

        const string sql = "INSERT INTO Question (Text, QuizId) VALUES(@Text, @QuizId); SELECT LAST_INSERT_ROWID();";
        var questionId = _connection.ExecuteScalar(sql, new { Text = value.Text, QuizId = id });
        return Created($"/api/quizzes/{id}/questions/{questionId}", null);
    }

    // PUT api/quizzes/5/questions/6
    [HttpPut("{id}/questions/{qid}")]
    public IActionResult PutQuestion(int id, int qid, [FromBody]QuestionUpdateModel value)
    {
        const string sql = "UPDATE Question SET Text = @Text, CorrectAnswerId = @CorrectAnswerId WHERE Id = @QuestionId";
        int rowsUpdated = _connection.Execute(sql, new {QuestionId = qid, Text = value.Text, CorrectAnswerId = value.CorrectAnswerId});
        if (rowsUpdated == 0)
            return NotFound();
        //TO DO: Return Updated Data.
        return NoContent();
    }

    // DELETE api/quizzes/5/questions/6
    [HttpDelete]
    [Route("{id}/questions/{qid}")]
    public IActionResult DeleteQuestion(int id, int qid)
    {
        const string sql = "DELETE FROM Question WHERE Id = @QuestionId";
        _connection.ExecuteScalar(sql, new {QuestionId = qid});
        return NoContent();
    }

    // POST api/quizzes/5/questions/6/answers
    [HttpPost]
    [Route("{id}/questions/{qid}/answers")]
    public IActionResult PostAnswer(int id, int qid, [FromBody]AnswerCreateModel value)
    {
        const string sql = "INSERT INTO Answer (Text, QuestionId) VALUES(@Text, @QuestionId); SELECT LAST_INSERT_ROWID();";
        var answerId = _connection.ExecuteScalar(sql, new {Text = value.Text, QuestionId = qid});
        return Created($"/api/quizzes/{id}/questions/{qid}/answers/{answerId}", null);
    }

    // PUT api/quizzes/5/questions/6/answers/7
    [HttpPut("{id}/questions/{qid}/answers/{aid}")]
    public IActionResult PutAnswer(int id, int qid, int aid, [FromBody]AnswerUpdateModel value)
    {
        const string sql = "UPDATE Answer SET Text = @Text WHERE Id = @AnswerId";
        int rowsUpdated = _connection.Execute(sql, new {AnswerId = qid, Text = value.Text});
        if (rowsUpdated == 0)
            return NotFound();
        //TO DO: Return Changed Data
        return NoContent();
    }

    // DELETE api/quizzes/5/questions/6/answers/7
    [HttpDelete]
    [Route("{id}/questions/{qid}/answers/{aid}")]
    public IActionResult DeleteAnswer(int id, int qid, int aid)
    {
        const string sql = "DELETE FROM Answer WHERE Id = @AnswerId";
        _connection.ExecuteScalar(sql, new {AnswerId = aid});
        return NoContent();
    }

    [HttpGet]
    [Route("{id}/score")]
    public IActionResult GetQuizScore(int id, [FromBody] QuizAnswersModel value)
    {
        try
        {
            var result = _quizService.GetScore(id, value);
            if (result == -1)
                return NotFound();
            return Ok(result);
        }
        catch(Exception ex)
        {
            _logger.LogError("QuizController - GetQuizScore ERROR: " + ex.Message);
            return BadRequest(ex.Message);
        }
       
    }
}