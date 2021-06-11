import { Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { RegisterContractualLiquidationRequestService } from 'src/app/core/_services/registerContractualLiquidationRequest/register-contractual-liquidation-request.service';
import { ListaMenuSolicitudLiquidacion, ListaMenuSolicitudLiquidacionId } from 'src/app/_interfaces/estados-solicitud-liquidacion-contractual';

@Component({
  selector: 'app-tabla-informe-final-gtlc',
  templateUrl: './tabla-informe-final-gtlc.component.html',
  styleUrls: ['./tabla-informe-final-gtlc.component.scss']
})
export class TablaInformeFinalGtlcComponent implements OnInit {

  @Input() verDetalleBtn;
  @Input() esVerDetalle: boolean;
  @Input() contratacionId: number;
  @Output() semaforoInformeFinal = new EventEmitter<string>();
  listaMenu: ListaMenuSolicitudLiquidacion = ListaMenuSolicitudLiquidacionId;
  total: number = 0;
  totalCompleto: number = 0;
  totalIncompleto: number = 0;
  
  ELEMENT_DATA: any[] = [];
  displayedColumns: string[] = [
    'fechaEnvio',
    'fechaAprobacion',
    'llaveMen',
    'tipoIntervencion',
    'institucionEducativa',
    'sede',
    'estadoVerificacion',
    'contratacionId'
  ];

  dataSource = new MatTableDataSource(this.ELEMENT_DATA);
  datosTabla = [];

  @ViewChild(MatSort, { static: true }) sort: MatSort;

  constructor(
    private registerContractualLiquidationRequestService: RegisterContractualLiquidationRequestService
  ) { 
  }

  ngOnInit(): void {
    this.gridInformeFinal(this.contratacionId);
  }

  gridInformeFinal(contratacionId: number) {
    this.registerContractualLiquidationRequestService.gridInformeFinal(contratacionId, this.listaMenu.gestionarSolicitudLiquidacionContratacion).subscribe(report => {
      if(report != null){
        report.forEach(element => {
          this.datosTabla.push({
            fechaEnvio : element.fechaEnvio.split('T')[0].split('-').reverse().join('/'),
            fechaAprobacion : element.fechaAprobacion.split('T')[0].split('-').reverse().join('/'),
            llaveMen: element.llaveMen,
            tipoIntervencion: element.tipoIntervencion,
            institucionEducativa: element.institucionEducativa,
            sede: element.sede,
            estadoVerificacion: element.registroCompleto ? 'Con validación' : 'Sin validación',
            registroCompleto: element.registroCompleto ? 'Completo' : 'Incompleto',
            contratacionId: contratacionId,
            proyectoId: element.proyectoId
          });
        })
      }
      this.dataSource.data = this.datosTabla;
      this.total = this.datosTabla.length;
      this.datosTabla.forEach(element => {
        if(element.registroCompleto === 'Completo')
          this.totalCompleto++;
        if(element.registroCompleto === 'Incompleto')
          this.totalIncompleto++;
      });
      if(this.total <= 0){
        this.semaforoInformeFinal.emit(null);
      }
      else if(this.totalCompleto >= this.total){
        this.semaforoInformeFinal.emit('Completo');
      }else if(this.totalIncompleto >= this.total){
        this.semaforoInformeFinal.emit(null);
      }else{
        this.semaforoInformeFinal.emit('Incompleto');
      }
    });
  }

  /*verDetalleInformeFinal(id) {
    this.router.navigate(['/gestionarTramiteLiquidacionContractual/detalleInformeFinal', id]);
  }*/

}
