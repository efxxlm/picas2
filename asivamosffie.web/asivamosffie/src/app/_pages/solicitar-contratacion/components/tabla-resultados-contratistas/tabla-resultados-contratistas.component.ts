import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { FormBuilder, FormControl, Validators } from '@angular/forms';


export interface TableElement {
  id: number;
  nombre: string;
  nombreRepresentanteLegak: string;
  numeroInvitacion: string;
  numeroIdentificacion: string;
}

const ELEMENT_DATA: TableElement[] = [
  {
    id: 0,
    nombre: 'Constructora del Atlántico',
    nombreRepresentanteLegak: 'Camilo Albeiro Aponte Diaz ',
    numeroInvitacion: '000234',
    numeroIdentificacion: '10995556644'
  },
  {
    id: 1,
    nombre: 'Constructora del Atlántico S.A',
    nombreRepresentanteLegak: 'Juan Felipe Otero',
    numeroInvitacion: '000333',
    numeroIdentificacion: '23875556644'
  }
];

@Component({
  selector: 'app-tabla-resultados-contratistas',
  templateUrl: './tabla-resultados-contratistas.component.html',
  styleUrls: ['./tabla-resultados-contratistas.component.scss']
})
export class TablaResultadosContratistasComponent implements OnInit {

  unionTemporal: FormControl;

  displayedColumns: string[] = [
    'nombre',
    'nombreRepresentanteLegak',
    'numeroInvitacion',
    'numeroIdentificacion',
    'id'
  ];
  dataSource = new MatTableDataSource(ELEMENT_DATA);

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, {static: true}) sort: MatSort;

  elementosSelecciondos: any[] = [];

  constructor() {
    this.declararUnionTemporal();
  }

  private declararUnionTemporal() {
    this.unionTemporal = new FormControl(['free', Validators.required]);
  }

  ngOnInit(): void {
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
    this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
    this.paginator._intl.nextPageLabel = 'Siguiente';
    this.paginator._intl.previousPageLabel = 'Anterior';
  }

  addElement(event: any, elemento: any) {
    console.log(event);
    this.elementosSelecciondos.push(elemento);
  }
}
