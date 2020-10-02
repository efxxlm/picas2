import { Component, OnInit, ViewChild, Input } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Contratacion } from 'src/app/_interfaces/project-contracting';
import { Router } from '@angular/router';

@Component({
  selector: 'app-table-fuentes-y-usos',
  templateUrl: './table-fuentes-y-usos.component.html',
  styleUrls: ['./table-fuentes-y-usos.component.scss']
})
export class TableFuentesYUsosComponent implements OnInit {

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

  constructor ( private routes: Router ) { }

  ngOnInit(): void {
    this.dataSource.sort = this.sort;
  }

  cargarRegistros(){
    this.dataSource = new MatTableDataSource( this.contratacion.contratacionProyecto );
  }

  definirFuentes ( id: number, municipio: any ) {
    this.routes.navigate( [ '/solicitarContratacion/definir-fuentes', id ], { state: {municipio: municipio} } )
  }

}
