import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-procesos-contractuales',
  templateUrl: './procesos-contractuales.component.html',
  styleUrls: ['./procesos-contractuales.component.scss']
})
export class ProcesosContractualesComponent implements OnInit {

  verAyuda = false;
  dataSinTramitar: any[] = [
    {
      fechaSolicitud   : '15/06/2020',
      numeroSolicitud  : 'PI_008',
      tipoSolicitud    : 'Contratación',
      estadoRegistro   : false,
      fechaEnvioTramite: null,
      id               : 0
    },
    {
      fechaSolicitud   : '15/06/2020',
      numeroSolicitud  : 'CO_003',
      tipoSolicitud    : 'Modificación contractual',
      estadoRegistro   : false,
      fechaEnvioTramite: null,
      id               : 1
    },
    {
      fechaSolicitud   : '15/06/2020',
      numeroSolicitud  : 'SL_001',
      tipoSolicitud    : 'Liquidación',
      estadoRegistro   : false,
      fechaEnvioTramite: null,
      id               : 2
    }
  ];
  dataEnviadasFiduciaria: any[] = [
    {
      fechaSolicitud   : '15/06/2020',
      numeroSolicitud  : 'PI_008',
      tipoSolicitud    : 'Contratación',
      estadoRegistro   : true,
      fechaEnvioTramite: null,
      id               : 0
    },
    {
      fechaSolicitud   : '15/06/2020',
      numeroSolicitud  : 'CO_003',
      tipoSolicitud    : 'Modificación contractual',
      estadoRegistro   : true,
      fechaEnvioTramite: null,
      id               : 1
    },
    {
      fechaSolicitud   : '15/06/2020',
      numeroSolicitud  : 'SL_001',
      tipoSolicitud    : 'Liquidación',
      estadoRegistro   : true,
      fechaEnvioTramite: null,
      id               : 2
    }
  ]

  constructor() { }

  ngOnInit(): void {
  }

};