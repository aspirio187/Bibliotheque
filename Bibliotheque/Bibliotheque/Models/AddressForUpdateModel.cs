using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bibliotheque.UI.Models
{

    public class AddressForUpdateModel
    {
        public Guid Id { get; set; }

        private string m_Street;

        public string Street
        {
            get => m_Street;
            set => m_Street = FormatAndCheck(value, "La rue", Fields.Street, true, 3, 100);
        }

        private string m_Appartment;

        public string Appartment
        {
            get => m_Appartment;
            set => m_Appartment = FormatAndCheck(value, "L'appartment", Fields.Appartment, false, 0, 5);
        }

        private string m_ZipCode;

        public string ZipCode
        {
            get => m_ZipCode;
            set => m_ZipCode = FormatAndCheck(value, "Le code postal", Fields.ZipCode, true, 4, 4);
        }

        private string m_City;

        public string City
        {
            get => m_City;
            set => m_City = FormatAndCheck(value, "La ville", Fields.City, true, 2, 100);
        }

        public ObservableCollection<AddressErrorRecord> ErrorsCollection { get; set; } = new();

        public record AddressErrorRecord(FieldErrors Error, Fields Field, string Message)
        {
            public override string ToString()
            {
                return $"{Message}";
            }
        }

        public enum FieldErrors
        {
            IsNull,
            IsRequired,
            MinimumLength,
            MaximumLength
        }

        public enum Fields
        {
            Street,
            Appartment,
            ZipCode,
            City
        }


        /// <summary>
        /// Vérifie si le champ est valide, selon toutes les contraintes en paramètre, si le champ est valide, une erreur sera crée et ajoutée à la collection,
        /// dans la cas contraire, il va vérifier si une erreur existe et la supprimer de la liste si c'est le cas.
        /// </summary>
        /// <param name="input">Champ qu'on traite</param>
        /// <param name="fieldName">Le nom du champ affiché à l'utilisateur</param>
        /// <param name="field">élement de l'enum Fields qui représente le champ sur lequel on travail</param>
        /// <param name="isRequired">Est-ce que le champ est requis ?</param>
        /// <param name="minimumLength">La taille minimum du champ</param>
        /// <param name="maximumLength">La taille maximum du champ</param>
        /// <returns>
        /// true Si l'input répond à toutes les contraintes. false Dans le cas contraire
        /// </returns>
        /// <exception cref="ArgumentNullException">Si fieldName est null</exception>
        /// <exception cref="ArgumentException">Si fieldName est une chaine vide</exception>
        private string FormatAndCheck(string input, string fieldName, Fields field, bool isRequired = false, int minimumLength = 1, int maximumLength = int.MaxValue)
        {
            // Vérifie que le nom du champ n'est ni null ni vide
            if (fieldName is null) throw new ArgumentNullException(nameof(fieldName));
            fieldName = fieldName.Trim();
            if (fieldName.Equals(string.Empty) || fieldName.Length == 0) throw new ArgumentException(nameof(fieldName));
            // Commence la vérification
            if (input == null && isRequired == true)
            {
                ErrorsCollection.Add(new AddressErrorRecord(FieldErrors.IsNull, field, $"Une erreur est survenue lors de l'encodage."));
                return string.Empty;
            }
            if (input == null && isRequired == false)
            {
                return string.Empty;
            }
            input = input.Trim();
            if (isRequired == true)
            {
                if (input.Equals(string.Empty) || input.Length == 0)
                {
                    ErrorsCollection.Add(new AddressErrorRecord(FieldErrors.IsRequired, field, $"{fieldName} est/sont requis(e)!"));
                }
                else
                {
                    int i = ErrorsCollection.IndexOf(ErrorsCollection.FirstOrDefault(x => x.Error == FieldErrors.IsRequired && x.Field == field));
                    if (i > 0) ErrorsCollection.RemoveAt(i);
                }
            }
            if (input.Length < minimumLength)
            {
                ErrorsCollection.Add(new AddressErrorRecord(FieldErrors.MinimumLength, field, $"{fieldName} doit contenir au moins {minimumLength} caractères!"));
            }
            else
            {
                int i = ErrorsCollection.IndexOf(ErrorsCollection.FirstOrDefault(x => x.Error == FieldErrors.MinimumLength && x.Field == field));
                if (i >= 0) ErrorsCollection.RemoveAt(i);
            }
            if (input.Length > maximumLength)
            {
                ErrorsCollection.Add(new AddressErrorRecord(FieldErrors.MaximumLength, field, $"{fieldName} ne peut excéder {maximumLength} caractères!"));
            }
            else
            {
                int i = ErrorsCollection.IndexOf(ErrorsCollection.FirstOrDefault(x => x.Error == FieldErrors.MaximumLength && x.Field == field));
                if (i >= 0) ErrorsCollection.RemoveAt(i);
            }
            return input.ToLower().Trim();
        }

        public bool FieldsAreValid()
        {
            return !string.IsNullOrEmpty(Street) && Street.Length > 3 && Street.Length < 100 &&
                (string.IsNullOrEmpty(Appartment) || Appartment.Length < 5) &&
                !string.IsNullOrEmpty(ZipCode) && ZipCode.Length == 4 &&
                !string.IsNullOrEmpty(City) && City.Length > 2 && City.Length < 100;
        }
    }
}
