using BO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TPPizza.Controllers;
using TPPizza.CustomAttributes;

namespace TPPizza.Models
{
    public class PizzaVM
    {
        public int Id { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Le nom de la pizza doit contenir entre {2} et {1} caractères")]
        [Remote(nameof(PizzaController.VerifNomPizzaUnique), nameof(Pizza), AdditionalFields = nameof(Id))]
        public string Nom { get; set; }

        [Required]
        [DisplayName("Pâte")]
        public int IdPate { get; set; }
        public SelectList ChoixPate => new SelectList(Pizza.PatesDisponibles, "Id", "Nom");

        [ListLength(Min = 2, Max = 5)]
       // [Test(Min = 4)]
        [Remote(nameof(PizzaController.VerifIngredientsOriginaux), nameof(Pizza), AdditionalFields = nameof(Id))]
        [DisplayName("Ingrédients")]
        public List<int> IdsIngredients { get; set; }
        public SelectList ChoixIngredients => new SelectList(Pizza.IngredientsDisponibles, "Id", "Nom");
    }
}
