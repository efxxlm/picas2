import { Component, Inject, ViewChild, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

export interface TableElement {
  id: number;
  tipoInterventor: string;
  llaveMEN: string;
  region: string;
  departamento: string;
  municipio: string;
  institucionEducativa: string;
  sede: string;
}

@Component({
  selector: 'app-dialog-table-proyectos-seleccionados',
  templateUrl: './dialog-table-proyectos-seleccionados.component.html',
  styleUrls: ['./dialog-table-proyectos-seleccionados.component.scss']
})
export class DialogTableProyectosSeleccionadosComponent implements OnInit {

  displayedColumns: string[] = [
    'tipoInterventor',
    'llaveMEN',
    'region',
    'departamento',
    'institucionEducativa',
    'sede',
    'id'
  ];

  listaEliminados: TableElement[] = [];

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;

  dataSource = new MatTableDataSource(this.data);

  constructor(
    public dialogRef: MatDialogRef<DialogTableProyectosSeleccionadosComponent>,
    @Inject(MAT_DIALOG_DATA) public data: TableElement[]
  ) { }

  ngOnInit(): void {
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
    this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
    this.paginator._intl.nextPageLabel = 'Siguiente';
    this.paginator._intl.previousPageLabel = 'Anterior';
  }

  addRemoveElement(seleccionado: boolean, elemento: any) {
    if (seleccionado){
      console.log( this.data.indexOf( elemento ) );
      this.listaEliminados.push( elemento );
    }
  }

  actualizar(){
    this.listaEliminados.forEach( e => {
      let i = this.data.indexOf( e );
      if (i > -1){
        this.data.splice(i,1);
        console.log(e);
      }
    })
    console.log(this.data);
  }

}
