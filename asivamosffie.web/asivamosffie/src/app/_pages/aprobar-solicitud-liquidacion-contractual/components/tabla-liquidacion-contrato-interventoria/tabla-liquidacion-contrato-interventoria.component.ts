import { Component, AfterViewInit, ViewChild, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { RegisterContractualLiquidationRequestService } from 'src/app/core/_services/registerContractualLiquidationRequest/register-contractual-liquidation-request.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { EstadosSolicitudLiquidacionContractual, EstadosSolicitudLiquidacionContractualCodigo, ListaMenuSolicitudLiquidacion, ListaMenuSolicitudLiquidacionId } from 'src/app/_interfaces/estados-solicitud-liquidacion-contractual';

@Component({
  selector: 'app-tabla-liquidacion-contrato-interventoria',
  templateUrl: './tabla-liquidacion-contrato-interventoria.component.html',
  styleUrls: ['./tabla-liquidacion-contrato-interventoria.component.scss']
})
export class TablaLiquidacionContratoInterventoriaComponent implements OnInit, AfterViewInit {

  ELEMENT_DATA: any[] = [];
  dataSource = new MatTableDataSource(this.ELEMENT_DATA);
  displayedColumns: string[] = [
    'fechaValidacionLiquidacion',
    'fechaPoliza',
    'numeroSolicitudLiquidacion',
    'numeroContrato',
    'valorSolicitud',
    'proyectosAsociados',
    'estadoAprobacionLiquidacionString',
    'contratacionProyectoId'
  ];

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  datosTabla = [];
  listaMenu: ListaMenuSolicitudLiquidacion = ListaMenuSolicitudLiquidacionId;
  listaEstadoLiquidacionSolicitud: EstadosSolicitudLiquidacionContractual = EstadosSolicitudLiquidacionContractualCodigo;

  constructor(
    private registerContractualLiquidationRequestService: RegisterContractualLiquidationRequestService,
    private dialog: MatDialog,
    private routes: Router
    ) { }

  ngOnInit(): void {
    this.getListContractualLiquidationInterventoria(this.listaMenu.aprobarSolicitudLiquidacionContratacion);
  }

  getListContractualLiquidationInterventoria(menuId: number) {
    this.registerContractualLiquidationRequestService.getListContractualLiquidationInterventoria(menuId).subscribe(report => {
      if(report != null){
        report.forEach(element => {
          this.datosTabla.push({
            fechaValidacionLiquidacion : element.fechaValidacionLiquidacion.split('T')[0].split('-').reverse().join('/'),
            fechaPoliza : element.fechaPoliza.split('T')[0].split('-').reverse().join('/'),
            numeroContrato: element.numeroContrato,
            valorSolicitud: element.valorSolicitud,
            proyectosAsociados: element.proyectosAsociados,
            estadoAprobacionLiquidacionString: element.estadoAprobacionLiquidacionString,
            estadoAprobacionLiquidacionCodigo: element.estadoAprobacionLiquidacionCodigo,
            numeroSolicitudLiquidacion: element.numeroSolicitudLiquidacion,
            contratacionProyectoId: element.contratacionProyectoId
          });
        })
      }
      this.dataSource.data = this.datosTabla;
    });
  }

  ngAfterViewInit() {
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
    this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
    this.paginator._intl.nextPageLabel = 'Siguiente';
    this.paginator._intl.getRangeLabel = (page, pageSize, length) => {
      if (length === 0 || pageSize === 0) {
        return '0 de ' + length;
      }
      length = Math.max(length, 0);
      const startIndex = page * pageSize;
      // If the start index exceeds the list length, do not try and fix the end index to the end.
      const endIndex = startIndex < length ?
        Math.min(startIndex + pageSize, length) :
        startIndex + pageSize;
      return startIndex + 1 + ' - ' + endIndex + ' de ' + length;
    };
    this.paginator._intl.previousPageLabel = 'Anterior';
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();

    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }

  openDialog(modalTitle: string, modalText: string) {
    const dialogRef = this.dialog.open( ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }

  SendToNovedades( pContratacionProyectoId: number ) {
    const pContratacionProyecto = {
        contratacionProyectoId: pContratacionProyectoId,
        estadoAprobacionLiquidacionCodigo: this.listaEstadoLiquidacionSolicitud.enviadoControlSeguimiento
    };

    this.registerContractualLiquidationRequestService.changeStatusLiquidacionContratacionProyecto( pContratacionProyecto, this.listaMenu.aprobarSolicitudLiquidacionContratacion )
        .subscribe(
            response => {
                this.openDialog( '', `<b>${ response.message }</b>` );
                this.routes.navigateByUrl( '/', {skipLocationChange: true} )
                .then( () => this.routes.navigate( ['/aprobarSolicitudLiquidacionContractual'] ) );
            }, err => this.openDialog( '', `<b>${ err.message }</b>` )
        );
    }


}