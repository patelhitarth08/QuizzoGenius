using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuizApplicationMVC.Data;
using QuizApplicationMVC.Models;

namespace QuizApplicationMVC.Controllers
{
    public class QuestionsController : Controller
    {
        private readonly ApplicationDBContext _context;

        public QuestionsController(ApplicationDBContext context)
        {
            _context = context;
        }

        // GET: Questions
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetInt32("Id") == null)
            {
                // Session is not set, so redirect to the login page or take appropriate action
                return RedirectToAction("Login", "Users");
            }
            if (HttpContext.Session.GetInt32("QuizId") == null)
            {
                return RedirectToAction("Index", "Home");
            }
            int? currentQuizId = (int)HttpContext.Session.GetInt32("QuizId");

            if (_context.Questions != null)
            {
                var questions = await _context.Questions
                    .Where(q => q.QuizId == currentQuizId) // Filter by QuizId
                    .ToListAsync();

                return View(questions);
            }
            else
            {
                return Problem("Entity set 'ApplicationDBContext.Questions' is null.");
            }
        }

        // GET: Questions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Questions == null)
            {
                return NotFound();
            }

            var questions = await _context.Questions
                .FirstOrDefaultAsync(m => m.Id == id);
            if (questions == null)
            {
                return NotFound();
            }

            return View(questions);
        }

        // GET: Questions/Create
        public IActionResult Create()
        {
            if (HttpContext.Session.GetInt32("Id") == null)
            {
                // Session is not set, so redirect to the login page or take appropriate action
                return RedirectToAction("Login", "Users");
            }
            if (HttpContext.Session.GetInt32("QuizId") == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        // POST: Questions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("QuestionsId,QuestionsName,OptionA,OptionB,OptionC,OptionD,CorrectOption")] Questions questions)
        {
            if (HttpContext.Session.GetInt32("Id") == null)
            {
                // Session is not set, so redirect to the login page or take appropriate action
                return RedirectToAction("Login", "Users");
            }

            if (HttpContext.Session.GetInt32("QuizId") == null)
            {
                return RedirectToAction("Index", "Home");
            }

            string COption = questions.CorrectOption;
            switch (questions.CorrectOption)
            {
                case "A":
                    COption = questions.OptionA;
                    break;

                case "B":
                    COption = questions.OptionB;
                    break;

                case "C":
                    COption = questions.OptionC;
                    break;

                case "D":
                    COption = questions.OptionD;
                    break;
            }

            questions.CorrectOption = COption;
            questions.QuizId = (int)HttpContext.Session.GetInt32("QuizId");
            questions.Quiz = await _context.Quiz.FindAsync(questions.QuizId);
            Console.WriteLine("************");
            Console.WriteLine(questions.Quiz.Title);
            Console.WriteLine("************");

            if (ModelState.IsValid)
            {
                _context.Add(questions);
                await _context.SaveChangesAsync();
                int quizId = (int)HttpContext.Session.GetInt32("QuizId");
                var quiz = await _context.Quiz.Include(q => q.Questions).FirstOrDefaultAsync(q => q.Id == quizId);

                // Add the new question to the Quiz's Questions list
                if (quiz != null)
                {
                    quiz.Questions.Add(questions);
                    await _context.SaveChangesAsync(); // Update the Quiz to reflect the changes
                }
                foreach (var que in quiz.Questions)
                {
                    Console.WriteLine(que.QuestionsName);
                }
                return RedirectToAction("Index", "Questions");
            }
            else
            {
                Console.WriteLine("Validation Errors:");
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        Console.WriteLine(error.ErrorMessage);
                    }
                }
            }
            return View(questions);
        }

        // GET: Questions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Questions == null)
            {
                return NotFound();
            }

            var questions = await _context.Questions.FindAsync(id);
            if (questions == null)
            {
                return NotFound();
            }
            return View(questions);
        }

        // POST: Questions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,QuestionsName,OptionA,OptionB,OptionC,OptionD,CorrectOption")] Questions updatedQuestion)
        {
            if (id != updatedQuestion.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // Load the existing question from the database
                var existingQuestion = await _context.Questions.FindAsync(id);

                if (existingQuestion == null)
                {
                    return NotFound();
                }

                // Update the fields of the existing question with the values from the updatedQuestion
                existingQuestion.QuestionsName = updatedQuestion.QuestionsName;
                existingQuestion.OptionA = updatedQuestion.OptionA;
                existingQuestion.OptionB = updatedQuestion.OptionB;
                existingQuestion.OptionC = updatedQuestion.OptionC;
                existingQuestion.OptionD = updatedQuestion.OptionD;
                string COption = updatedQuestion.CorrectOption;
                switch (updatedQuestion.CorrectOption)
                {
                    case "A":
                        COption = updatedQuestion.OptionA;
                        break;

                    case "B":
                        COption = updatedQuestion.OptionB;
                        break;

                    case "C":
                        COption = updatedQuestion.OptionC;
                        break;

                    case "D":
                        COption = updatedQuestion.OptionD;
                        break;
                }

                existingQuestion.CorrectOption = COption;

                try
                {
                    _context.Update(existingQuestion);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuestionsExists(existingQuestion.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(updatedQuestion);
        }


        // GET: Questions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Questions == null)
            {
                return NotFound();
            }

            var questions = await _context.Questions
                .FirstOrDefaultAsync(m => m.Id == id);
            if (questions == null)
            {
                return NotFound();
            }

            return View(questions);
        }

        // POST: Questions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Questions == null)
            {
                return Problem("Entity set 'ApplicationDBContext.Questions' is null.");
            }
            var questions = await _context.Questions.FindAsync(id);
            if (questions != null)
            {
                _context.Questions.Remove(questions);
            }
            int quizId = (int)HttpContext.Session.GetInt32("QuizId");
            var quiz = await _context.Quiz.Include(q => q.Questions).FirstOrDefaultAsync(q => q.Id == quizId);

            // Remove the deleted question from the Quiz's Questions list
            if (quiz != null)
            {
                var questionToDelete = quiz.Questions.FirstOrDefault(q => q.Id == id);
                if (questionToDelete != null)
                {
                    quiz.Questions.Remove(questionToDelete);
                    await _context.SaveChangesAsync(); // Update the Quiz to reflect the changes
                }
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool QuestionsExists(int id)
        {
            return (_context.Questions?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
