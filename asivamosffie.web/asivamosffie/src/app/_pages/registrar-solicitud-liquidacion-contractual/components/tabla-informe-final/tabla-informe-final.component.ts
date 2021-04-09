import { Component, ViewChild, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { RegisterContractualLiquidationRequestService } from 'src/app/core/_services/registerContractualLiquidationRequest/register-contractual-liquidation-request.service';
import { ListaMenuSolicitudLiquidacion, ListaMenuSolicitudLiquidacionId } from 'src/app/_interfaces/estados-solicitud-liquidacion-contractual';

@Component({
  selector: 'app-tabla-informe-final',
  templateUrl: './tabla-informe-final.component.html',
  styleUrls: ['./tabla-informe-final.component.scss']
})
export class TablaInformeFinalComponent implements OnInit {

  ELEMENT_DATA: any[] = [];
  displayedColumns: string[] = [
    'fechaEnvio',
    'fechaAprobacion',
    'llaveMen',
    'tipoIntervencion',
    'institucionEducativa',
    'sede',
    'estadoValidacion',
    'contratacionProyectoId'
  ];

  dataSource = new MatTableDataSource(this.ELEMENT_DATA);
  @Input() contratacionProyectoId: number;
  @Input() esVerDetalle: boolean;
  @Output() semaforoInformeFinal = new EventEmitter<string>();
  listaMenu: ListaMenuSolicitudLiquidacion = ListaMenuSolicitudLiquidacionId;

  datosTabla = [];

  @ViewChild(MatSort) sort: MatSort;
  constructor(
    private registerContractualLiquidationRequestService: RegisterContractualLiquidationRequestService
  ) { 
  }

  ngOnInit(): void {
    this.gridInformeFinal(this.contratacionProyectoId);
  }

  gridInformeFinal(contratacionProyectoId: number) {
    this.registerContractualLiquidationRequestService.gridInformeFinal(contratacionProyectoId, this.listaMenu.registrarSolicitudLiquidacionContratacion).subscribe(report => {
      if(report != null){
        report.forEach(element => {
          this.datosTabla.push({
            fechaEnvio : element.fechaEnvio.split('T')[0].split('-').reverse().join('/'),
            fechaAprobacion : element.fechaAprobacion.split('T')[0].split('-').reverse().join('/'),
            llaveMen: element.llaveMen,
            tipoIntervencion: element.tipoIntervencion,
            institucionEducativa: element.institucionEducativa,
            sede: element.sede,
            estadoValidacion: element.registroCompleto ? 'Con validación' : 'Sin validación',
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

}