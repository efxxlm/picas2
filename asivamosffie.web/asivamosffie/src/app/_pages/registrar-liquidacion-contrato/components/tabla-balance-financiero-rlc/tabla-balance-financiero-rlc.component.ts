import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { RegisterContractualLiquidationRequestService } from 'src/app/core/_services/registerContractualLiquidationRequest/register-contractual-liquidation-request.service';
import { ListaMenuSolicitudLiquidacion, ListaMenuSolicitudLiquidacionId } from 'src/app/_interfaces/estados-solicitud-liquidacion-contractual';

@Component({
  selector: 'app-tabla-balance-financiero-rlc',
  templateUrl: './tabla-balance-financiero-rlc.component.html',
  styleUrls: ['./tabla-balance-financiero-rlc.component.scss']
})
export class TablaBalanceFinancieroRlcComponent implements OnInit {
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
  dataSource = new MatTableDataSource();

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  @Input() contratacionId: number;
  listaMenu: ListaMenuSolicitudLiquidacion = ListaMenuSolicitudLiquidacionId;


  datosTabla = [];

  constructor(private router: Router,private registerContractualLiquidationRequestService: RegisterContractualLiquidationRequestService) { }

  ngOnInit(): void {
    console.log(this.contratacionId);
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
    });
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  };
  verDetalle(id){
    this.router.navigate(['/registrarLiquidacionContrato/detalleBalanceFinanciero', id]);
  }
}
