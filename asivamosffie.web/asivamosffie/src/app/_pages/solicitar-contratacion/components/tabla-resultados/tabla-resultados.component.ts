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
  @Input() esMultiproyecto;

  displayedColumns: string[] = [
    'tipoInterventor',
    'llaveMEN',
    'region',
    'departamento',
    'institucionEducativa',
    'sede',
    'id'
  ];
  dataSource = new MatTableDataSource();

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, {static: true}) sort: MatSort;

  elementosSelecciondos: any[] = [];
   

  constructor ( public dialog: MatDialog ) { }

  ngOnInit(): void {
    console.log('lista', this.esMultiproyecto );
    let lista = [];
    if ( this.elementosSelecciondos.length > 0 ) {
      this.elementosSelecciondos.forEach( seleccionados => {
        this.listaResultados.forEach( ( proyecto, value ) => {
          if ( proyecto.proyectoId === seleccionados.proyectoId ) {
            this.listaResultados.splice( value, 1 );
          };
        } );
      } );
      lista = this.listaResultados;
    } else {
      lista = this.listaResultados
    }

    this.dataSource = new MatTableDataSource(lista);

    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
    this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
    this.paginator._intl.nextPageLabel = 'Siguiente';
    this.paginator._intl.previousPageLabel = 'Anterior';
  }

  addElement(seleccionado: boolean, elemento: any) {
    if (seleccionado) {
      this.elementosSelecciondos.push(elemento);
    } else {
      this.elementosSelecciondos.forEach( ( value, index ) => {
        if ( value.proyectoId === elemento.proyectoId ) {
          this.elementosSelecciondos.splice( index, 1 );
        };
      } );
    };
  };

  verSeleccionados() {
    //console.log(this.elementosSelecciondos);
    const dialogRef = this.dialog.open(DialogTableProyectosSeleccionadosComponent, {
      data: this.elementosSelecciondos
    });

    dialogRef.afterClosed().subscribe( console.log );
  }

  openPopup() {

    this.dialog.open(AsociadaComponent, {
      data: { data: this.elementosSelecciondos, esMultiproyecto: this.esMultiproyecto }
    });

  }
}
