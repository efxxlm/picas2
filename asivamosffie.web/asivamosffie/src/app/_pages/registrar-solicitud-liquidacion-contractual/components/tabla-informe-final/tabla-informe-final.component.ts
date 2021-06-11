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
    'contratacionId'
  ];

  dataSource = new MatTableDataSource(this.ELEMENT_DATA);
  @Input() contratacionId: number;
  @Input() esVerDetalle: boolean;
  @Output() semaforoInformeFinal = new EventEmitter<string>();
  listaMenu: ListaMenuSolicitudLiquidacion = ListaMenuSolicitudLiquidacionId;
  total: number = 0;
  totalCompleto: number = 0;
  totalIncompleto: number = 0;
  datosTabla = [];

  @ViewChild(MatSort) sort: MatSort;
  constructor(
    private registerContractualLiquidationRequestService: RegisterContractualLiquidationRequestService
  ) { 
  }

  ngOnInit(): void {
    this.gridInformeFinal(this.contratacionId);
  }

  gridInformeFinal(contratacionId: number) {
    this.registerContractualLiquidationRequestService.gridInformeFinal(contratacionId, this.listaMenu.registrarSolicitudLiquidacionContratacion).subscribe(report => {
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

}