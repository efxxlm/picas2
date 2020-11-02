import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { TechnicalCommitteSessionService } from 'src/app/core/_services/technicalCommitteSession/technical-committe-session.service';
import { SesionComiteSolicitud } from 'src/app/_interfaces/technicalCommitteSession';
import { BudgetAvailabilityService } from 'src/app/core/_services/budgetAvailability/budget-availability.service';
import { GrillaDisponibilidadPresupuestal } from 'src/app/_interfaces/budgetAvailability';
import { forkJoin } from 'rxjs';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';


@Component({
  selector: 'app-tabla-crear-solicitud-tradicional',
  templateUrl: './tabla-crear-solicitud-tradicional.component.html',
  styleUrls: ['./tabla-crear-solicitud-tradicional.component.scss']
})
export class TablaCrearSolicitudTradicionalComponent implements OnInit {

  verAyuda = false;

  listaSolicitudes: GrillaDisponibilidadPresupuestal[] = [];

  displayedColumns: string[] = [
    'fecha',
    'tipo',
    'numero',
    'opcionPorContratar',
    'valorSolicitado',
    'estado',
    'id'
  ];
  dataSource = new MatTableDataSource();

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  constructor(
                private budgetAvailabilityService: BudgetAvailabilityService,
                public dialog: MatDialog,

             )
  {

  }

  ngOnInit(): void {

    forkJoin([
      //this.budgetAvailabilityService.getGridBudgetAvailability(),
      this.budgetAvailabilityService.getReuestCommittee(),

    ]).subscribe( response => {
        this.listaSolicitudes = response[0];
        this.dataSource = new MatTableDataSource( this.listaSolicitudes) ;
        console.log( response[0] );
      })

    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
    this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
    this.paginator._intl.nextPageLabel = 'Siguiente';
    this.paginator._intl.previousPageLabel = 'Anterior';
    this.paginator._intl.getRangeLabel = (page, pageSize, length) => {
      if (length === 0 || pageSize === 0) { return '0 de ' + length; }
      length = Math.max(length, 0);
      const startIndex = page * pageSize;
      const endIndex = startIndex < length ? Math.min(startIndex + pageSize, length) : startIndex + pageSize;
      return startIndex + 1 + ' - ' + endIndex + ' de ' + length;
    };
  }

  openDialog(modalTitle: string, modalText: string) {
    const dialogRef = this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }

  enviarSolicitud(e: number) {
    console.log(e);
    this.budgetAvailabilityService.sendRequest( e )
      .subscribe( respuesta => {
        this.openDialog( '', `<b>${respuesta.message}</b>` );
        if (respuesta.code == "200")
          this.ngOnInit();
      })
  }

  openDialogSiNo(modalTitle: string, modalText: string, e:number) {
    let dialogRef =this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText,siNoBoton:true }
    });
    dialogRef.afterClosed().subscribe(result => {
      console.log(`Dialog result: ${result}`);
      if(result === true)
      {
        this.budgetAvailabilityService.deleteRequest( e )
        .subscribe( respuesta => {
          console.log(respuesta);
        this.openDialog( '', `<b>${respuesta.message}</b>` );
        if (respuesta.code == "200")
          this.ngOnInit();
      })
      }
    });
  }


  eliminarSolicitud(e: number) {
    console.log(e);
    this.openDialogSiNo('', '<b>¿Está seguro de eliminar este registro?</b>', e)

  }

  verDetalle(e: number) {
    console.log(e);
  }

  registrarInformacion(e: number) {
    console.log(e);
  }

}
