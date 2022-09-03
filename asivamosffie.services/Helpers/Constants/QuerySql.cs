namespace asivamosffie.services.Helpers.Constant
{
    public static class QuerySql
    {
        #region ControlRecursos

        public const string GetResourceControlGridBySourceFunding = "SELECT cr.* FROM dbo.ControlRecurso AS cr " +
            "LEFT JOIN dbo.FuenteFinanciacion AS ff ON cr.FuenteFinanciacionId = ff.FuenteFinanciacionId " +
            "LEFT JOIN dbo.CofinanciacionAportante AS ca ON ff.AportanteId = ca.CofinanciacionAportanteId " +
            "LEFT JOIN dbo.Cofinanciacion AS c ON ca.CofinanciacionId = c.CofinanciacionId	" +
            "LEFT JOIN dbo.CuentaBancaria AS cb ON cr.CuentaBancariaId = cb.CuentaBancariaId " +
            "LEFT JOIN dbo.RegistroPresupuestal AS rp ON cr.RegistroPresupuestalId = rp.RegistroPresupuestalId " +
            "LEFT JOIN dbo.CofinanciacionDocumento AS cd ON rp.CofinanciacionDocumentoId = cd.CofinanciacionDocumentoId 	" +
            "LEFT JOIN dbo.VigenciaAporte AS va ON cr.VigenciaAporteId = va.VigenciaAporteId " +
            "WHERE 	ca.CofinanciacionAportanteId = ";
         
        #endregion


        #region Preconstruccion 
        //Preconstruccion Registrar
        //Lista Contratacion
        public const string GetListContratacion = "SELECT c.* FROM dbo.Contrato AS c " +
              "INNER JOIN dbo.Contratacion AS ctr ON c.ContratacionId = ctr.ContratacionId " +
              "INNER JOIN dbo.DisponibilidadPresupuestal AS dp ON ctr.ContratacionId = dp.ContratacionId " +
              "INNER JOIN dbo.ContratoPoliza AS cp ON c.ContratoId = cp.ContratoId " +
              "WHERE dp.NumeroDRP IS NOT NULL " +     //Documento Registro Presupuestal
              "AND cp.FechaAprobacion is not null " + //Fecha Aprobacion Poliza
              "AND ctr.TipoSolicitudCodigo = 1" +   //Solo contratos Tipo Obra
              "OR  c.EstadoVerificacionCodigo = 1" +  //Sin aprobación de requisitos técnicos
              "OR  c.EstadoVerificacionCodigo = 2" +  //En proceso de aprobación de requisitos técnicos
              "OR  c.EstadoVerificacionCodigo = 3" +  //Con requisitos técnicos aprobados
              "OR  c.EstadoVerificacionCodigo = 4" +  //Con requisitos técnicos aprobados
              "OR  c.EstadoVerificacionCodigo = 10"; //Enviado al interventor -- Enviado por el supervisor; 

        //Preconstruccion Verificar
        //Lista Contratacion      
        public const string GetListContratacionVerificar = "SELECT c.* FROM dbo.Contrato AS c " +
                      "INNER JOIN dbo.Contratacion AS ctr ON c.ContratacionId = ctr.ContratacionId " +
                      "INNER JOIN dbo.DisponibilidadPresupuestal AS dp ON ctr.ContratacionId = dp.ContratacionId " +
                      "INNER JOIN dbo.ContratoPoliza AS cp ON c.ContratoId = cp.ContratoId " +
                      "WHERE dp.NumeroDRP IS NOT NULL " +     //Documento Registro Presupuestal
                      "AND cp.FechaAprobacion is not null " + //Fecha Aprobacion Poliza
                      "AND ctr.TipoSolicitudCodigo = 1";

        public const string GetListContratacionVerificarInterventoria = "SELECT c.* FROM dbo.Contrato AS c " +
                     "INNER JOIN dbo.Contratacion AS ctr ON c.ContratacionId = ctr.ContratacionId " +
                     "INNER JOIN dbo.DisponibilidadPresupuestal AS dp ON ctr.ContratacionId = dp.ContratacionId " +
                     "INNER JOIN dbo.ContratoPoliza AS cp ON c.ContratoId = cp.ContratoId " +
                     "WHERE dp.NumeroDRP IS NOT NULL " +     //Documento Registro Presupuestal
                     "AND cp.FechaAprobacion is not null " + //Fecha Aprobacion Poliza
                     "AND c.EstadoVerificacionCodigo is not null " +
                     "AND ctr.TipoSolicitudCodigo = 2"; //Enviado al apoyo

        //Preconstruccion Validar
        //Lista Contratacion
        public const string GetListContratacionValidar = "SELECT c.* FROM dbo.Contrato AS c " +
                "INNER JOIN dbo.Contratacion AS ctr ON c.ContratacionId = ctr.ContratacionId " +
                "INNER JOIN dbo.DisponibilidadPresupuestal AS dp ON ctr.ContratacionId = dp.ContratacionId " +
                "INNER JOIN dbo.ContratoPoliza AS cp ON c.ContratoId = cp.ContratoId " +
                "WHERE dp.NumeroDRP IS NOT NULL " + //Documento Registro Presupuestal
                "AND c.EstadoVerificacionCodigo is not null " +
                "AND cp.FechaAprobacion IS NOT NULL ";//Enviado al apoyo

        #endregion

        #region Compromisos y Actas
        public const string GetManagementReport =
                 "SELECT ComiteTecnico.*" +
                " FROM  dbo.ComiteTecnico " +
                "INNER JOIN dbo.SesionParticipante  ON   ComiteTecnico.ComiteTecnicoId = SesionParticipante.ComiteTecnicoId " +
                "WHERE  SesionParticipante.UsuarioId = [pUserId] AND   ComiteTecnico.Eliminado = 0 AND  SesionParticipante.Eliminado = 0";
        #endregion

    }
}