﻿
namespace asivamosffie.services.Helpers.Constant
{
    public class ConstantMessagesUsuarios
    {
        #region Mensajes Informativos

        //Menu Usuario 
        public const string ContrasenaGenerada = "101";
        public const string UsuarioNoExiste = "102";
        public const string UsuarioInactivo = "103";
        public const string UsuarioBloqueado = "104";
        public const string ContrasenaIncorrecta = "105";
        public const string CorreoNoExiste = "106";
        public const string ErrorEnviarCorreo = "107";
        public const string EmailObligatorio = "108";
        public const string ErrorGuardarCambios = "109";

        //        101	<b>Se ha enviado un correo de notificacion
        //       102	  Se ha actualizado registro a estado "Con aprobación de pólizas"
        //102	<b>El usuario no existe en el sistema.</b> Contacte al administrador.     NOO
        //103	<b>El usuario se encuentra inactivo.</b> Contacte al administrador.   NOOOOO
        //104	<b>El usuario se encuentra bloqueado,</b> debe remitirse a la opción “Recordar Contraseña”.   NOOOOOO
        //105	<b>La contraseña es incorrecta.</b>     NOOOO
        //301	Será direccionado para cambiar su contraseña.      NOOOOO
        //200	<b>La información ha sido guardada exitosamente.</b>    NOOOO  YAAA
        //106	<b>El usuario no existe en el sistema.</b> Contacte al administrador.   NOOOO
        //107	Ocurrio un error al enviar el correo.  
        //108	El email es obligatorio.    NOOOO
        //109	Error al guardar cambios     nooo??
        //500	Error al guardar cambios     nooo   YAAAA
        //501 Ha ocurrido un error al interno


        //public const string CorreoEnviado = "101";
        //public const string OperacionExitosa = "200";
        //public const string CreadoCorrrectamente = "103";     ?????
        //public const string EditarContratoPolizaCorrrectamente = "102";
        //public const string Error = "500";
        //public const string ErrorInterno = "501";
        //public const string ErrorEnviarCorreo = "107";
        //public const string CorreoNoExiste = "106";

        #endregion Mensajes Informativos

        #region Mensajes Exitoso

        public const string OperacionExitosa = "200";

        #endregion Mensajes Exitoso

        #region Mensajes Redireccion

        public const string DirecCambioContrasena = "301";

        #endregion Mensajes Redireccion



    }

    public class ConstantMessagesContrasena
    {
        #region Mensajes Informativos
       
        //cambio de contraseña
        public const string ErrorContrasenaAntigua = "101";
        public const string ErrorSesion = "102";

        #endregion Mensajes Informativos

        #region Mensajes Exitoso

        public const string OperacionExitosa = "200";

        #endregion Mensajes Exitoso      

    }

    public class ConstantMessagesCofinanciacion
    {

        #region Mensajes Informativos


        public const string CamposIncompletos = "101";
        public const string EditadoCorrrectamente = "102";
        public const string CreadoCorrrectamente = "103";
        public const string RecursoNoEncontrado = "104";

        

        #endregion Mensajes Informativos

        #region Mensajes Exitoso

        public const string OperacionExitosa = "200";
        public const string EliminacionExitosa = "201";
        public const string EliminacionCancelada = "202";

        #endregion Mensajes Exitoso

        #region Mensajes Redireccion

        public const string DirecCambioContrasena = "301";

        #endregion Mensajes Redireccion
        
    }

    public class ConstantMessagesContributor
    {
        #region Mensajes Error


        public const string ErrorInterno = "501";



        #endregion Mensajes Informativos

        #region Mensajes Informativos


        public const string CamposIncompletos = "101";
        public const string EditadoCorrrectamente = "102";
        public const string CreadoCorrrectamente = "103";
        public const string RecursoNoEncontrado = "104";



        #endregion Mensajes Informativos

        #region Mensajes Exitoso

        public const string OperacionExitosa = "200";

        #endregion Mensajes Exitoso

        #region Mensajes Redireccion

        public const string DirecCambioContrasena = "301";

        #endregion Mensajes Redireccion

    }

    public class ConstantMessagesSourceFunding
    {

        #region Mensajes Informativos
        public const string EditadoCorrrectamente = "102";

        #endregion Mensajes Informativos

        #region Mensajes Exitoso

        public const string OperacionExitosa = "200";

        #endregion Mensajes Exitoso

        #region Mensajes Redireccion

        #endregion Mensajes Redireccion

        #region 
        public const string Error = "500";
        #endregion
    }

    public class ConstantMessagesBankAccount
    {

        #region Mensajes Informativos
        public const string EditadoCorrrectamente = "102";

        #endregion Mensajes Informativos

        #region Mensajes Exitoso

        public const string OperacionExitosa = "200";

        #endregion Mensajes Exitoso

        #region Mensajes Redireccion

        #endregion Mensajes Redireccion

        #region 
        public const string Error = "500";
        #endregion
    }

    public class ConstantMessagesResourceControl
    {

        #region Mensajes Informativos
        public const string EditadoCorrrectamente = "102";

