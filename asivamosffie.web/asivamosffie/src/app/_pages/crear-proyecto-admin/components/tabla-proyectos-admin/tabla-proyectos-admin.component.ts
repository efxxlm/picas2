import { Component, ViewChild } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatTableDataSource,MatTableModule } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';

export interface RegistrosCargados {
  id: number;    
  consecutivo: number;
  estado:string;
}

const ELEMENT_DATA: RegistrosCargados[] = [
  {
    id: 1,
    consecutivo: 1,
    estado: "Completo"
  },
  {
    id: 2,
    consecutivo: 2,
    estado: "Completo"
  },
];

@Component({
  selector: 'app-tabla-proyectos-admin',
  templateUrl: './tabla-proyectos-admin.component.html',
  styleUrls: ['./tabla-proyectos-admin.component.scss']
})
export class TablaProyectosAdminComponent {
  displayedColumns: string[] = ['consecutivo', 'estado','gestion'];
  dataSource = new MatTableDataSource(ELEMENT_DATA);

  columnas = [
    { titulo: 'Consecutivo proyecto administrativo', name: 'consecutivo' },
    { titulo: 'Estado del registro', name: 'estado' }
  ];

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  constructor() {
  }

  ngOnInit(): void {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
    this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
    this.paginator._intl.nextPageLabel = 'Siguiente';
    this.paginator._intl.previousPageLabel = 'Anterior';
  }

  ver(gestion:any)
  {
    console.log(gestion);    
  }

  eliminar(gestion:any)
  {
    console.log(gestion);    
  }
  
  enviar(gestion:any)
  {
    console.log(gestion);
  }

}
