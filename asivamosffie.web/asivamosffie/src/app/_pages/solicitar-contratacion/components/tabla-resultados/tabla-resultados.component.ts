import { Component, OnInit, ViewChild, Input } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialog } from '@angular/material/dialog';

import { DialogTableProyectosSeleccionadosComponent } from '../dialog-table-proyectos-seleccionados/dialog-table-proyectos-seleccionados.component';
import { AsociadaComponent } from '../asociada/asociada.component';
import { ProyectoGrilla } from 'src/app/core/_services/project/project.service';



@Component({
  selector: 'app-tabla-resultados',
  templateUrl: './tabla-resultados.component.html',
  styleUrls: ['./tabla-resultados.component.scss']
})

export class TablaResultadosComponent implements OnInit {

  @Input() listaResultados: ProyectoGrilla[];

  displayedColumns: string[] = [
    'tipoInterventor',
    'llaveMEN',
    'region',
    'departamento',
    'institucionEducativa',
    'sede',
    'id'
  ];
  dataSource = new MatTableDataSource(this.listaResultados);

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, {static: true}) sort: MatSort;

  elementosSelecciondos: any[] = [];

  constructor(public dialog: MatDialog) { }

  ngOnInit(): void {
    console.log('entró');
    console.log('lista', this.elementosSelecciondos);

    this.dataSource = new MatTableDataSource(this.listaResultados);

    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
    this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
    this.paginator._intl.nextPageLabel = 'Siguiente';
    this.paginator._intl.previousPageLabel = 'Anterior';
  }

  addElement(seleccionado: boolean, elemento: any) {
    if (seleccionado)
      this.elementosSelecciondos.push(elemento);
  }

  verSeleccionados() {
    console.log(this.elementosSelecciondos);
    const dialogRef = this.dialog.open(DialogTableProyectosSeleccionadosComponent, {
      data: this.elementosSelecciondos
    });
  }

  openPopup() {
    this.dialog.open(AsociadaComponent, {
      data: this.elementosSelecciondos
    });
  }
}
