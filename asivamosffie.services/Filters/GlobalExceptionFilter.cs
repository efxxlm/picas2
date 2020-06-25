using asivamosffie.services.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using Microsoft.Data.SqlClient;
using asivamosffie.model.Models;
using asivamosffie.model.APIModels;

namespace asivamosffie.services.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {        
        public void OnException(ExceptionContext context)
        {
            Respuesta respuesta = new Respuesta()
                {
                    IsSuccessful = false,
                    IsValidation = false,
                    IsException = true,
                    Code = "500",
                    Data = context.Exception
                };
    
            if (context.Exception is BusinessException){
                BusinessException exception = (BusinessException)context.Exception;
                respuesta.Message = string.Concat("Mensaje Negocio: ",exception.Message);    
            }
            else if (context.Exception is SqlException){
                SqlException exception = (SqlException)context.Exception;
                respuesta.Message = string.Concat("Excepcion Base Datos.");    
            }
            else{
                respuesta.Message = context.Exception.Message;    
            }

            context.Result = new BadRequestObjectResult(respuesta);
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            context.ExceptionHandled = true;
        }
    }

   
}
