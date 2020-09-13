import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { TechnicalCommitteSessionService } from 'src/app/core/_services/technicalCommitteSession/technical-committe-session.service';
import { SesionComiteSolicitud } from 'src/app/_interfaces/technicalCommitteSession';
import { BudgetAvailabilityService } from 'src/app/core/_services/budgetAvailability/budget-availability.service';
import { GrillaDisponibilidadPresupuestal } from 'src/app/_interfaces/budgetAvailability';
import { forkJoin } from 'rxjs';


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
                private budgetAvailabilityService: BudgetAvailabilityService
             )
  {

  }

  ngOnInit(): void {

    forkJoin([
      this.budgetAvailabilityService.getGridBudgetAvailability(),
      this.budgetAvailabilityService.getReuestCommittee(),

    ]).subscribe( response => {
        this.listaSolicitudes = response[0].value;
        this.dataSource = new MatTableDataSource( this.listaSolicitudes) ;
        console.log( response[1] );
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

  enviarSolicitud(e: number) {
    console.log(e);
  }

  verDetalle(e: number) {
    console.log(e);
  }

  registrarInformacion(e: number) {
    console.log(e);
  }

}
