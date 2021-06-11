import { Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { RegisterContractualLiquidationRequestService } from 'src/app/core/_services/registerContractualLiquidationRequest/register-contractual-liquidation-request.service';
import { ListaMenuSolicitudLiquidacion, ListaMenuSolicitudLiquidacionId } from 'src/app/_interfaces/estados-solicitud-liquidacion-contractual';

@Component({
  selector: 'app-tabla-balance-financiero-gtlc',
  templateUrl: './tabla-balance-financiero-gtlc.component.html',
  styleUrls: ['./tabla-balance-financiero-gtlc.component.scss']
})
export class TablaBalanceFinancieroGtlcComponent implements OnInit {

  @Input() verDetalleBtn;
  @Input() esVerDetalle: boolean;
  @Input() contratacionId: number;
  @Output() semaforoBalanceFinanciero = new EventEmitter<string>();
  listaMenu: ListaMenuSolicitudLiquidacion = ListaMenuSolicitudLiquidacionId;
  total: number = 0;
  totalCompleto: number = 0;
  totalIncompleto: number = 0;
  
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
  
  datosTabla = [];
  dataSource = new MatTableDataSource(this.ELEMENT_DATA);
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;


  constructor(private router: Router,private registerContractualLiquidationRequestService: RegisterContractualLiquidationRequestService
    ) { }

  ngOnInit(): void {
    this.getBalanceBycontratacionId(this.contratacionId);
  }

  getBalanceBycontratacionId(contratacionId: number) {
    this.registerContractualLiquidationRequestService.getBalanceByContratacionId(contratacionId, this.listaMenu.gestionarSolicitudLiquidacionContratacion).subscribe(report => {
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

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  };
}