        #endregion Mensajes Informativos

        #region Mensajes Exitoso

        public const string OperacionExitosa = "200";
        public const string EliminadoExitosamente = "201";

        #endregion Mensajes Exitoso

        #region Mensajes Redireccion

        #endregion Mensajes Redireccion

        #region 
        public const string Error = "500";
        #endregion
    }

    public class ConstantMessagesRegisterBudget
    {

        #region Mensajes Informativos
        public const string EditadoCorrrectamente = "102";

        #endregion Mensajes Informativos

        #region Mensajes Exitoso

        public const string OperacionExitosa = "200";

        #endregion Mensajes Exitoso

        #region Mensajes Redireccion

        #endregion Mensajes Redireccion

        #region 
        public const string Error = "500";
        #endregion
    }


    public class ConstantMessagesFuentesFinanciacion
    {

        #region Mensajes Informativos
        

        #endregion Mensajes Informativos

        #region Mensajes Exitoso

        public const string OperacionExitosa = "200";
        public const string EliminacionExitosa = "201";

        #endregion Mensajes Exitoso

        #region Mensajes Redireccion

        #endregion Mensajes Redireccion

        #region 
        public const string Error = "500";
        #endregion
    }

    public class ConstantMessagesCargueProyecto
    {

        #region Mensajes Informativos


        public const string NoExitenArchivos = "100";

        public const string CamposVacios = "101";

        #endregion Mensajes Informativos

        #region Mensajes Exitoso

        public const string OperacionExitosa = "200";

        public const string DescargaExcelExitosa = "201";

        #endregion Mensajes Exitoso

        #region Mensajes Redireccion

        public const string DirecCambioContrasena = "301";

        #endregion Mensajes Redireccion

        #region 
        public const string Error = "500";
        public const string ErrorDescargarArchivo = "501";
        #endregion
    }

    public class ConstantMessagesCargueElegibilidad
    {

        #region Mensajes Informativos


        public const string NoExitenArchivos = "100";

        public const string CamposVacios = "101";

        #endregion Mensajes Informativos

        #region Mensajes Exitoso

        public const string OperacionExitosa = "200";

        public const string DescargaExcelExitosa = "201";

        #endregion Mensajes Exitoso

        #region Mensajes Redireccion

        public const string DirecCambioContrasena = "301";

        #endregion Mensajes Redireccion

        #region 
        public const string Error = "500";
        public const string ErrorDescargarArchivo = "501";
        #endregion
    }

    public class ConstantMessagesProyecto
    {

        #region Mensajes Informativos


        #endregion Mensajes Informativos

        #region Mensajes Exitoso

        public const string OperacionExitosa = "200";

        #endregion Mensajes Exitoso

        #region Mensajes Redireccion

        #endregion Mensajes Redireccion

        #region 
        public const string Error = "500";
        #endregion
    }

    public class ConstantMessagesDisponibilidadPresupuesta
    {

        #region Mensajes Informativos


        #endregion Mensajes Informativos

        #region Mensajes Exitoso

        public const string OperacionExitosa = "200";

        #endregion Mensajes Exitoso

        #region Mensajes Redireccion

        #endregion Mensajes Redireccion

        #region 
        public const string Error = "500";
        #endregion
    }
    public class ConstantSesionComiteTecnico
    {

        #region Mensajes Informativos


        #endregion Mensajes Informativos

        #region Mensajes Exitoso

        public const string OperacionExitosa = "200";

        #endregion Mensajes Exitoso

        #region Mensajes Redireccion

        #endregion Mensajes Redireccion

        #region 
        public const string Error = "500";
        #endregion
    }

    public class ConstantSesionComiteFiduciario
    {

        #region Mensajes Informativos


        #endregion Mensajes Informativos

        #region Mensajes Exitoso

        public const string OperacionExitosa = "200";

        #endregion Mensajes Exitoso

        #region Mensajes Redireccion

        #endregion Mensajes Redireccion

        #region 
        public const string Error = "500";
        #endregion
    }

    public class ConstantMessagesContratacionProyecto
    {

        #region Mensajes Informativos


        #endregion Mensajes Informativos

        #region Mensajes Exitoso

        public const string OperacionExitosa = "200";

        #endregion Mensajes Exitoso

        #region Mensajes Redireccion

        #endregion Mensajes Redireccion

        #region 
        public const string Error = "500";
        #endregion
    }
    public class ConstantMessagesGenerateBudget
    {

        #region Mensajes Informativos
        public const string CanceladoCorrrectamente = "201";
        public const string DevueltoCorrrectamente = "202";
        #endregion Mensajes Informativos

        #region Mensajes Exitoso

        public const string OperacionExitosa = "200";

        #endregion Mensajes Exitoso

        #region Mensajes Redireccion

        #endregion Mensajes Redireccion

        #region 
        public const string Error = "500";
        #endregion
    }

    public class ConstantGestionarProcesosContractuales
    {

        #region Mensajes Informativos


        #endregion Mensajes Informativos

        #region Mensajes Exitoso

        public const string OperacionExitosa = "200";

        #endregion Mensajes Exitoso

