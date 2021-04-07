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
  @Input() contratacionProyectoId: number;
  @Output() semaforoInformeFinal = new EventEmitter<string>();
  listaMenu: ListaMenuSolicitudLiquidacion = ListaMenuSolicitudLiquidacionId;

  ELEMENT_DATA: any[] = [];
  displayedColumns: string[] = [
    'fechaEnvio',
    'fechaAprobacion',
    'llaveMen',
    'tipoIntervencion',
    'institucionEducativa',
    'sede',
    'estadoVerificacion',
    'contratacionProyectoId'
  ];

  dataSource = new MatTableDataSource(this.ELEMENT_DATA);
  datosTabla = [];

  @ViewChild(MatSort, { static: true }) sort: MatSort;

  constructor(
    private registerContractualLiquidationRequestService: RegisterContractualLiquidationRequestService
  ) { 
  }

  ngOnInit(): void {
    this.gridInformeFinal(this.contratacionProyectoId);
  }

  gridInformeFinal(contratacionProyectoId: number) {
    this.registerContractualLiquidationRequestService.gridInformeFinal(contratacionProyectoId, this.listaMenu.gestionarSolicitudLiquidacionContratacion).subscribe(report => {
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
            contratacionProyectoId: contratacionProyectoId,
            proyectoId: element.proyectoId
          });
        })
      }
      this.dataSource.data = this.datosTabla;
      if(this.datosTabla.length > 0){
        this.semaforoInformeFinal.emit(this.datosTabla[0].registroCompleto);
      }
    });
  }

  /*verDetalleInformeFinal(id) {
    this.router.navigate(['/gestionarTramiteLiquidacionContractual/detalleInformeFinal', id]);
  }*/

}
