import { Component, OnInit, Input, AfterViewInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialog } from '@angular/material/dialog';
import { DialogObservacionesComponent } from '../dialog-observaciones/dialog-observaciones.component'
import { CargarFlujoComponent } from '../cargar-flujo/cargar-flujo.component';
import { ReprogrammingService } from 'src/app/core/_services/reprogramming/reprogramming.service';

export interface VerificacionDiaria {
  id: string;
  fechaCargue: string;
  numeroToalRegistros: string;
  numeroRegistrosValidos: string;
  numeroRegistrosInalidos: string;
  estadoCargue: string;
}

const ELEMENT_DATA: VerificacionDiaria[] = [];

@Component({
  selector: 'app-flujo-intervencion-recursos',
  templateUrl: './flujo-intervencion-recursos.component.html',
  styleUrls: ['./flujo-intervencion-recursos.component.scss']
})
export class FlujoIntervencionRecursosComponent implements AfterViewInit, OnInit  {

  displayedColumns: string[] = [
    'fechaCreacion',
    'cantidadRegistros',
    'cantidadRegistrosValidos',
    'cantidadRegistrosInvalidos',
    'estadoCargue',
    'id'
  ];
  dataSource = new MatTableDataSource(ELEMENT_DATA);

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;

  @Input() ajusteProgramacionInfo:any;

  constructor(
    public dialog: MatDialog,
    private reprogrammingService : ReprogrammingService
  ) { }

  ngOnInit(): void {
    if (this.ajusteProgramacionInfo?.ajusteProgramacionId !== 0 && this.ajusteProgramacionInfo?.ajusteProgramacionId !== undefined) {
      this.reprogrammingService.getLoadAdjustInvestmentFlowGrid(this.ajusteProgramacionInfo?.ajusteProgramacionId)
        .subscribe((response: any[]) => {
          //response = response.filter( p => p.estadoCargue == 'Válidos' )
          this.dataSource = new MatTableDataSource(response);
          this.dataSource.paginator = this.paginator;
          this.dataSource.sort = this.sort;
          this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
        })
    };
  }

  openCargarFlujo() {
    const dialogRef = this.dialog.open(CargarFlujoComponent, {
      width: '75em',
      data: { ajusteProgramacionInfo: this.ajusteProgramacionInfo }
    });
    dialogRef.afterClosed()
    .subscribe(response => {
      if (response) {
        console.log(response);
      };
    })
  }

  openObservaciones(id: string) {
    const dialogCargarProgramacion = this.dialog.open(DialogObservacionesComponent, {
      width: '75em',
      // data: { }
    });
    dialogCargarProgramacion.afterClosed()
      .subscribe(response => {
        if (response) {
          console.log(response);
        };
      })
  }

  ngAfterViewInit() {
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();

    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }

}
