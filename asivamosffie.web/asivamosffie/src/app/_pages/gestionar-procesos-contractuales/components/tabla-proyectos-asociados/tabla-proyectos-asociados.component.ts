import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-tabla-proyectos-asociados',
  templateUrl: './tabla-proyectos-asociados.component.html',
  styleUrls: ['./tabla-proyectos-asociados.component.scss']
})
export class TablaProyectosAsociadosComponent implements OnInit {
  @Input() proyectos : any[];
  @ViewChild( MatPaginator, { static: true } ) paginator: MatPaginator;
  @ViewChild( MatSort, { static: true } ) sort          : MatSort;
  dataSource                 = new MatTableDataSource();
  displayedColumns: string[] = [ 'idMen', 'tipoIntervencion', 'departamentoMunicipio', 'instEducativa', 'sede' ];
  ELEMENT_DATA    : any[]    = [
    { titulo: 'Id MEN', name: 'idMen' },
    { titulo: 'Tipo de Intervención', name: 'tipoIntervencion' },
    { titulo: 'Departamento/Municipio', name: 'departamentoMunicipio' },
    { titulo: 'Institución educativa', name: 'instEducativa'},
    { titulo: 'Sede', name: 'sede' },
  ];
  data: any[] = [];

  constructor() { }

  ngOnInit(): void {
    if(this.proyectos.length > 0){
      this.proyectos.forEach(element => {
        this.data.push({
          idMen                : element.llaveMen,
          tipoIntervencion     : element.tipoIntervencion,
          departamentoMunicipio: element.departamento+"/"+element.municipio,
          instEducativa        : element.institucionEducativa,
          sede                 : element.sede
        });
      });
    }
    this.dataSource                        = new MatTableDataSource( this.data );
    this.dataSource.paginator              = this.paginator;
    this.dataSource.sort                   = this.sort;
    this.paginator._intl.itemsPerPageLabel = 'El. por página';
  }

}
