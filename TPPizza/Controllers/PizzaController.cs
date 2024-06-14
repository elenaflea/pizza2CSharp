using BO;
using Microsoft.AspNetCore.Mvc;
using TPPizza.Models;

namespace TPPizza.Controllers
{
    public class PizzaController : Controller
    {
        private static readonly List<Pizza> pizzas = new()
        {
            new Pizza{Id = 1, Nom = "Calzone", Pate = Pizza.PatesDisponibles[1], Ingredients = new List<Ingredient>{ Pizza.IngredientsDisponibles[0], Pizza.IngredientsDisponibles[1]} },
            new Pizza{Id = 2, Nom = "Reine", Pate = Pizza.PatesDisponibles[0], Ingredients = new List<Ingredient>{ Pizza.IngredientsDisponibles[1], Pizza.IngredientsDisponibles[3], Pizza.IngredientsDisponibles[4]} },
        };

        // GET: PizzaController
        public ActionResult Index()
        {
            return View(pizzas);
        }

        // GET: PizzaController/Details/5
        public ActionResult Details(int id)
        {
            Pizza? pizza = pizzas.Find(p => p.Id == id);
            return pizza == null ? NotFound() : View(pizza);
        }

        // GET: PizzaController/Create
        public ActionResult Create()
        {
            return View(new PizzaVM());
        }

        // POST: PizzaController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PizzaVM pizzaVM)
        {
            try
            {
                if (!Valider(pizzaVM))
                {
                    throw new Exception("Erreur de validation");
                }

                pizzas.Add(new Pizza
                {
                    Id = pizzas.Any() ? pizzas.Max(p => p.Id) + 1 : 1,
                    Nom = pizzaVM.Nom,
                    Pate = Pizza.PatesDisponibles.First(p => p.Id == pizzaVM.IdPate),
                    Ingredients = Pizza.IngredientsDisponibles.Where(i => pizzaVM.IdsIngredients.Contains(i.Id)).ToList()
                });
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(pizzaVM);
            }
        }

        // GET: PizzaController/Edit/5
        public ActionResult Edit(int id)
        {
            Pizza? pizza = pizzas.Find(p => p.Id == id);
            return pizza == null
                ? NotFound()
                : View(new PizzaVM
                {
                    Id = pizza.Id,
                    Nom = pizza.Nom,
                    IdPate = pizza.Pate.Id,
                    IdsIngredients = pizza.Ingredients.Select(i => i.Id).ToList()
                });
        }

        // POST: PizzaController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PizzaVM pizzaVM)
        {
            try
            {
                if (!Valider(pizzaVM))
                {
                    return View(pizzaVM);
                }

                Pizza? pizza = pizzas.Find(p => p.Id == pizzaVM.Id);
                if (pizza == null)
                {
                    return NotFound();
                }
                pizza.Nom = pizzaVM.Nom;
                pizza.Pate = Pizza.PatesDisponibles.First(p => p.Id == pizzaVM.IdPate);
                pizza.Ingredients = Pizza.IngredientsDisponibles.Where(i => pizzaVM.IdsIngredients.Contains(i.Id)).ToList();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(pizzaVM);
            }
        }

        // GET: PizzaController/Delete/5
        public ActionResult Delete(int id)
        {
            Pizza? pizza = pizzas.Find(p => p.Id == id);
            return pizza == null ? NotFound() : View(pizza);
        }

        // POST: PizzaController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                Pizza? pizza = pizzas.Find(p => p.Id == id);
                if (pizza == null)
                {
                    return NotFound();
                }
                _ = pizzas.Remove(pizza);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [HttpGet]
        [HttpPost]
        public ActionResult VerifNomPizzaUnique(string nom, int id)
        {
            return NomPizzaExistant(nom, id) ? Json("Une autre pizza porte déjà ce nom") : (ActionResult)Json(true);
        }

        [HttpGet]
        [HttpPost]
        public ActionResult VerifIngredientsOriginaux(string idsIngredients, int id)
        {
            List<int> ids = new();
            foreach (string idI in idsIngredients.Split(','))
            {
                ids.Add(int.Parse(idI));
            }
            return MemesIngredients(ids, id) ? Json("Une autre pizza utilise exactement ces mêmes ingrédients") : (ActionResult)Json(true);
        }

        private bool Valider(PizzaVM pizzaVM)
        {
            if (!ModelState.IsValid)
            {
                return false;
            }

            if (NomPizzaExistant(pizzaVM.Nom, pizzaVM.Id))
            {
                ModelState.AddModelError(nameof(PizzaVM.Nom), "Une autre pizza porte déjà ce nom");
                return false;
            }

            if (MemesIngredients(pizzaVM.IdsIngredients, pizzaVM.Id))
            {
                ModelState.AddModelError(nameof(PizzaVM.IdsIngredients), "Une autre pizza utilise exactement ces mêmes ingrédients");
                return false;
            }

            return true;
        }

        public static bool NomPizzaExistant(string nomPizza, int id)
        {
            nomPizza = nomPizza.ToLower();
            return pizzas.Any(p => p.Nom.ToLower() == nomPizza && p.Id != id);
        }

       public static bool MemesIngredients(List<int> idsIngredients, int id)
        {
            return pizzas.Where(p => p.Id != id && p.Ingredients.Count() == idsIngredients.Count()).Any(p => p.Ingredients.All(i => idsIngredients.Contains(i.Id)));
        }
    }
}
