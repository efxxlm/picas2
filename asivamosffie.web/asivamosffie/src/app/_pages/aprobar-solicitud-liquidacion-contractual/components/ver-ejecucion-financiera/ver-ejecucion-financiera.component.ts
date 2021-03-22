import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-ver-ejecucion-financiera',
  templateUrl: './ver-ejecucion-financiera.component.html',
  styleUrls: ['./ver-ejecucion-financiera.component.scss']
})
export class VerEjecucionFinancieraComponent implements OnInit {

  listaejecucionPresupuestal = [
    {
      componente: 'Obra',
      totalComprometido: '$65.000.000',
      facturadoAntesImpuestos: '$40.000.000',
      saldo: '$25.000.000',
      porcentajeEjecucionPresupuestal: '65%'
    },
    {
      componente: 'Interventoría',
      totalComprometido: '$40.000.000',
      facturadoAntesImpuestos: '$28.000.000',
      saldo: '$12.000.000',
      porcentajeEjecucionPresupuestal: '70%'
    }
  ]
  
  constructor() { }

  ngOnInit(): void {
  }

}
