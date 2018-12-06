using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using TransporteCarga.Models;

namespace TransporteCarga.Validator
{
    public class CustomValidator : ValidationAttribute
    {
        private TransporteCargaContext db = new TransporteCargaContext();

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                List<Orden> ordenes = db.Ordenes.Where(a => ((int[])value).Contains(a.ordenId)).ToList();

                int? clienteId = null;
                foreach (var item in ordenes)
                {
                    if (clienteId == null)
                    {
                        clienteId = item.ClientePago.clienteid;
                    }
                    else if (clienteId != item.ClientePago.clienteid)
                    {
                        return new ValidationResult("El Cliente debe ser el mismo");
                    }
                }

                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult("Guía es obligatorio");

            }
           // return base.IsValid(value, validationContext);
        }
    }
}