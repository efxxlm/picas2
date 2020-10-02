import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { DialogObservacionesComponent } from '../dialog-observaciones/dialog-observaciones.component';

@Component({
  selector: 'app-detalle-planes-programas',
  templateUrl: './detalle-planes-programas.component.html',
  styleUrls: ['./detalle-planes-programas.component.scss']
})
export class DetallePlanesProgramasComponent implements OnInit {

  dataPlanesProgramas: any[] = [];
  dataSource                 = new MatTableDataSource();
  displayedColumns: string[] = [ 
    'planesProgramas',
    'recibioRequisito',
    'fechaRadicado',
    'fechaAprobacion',
    'requiereObservacion',
    'observaciones'
  ];
  booleanosRequisitos: any[] = [
    { value: true, viewValue: 'Si' },
    { value: false, viewValue: 'No' }
  ]
  require: any;
  booleanosObservacion: any[] = [
    { value: true, viewValue: 'Si' },
    { value: false, viewValue: 'No' }
  ]
  urlSoporte: string;

  constructor ( private dialog: MatDialog ) {
    this.getDataPlanesProgramas();
  }

  ngOnInit(): void {
    this.dataSource = new MatTableDataSource( this.dataPlanesProgramas );
  }

  openDialog ( planPrograma: string, id: number ) {
    const dialogObservacion = this.dialog.open(DialogObservacionesComponent, {
      width: '60em',
      data : { planPrograma }
    });

    dialogObservacion.afterClosed().subscribe( resp => {
      this.dataPlanesProgramas.forEach( data => {
        if ( data.id === id ) {
          data.observaciones = resp.data
        }
      } )
    } );
  };

  guardar () {
    console.log( this.dataPlanesProgramas );
    console.log( this.urlSoporte );
  }

  getDataPlanesProgramas () {
    this.dataPlanesProgramas.push(
      {
        nombrePlanesProgramas: 'Licencia vigente',
        recibioRequisito: 'Si',
        fechaRadicado: '20/07/2020',
        fechaAprobacion: '19/07/2020',
        requiereObservacion: 'No',
        observaciones: null,
        id: 1
      },
      {
        nombrePlanesProgramas: 'Cambio constructor responsable de la licencia',
        recibioRequisito: 'Si',
        fechaRadicado: '20/07/2020',
        fechaAprobacion: '19/07/2020',
        requiereObservacion: 'No',
        observaciones: null,
        id: 2
      },
      {
        nombrePlanesProgramas: 'Acta aceptación y apropiación diseños',
        recibioRequisito: 'Si',
        fechaRadicado: '20/07/2020',
        fechaAprobacion: '19/07/2020',
        requiereObservacion: 'No',
        observaciones: null,
        id: 3
      },
      {
        nombrePlanesProgramas: '¿Cuenta con plan de residuos de construcción y demolición (RCD) aprobado?',
        recibioRequisito: 'Si',
        fechaRadicado: '20/07/2020',
        fechaAprobacion: '19/07/2020',
        requiereObservacion: 'Si',
        observaciones: null,
        id: 4
      },
      {
        nombrePlanesProgramas: '¿Cuenta con plan de manejo de tránsito (PMT) aprobado?',
        recibioRequisito: 'Si',
        fechaRadicado: '20/07/2020',
        fechaAprobacion: '19/07/2020',
        requiereObservacion: 'No',
        observaciones: null,
        id: 5
      },
      {
        nombrePlanesProgramas: '¿Cuenta con plan de manejo ambiental aprobado?',
        recibioRequisito: 'Si',
        fechaRadicado: '20/07/2020',
        fechaAprobacion: '19/07/2020',
        requiereObservacion: 'No',
        observaciones: null,
        id: 6
      },
      {
        nombrePlanesProgramas: '¿Cuenta con plan de aseguramiento de la calidad de obra aprobado?',
        recibioRequisito: 'Si',
        fechaRadicado: '20/07/2020',
        fechaAprobacion: '19/07/2020',
        requiereObservacion: 'No',
        observaciones: null,
        id: 7
      },
      {
        nombrePlanesProgramas: '¿Cuenta con programa de Seguridad industrial aprobado?',
        recibioRequisito: 'Si',
        fechaRadicado: '20/07/2020',
        fechaAprobacion: '19/07/2020',
        requiereObservacion: 'No',
        observaciones: null,
        id: 8
      },
      {
        nombrePlanesProgramas: '¿Cuenta con programa de salud ocupacional aprobado?',
        recibioRequisito: 'Si',
        fechaRadicado: '20/07/2020',
        fechaAprobacion: '19/07/2020',
        requiereObservacion: 'No',
        observaciones: null,
        id: 9
      },
      {
        nombrePlanesProgramas: '¿Cuenta con un plan inventario arbóreo (talas) aprobado?',
        recibioRequisito: 'No se requiere',
        fechaRadicado: '20/07/2020',
        fechaAprobacion: '19/07/2020',
        requiereObservacion: 'Si',
        observaciones: null,
        id: 10
      },
      {
        nombrePlanesProgramas: '¿Cuenta con plan de aprovechamiento forestal aprobado?',
        recibioRequisito: 'No se requiere',
        fechaRadicado: '20/07/2020',
        fechaAprobacion: '19/07/2020',
        requiereObservacion: 'No',
        observaciones: null,
        id: 11
      },
      {
        nombrePlanesProgramas: '¿Cuenta con plan de manejo de aguas lluvias aprobado?',
        recibioRequisito: 'No se requiere',
        fechaRadicado: '20/07/2020',
        fechaAprobacion: '19/07/2020',
        requiereObservacion: 'Si',
        observaciones: null,
        id: 12
      }
    );
  };

}
