
using asivamosffie.model.APIModels;

namespace asivamosffie.api.Responses
{
    public class ApiResponse<T>
    {
        public ApiResponse(T data)
        {
            Data = data;
        }

        public T Data { get; set; }
        public Respuesta Meta { get; set; }
    }
}
