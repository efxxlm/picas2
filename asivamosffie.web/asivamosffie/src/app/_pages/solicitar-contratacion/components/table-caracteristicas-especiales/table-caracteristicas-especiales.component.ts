import { Component, OnInit, ViewChild, Input } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Contratacion } from 'src/app/_interfaces/project-contracting';

@Component({
  selector: 'app-table-caracteristicas-especiales',
  templateUrl: './table-caracteristicas-especiales.component.html',
  styleUrls: ['./table-caracteristicas-especiales.component.scss']
})
export class TableCaracteristicasEspecialesComponent implements OnInit {

  @Input() contratacion: Contratacion

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

  @ViewChild(MatSort, {static: true}) sort: MatSort;

  constructor() { }

  ngOnInit(): void {
    //this.dataSource = new MatTableDataSource();
    this.dataSource.sort = this.sort;
  }

  cargarRegistros(){
    this.dataSource = new MatTableDataSource( this.contratacion.contratacionProyecto );
  }

}
