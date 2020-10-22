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
  };

  cargarRegistros(){
    this.dataSource = new MatTableDataSource( this.contratacion.contratacionProyecto );
  };

  definirFuentes ( id: number, municipio: any ) {
    this.routes.navigate( [ '/solicitarContratacion/definir-fuentes', id ], { state: {municipio: municipio} } )
  };

  getSemaforo ( elemento: any[] ) {
    let registroCompletos = 0;
    let registroEnProceso = 0;
    let registroSinDiligenciar = 0;
    elemento.forEach( value => {

      value.componenteAportante.forEach( componente => {
        if ( componente.registroCompleto === undefined ) {
          registroSinDiligenciar++;
        };
        if ( componente.componenteUso.length === 0 ) {
          registroSinDiligenciar++
        };
        if ( componente.registroCompleto === false ) {
          registroEnProceso++;
        };
        if ( componente.registroCompleto === true ) {
          registroCompletos++;
        };
      } );
    } );
    if ( registroSinDiligenciar === elemento.length ) {
      return 'sin-diligenciar';
    };
    if ( registroEnProceso > registroSinDiligenciar || registroEnProceso < registroCompletos ) {
      return 'en-proceso';
    };
    if ( registroCompletos === elemento.length ) {
      return 'completo';
    };
  };

}
