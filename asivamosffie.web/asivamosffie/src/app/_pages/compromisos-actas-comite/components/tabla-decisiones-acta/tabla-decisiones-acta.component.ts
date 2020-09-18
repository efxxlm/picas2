import { Component, OnInit, ViewChild } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-tabla-decisiones-acta',
  templateUrl: './tabla-decisiones-acta.component.html',
  styleUrls: ['./tabla-decisiones-acta.component.scss']
})
export class TablaDecisionesActaComponent implements OnInit {

  dataSource = new MatTableDataSource();
  @ViewChild( MatSort, { static: true } ) sort: MatSort;
  data: any[] = [];
  displayedColumns: string[] = [ 'idMen', 'tipoIntervencion', 'departamentoMunicipio', 'institucionEducativa', 'sede', 'estadoProyecto', 'gestion' ];
  ELEMENT_DATA: any[] = [
    {titulo: 'Id MEN', name: 'idMen'},
    { titulo: 'Tipo de Intervención', name: 'tipoIntervencion' },
    { titulo: 'Departamento/Municipio', name: 'departamentoMunicipio' },
    { titulo: 'Institución educativa', name: 'institucionEducativa' },
    { titulo: 'Sede', name: 'sede' },
  ];

  constructor() { }

  ngOnInit(): void {
    this.getData();
    this.dataSource = new MatTableDataSource( this.data );
    this.dataSource.sort = this.sort;
  }

  //getDataTabla
  getData () {

    this.data.push(
      { 
        idMen: 'LL000004',
        tipoIntervencion: 'Ampliación',
        departamentoMunicipio: 'Atlántico/Manatí',
        institucionEducativa: 'I.E. Antonio Nariño',
        sede: 'Única Sede',
        estadoProyecto: {
          devuelta: true,
          aprobada: false
        }
      },
      { 
        idMen: 'LL000006',
        tipoIntervencion: 'Reconstrucción',
        departamentoMunicipio: 'Valle del Cauca/Jamundí',
        institucionEducativa: 'I.E. Alfredo Bonilla Montaño',
        sede: 'Única Sede',
        estadoProyecto: {
          devuelta: false,
          aprobada: true
        }
      }
    );

  }

}
