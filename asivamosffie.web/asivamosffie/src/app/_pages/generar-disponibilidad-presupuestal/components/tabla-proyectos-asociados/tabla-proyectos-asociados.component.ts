import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

export interface TableElement {
  id: number;
  llaveMen: string;
  tipoInterventor: string;
  departamento: string;
  municipio: string;
  institucionEducativa: string;
  sede: string;
  tipoAportante: string;
  nombreAportante: string;
  fuente: string;
  valorDelAportante: number;
}

const ELEMENT_DATA: TableElement[] = [
];

@Component({
  selector: 'app-tabla-proyectos-asociados',
  templateUrl: './tabla-proyectos-asociados.component.html',
  styleUrls: ['./tabla-proyectos-asociados.component.scss']
})
export class TablaProyectosAsociadosComponent implements OnInit {

  @Input()proyectos: any;
  @Input()codigo: any;
  @Input()ver: any;
  
  displayedColumns: string[] = [
    'llaveMen',
    'tipoInterventor',
    'departamento',
    'institucionEducativa',
    'tipoAportante',
    'nombreAportante',
    'fuente',
    'valorDelAportante'
  ];
  dataSource = new MatTableDataSource(ELEMENT_DATA);

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  constructor() { }

  ngOnInit(): void {
    let elements:TableElement[]=[];
    this.proyectos.forEach(element => {
      elements.push({
        llaveMen:element.llaveMen,
        departamento:element.departamento,
        //estado:element.valorGestionado>0,//
        id:element.aportanteID,//el aprotante id
        institucionEducativa:element.institucionEducativa,
        municipio:element.municipio,
        sede:element.sede,
        nombreAportante:element.nombreAportante,
        tipoInterventor:element.tipoIntervencion,//revisar
        valorDelAportante:element.valorAportante,
        //disponibilidadPresupuestalProyectoid:element.disponibilidadPresupuestalProyecto,
        //valorGestionado:element.valorGestionado,
        //ver:this.ver
        fuente:"Recursos propios",
        tipoAportante:"FFIE",
        
      });  
    });
    console.log(elements);
    this.dataSource = new MatTableDataSource(elements);
    this.inicializarTabla();
    
  }

  inicializarTabla(){
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
    this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
    this.paginator._intl.getRangeLabel = (page, pageSize, length) => {
      if (length === 0 || pageSize === 0) {
        return '0 de ' + length;
      }
      length = Math.max(length, 0);
      const startIndex = page * pageSize;
      // If the start index exceeds the list length, do not try and fix the end index to the end.
      const endIndex = startIndex < length ?
        Math.min(startIndex + pageSize, length) :
        startIndex + pageSize;
      return startIndex + 1 + ' - ' + endIndex + ' de ' + length;
    };
  }

}
