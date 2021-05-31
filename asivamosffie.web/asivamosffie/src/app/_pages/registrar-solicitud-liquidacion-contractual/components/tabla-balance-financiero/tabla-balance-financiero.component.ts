import { Component, ViewChild, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { RegisterContractualLiquidationRequestService } from 'src/app/core/_services/registerContractualLiquidationRequest/register-contractual-liquidation-request.service';
import { ListaMenuSolicitudLiquidacion, ListaMenuSolicitudLiquidacionId } from 'src/app/_interfaces/estados-solicitud-liquidacion-contractual';

@Component({
  selector: 'app-tabla-balance-financiero',
  templateUrl: './tabla-balance-financiero.component.html',
  styleUrls: ['./tabla-balance-financiero.component.scss']
})
export class TablaBalanceFinancieroComponent implements OnInit {

  ELEMENT_DATA: any[] = [];
  displayedColumns: string[] = [
    'fechaterminacionProyecto',
    'llaveMEN',
    'tipoIntervencion',
    'institucionEducativa',
    'sede',
    'numeroTraslados',
    'estadoValidacion',
    'contratacionId'
  ];

  @Input() contratacionId: number;
  @Input() esVerDetalle: boolean;
  @Output() semaforoBalanceFinanciero = new EventEmitter<string>();
  listaMenu: ListaMenuSolicitudLiquidacion = ListaMenuSolicitudLiquidacionId;
  total: number = 0;
  totalCompleto: number = 0;
  totalIncompleto: number = 0;

  datosTabla = [];
  dataSource = new MatTableDataSource(this.ELEMENT_DATA);

  @ViewChild(MatSort) sort: MatSort;
  constructor(
    private registerContractualLiquidationRequestService: RegisterContractualLiquidationRequestService
  ) { }

  ngOnInit(): void {
    this.getBalanceBycontratacionId(this.contratacionId);
  }

  getBalanceBycontratacionId(contratacionId: number) {
    this.registerContractualLiquidationRequestService.getBalanceByContratacionId(contratacionId, this.listaMenu.registrarSolicitudLiquidacionContratacion).subscribe(report => {
      if(report != null){
        report.forEach(element => {
          this.datosTabla.push({
            fechaterminacionProyecto : element.balance.fechaTerminacionProyecto.split('T')[0].split('-').reverse().join('/'),
            llaveMen: element.balance.llaveMen,
            tipoIntervencion: element.balance.tipoIntervencion,
            institucionEducativa: element.balance.institucionEducativa,
            sede: element.balance.sedeEducativa,
            numeroTraslados: element.balance.numeroTraslado,
            estadoValidacion: element.registroCompleto ? 'Con validación' : 'Sin validación',
            registroCompleto: element.registroCompleto ? 'Completo' : 'Incompleto',
            contratacionId: contratacionId,
            proyectoId: element.balance.proyectoId
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
        this.semaforoBalanceFinanciero.emit(null);
      }
      else if(this.totalCompleto >= this.total){
        this.semaforoBalanceFinanciero.emit('Completo');
      }else if(this.totalIncompleto >= this.total){
        this.semaforoBalanceFinanciero.emit(null);
      }else{
        this.semaforoBalanceFinanciero.emit('Incompleto');
      }
    });
  }

}