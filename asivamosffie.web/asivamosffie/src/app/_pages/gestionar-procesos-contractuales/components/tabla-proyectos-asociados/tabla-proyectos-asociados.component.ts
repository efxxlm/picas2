import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-tabla-proyectos-asociados',
  templateUrl: './tabla-proyectos-asociados.component.html',
  styleUrls: ['./tabla-proyectos-asociados.component.scss']
})
export class TablaProyectosAsociadosComponent implements OnInit {

  dataSource = new MatTableDataSource();
  @ViewChild( MatPaginator, { static: true } ) paginator: MatPaginator;
  @ViewChild( MatSort, { static: true } ) sort: MatSort;
  displayedColumns: string[] = [ 'idMen', 'tipoIntervencion', 'departamentoMunicipio', 'instEducativa', 'sede' ];
  ELEMENT_DATA: any[] = [
    {titulo: 'Id MEN', name: 'idMen'},
    { titulo: 'Tipo de Intervención', name: 'tipoIntervencion' },
    { titulo: 'Departamento/Municipio', name: 'departamentoMunicipio' },
    {titulo: 'Institución educativa', name: 'instEducativa'},
    {titulo: 'Sede', name: 'sede'},
  ];
  data: any[] = [
    {
      idMen: 'LL000012',
      tipoIntervencion: 'Ampliación',
      departamentoMunicipio: 'Valle del Cauca/Buga',
      instEducativa: 'I.E. Manuela Beltran',
      sede: 'Sede principal'
    }
  ]

  constructor() { }

  ngOnInit(): void {
    this.dataSource = new MatTableDataSource( this.data );
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
    this.paginator._intl.itemsPerPageLabel = 'El. por página';
  }

}
