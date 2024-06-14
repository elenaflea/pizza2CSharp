using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace TPPizza.CustomAttributes
{
    public class ListLengthAttribute : ValidationAttribute, IClientModelValidator
    {
        public int Max { get; set; }
        public int Min { get; set; }
        public void AddValidation(ClientModelValidationContext context)
        {
            context.Attributes.Add("data-val", "true");
            context.Attributes.Add("data-val-liste", $"Vous devez sélectionner entre {Min} et {Max} éléments");
            context.Attributes.Add("data-val-liste-min", Min.ToString());
            context.Attributes.Add("data-val-liste-max", Max.ToString());
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is not IList liste)
            {
                return new ValidationResult("Cet élément n'est pas une liste");
            }

            if (liste.Count < Min)
            {
                return new ValidationResult($"Pas assez d'éléments choisis. Au minimum, il est faut {Min}.");
            }

            return liste.Count > Max
                ? new ValidationResult($"Trop d'éléments choisis. Au maximum, il faut en choisir {Max}.")
                : ValidationResult.Success;
        }
    }
}