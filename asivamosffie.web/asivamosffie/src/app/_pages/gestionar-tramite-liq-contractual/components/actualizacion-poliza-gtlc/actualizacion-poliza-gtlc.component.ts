import { Component, EventEmitter, OnInit, Output, ViewChild } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ActivatedRoute, Params } from '@angular/router';
import { RegisterContractualLiquidationRequestService } from 'src/app/core/_services/registerContractualLiquidationRequest/register-contractual-liquidation-request.service';

@Component({
  selector: 'app-actualizacion-poliza-gtlc',
  templateUrl: './actualizacion-poliza-gtlc.component.html',
  styleUrls: ['./actualizacion-poliza-gtlc.component.scss']
})
export class ActualizacionPolizaGtlcComponent implements OnInit {

  dataSource = new MatTableDataSource();
  contratacionProyectoId: number = null;
  contratoPolizaActualizacionId: number = null;//definir
  listaTipoObservacionLiquidacionContratacion: any;
  listaMenu: any;
  @Output() semaforoActualizacionPoliza = new EventEmitter<string>();

  @ViewChild(MatSort, { static: true }) sort: MatSort;
  displayedColumns: string[] = [
    'polizaYSeguros',
    'responsableAprobacion'
  ];
  dataTable: any[] = [
    {
      polizaYSeguros: 'Buen manejo y correcta inversión del anticipo',
      responsableAprobacion: 'Andres Nikolai Montealegre Rojas'
    },
    {
      polizaYSeguros: 'Garantía de estabilidad y calidad de la obra',
      responsableAprobacion: 'Andres Nikolai Montealegre Rojas'
    }
  ]

  estaEditando = false;

  constructor(
    private route: ActivatedRoute,
    private registerContractualLiquidationRequestService: RegisterContractualLiquidationRequestService
  ) {
    this.route.params.subscribe((params: Params) => {
      this.contratacionProyectoId = params.id;
    });
   }

   ngOnInit(): void {
    this.registerContractualLiquidationRequestService.listaTipoObservacionLiquidacionContratacion()
    .subscribe( response => {
        this.listaTipoObservacionLiquidacionContratacion = response;
    });
    this.registerContractualLiquidationRequestService.listaMenu()
    .subscribe( response => {
        this.listaMenu = response;
    });
  }  
}
