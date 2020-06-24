using AuthorizationTest.JwtHelpers;

namespace asivamosffie.model.APIModels
{
    public class Respuesta{       
        public bool IsSuccessful { get; set; }
        public bool IsValidation { get;set; }
        public bool IsException { get; set; }
        public string Code { get; set; } 
        public string Message { get;set; }
        public JwtToken Token { get; set; }
        public object Data { get; set; }


    }
}