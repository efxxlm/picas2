import { Component, OnInit, ViewChild } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { FormGroup, FormBuilder } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { DialogObservacionesComponent } from '../dialog-observaciones/dialog-observaciones.component';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';

@Component({
  selector: 'app-planes-programas',
  templateUrl: './planes-programas.component.html',
  styleUrls: ['./planes-programas.component.scss']
})
export class PlanesProgramasComponent implements OnInit {

  @ViewChild( MatPaginator, { static: true } ) paginator: MatPaginator;
  @ViewChild( MatSort, { static: true } ) sort          : MatSort;
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
    this.dataSource.paginator              = this.paginator;
    this.dataSource.sort                   = this.sort;
    this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
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
        recibioRequisito: null,
        fechaRadicado: null,
        fechaAprobacion: null,
        requiereObservacion: null,
        observaciones: null,
        id: 1
      },
      {
        nombrePlanesProgramas: 'Cambio constructor responsable de la licencia',
        recibioRequisito: null,
        fechaRadicado: null,
        fechaAprobacion: null,
        requiereObservacion: null,
        observaciones: null,
        id: 2
      },
      {
        nombrePlanesProgramas: 'Acta aceptación y apropiación diseños',
        recibioRequisito: null,
        fechaRadicado: null,
        fechaAprobacion: null,
        requiereObservacion: null,
        observaciones: null,
        id: 3
      },
      {
        nombrePlanesProgramas: '¿Cuenta con plan de residuos de construcción y demolición (RCD) aprobado?',
        recibioRequisito: null,
        fechaRadicado: null,
        fechaAprobacion: null,
        requiereObservacion: null,
        observaciones: null,
        id: 4
      },
      {
        nombrePlanesProgramas: '¿Cuenta con plan de manejo de tránsito (PMT) aprobado?',
        recibioRequisito: null,
        fechaRadicado: null,
        fechaAprobacion: null,
        requiereObservacion: null,
        observaciones: null,
        id: 5
      },
      {
        nombrePlanesProgramas: '¿Cuenta con plan de manejo ambiental aprobado?',
        recibioRequisito: null,
        fechaRadicado: null,
        fechaAprobacion: null,
        requiereObservacion: null,
        observaciones: null,
        id: 6
      },
      {
        nombrePlanesProgramas: '¿Cuenta con plan de manejo ambiental aprobado?',
        recibioRequisito: null,
        fechaRadicado: null,
        fechaAprobacion: null,
        requiereObservacion: null,
        observaciones: null,
        id: 7
      },
      {
        nombrePlanesProgramas: '¿Cuenta con plan de aseguramiento de la calidad de obra aprobado?',
        recibioRequisito: null,
        fechaRadicado: null,
        fechaAprobacion: null,
        requiereObservacion: null,
        observaciones: null,
        id: 8
      },
      {
        nombrePlanesProgramas: '¿Cuenta con programa de Seguridad industrial aprobado?',
        recibioRequisito: null,
        fechaRadicado: null,
        fechaAprobacion: null,
        requiereObservacion: null,
        observaciones: null,
        id: 9
      },
      {
        nombrePlanesProgramas: '¿Cuenta con programa de salud ocupacional aprobado?',
        recibioRequisito: null,
        fechaRadicado: null,
        fechaAprobacion: null,
        requiereObservacion: null,
        observaciones: null,
        id: 10
      },
      {
        nombrePlanesProgramas: '¿Cuenta con un plan inventario arbóreo (talas) aprobado?',
        recibioRequisito: null,
        fechaRadicado: null,
        fechaAprobacion: null,
        requiereObservacion: null,
        observaciones: null,
        id: 11
      },
      {
        nombrePlanesProgramas: '¿Cuenta con plan de aprovechamiento forestal aprobado?',
        recibioRequisito: null,
        fechaRadicado: null,
        fechaAprobacion: null,
        requiereObservacion: null,
        observaciones: null,
        id: 12
      },
      {
        nombrePlanesProgramas: '¿Cuenta con plan de manejo de aguas lluvias aprobado?',
        recibioRequisito: null,
        fechaRadicado: null,
        fechaAprobacion: null,
        requiereObservacion: null,
        observaciones: null,
        id: 13
      }
    );
  };

};