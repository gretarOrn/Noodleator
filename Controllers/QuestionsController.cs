using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Noodleator.Models;
using Noodleator.Services;


namespace Noodleator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionsController : ControllerBase
    {
        private readonly QuestionService _questSvc;
        
        public QuestionsController(QuestionService questionService)
        {
            _questSvc = questionService;
        }

        [HttpGet("random")]
        public ActionResult<Question> Random() => _questSvc.Random();

        [HttpGet]
        public ActionResult<List<Question>> Get() => _questSvc.Get();

        [HttpGet("{id:length(24)}", Name = "GetQuestion")]
        public ActionResult<Question> Get(string id)
        {
            var question = _questSvc.Get(id);

            if (question == null)
            {
                return NotFound();
            }
            return question;
        }

        [HttpPost]
        public ActionResult<Question> Create(Question question)
        {
            question.Created = question.LastUpdated = DateTime.Now;
            if (ModelState.IsValid)
            {
                _questSvc.Create(question);
            }

            return CreatedAtRoute("GetQuestion", new { id = question.Id.ToString() }, question);
        }

        [HttpPut("{id:length(24)}")]
        public ActionResult Update(Question questionIn)
        {
            var question = _questSvc.Get(questionIn.Id);

            if (question == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                questionIn.LastUpdated = DateTime.Now;
                questionIn.Created = question.Created.ToLocalTime(); /// gert til að koma í veg fyrir að mongodb fucki upp dagsetningunni
                _questSvc.Update(questionIn);
            }

            return CreatedAtRoute("GetQuestion", new { id = questionIn.Id.ToString() }, questionIn);

        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var question = _questSvc.Get(id);

            if (question == null)
            {
                return NotFound();
            }

            _questSvc.Delete(id);

            return NoContent();
        }
    }

    
}