        #region Mensajes Redireccion

        #endregion Mensajes Redireccion

        #region 
        public const string Error = "500";
        #endregion
    }

    public class ConstantMessagesProcessSchedule
    {
        #region Mensajes Error


        public const string ErrorInterno = "501";



        #endregion Mensajes Informativos

        #region Mensajes Informativos


        public const string CamposIncompletos = "101";
        public const string EditadoCorrrectamente = "102";
        public const string CreadoCorrrectamente = "103";
        public const string RecursoNoEncontrado = "104";



        #endregion Mensajes Informativos

        #region Mensajes Exitoso

        public const string OperacionExitosa = "200";

        #endregion Mensajes Exitoso

        #region Mensajes Redireccion

        public const string DirecCambioContrasena = "301";

        #endregion Mensajes Redireccion

    }

    public class ConstantMessagesProcessQuotation
    {
        #region Mensajes Error


        public const string ErrorInterno = "501";



        #endregion Mensajes Informativos

        #region Mensajes Informativos


        public const string CamposIncompletos = "101";
        public const string EditadoCorrrectamente = "102";
        public const string CreadoCorrrectamente = "103";
        public const string RecursoNoEncontrado = "104";



        #endregion Mensajes Informativos

        #region Mensajes Exitoso

        public const string OperacionExitosa = "200";

        #endregion Mensajes Exitoso

        #region Mensajes Redireccion

        public const string DirecCambioContrasena = "301";

        #endregion Mensajes Redireccion

    }

    public class ConstantMessagesProcesoSeleccion
    {
        #region Mensajes Error


        public const string ErrorInterno = "501";



        #endregion Mensajes Informativos

        #region Mensajes Informativos


        public const string CamposIncompletos = "101";
        public const string EditadoCorrrectamente = "102";
        public const string CreadoCorrrectamente = "103";
        public const string RecursoNoEncontrado = "104";



        #endregion Mensajes Informativos

        #region Mensajes Exitoso

        public const string OperacionExitosa = "200";

        #endregion Mensajes Exitoso

        #region Mensajes Redireccion

        public const string DirecCambioContrasena = "301";

        #endregion Mensajes Redireccion

    }

    public class ConstantMessagesSesionComiteTema
    {

        #region Mensajes Informativos


        #endregion Mensajes Informativos

        #region Mensajes Exitoso

        public const string OperacionExitosa = "200";

        #endregion Mensajes Exitoso

        #region Mensajes Redireccion

        #endregion Mensajes Redireccion

        #region 
        public const string Error = "500";
        #endregion
    }
    
    
    
    public class ConstantMessagesSelectionProcessGroup
    {
        #region Mensajes Error


        public const string ErrorInterno = "501";



        #endregion Mensajes Informativos

        #region Mensajes Informativos


        public const string CamposIncompletos = "101";
        public const string EditadoCorrrectamente = "102";
        public const string CreadoCorrrectamente = "103";
        public const string RecursoNoEncontrado = "104";



        #endregion Mensajes Informativos

        #region Mensajes Exitoso

        public const string OperacionExitosa = "200";

        #endregion Mensajes Exitoso

        #region Mensajes Redireccion

        public const string DirecCambioContrasena = "301";

        #endregion Mensajes Redireccion

    }

     public class ConstantMessagesActaInicio
    {

        #region Mensajes Informativos


        #endregion Mensajes Informativos

        #region Mensajes Exitoso

        public const string OperacionExitosa = "200";
        public const string CreadoCorrectamente = "103";
        public const string EditadoCorrrectamente = "102";

        #endregion Mensajes Exitoso

        #region Mensajes Redireccion

        #endregion Mensajes Redireccion

        #region 
        public const string Error = "500";
        public const string ErrorInterno = "501";
        #endregion
    }

    public class ConstantMessagesContratoPoliza
    {

        #region Mensajes Informativos

        public const string RecursoNoEncontrado = "104";


        #endregion Mensajes Informativos

        #region Mensajes Exitoso

        public const string CorreoEnviado = "101";
        public const string OperacionExitosa = "200";
        public const string CreadoCorrrectamente = "103";
        public const string EditarContratoPolizaCorrrectamente = "102";

        #endregion Mensajes Exitoso

        #region Mensajes Redireccion

        #endregion Mensajes Redireccion

        #region 
        public const string Error = "500";
        public const string ErrorInterno = "501";
        public const string ErrorEnviarCorreo = "107";
        public const string CorreoNoExiste = "106";
        #endregion
    }


    

    public class ConstantGestionarActaInicioFase2
    {

        #region Mensajes Informativos

        public const string CorreoEnviado = "101";
        public const string EditadoCorrectamente = "102";

        #endregion Mensajes Informativos

        #region Mensajes Exitoso

        public const string OperacionExitosa = "200";

        #endregion Mensajes Exitoso

        #region Mensajes Redireccion

        #endregion Mensajes Redireccion

        #region 
        public const string Error = "500";
        public const string ErrorInterno = "501";
        public const string ErrorEnviarCorreo = "107";
         

        #endregion
    }
}
