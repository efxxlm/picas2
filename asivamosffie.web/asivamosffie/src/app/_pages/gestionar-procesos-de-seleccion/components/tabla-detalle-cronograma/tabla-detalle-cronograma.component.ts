import { Component, OnInit, ViewChild, Input } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ProcesoSeleccionService, ProcesoSeleccion } from 'src/app/core/_services/procesoSeleccion/proceso-seleccion.service';
import { ActivatedRoute } from '@angular/router';

export interface ProcesosElement {
  id: number;
  tipo: string;
  numero: string;
  fechaSolicitud: string;
  numeroSolicitud: string;
  estadoDelSolicitud: string;
}

const ELEMENT_DATA: ProcesosElement[] = [
  {
    id: 1,
    tipo: 'Selección privada',
    numero: 'SP 0007-2020',
    fechaSolicitud: '01/06/2020',
    numeroSolicitud: '0001',
    estadoDelSolicitud: 'Creada',
  }
];

@Component({
  selector: 'app-tabla-detalle-cronograma',
  templateUrl: './tabla-detalle-cronograma.component.html',
  styleUrls: ['./tabla-detalle-cronograma.component.scss']
})
export class TablaDetalleCronogramaComponent implements OnInit {

  @Input() editMode: any = {};
  idProcesoseleccion: number = 0;

  displayedColumns: string[] = [ 'tipo', 'numero', 'fechaSolicitud', 'numeroSolicitud', 'estadoDelSolicitud', 'id'];
  dataSource = new MatTableDataSource();

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, {static: true}) sort: MatSort;

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  constructor(
                private procesoSeleccionService: ProcesoSeleccionService,
                private activatedRoute: ActivatedRoute,

             ) 
  { }

  ngOnInit(): void {

    this.activatedRoute.params.subscribe( parametros => {
      this.idProcesoseleccion = parametros['id'];

      let listaProcesos: ProcesoSeleccion[] = []; 
      this.procesoSeleccionService.getProcesoSeleccionById( this.idProcesoseleccion ).subscribe( proceso => {
        
        listaProcesos.push( proceso );
        console.log( proceso );
        this.dataSource = new MatTableDataSource( listaProcesos );

        this.dataSource.sort = this.sort;
        this.dataSource.paginator = this.paginator;
        this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
        this.paginator._intl.nextPageLabel = 'Siguiente';
        this.paginator._intl.previousPageLabel = 'Anterior';
      })

    })

    
  }

  onDetalle(){
    this.editMode.valor = !this.editMode.valor;
    console.log( this.editMode.valor );
  }

}
