import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-ejecucion-financiera',
  templateUrl: './ejecucion-financiera.component.html',
  styleUrls: ['./ejecucion-financiera.component.scss']
})
export class EjecucionFinancieraComponent implements OnInit {

  listaModificaciones = [
    {
      numeroActualizacion: 'ACT-DDP 0001',
      tipoModificacion: 'Adición',
      valorAdicional: '$5.000.000',
      numeroComite: 'CT_0060',
      fechaComite: '20/11/2020',
      urlSoporte: 'http://AdicionDDP_0001.onedrive.com'
    }
  ];

  listaDRP = [
    {
      drp: '1',
      numero: 'IP_00090',
      Valor: '$100.000.000'
    }
  ];

  listaejEcucionPresupuestal = [
    {
      componente: 'Obra',
      totalComprometido: '$65.000.000',
      facturadoAntesImpuestos: '$40.000.000',
      saldo: '$25.000.000',
      porcentajeEjecucionPresupuestal: '65%'
    }
  ];

  listaEjecucionFinanciera = [
    {
      componente: 'Obra',
      totalComprometido: '$65.000.000',
      ordenadoGirarAntesImpuestos: '$40.000.000',
      saldo: '$25.000.000',
      porcentajeEjecucionPresupuestal: '65%'
    }
  ];

  constructor() {}

  ngOnInit(): void {}
}
